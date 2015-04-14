using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDetection : MonoBehaviour, IDetection
{

    private PursuePlayer _pursue;
    private GuardAI _regularAi;
    private GameObject _player;
    private GameObject _gameMap;

   
    private const int LayerLiving = 11;
    private const int LayerEnvi = 10;
    private LayerMask _detectLiving;
    private LayerMask _detectEnvi;
    private LayerMask _detectLayerMask;

    private const string PlayerTag = "Player";

    private List<Vector2> _leftVision = new List<Vector2>();
    private List<Vector2> _rightVision = new List<Vector2>();
    private PolygonCollider2D _visionCone;

    private bool _sensePlayer = false;
    private bool _seenPlayer = false;  

	Ray2D _lineOfSight;
    private int _sightDistance;

    private VisionConeRender _coneRender;
    private Color _suspicion = Color.yellow;
    private Color _alarmed = Color.red;
    private Color _blind = Color.grey;


	// Use this for initialization
    /**
     * Ray cast only collides with player or environment that can block vision
     * Vision cone is just reverse of one another
     */
	void Start () {

	    _detectLiving = 1 << LayerLiving;
	    _detectEnvi = 1 << LayerEnvi;

	    _detectLayerMask = _detectLiving | _detectEnvi;

	    _pursue = gameObject.GetComponentInParent<PursuePlayer>();
	    _regularAi = gameObject.GetComponentInParent<GuardAI>();
	    _player = GameObject.FindGameObjectWithTag(PlayerTag);
	    _gameMap = GameObject.FindGameObjectWithTag("Map");

        _sightDistance = 5;

        _leftVision.Add(new Vector2(-0.2f, 0.5f));
        _leftVision.Add(new Vector2(-5f, 2f));
        _leftVision.Add(new Vector2(-5f, -0.8f));
        _leftVision.Add(new Vector2(-0.2f, 0.4f));


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
	
	// Update is called once per frame
	void FixedUpdate ()
	{
	    if (_sensePlayer)
	    {            
	        CheckLineOfSight();
	    }
			
	}

    /**
     * Sets the vision cone to the direction guard walking
     */
    public void SetVisionCone(bool goingLeft)
    {
        _visionCone.points = goingLeft ? _leftVision.ToArray() : _rightVision.ToArray();
        _coneRender.SetConeShape(_visionCone.points);
    }

    /**
     * Calculates the direction to cast ray, should be direction towards player
     */
    private Vector2 CalculateDirection()
    {
        return new Vector2(_player.transform.position.x - gameObject.transform.position.x, _player.transform.position.y - gameObject.transform.position.y);
    }

    /**
     * Ray cast towards player if within vision cone
     * If ray cast hits player, i.e. no obstacles between them, then enable guard's pursue behaviour, turning off patrol behaviour
     */
    public void CheckLineOfSight()
	{
        _lineOfSight = new Ray2D(new Vector2(gameObject.transform.position.x + 0.9f, gameObject.transform.position.y + 0.1f), CalculateDirection());
        RaycastHit2D detectPlayer = Physics2D.Raycast(_lineOfSight.origin, _lineOfSight.direction, _sightDistance, _detectLayerMask); // distance is x distance                       
        Debug.DrawLine(_lineOfSight.origin, detectPlayer.point);

        if (detectPlayer.collider != null && detectPlayer.collider.tag == PlayerTag)
        {
            _regularAi.enabled = false;
            _sensePlayer = false;
            _seenPlayer = true;
            StartCoroutine(ReactToDetection(detectPlayer));
        }
	}

    IEnumerator ReactToDetection(RaycastHit2D playerLastSeen)
    {
        float baffledTime = 0f;
        _coneRender.ActivateState(_suspicion);
        RaycastHit2D detectPlayer = new RaycastHit2D();
        GraphOfMap graph = _gameMap.GetComponent<GenerateNodes>().ReturnGeneratedGraph();

        while (baffledTime < 0.5f)
        {
            _lineOfSight = new Ray2D(new Vector2(gameObject.transform.position.x + 0.9f, gameObject.transform.position.y + 0.1f), CalculateDirection());
            detectPlayer = Physics2D.Raycast(_lineOfSight.origin, _lineOfSight.direction, _sightDistance, _detectLayerMask);
            Debug.DrawLine(_lineOfSight.origin, detectPlayer.point);

            baffledTime += 1f*Time.deltaTime;            
            yield return null;
        }

        if (detectPlayer.collider != null && detectPlayer.collider.tag == PlayerTag)
        {            
            _coneRender.ActivateState(_alarmed);
            _pursue.SetSpeed(4f);
            _pursue.SetGoal(graph.nodeWith(_player.GetComponent<PlayerMapRelation>().ReturnNodePlayerAt()));
        }
        else
        {
            
            _pursue.SetSpeed(1f);
            _pursue.SetGoal(CalculateNodeLastSeen(playerLastSeen, graph));
        }

        Transform parentTransfrom = gameObject.transform.parent;
        GameObject guard = parentTransfrom.gameObject;
        guard.layer = 11;
        

        _pursue.enabled = true;
    }

    private Node CalculateNodeLastSeen(RaycastHit2D playerLastSeen, GraphOfMap graph)
    {
        Vector2 pointLastSeen = playerLastSeen.point;
        

        for (int i = 0; i < _gameMap.transform.childCount; i++)
        {
            Collider2D nodeCollider = _gameMap.transform.GetChild(i).GetComponent<CircleCollider2D>();

            if (nodeCollider.OverlapPoint(pointLastSeen))                
                return graph.nodeWith(nodeCollider.gameObject.GetComponent<Node>());
        }

        return null; // shouldn't reach here
    }
    

    /**
     * If vision cone collides with player, cast ray cast
     */
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == PlayerTag && !_seenPlayer)
        {                       
            _sensePlayer = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == PlayerTag && !_seenPlayer)
        {            
            _sensePlayer = false;
        }
    }
    
}
