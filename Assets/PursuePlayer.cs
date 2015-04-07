using System;
using System.CodeDom;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.VersionControl;

public class PursuePlayer : MonoBehaviour
{

    private GameObject _player;
    private const float SpeedScale = 2.5f;
    private bool _endOfRoute = false;
    private bool _routeCalculated = false;
    private Node[] routeToPlayer;
    private int _currentNode;

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
        
	    if (!_routeCalculated)
	    {
            routeToPlayer = CalculateRouteToPlayer(); 
	        _routeCalculated = true;
	        _currentNode = 0;
       	 }
	    
        if (!_endOfRoute && _routeCalculated)
            NavigateToPlayer(_currentNode);
         else
         {
              this.enabled = false;
              gameObject.GetComponent<GuardAI>().enabled = true; // end after 1 turn    
         }

    }

    private void NavigateToPlayer(int currentNode)
    {
        Node nextPosition = routeToPlayer[currentNode];
        Debug.Log(" guard next pos " + nextPosition.GetX() + ", " + nextPosition.GetY());

        gameObject.rigidbody2D.AddForce(new Vector2((nextPosition.gameObject.transform.position.x - gameObject.transform.position.x) * SpeedScale/2, (nextPosition.gameObject.transform.position.y - gameObject.transform.position.y) * (SpeedScale * 2)));

        currentNode++;
        Debug.Log("made move");
        if (currentNode == routeToPlayer.Length - 1)
        {
            Debug.Log(" at last node");
            _endOfRoute = true;
        }
    }

    private Node[] CalculateRouteToPlayer()
    {
     
        BFS newSearch = new BFS();
        newSearch._frontier = new List<Node>();
        newSearch._possiblePath = new Dictionary<Node, Node>();
        newSearch._visited = new HashSet<Node>();

        Node start = nodeAt;
        Debug.Log(nodeAt.GetX() + ", " + nodeAt.GetY());
        Node goal = _player.GetComponent<PlayerMapRelation>().ReturnNodePlayerAt();
        Debug.Log(goal.GetX() + ", " + goal.GetY());


        return newSearch.FindRouteFrom(start, goal);
        
    }


    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.layer == 12)
        {
            nodeAt = _gameMap.GetComponent<GenerateNodes>().ReturnGeneratedGraph().nodeWith(col.gameObject.GetComponent<Node>());                             
        }
    }

    public Node ReturnNodeAt()
    {
        return nodeAt;
    }



}
