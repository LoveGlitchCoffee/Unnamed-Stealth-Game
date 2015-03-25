using UnityEngine;
using System.Collections;

public class GuardAI : MonoBehaviour {

    bool pursue;
    bool goingLeft;
    bool outOfPatrolArea;
    float waitTime = 0;
    float turnedAround;

    private const string PatrolAreaTag = "PatrolRegion";

    private const int WalkSpeed = 3;

    Ray2D lineOfSight;
    

	// Use this for initialization
	void Start () {
        pursue = false;
        goingLeft = true;
        outOfPatrolArea = false;
        turnedAround = -1;
	}
	
	// Update is called once per frame
	void Update () {

        CheckLineOfSight();

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
                turnedAround = 1;
                outOfPatrolArea = false;
            }
            else if (!(goingLeft) && waitTime >= 5)
            {
                waitTime = 0f;
                goingLeft = true;
                turnedAround = -1;
                outOfPatrolArea = false;
            }
        }
	}

    private void Wait()
    {
        if (waitTime < 5f)
        {
            Debug.Log(waitTime);
            waitTime += 1f * Time.deltaTime;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == PatrolAreaTag)
        {
            Debug.Log("exit");
            outOfPatrolArea = true;
        }
    }

    void CheckLineOfSight()
    {
        lineOfSight = new Ray2D(gameObject.transform.position, Vector2.right * turnedAround);
        RaycastHit2D detectPlayer = Physics2D.Raycast(lineOfSight.origin, lineOfSight.direction, 35f);
        Debug.DrawRay(lineOfSight.origin, lineOfSight.direction);
    }

}
