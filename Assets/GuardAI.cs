using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GuardAI : MonoBehaviour {

    private bool _goingLeft;
    private bool _outOfPatrolArea;

    private PlayerDetection _playerDetection;
    private PursuePlayer _pursuePlayer;

    private const string PatrolAreaTag = "PatrolRegion";
    private const string PlayerTag = "Player";

    private const int WalkSpeed = 1;
    private float _waitTime = 0;

    
    private Node _nodeGuardAt;

    /**
     * Guard Patrol pattern
     * Always in patrol region going left
     * Also defines vectors for vision cone
     */

    private void Start()
    {
        _goingLeft = true;
        _outOfPatrolArea = false;


        _playerDetection = gameObject.GetComponentInChildren<PlayerDetection>();
        
        _pursuePlayer = gameObject.GetComponent<PursuePlayer>();
        _pursuePlayer.enabled = false;

    }

    // Update is called once per frame
    /**
     * Whilst in Patrol region, walks left and right
     * once out of patrol region, turns around and continues patrol
     */
	void Update () {

        if (!_outOfPatrolArea)
        {
            gameObject.transform.Translate(_goingLeft ? new Vector2(-WalkSpeed * Time.deltaTime, 0) : new Vector2(WalkSpeed * Time.deltaTime, 0));
        }
        else
        {
            TurnAround();
        }
	}

    /**
     * Waits for 5 seconds and turns around according to previous walking direction, then continue patrol
     * changes vision cone direction as well
     */
    private void TurnAround()
    {
        Wait();

        if (_waitTime >= 3)
        {
            _goingLeft = !_goingLeft;
            _waitTime = 0f;
            _outOfPatrolArea = false;
            _playerDetection.SetVisionCone(_goingLeft);
        }
    }


    /**
     * Wait 5 seconds
     */
    private void Wait()
    {
        if (_waitTime < 3f)
        {
            _waitTime += 1f * Time.deltaTime;
        }
    }

    


    /**
     * If vision cone no longer detects player, turns of player Detection ray cast, should turn on prob calc
     */
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == PatrolAreaTag)
        {
            _outOfPatrolArea = true;
        }

    }
    

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.layer == 12)
        {
            _nodeGuardAt =
                GameObject.FindGameObjectWithTag("Map")
                    .GetComponent<GenerateNodes>()
                    .ReturnGeneratedGraph()
                    .nodeWith(col.GetComponent<Node>());

            //Debug.Log(_nodeGuardAt.GetX() + ", " + _nodeGuardAt.GetY());
        }
    }

    public Node ReturnNodeGuardAt()
    {
        return _nodeGuardAt;
    }

}
