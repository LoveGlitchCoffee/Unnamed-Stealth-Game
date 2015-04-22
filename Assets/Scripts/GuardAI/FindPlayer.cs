using System.Collections;
using UnityEngine;

public class FindPlayer : MonoBehaviour
{
    
    private float _scanAngle = 0.5f; //  euler angles
    
        
	// Use this for initialization
	void Start () {
	    
	}
		

    public void VisualSearch()
    {
        
        StartCoroutine(VisualScan());
        
        //reset visual rotation
        enabled = false;
    }

    IEnumerator VisualScan()
    {
        Debug.Log(_scanAngle);
        const float stepLook = 1;
        const int turns = 3;
        int turnTaken = 0;

        while (turnTaken < turns)
        {
             while (gameObject.transform.rotation.z < _scanAngle)
             {
                 Debug.Log(gameObject.transform.rotation.z);
                gameObject.transform.Rotate(new Vector3(0, 0, stepLook));
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
