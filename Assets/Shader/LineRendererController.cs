using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererController : MonoBehaviour
{
    [SerializeField] List<LineRenderer> lineRenderers = new List<LineRenderer>();

    public void SetPosition(Transform startPosl, Transform endPos)
    {
        if(lineRenderers.Count > 0)
        {
            for( int i = 0;   i < lineRenderers.Count; i++)
            {
                if (lineRenderers[i].positionCount >= 2)
                {
                    lineRenderers[i].SetPosition(0, startPosl.position);
                    lineRenderers[i].SetPosition(1, endPos.position);
                }
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
