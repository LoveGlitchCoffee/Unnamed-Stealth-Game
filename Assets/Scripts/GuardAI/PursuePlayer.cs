using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursuePlayer : MonoBehaviour
{
   
    private GameObject _gameMap;
    private FindPlayer _postSearch;
    private PlayerDetection _detector;
    private GameObject _player;

    private float _speed = 4f;

    private Node[] _routeToPlayer;
    private Node _goal;
    private bool _searching; 

    /*
     * calculates the route to the player, using the map
     * then goes to the player's position WHEN PLAYER LAST SEEN
     */
	void Start ()
	{	    
	    _gameMap = GameObject.FindGameObjectWithTag("Map");
	    _detector = transform.GetChild(0).GetComponent<PlayerDetection>();
	    _postSearch = GetComponentInChildren<FindPlayer>();
	    _player = GameObject.FindGameObjectWithTag("Player");
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

        Node start = gameObject.GetComponent<GuardAI>().ReturnNodeGuardAt();
        Debug.Log("guard at " + start.GetX() + ", " + start.GetY());
              
        Debug.Log("player last seen at " + _goal.GetX() + ", " + _goal.GetY());


        return newSearch.FindRouteFrom(start, _goal);
    }


    /**
     * Go to each position in the route
     * Should end up where player last seen, then do the post searching behaviour
     */
    IEnumerator NavigateToPlayer(bool searching)
    {        
        do
        {
            for (int i = 0; i < _routeToPlayer.Length; i++)
            {
                
                yield return StartCoroutine(MoveToNextPosition(_routeToPlayer[i]));
                
            }

            searching = false;

        } while (searching);

        //Debug.Log("finsihed search");

        if (_player.GetComponent<PlayerNPCRelation>().dead)
        {
            enabled = false;
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

        if (_postSearch.ResumePatrol)
        {
            if (!(_player.GetComponent<PlayerNPCRelation>().dead))
                GetComponent<Spritehandler>().FlipSprite();  
          
            GetComponent<GuardAI>().enabled = true;
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


    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    public void SetGoal(Node goal)
    {
        _goal = goal;
    }

    /*
     * Starts a new search and navigation from current position to player's last seen position
     */
    public void StartSearch()
    {
        _routeToPlayer = CalculateRouteToPlayer();       
        _searching = true;
        Debug.Log(_routeToPlayer.Length);
        StartCoroutine(NavigateToPlayer(_searching));            
    }

    public void ReturnToPatrol()
    {
        _searching = true;
        SetSpeed(1f);
        StartCoroutine(NavigateToPlayer(_searching));        
    }

    public Node[] ReverseRoute()
    {
        Node[] newRoute = new Node[_routeToPlayer.Length];
        int counter = 0;

        for (int i = _routeToPlayer.Length - 1; i > 0; i--)
        {
            newRoute[counter] = _routeToPlayer[i];
            counter++;
        }

        Debug.Log(_routeToPlayer.Length);
        _routeToPlayer = newRoute;
        return _routeToPlayer;
    }

}
