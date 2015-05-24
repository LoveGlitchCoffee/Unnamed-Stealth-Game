using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{

    private Pathfinding _pathFinder;
    private Patrol _patrolBehav;
    private GuardSoundHandler _soundHandler;
    
    private GameObject _player;
    private GameObject _gameMap;
   

    private const string PlayerTag = "Player";    

    private List<Vector2> _leftVision = new List<Vector2>();
    private List<Vector2> _rightVision = new List<Vector2>();
    private PolygonCollider2D _visionCone;

    private bool _sensePlayer = false;
    [HideInInspector] public bool SeenPlayer = false;  
	    

    private VisionConeRender _coneRender;
    private Color _suspicion = Color.yellow;
    private Color _alarmed = Color.red;
    private Color _blind = Color.grey;

    private RaycastHit2D detectPlayer;
    private DetectionCommon _detection;

    /*
     * Set up script dependencies
     */
    void Awake()
    {
        _pathFinder = gameObject.GetComponentInParent<Pathfinding>();
        _patrolBehav = gameObject.GetComponentInParent<Patrol>();
        _player = GameObject.FindGameObjectWithTag(PlayerTag);
        _gameMap = GameObject.FindGameObjectWithTag("Map");
        _soundHandler = GetComponentInParent<GuardSoundHandler>();
        _detection = GetComponent<DetectionCommon>();
    }

    /**
     * Ray cast only collides with player or environment that can block vision
     * Ray cast distance will be 5 units
     * Vision cone is just reverse of one another
     */
	void Start () {
	    
	    SetUpVisionCone();        
	}

    /**
     * Creates 4 points for vision cone shape, left and right are just reverse of each other
     * Get the appropriate compoenent and assign them starting values
     */
    private void SetUpVisionCone()
    {
        _leftVision.Add(new Vector2(-0.2f, 0.5f));
        _leftVision.Add(new Vector2(-5f, 1.6f));
        _leftVision.Add(new Vector2(-5f, -0.8f));
        _leftVision.Add(new Vector2(-0.2f, _detection.GetEyeDistance()));


        for (int i = 0; i < 4; i++)
        {
            var rightEye = Vector2.Scale(_leftVision.ElementAt(i), new Vector2(-1f, 1f));
            _rightVision.Add(rightEye);
        }

        _visionCone = GetComponent<PolygonCollider2D>();
	    _visionCone.points = _rightVision.ToArray();

	    _coneRender = GetComponent<VisionConeRender>();        
        _coneRender.SetConeShape(_visionCone.points);
        _coneRender.ActivateState(_blind);
    }
	
	/*
     * Checks line of sight if player is in vision cone but not detected yet
     */
	void FixedUpdate ()
	{
	    if (_sensePlayer && !SeenPlayer)
	    {
	        CheckSightForPlayer(_player);
	    }
			
	}


    /**
     * Sets the vision cone to the direction guard walking
     */
    public void SetVisionCone(bool goingLeft)
    {            
        _coneRender.SetConeShape(_visionCone.points);
    }

    

    /**
     * Ray cast towards player if within vision cone
     * If ray cast hits player, i.e. no obstacles between them, then enable guard's pursue behaviour, turning off patrol behaviour     
     */
    public void CheckSightForPlayer(GameObject player)
    {
        detectPlayer = _detection.CheckIfHit(player);

        if (detectPlayer.collider != null && detectPlayer.collider.tag == PlayerTag)
        {
            _patrolBehav.enabled = false;
            _sensePlayer = false;
            SeenPlayer = true;            
            StartCoroutine(ReactToDetection(detectPlayer));
        }
	}

    /*
     * reaciton is to be astonished for 0.5 seconds before assuring that player is defintely within line of sight or not
     * if player is defenitely seen, pursue player, else go to where player was last seen, cautiously.
     */
    IEnumerator ReactToDetection(RaycastHit2D playerLastSeen)
    {
        
        _coneRender.ActivateState(_suspicion);
        detectPlayer = new RaycastHit2D();
        GraphOfMap graph = _gameMap.GetComponent<NodeGenerator>().ReturnGeneratedGraph();

        yield return StartCoroutine(Baffled());        

        SetNavigationState(playerLastSeen, detectPlayer, graph);        
                
        StartCoroutine(_pathFinder.StartPursuit());
    }

    /*
     * Simulate being baffled
     * guard stops and enter suspicion state for 0.5 seconds
     * If guard still see player after baffled, enter alarmed pursuit     
     */
    private IEnumerator Baffled()
    {
        float baffledTime = 0f;        

        while (baffledTime < 0.5f)
        {
            detectPlayer = _detection.CheckIfHit(_player);

            baffledTime += 1f*Time.deltaTime;
            yield return null;
        }
    }
    
    /*
         * Depending on whether guard still detects player or not after baffling
         * Guard will set either suspicion or alarmed state for the pursuit (travel to last seen position)
         */
    private void SetNavigationState(RaycastHit2D playerLastSeen, RaycastHit2D detectPlayer, GraphOfMap graph)
    {
        Node nodeLastSeen = CalculateNodeLastSeen(playerLastSeen, graph);

        if (detectPlayer.collider != null && detectPlayer.collider.tag == PlayerTag)
        {
            //Debug.Log("pursue");
            _coneRender.ActivateState(_alarmed);
            _soundHandler.PlaySound("Alarmed", 0.75f);
            _pathFinder.SetSpeed(4f);
            _pathFinder.SetGoal(nodeLastSeen); // last seen when detect, not after baffled
        }
        else
        {
            _pathFinder.SetSpeed(1f);
            _pathFinder.SetGoal(nodeLastSeen);
            SeenPlayer = false; //so still chase
        }

        if (nodeLastSeen.GetY() > _patrolBehav.ReturnNodeGuardAt().GetY())
            transform.parent.gameObject.layer = 11;
    }

   

    /*
     * Calculates the node player last seen (checks which collider overlaps point of last seen
     * if last seen is outside collider, calculate closest point
     */
    private Node CalculateNodeLastSeen(RaycastHit2D playerLastSeen, GraphOfMap graph)
    {
        Vector2 pointLastSeen = playerLastSeen.point;
        Node alternateNode = null;
        //Debug.Log("point last seen " + pointLastSeen);

        alternateNode = CheckIfOverlapPoint(pointLastSeen, alternateNode, graph);        

        if (alternateNode == null)
        {
            //Debug.Log("point last seen " + pointLastSeen.x + ", " + pointLastSeen.y);            

            if (_patrolBehav.GoingLeft)
            {                
                for (int i = 0; i < _gameMap.transform.childCount; i++)
                {
                    CircleCollider2D nodeCollider = _gameMap.transform.GetChild(i).GetComponent<CircleCollider2D>();

                    if (nodeCollider != null)
                    {
                        Vector2 nodePosition = nodeCollider.transform.position;                        

                        if (!(nodePosition.x > pointLastSeen.x) && !(nodePosition.y > pointLastSeen.y))
                            alternateNode = nodeCollider.gameObject.GetComponent<Node>();
                    }                    
                }
            }
            else if (!_patrolBehav.GoingLeft)
            {                
                
                for (int i = _gameMap.transform.childCount - 1; i > -1; i--)
                {
                    CircleCollider2D nodeCollider = _gameMap.transform.GetChild(i).GetComponent<CircleCollider2D>();

                    if (nodeCollider != null)
                    {
                        Vector2 nodePosition = nodeCollider.transform.position;                        
                        //bug here
                        if (!(nodePosition.x < pointLastSeen.x) && !(nodePosition.y > pointLastSeen.y))
                            alternateNode = nodeCollider.gameObject.GetComponent<Node>();
                    }
                }
            
            }    
        }
        
        return alternateNode;
    }

    private Node CheckIfOverlapPoint(Vector2 pointLastSeen, Node alternate, GraphOfMap graph)
    {
            for (int i = 0; i < _gameMap.transform.childCount; i++)
            {
                Collider2D nodeCollider = _gameMap.transform.GetChild(i).GetComponent<CircleCollider2D>();

                if (nodeCollider != null)
                {
                    if (nodeCollider.OverlapPoint(pointLastSeen))
                        return graph.nodeWith(nodeCollider.gameObject.GetComponent<Node>());
                }
            }

        return alternate;
    }


    /**
     * If vision cone collides with player, cast ray cast
     */
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == PlayerTag && !SeenPlayer)
        {                                
            _sensePlayer = true;            
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == PlayerTag && !SeenPlayer)
        {            
            _sensePlayer = false;
        }
    }
    
}
