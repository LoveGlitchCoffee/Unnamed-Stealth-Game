using System.Collections;
using UnityEngine;

public class FindPlayer : MonoBehaviour
{
    
    private float _scanAngle = 0.3f; //euler angles    
    private const float StepLook = 1;
    public bool ResumePatrol;
    
    /*
     * Starts a new visual search for player
     */
    public IEnumerator VisualSearch()
    {        
        StopAllCoroutines();        
        yield return StartCoroutine(VisualScan());        
        enabled = false;
    }

    /**
     * restore sight to 0 on z rotation, looks striaght again
     */
    IEnumerator RestoreVisual()
    {        

        Quaternion normalise = new Quaternion(0,0,0,1);        

        while (gameObject.transform.rotation.z != 0)
        {
            gameObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, normalise, StepLook);
            yield return null;
        }
    }


    /*
     * look up , wait for a couple of seconds then look down to find the player
     * method is just to rotate the detector object on the z axis in euler angles
     * could split for more flexible and smooth visual
     */
    IEnumerator VisualScan()
    {       
        
        const int turns = 4;
        int turnTaken = 0;
        float delayCounter;
        float delayTime = 3f;
        bool lookedUp = false;


        while (turnTaken < turns && !GetComponent<PlayerDetection>().SeenPlayer)
        {            
            delayCounter = 0;

            if (!lookedUp)
            {
                while (gameObject.transform.rotation.z < _scanAngle)
                {
                   RotateView(StepLook);
                   yield return 0;
                }                
            }
            else
            {
                while (gameObject.transform.rotation.z > -_scanAngle)
                {
                   RotateView(-StepLook);
                   yield return 0;
                }                
            }

            lookedUp = !lookedUp;


            while (delayCounter < delayTime)
            {                
                delayCounter += 0.2f;
                yield return 0;
            }
            
            turnTaken++;            
        }
        
        StartCoroutine(RestoreVisual());

        
        if (!GetComponent<PlayerDetection>().SeenPlayer)
        {            
            ResumePatrol = true;
            //Debug.Log("finish looking");
            gameObject.transform.parent.gameObject.layer = 9;
            GetComponent<VisionConeRender>().ActivateState(Color.grey);            
        }            

    }

    /*Roates view in specified step each time
     */
    private void RotateView(float stepLook)
    {
        gameObject.transform.Rotate(new Vector3(0, 0, stepLook));
    }
}
