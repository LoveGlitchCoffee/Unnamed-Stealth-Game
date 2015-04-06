using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Node : MonoBehaviour, IComparable<Node>
{

    private List<Node> _successors;
    private GraphOfMap _graph;

    private GameObject _gameMap;

    bool justSpawned = true;

	void Start () {
	    
	    _gameMap = GameObject.FindGameObjectWithTag("Map");
	}
	
	// Update is called once per frame
	void Update () {
	   
	}

    public void AddSuccessor(Node n, ref GraphOfMap graph)
    {
        Node succesor = graph.nodeWith(n);
        Debug.Log("successor: " + succesor.GetX() + ", " + succesor.GetY());
        _successors.Add(succesor);
    }

    public List<Node> GetSuccessors()
    {
        return _successors;
    }

    public void SetUpNode(ref GraphOfMap graph)
    {
        //_graph = graph;
        _successors = new List<Node>();
        AddToGraph(ref graph);        
    }

    private void AddToGraph(ref GraphOfMap graph)
    {
        Node newNode = graph.nodeWith(this);
    }

    public void AddNeighbour(int direction, ref GraphOfMap graph)
    {
        for(int i = 0; i < graph.ReturnGraph().Count; i++)
        {
            if (graph.ReturnGraph().ElementAt(i).GetY() == this.GetY() && graph.ReturnGraph().ElementAt(i).GetX() == this.GetX() - direction && graph.ReturnGraph().ElementAt(i).CompareTo(this) != 0)
            {
                AddSuccessor(graph.ReturnGraph().ElementAt(i), ref graph); 
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

            /*foreach (Transform node in _gameMap.transform)
            {
                if ((node.position.x == this.gameObject.transform.position.x - 2 || node.position.x == this.gameObject.transform.position.x + 2) && node.position.y == this.gameObject.transform.position.y)
                {
                    node.gameObject.GetComponent<Node>().AddSuccessor(platformNode);
                    
                }
            }*/

            justSpawned = false;
        }
    }

    public float GetX()
    {
        return gameObject.transform.position.x;
    }

    public float GetY()
    {
        return gameObject.transform.position.y;
    }
}
