using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainShoot : MonoBehaviour
{
    [SerializeField] float refreshRate = 0.01f;
    [SerializeField][Range(1, 10)] int maximunEnemisInChain = 3;
    [SerializeField] float delayBetweeneachChain = 0.3f;
    [SerializeField] Transform playerFirePoint;
    [SerializeField] EnemyDetector playerEnemyDetector;
    [SerializeField] GameObject lineRendererPrefab;

    bool shooting;
    bool shot;
    float counter = 1;
    GameObject currentClosestEnemy;
    List<GameObject> spawnedLineRenderers = new List<GameObject>();
    List<GameObject> enemiesInChain = new List<GameObject>();
    List<GameObject> activeEffect = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
   

    void StartShooting()
    {
        shooting = false;
        shot = false;
        counter = 1;

        for (int i = 0; i < spawnedLineRenderers.Count; i++)
        {
            Destroy(spawnedLineRenderers[i]);
        }

        spawnedLineRenderers.Clear();
        enemiesInChain.Clear();

        for ( int i = 0; i < activeEffect.Count; i++)
        {
            Destroy(activeEffect[i]);
        }

        activeEffect.Clear();

    }
    IEnumerator UpdateLineRenderer (GameObject lineR, Transform startPos, Transform endPos, bool getClosesEnemyToPlayer = false)
    {
        if(shooting && shot && lineR != null)
        {
            lineR.GetComponent<LineRendererController>().SetPosition(startPos, endPos);

            yield return new WaitForSeconds(refreshRate);

            if (getClosesEnemyToPlayer)
            {
                StartCoroutine(UpdateLineRenderer(lineR, startPos, playerEnemyDetector.GetClosestEnemy().transform.transform, true));

                if (currentClosestEnemy != playerEnemyDetector.GetClosestEnemy())
                {
                    StopShooting();
                    //StartShooting();
                }

            }
            else
            {
                StartCoroutine(UpdateLineRenderer(lineR, startPos, endPos));
            }
        }
    }
    void NewLineRenderer(Transform startPos, Transform endPos, bool getClosestEnemyToPlayer = false)
    {
        GameObject lineR = Instantiate(lineRendererPrefab);
        spawnedLineRenderers.Add(lineR);
        StartCoroutine(UpdateLineRenderer(lineR, startPos, endPos, getClosestEnemyToPlayer));
    }

    IEnumerator ChainReaction(GameObject closestEnemey)
    {
        yield return new WaitForSeconds(delayBetweeneachChain);

        if(counter == maximunEnemisInChain)
        {
            yield return null;
        }
        else
        {
            if (shooting)
            {
                counter++;
                enemiesInChain.Add(closestEnemey);

                if(!enemiesInChain.Contains(closestEnemey.GetComponent<EnemyDetector>().GetClosestEnemy()))
                {
                    NewLineRenderer(closestEnemey.transform, closestEnemey.GetComponent<EnemyDetector>().GetClosestEnemy().transform);
                    StartCoroutine(ChainReaction(closestEnemey.GetComponent<EnemyDetector>().GetClosestEnemy()));
                }
            }
        }
    }
    void StopShooting()
    {
        shooting = false;
        shot = false;
        counter = 1;

        for (int i = 0; i < spawnedLineRenderers.Count; i++)
        {
            Destroy(spawnedLineRenderers[i]);
        }

        spawnedLineRenderers.Clear();
        enemiesInChain.Clear();

        for (int i = 0; i < activeEffect.Count; i++)
        {
            Destroy(activeEffect[i]);
        }

        activeEffect.Clear();
    }

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            if(playerEnemyDetector.GetEnemiesInRange().Count > 0)
            {
                if(!shooting)
                {
                    StartShooting();
                }    
            }
            else
            {
                StopShooting();
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            StopShooting();
        }
    }

}
