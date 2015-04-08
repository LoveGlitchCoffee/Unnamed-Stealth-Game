using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursuePlayer : MonoBehaviour
{

    private GameObject _player;
    private GameObject _gameMap;
    

    private bool _endOfRoute = false;
    private bool _routeCalculated = false;
    private bool _startedMove = false;
    private bool _startedMove2 = false;

    private Node[] _routeToPlayer;

    private bool _loop = true;

    // Use this for initialization
	void Start ()
	{	    
	    _player = GameObject.FindGameObjectWithTag("Player");
	    _gameMap = GameObject.FindGameObjectWithTag("Map");

        _routeToPlayer = CalculateRouteToPlayer(); 
	       
        Debug.Log(_routeToPlayer.Length);	       
            	        
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
        Node goal = _player.GetComponent<PlayerMapRelation>().ReturnNodePlayerAt();
        Debug.Log("palyer at " + goal.GetX() + ", " + goal.GetY());


        return newSearch.FindRouteFrom(start, goal);
    }


    /**
     * moves guard to the next step in the route returned from search
     */
    IEnumerator NavigateToPlayer(bool loop)
    {

        do
        {
            for (int i = 0; i < _routeToPlayer.Length; i++)
            {
                yield return StartCoroutine(MoveToNextPosition(_routeToPlayer[i]));

                Debug.Log("moved " + i);
            }
            loop = false;
        } while (loop);

        

    }

    IEnumerator MoveToNextPosition(Node nextPosition)
    {
        

        while (transform.position.x != nextPosition.GetX() && transform.position.y != nextPosition.GetY())
        {            
            transform.Translate((nextPosition.GetX() - transform.position.x), (nextPosition.GetY() - transform.position.y), gameObject.transform.position.z);
            yield return new WaitForSeconds(1f);    
        }
        
    }
   



}
