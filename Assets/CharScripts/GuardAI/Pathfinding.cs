using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
   
    private GameObject _gameMap;
    private FindPlayer _postSearch;
    private PlayerDetection _detector;
    private GameObject _player;
    private Patrol _patrolBehav;

    private float _speed = 4f;

    private Node[] _routeToPlayer;
    private Node _goal;
    private bool _travelling;
    private Node _nodeStartPursuit;

    /*
     * calculates the route to a destination, using the map
     */
	void Start ()
	{	    
	    _gameMap = GameObject.FindGameObjectWithTag("Map");
	    _detector = transform.GetChild(0).GetComponent<PlayerDetection>();
	    _postSearch = GetComponentInChildren<FindPlayer>();
	    _player = GameObject.FindGameObjectWithTag("Player");
	    _patrolBehav = GetComponent<Patrol>();
	}
    

    /**
     * Calculates route from guard current position to where it suppose to be, returning the route as an array
     */
    private Node[] CalculateRouteToDestination()
    {
     
        BFS newSearch = new BFS();
        newSearch._frontier = new List<Node>();
        newSearch._possiblePath = new Dictionary<Node, Node>();
        newSearch._visited = new HashSet<Node>();

        Node start = gameObject.GetComponent<Patrol>().ReturnNodeGuardAt();
        _nodeStartPursuit = start;
        //Debug.Log("guard at " + start.GetX() + ", " + start.GetY());
              
        //Debug.Log("player last seen at " + _goal.GetX() + ", " + _goal.GetY());


        return newSearch.FindRouteFrom(start, _goal);
    }


    /**
     * Go to each position in the route
     * if pursuing player then will use FindPlayer component to search for player, otherwise just continue as was before
     */
    IEnumerator NavigateToGoal(bool searching, bool pursue)
    {        
        do
        {
            for (int i = 0; i < _routeToPlayer.Length; i++)
            {                              
                yield return StartCoroutine(MoveToNextPosition(_routeToPlayer[i]));                
            }

            searching = false;

        } while (searching);
        

        if (pursue)
        {
            if (_player.GetComponent<PlayerNPCRelation>().dead)
            {
                //enabled = false;
                gameObject.layer = 9;
                transform.GetChild(0).GetComponent<VisionConeRender>().ActivateState(Color.grey);
                _postSearch.ResumePatrol = true;
            }
            else
            {            
                _detector.SeenPlayer = false;
                _postSearch.enabled = true;
                _postSearch.VisualSearch();
            }
        }

        if (_postSearch.ResumePatrol)
            {
                if (!(_player.GetComponent<PlayerNPCRelation>().dead))
                    GetComponent<Spritehandler>().FlipSprite();  
          
                GetComponent<Patrol>().enabled = true;
                _postSearch.ResumePatrol = false;
            }
        
        
    }
    
    /*
     *moves guard to the next step in the route returned from search
     */     
    IEnumerator MoveToNextPosition(Node nextPosition)
    {        
        while (!(transform.position.x == nextPosition.GetX()))
        {
            //Debug.Log("next position's x is: " + nextPosition.GetX());

            if (nextPosition.GetY() > transform.position.y)
            {                                 
                yield return StartCoroutine(JumpToPlatform(transform.position, nextPosition.gameObject.transform.position));                         
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, nextPosition.gameObject.transform.position, _speed * Time.deltaTime);
                yield return 0;
            }
            
        }        
    }

    /*
     * Moves guard to platform by lerping but y position + sin(y) so larger proportion to x and creates curve
     */
    IEnumerator JumpToPlatform(Vector2 startPosition, Vector2 platformPosition)
    {
        Vector2 bendPosition = Vector2.up;
        float timeToJump = 1.5f;
        float timeStamp = Time.time;
        

        while (Time.time - timeStamp < timeToJump)
        {            
            transform.position = Vector2.MoveTowards(transform.position, platformPosition, (Time.time - timeStamp)/(timeToJump));
            
            //bug is when make jump, y suddenly decrease

            //Debug.Log("fraction of tiem jumped " + Mathf.Clamp01(Time.time - timeStamp) / timeToJump);
            //Debug.Log("sin of angle " + (transform.position.y + bendPosition.y * Mathf.Sin(Mathf.Clamp01((Time.time - timeStamp)/timeToJump)*Mathf.PI)));
            float newY = transform.position.y + bendPosition.y * Mathf.Sin(Mathf.Clamp01((Time.time - timeStamp)/timeToJump)*Mathf.PI);            
            float newX = transform.position.x + bendPosition.x * Mathf.Sin(Mathf.Clamp01((Time.time - timeStamp) / timeToJump) * Mathf.PI);
            
            //Debug.Log("new y position " + newY);

            if (transform.position.y != platformPosition.y)
                transform.position = new Vector2(newX, newY * 0.8f);           
            
            yield return 0;
        }
    }

    /*
     * set speed to which guard moves while finding path
     */
    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    /*
     * set goal of current path finding
     */
    public void SetGoal(Node goal)
    {
        _goal = goal;
    }

    /*
     * Starts a new search and navigation from current position to player's last seen position
     */
    public void StartPursuit()
    {
        _routeToPlayer = CalculateRouteToDestination();       
        _travelling = true;
        //Debug.Log(_routeToPlayer.Length);
        StartCoroutine(NavigateToGoal(_travelling, true));            
    }

    /*
     * reverses the route of pursuit and returns guard to where they were before pursuit
     */
    public void ReturnToPatrol()
    {
        _routeToPlayer =  ReverseRoute(_routeToPlayer);
        _travelling = true;
        SetSpeed(1f);
        StartCoroutine(NavigateToGoal(_travelling, false));        
    }


    public IEnumerator PatrolOnRoute(Node[] route)
    {
        _routeToPlayer = route;        
        _travelling = true;
        SetSpeed(2f);

        yield return StartCoroutine(NavigateToGoal(_travelling, false));        
    }

    /*
     * reverse the route that was taken in previous path finding, used only for return to patrol
     */
    public Node[] ReverseRoute(Node[] routeToReverse)
    {        
        Node[] newRoute = new Node[routeToReverse.Length];
        int counter = 0;

        Debug.Log("new route length will be " + newRoute.Length);

        for (int i = _routeToPlayer.Length - 1; i > -1; i--)
        {
            newRoute[counter] = _routeToPlayer[i];
            Debug.Log(newRoute[counter].GetX() +", " + newRoute[counter].GetY());
            counter++;
        }
        
        return newRoute;
    }

}
