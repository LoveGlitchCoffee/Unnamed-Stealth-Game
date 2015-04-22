using System.Collections;
using UnityEngine;

public class FindPlayer : MonoBehaviour
{
    
    private float _scanAngle = 0.3f; //  euler angles
    
        
	// Use this for initialization
	void Start () {
	    
	}
		

    public void VisualSearch()
    {
        // pass in to say found or not
        StartCoroutine(VisualScan());

        //StartCoroutine(RestoreVisual());
        enabled = false;
    }

    //IEnumerator RestoreVisual()
    //{
     //   while ()
    //}

    IEnumerator VisualScan()
    {       
        const float stepLook = 1;
        const int turns = 3;
        int turnTaken = 0;
        float delayCounter;
        float delayTime = 3f;

        while (turnTaken < turns)
        {
            delayCounter = 0;

             while (gameObject.transform.rotation.z < _scanAngle)
             {                 
                gameObject.transform.Rotate(new Vector3(0, 0, stepLook));
                yield return null;
             }

            while (delayCounter < delayTime)
            {
                delayCounter += 0.4f;                
                yield return null;
            }
             turnTaken++;// won't terminate

             while (gameObject.transform.rotation.z > -_scanAngle)
            {
                gameObject.transform.Rotate(new Vector3(0, 0, -stepLook));
                yield return null;
            }

            turnTaken++;
        }

    
    }

    
}
