using System.Collections;
using System.Text;
using UnityEngine;

public class FindPlayer : MonoBehaviour
{
    
    private float _scanAngle = 0.3f; //euler angles    
    private const float StepLook = 1;
    public bool ResumePatrol;
    
    public void VisualSearch()
    {        
        StartCoroutine(VisualScan());        
        enabled = false;
    }

    /**
     * restore sight to 0 on z rotation, looks striaght again
     */
    IEnumerator RestoreVisual()
    {
        Debug.Log("restoring visuals");
        while (gameObject.transform.rotation.z != 0)
        {
            if (gameObject.transform.rotation.z > 0)
                RotateView(-StepLook);
            else
            {
                RotateView(StepLook);
            }
            
            yield return null;
        }

        gameObject.transform.rotation = new Quaternion(); //(0,0,0,0) ?
        
        
    }

    /*
     * look up , wait for a couple of seconds then look down to find the player
     * method is just to rotate the detector object on the z axis in euler angles
     */
    IEnumerator VisualScan()
    {       
        
        const int turns = 2;
        int turnTaken = 0;
        float delayCounter;
        float delayTime = 3f;        
        
        
        while (turnTaken < turns && !GetComponent<PlayerDetection>().SeenPlayer)
        {                       
            delayCounter = 0;

            while (gameObject.transform.rotation.z < _scanAngle)
             {                           
                RotateView(StepLook);
                 yield return 0;
             }

            
            while (delayCounter < delayTime)
            {
                delayCounter += 0.2f;
                yield return 0;
            }

             while (gameObject.transform.rotation.z > -_scanAngle)
             {                 
                 //weird bug here                 
                 RotateView(-StepLook);
                 yield return 0;
             }

            delayCounter = 0f;

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
            Debug.Log("resume patrol " + ResumePatrol);
            gameObject.transform.parent.gameObject.layer = 9;
            GetComponent<VisionConeRender>().ActivateState(Color.grey);
            GetComponentInParent<Spritehandler>().FlipSprite();
            GetComponentInParent<PursuePlayer>().ReverseRoute();
            GetComponentInParent<PursuePlayer>().ReturnToPatrol();
            GetComponentInParent<PursuePlayer>().enabled = false;
        }            

    }


    private void RotateView(float stepLook)
    {
        gameObject.transform.Rotate(new Vector3(0, 0, stepLook));
    }
}
