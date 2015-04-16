using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursuePlayer : MonoBehaviour
{

    //private GameObject _player;
    private GameObject _gameMap;

    private float _speed = 4f;

    private Node[] _routeToPlayer;
    private Node _goal;

    // Use this for initialization
    /*
     * calculates the route to the player, using the map
     * then goes to the player's position WHEN PLAYER LAST SEEN
     */
	void Start ()
	{	    
	    //_player = GameObject.FindGameObjectWithTag("Player");
	    _gameMap = GameObject.FindGameObjectWithTag("Map");

        _routeToPlayer = CalculateRouteToPlayer(); 	               
	    Debug.Log(_routeToPlayer.Length);
        bool _loop = true;            	        
        StartCoroutine(NavigateToPlayer(_loop));                            
    }
    
	// Update is called once per frame
    public void FixedUpdate()
    {

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

        
        //Node goal = _player.GetComponent<PlayerMapRelation>().ReturnNodePlayerAt();
        Debug.Log("player last seen at " + _goal.GetX() + ", " + _goal.GetY());


        return newSearch.FindRouteFrom(start, _goal);
    }


    /**
     * Go to each position in the route
     * Should end up where player last seen
     */
    IEnumerator NavigateToPlayer(bool loop)
    {

        do
        {
            for (int i = 0; i < _routeToPlayer.Length; i++)
            {
                
                yield return StartCoroutine(MoveToNextPosition(_routeToPlayer[i]));
                
            }
            loop = false;
        } while (loop);

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
                Debug.Log("platform to jump");
                    yield return StartCoroutine(JumpToPlatform(nextPosition.gameObject.transform.position));                         
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, nextPosition.gameObject.transform.position, _speed * Time.deltaTime);
                yield return 0;
            }
            
        }        
    }

    IEnumerator JumpToPlatform(Vector2 platformPosition)
    {
        Vector2 bendPosition = Vector2.up;
        float timeToJump = 1.7f; // need to calc time to jump
        float timeStamp = Time.time;

        while (Time.time - timeStamp < timeToJump)
        {                        
            transform.position = Vector2.MoveTowards(transform.position, platformPosition, (Time.time - timeStamp)/timeToJump);            
            float newY = transform.position.y + bendPosition.y*Mathf.Sin(Mathf.Clamp01((Time.time - timeStamp)/timeToJump)*Mathf.PI);            
            float newX = transform.position.x + bendPosition.x * Mathf.Sin(Mathf.Clamp01((Time.time - timeStamp) / timeToJump) * Mathf.PI);
            
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
   
}
