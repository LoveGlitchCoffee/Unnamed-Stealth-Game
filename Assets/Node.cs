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
        //Debug.Log("successor: " + succesor.GetX() + ", " + succesor.GetY());
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

    public void AddNeighbour(float direction, ref GraphOfMap graph)
    {
        for(int i = 0; i < graph.ReturnGraph().Count; i++)
        {
            Node node = graph.ReturnGraph().ElementAt(i);
            
            if (node.GetX() == GetX() - direction && node.GetY() == GetY())
            {
                AddSuccessor(node, ref graph); 
                Debug.Log("successor " + node.GetX() + ", " + node.GetY());
            }
        }
    }

    public int CompareTo(Node comparedNode)
    {
        if (this.GetX() == comparedNode.GetX() &&
            this.GetY() == comparedNode.GetY())
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
            GraphOfMap graph = _gameMap.GetComponent<GenerateNodes>().ReturnGeneratedGraph();

            for (int i = 0; i < graph.ReturnGraph().Count; i++)
            {
                Node node = graph.ReturnGraph().ElementAt(i);

                if (node.GetX() == this.GetX() &&
                    node.GetY() == this.GetY() + 2)
                {
                    platformNode = node;
                }
            }

           for (int j = 0; j < graph.ReturnGraph().Count; j++)
           {
               Node node = graph.ReturnGraph().ElementAt(j);

                if ((node.GetX() == this.GetX() - 2 || node.GetX() == this.GetX() + 2) && node.GetY() == this.GetY())
                {
                    Debug.Log(GetX() + ", " + GetY() + " platform sucessor");
                    node.AddSuccessor(platformNode, ref graph);
                    //platform node add them ass successors
                }
            }

            
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
