using UnityEngine;
using System.Collections;

public class GuardAI : MonoBehaviour {

    bool pursue;
    bool goingLeft;
    bool outOfPatrolArea;
    float waitTime = 0;

    private PlayerDetection playerDetection;


	
    private const string PatrolAreaTag = "PatrolRegion";

    private const int WalkSpeed = 3;    

	// Use this for initialization
	void Start () {
        pursue = false;
        goingLeft = true;
        outOfPatrolArea = false;
        playerDetection = gameObject.GetComponent<PlayerDetection>();
	}
	
	// Update is called once per frame
	void Update () {

        if (!outOfPatrolArea)
        {
            if (goingLeft)
            {
                gameObject.rigidbody2D.AddForce(new Vector2(-WalkSpeed, 0));
            }
            else
            {
                gameObject.rigidbody2D.AddForce(new Vector2(WalkSpeed, 0));
            }
        }
        else
        {

            Wait();

            if (goingLeft && waitTime >= 5)
            {
                waitTime = 0f;
                goingLeft = false;
                playerDetection.SetTurnAround(1);
                outOfPatrolArea = false;
            }
            else if (!(goingLeft) && waitTime >= 5)
            {
                waitTime = 0f;
                goingLeft = true;
                playerDetection.SetTurnAround(-1);
                outOfPatrolArea = false;
            }
        }
	}

    private void Wait()
    {
        if (waitTime < 5f)
        {
            waitTime += 1f * Time.deltaTime;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == PatrolAreaTag)
        {
            outOfPatrolArea = true;
        }
    }

   

}
