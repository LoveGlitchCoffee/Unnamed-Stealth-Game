using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.VersionControl;

public class PursuePlayer : MonoBehaviour
{

    private GameObject _player;
    private const int SpeedScale = 4;
    private bool _endOfRoute = false;
    private bool _routeCalculated = false;
    private List<Node> routeToPlayer;

    private Node nodeAt;

    private GameObject _gameMap;

    // Use this for initialization
	void Start ()
	{
	    this.enabled = false;
	    _player = GameObject.FindGameObjectWithTag("Player");
	    _gameMap = GameObject.FindGameObjectWithTag("Map");
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
        //float xDiff = CalculateMoveVelocity(transform.position.x, _player.transform.position.x);
        //CalculatePositionDifference(transform.position.y, player.transform.position.y);
        // GoToPlayerX(xDiff);
	    if (!_routeCalculated)
	    {
           
            routeToPlayer = CalculateRouteToPlayer(); 
	        _routeCalculated = true;
	    }
	    
        if (!_endOfRoute)
	    NavigateToPlayer();
        else
        {
            this.enabled = false;
            gameObject.GetComponent<GuardAI>().enabled = true; // end after 1 turn    
        }
	    
	}

    private void NavigateToPlayer()
    {
        Node nextPosition = routeToPlayer.ElementAt(0);

        gameObject.rigidbody2D.AddForce(new Vector2((nextPosition.gameObject.transform.position.x - gameObject.transform.position.x) * SpeedScale, (nextPosition.gameObject.transform.position.y - gameObject.transform.position.y) * SpeedScale));

        routeToPlayer.RemoveAt(0);

        if (routeToPlayer.Count == 0)
        {
            _endOfRoute = true;
        }
    }

    private List<Node> CalculateRouteToPlayer()
    {
     
        BFS newSearch = new BFS();

        Node start = nodeAt;
        Node goal = _player.GetComponent<PlayerMapRelation>().ReturnNodePlayerAt();

        return newSearch.FindRouteFrom(start, goal);
        
    }

    /**
     * Calculates the velocity to move in a certain axis to get to player
     */
    private float CalculateMoveVelocity(float guardPos, float playerPos)
    {
        return (guardPos - playerPos);
    }

    /**
     * Run towards player, direction depend on velocity passed
     */
    /*private void GoToPlayerX(float xDiff)
    {
        if (xDiff > 0) // quite useless, could be used for something later
        gameObject.rigidbody2D.AddForce(new Vector2(-Speed, 0));
        else
        {
            gameObject.rigidbody2D.AddForce(new Vector2(Speed, 0));
        }
    }*/

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.layer == 12)
        {
            nodeAt = _gameMap.GetComponent<GenerateNodes>().ReturnGraph().nodeWith(col.gameObject.GetComponent<Node>());
           /* Debug.Log("node at x" + nodeAt.GetX());
            Debug.Log("node at y" + nodeAt.GetY());
            Debug.ClearDeveloperConsole();*/
        }
    }

 



}
