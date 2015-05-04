using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
   
    private GameObject _gameMap;
    private FindPlayer _postSearch;
    private PlayerDetection _detector;
    private GameObject _player;
    private Patrol _patrolBehav;

    private float _speed = 4f;

    private Node[] _routeToGoal;
    private Node _goal;
    private bool _travelling;    

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
     * Calculates route from guard current position to where its suppose to be, returning the route as an array
     */
    private Node[] CalculateRouteToDestination()
    {
     
        BFS newSearch = new BFS();
        newSearch._frontier = new List<Node>();
        newSearch._possiblePath = new Dictionary<Node, Node>();
        newSearch._visited = new HashSet<Node>();

        Node start = gameObject.GetComponent<Patrol>().ReturnNodeGuardAt();        
        //Debug.Log("guard at " + start.GetX() + ", " + start.GetY());
              
        Debug.Log("player last seen at " + _goal.GetX() + ", " + _goal.GetY());

        return newSearch.FindRouteFrom(start, _goal);
    }


    /**
     * Go to each position in the route
     * if pursuing the player, guard then will use FindPlayer component to search for player
     * otherwise continue as it was on patrol
     */
    IEnumerator NavigateToGoal(bool travelling)
    {
        /*for (int i = 0; i < _routeToGoal.Length; i++)
        {
            Node node = _routeToGoal[i];
            Debug.Log(node.GetX() + ", " + node.GetY());
        }*/

        do
        {
            for (int i = 0; i < _routeToGoal.Length; i++)
            {                              
                yield return StartCoroutine(MoveToNextPosition(_routeToGoal[i]));                
            }

            travelling = false;

        } while (travelling);

    }

    /*
     * behaviour if does not find player
     * Guard finished the previously patrolled route 
     * search for route to the end point and patrols
     */
    public IEnumerator FinishPatrol()
    {        
        SetGoal(_patrolBehav.ReturnNodeInRouteAt(_patrolBehav.ReturnPatrolRouteLength() - 1));        
        _routeToGoal = CalculateRouteToDestination();        
        yield return StartCoroutine(NavigateToGoal(true));        
    }


    /*
     *moves guard to the next step in the route returned from search
     */     
    public IEnumerator MoveToNextPosition(Node nextPosition)
    {        
        while (!(transform.position.x == nextPosition.GetX()))
        {
           // Debug.Log("next position's x is: " + nextPosition.GetX());

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
        float timeToJump = 1.15f;
        float timeStamp = Time.time;
        

        while (Time.time - timeStamp < timeToJump)
        {
            //Debug.Log("time passed " + (Time.time - timeStamp));

            transform.position = Vector2.MoveTowards(transform.position, platformPosition, ((Time.time - timeStamp)/(timeToJump))/1.25f);
            
            /*Debug.Log("old y position " + transform.position.y);
            Debug.Log("add on " + (bendPosition.y * Mathf.Sin(Mathf.Clamp01((Time.time - timeStamp)/timeToJump)*Mathf.PI)));*/
            
            float newY = transform.position.y + bendPosition.y * Mathf.Sin(Mathf.Clamp01((Time.time - timeStamp)/timeToJump)*Mathf.PI);            
            float newX = transform.position.x + bendPosition.x * Mathf.Sin(Mathf.Clamp01((Time.time - timeStamp) / timeToJump) * Mathf.PI);
            
            //Debug.Log("new y position " + newY);

            if (transform.position.y != platformPosition.y)
                transform.position = new Vector2(newX, newY * 0.75f);

                           
            yield return 0;
        }

        //Debug.Log("finish jump");
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
     * Stops all current actions, patrol etc
     * Starts a new search, and navigation action, from current position to player's last seen position
     * 
     */
    public IEnumerator StartPursuit()
    {
        
        StopAllCoroutines();
        _routeToGoal = CalculateRouteToDestination();       
        _travelling = true;        
        yield return StartCoroutine(NavigateToGoal(_travelling));
                
        yield return StartCoroutine(PostPursuit());
    }

    /*
     * For certain conditions after finishing a pursuit performs the according action
     * If player is dead or not seen after visual search, return to patrol
     */
    private IEnumerator PostPursuit()
    {
        if (_player.GetComponent<PlayerNPCRelation>().dead)
        {
            gameObject.layer = 9;
            transform.GetChild(0).GetComponent<VisionConeRender>().ActivateState(Color.grey);
            _postSearch.ResumePatrol = true;
        }
        else
        {
            _detector.SeenPlayer = false;
            _postSearch.enabled = true;
            yield return StartCoroutine(_postSearch.VisualSearch());
        }

        if (_postSearch.ResumePatrol)
        {
            Debug.Log("returning to patrol");
            SetSpeed(1.5f);
            yield return StartCoroutine(FinishPatrol());
            yield return StartCoroutine(_patrolBehav.Wait());

            ResumePatrolStabaliser();

            _patrolBehav.enabled = true;
            StartCoroutine(_patrolBehav.Patrolling());
        }
    }

    /*
     * Behaviours required so that returning to patrol looks normal
     * Without this, guard could do moon-walk
     */
    public void ResumePatrolStabaliser()
    {
        _patrolBehav.TurnAround();
        _patrolBehav.ReversePatrolRoute();
    }

    /*
     * Guard will navigate through the passed route.
     */
    public IEnumerator PatrolOnRoute(Node[] route)
    {
        _routeToGoal = route;        
        _travelling = true;
        SetSpeed(1.5f);

        yield return StartCoroutine(NavigateToGoal(_travelling));        
    }

    /*
     * reverse the route that was taken in previous path finding
     */
    public Node[] ReverseRoute(Node[] routeToReverse)
    {        
        Node[] newRoute = new Node[routeToReverse.Length];
        int counter = 0;

        //Debug.Log("new route length will be " + newRoute.Length);

        for (int i = routeToReverse.Length - 1; i > -1; i--)
        {
            newRoute[counter] = routeToReverse[i];
          //  Debug.Log("new route " + newRoute[counter].GetX() +", " + newRoute[counter].GetY());
            counter++;
        }
        
        return newRoute;
    }


}
