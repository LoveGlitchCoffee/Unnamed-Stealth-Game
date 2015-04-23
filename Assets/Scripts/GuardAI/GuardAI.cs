using System.Collections;
using UnityEngine;

public class GuardAI : MonoBehaviour {

    [HideInInspector] public bool GoingLeft;
    [HideInInspector] public bool OutOfPatrolArea;

    private PlayerDetection _playerDetection;
    private PursuePlayer _pursuePlayer;

    private const string PatrolAreaTag = "PatrolRegion";
    private const string PlayerTag = "Player";

    private const int WalkSpeed = 1;
    private float _waitTime = 0;
    private const float MaxWaitTime = 2f;

    
    private Node _nodeGuardAt;

    /**
     * Guard Patrol pattern
     * Always in patrol region going left     
     */
    private void Start()
    {        
        GoingLeft = false;
        OutOfPatrolArea = false;

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
        
        if (!OutOfPatrolArea)
        {
            gameObject.transform.Translate(GoingLeft ? new Vector2(-WalkSpeed * Time.deltaTime, 0) : new Vector2(WalkSpeed * Time.deltaTime, 0));
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

        if (_waitTime >= MaxWaitTime)
        {
            GoingLeft = !GoingLeft;
            GetComponent<Spritehandler>().FlipSprite();
            _waitTime = 0f;
            OutOfPatrolArea = false;
            _playerDetection.SetVisionCone(GoingLeft);
        }
    }


    /**
     * Wait 5 seconds
     */
    private void Wait()
    {
        if (_waitTime < MaxWaitTime)
        {
            _waitTime += 1f * Time.deltaTime;
        }
    }
   
    /**
     *If out of patrol, turn on flag
     */
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == PatrolAreaTag)
        {
            OutOfPatrolArea = true;
        }

    }
    
    /**
     * tracks which map node guard is currently at
     */
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

    /**
     * returns node on map guard is at at time of method call
     */
    public Node ReturnNodeGuardAt()
    {        
        return _nodeGuardAt;
    }
   

}
