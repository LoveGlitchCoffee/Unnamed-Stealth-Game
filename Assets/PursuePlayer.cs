using System.Collections.Generic;
using UnityEngine;

public class PursuePlayer : MonoBehaviour
{

    private GameObject _player;
    private GameObject _gameMap;
    

    private bool _endOfRoute = false;
    private bool _routeCalculated = false;

    private Node[] _routeToPlayer;
    private int _currentNode;

    private const float SpeedScale = 2.5f;

    private Node _nodeGuardAt;

    // Use this for initialization
	void Start ()
	{
	    this.enabled = false;
	    _player = GameObject.FindGameObjectWithTag("Player");
	    _gameMap = GameObject.FindGameObjectWithTag("Map");
	}
	
    /**
     * when script is enabled, calculates a route from currrent node guard is at to ndoe where player is at, once, and make the movements there
     */
	// Update is called once per frame
	void FixedUpdate ()
	{        
	    if (!_routeCalculated)
	    {
            _routeToPlayer = CalculateRouteToPlayer(); 
	        _routeCalculated = true;
            Debug.Log(_routeToPlayer.Length);
	        _currentNode = 0;
       	 }

	    if (!_endOfRoute && _routeCalculated)
	    {            
            NavigateToPlayer(_currentNode); 
	    }
        else
         {
              this.enabled = false;
              gameObject.GetComponent<GuardAI>().enabled = true;
         }
    }

    /**
     * Calculates route from guard to player, returning the route as an array
     */
    private Node[] CalculateRouteToPlayer()
    {
     
        BFS newSearch = new BFS();
        newSearch._frontier = new List<Node>();
        newSearch._possiblePath = new Dictionary<Node, Node>();
        newSearch._visited = new HashSet<Node>();

        Node start = _nodeGuardAt;        
        Debug.Log("guard at " + _nodeGuardAt.GetX() + ", " + _nodeGuardAt.GetY());
        Node goal = _player.GetComponent<PlayerMapRelation>().ReturnNodePlayerAt();
        Debug.Log("palyer at " + goal.GetX() + ", " + goal.GetY());


        return newSearch.FindRouteFrom(start, goal);
    }

    /**
     * moves guard to the next step in the route returned from search
     */
    private void NavigateToPlayer(int currentNode)
    {
        Node nextPosition = _routeToPlayer[currentNode];
        Debug.Log(" guard next pos " + nextPosition.GetX() + ", " + nextPosition.GetY());

        float guardX = gameObject.transform.position.x;
        float guardY = gameObject.transform.position.y;
        float playerX = _player.transform.position.x;
        float playerY = _player.transform.position.y;

        gameObject.transform.Translate((playerX - guardX) * Time.deltaTime, (playerY - guardY) * Time.deltaTime, gameObject.transform.position.z);

        //if (guardX == nextPosition.GetX() && guardY == nextPosition.GetY())
        //{
            currentNode++;
            Debug.Log("made move");
            if (currentNode == _routeToPlayer.Length - 1)
            {
              //  Debug.Log(" at last node");
                _endOfRoute = true;
            }
       // }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 12)
        {
            _nodeGuardAt = _gameMap.GetComponent<GenerateNodes>().ReturnGeneratedGraph().nodeWith(col.gameObject.GetComponent<Node>());             
        }
    }

    public Node ReturnNodeAt()
    {
        return _nodeGuardAt;
    }



}
