using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour, IComparable<Node>
{

    private List<Node> _successors;
    private GraphOfMap _graph;

    private GameObject _gameMap;

    bool justSpawned = true;
    private bool _neighboursAdded = false;

	void Start () {
	    
	    _gameMap = GameObject.FindGameObjectWithTag("Map");
	}
	
	// Update is called once per frame
	void Update () {
	    if (!_neighboursAdded)
	    {
            AddNeighbour(-2);
            AddNeighbour(2);
	        _neighboursAdded = true;
	    }
	}

    public void AddSuccessor(Node n)
    {
        Node succesor = _graph.nodeWith(n);
        _successors.Add(succesor);
    }

    public List<Node> GetSuccessors()
    {
        return _successors;
    }

    public void SetUpNode(GraphOfMap graph)
    {
        _graph = graph;
        _successors = new List<Node>();
        AddToGraph();        
    }

    private void AddToGraph()
    {
        Node newNode = _graph.nodeWith(this);
    }

    private void AddNeighbour(int direction)
    {
        foreach (Node existingNode in _graph.ReturnGraph())
        {
            if (existingNode.gameObject.transform.position.y == gameObject.transform.position.y && existingNode.gameObject.transform.position.x == gameObject.transform.position.x - direction && existingNode.CompareTo(this) != 0)
            {
                AddSuccessor(existingNode);                
            }
        }
    }

    public int CompareTo(Node comparedNode)
    {
        if (gameObject.transform.position.x == comparedNode.gameObject.transform.position.x &&
            gameObject.transform.position.y == comparedNode.gameObject.transform.position.y)
            return 0;
        else
        {
            return -1;
        }
    }

    void OnTriggerStay2D(Collider2D col) // could consider delete succesor
    {
        
        if (col.gameObject.layer == 10 && col.gameObject.tag == "RisingPlatform" && justSpawned)
        {
            Node platformNode = null;

            foreach (Transform node in _gameMap.transform)
            {
                if (node.position.x == this.gameObject.transform.position.x &&
                    node.position.y == this.gameObject.transform.position.y + 2)
                {
                    platformNode = node.gameObject.GetComponent<Node>();
                }
            }

            foreach (Transform node in _gameMap.transform)
            {
                if ((node.position.x == this.gameObject.transform.position.x - 2 || node.position.x == this.gameObject.transform.position.x + 2) && node.position.y == this.gameObject.transform.position.y)
                {
                    node.gameObject.GetComponent<Node>().AddSuccessor(platformNode);
                    
                }
            }

            justSpawned = false;
        }
    }

}
