using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Node : MonoBehaviour, IComparable<Node>
{

    private List<Node> _successors;
    private GraphOfMap _graph;

    private GameObject _gameMap;

    bool _addedRising = false;

    private LayerMask _enviLayer;

	void Start () {	    
	    _gameMap = GameObject.FindGameObjectWithTag("Map");
	    _enviLayer = 1 << 10;
	}
	

    /**
     * Add the passed node as this node's successor
     */
    public void AddSuccessor(Node n, ref GraphOfMap graph)
    {
        Node succesor = graph.nodeWith(n);        
        _successors.Add(succesor);
        //Debug.Log("succesor is: " + succesor.GetX() + ", " + succesor.GetY());
    }

    private void DeleteSuccessor(Node successor, ref GraphOfMap graph)
    {
        Node node = graph.nodeWith(successor);
        _successors.Remove(node);
    }

    /**
     * returns the list of this node's successors
     */
    public List<Node> GetSuccessors()
    {
        return _successors;
    }

    /**
     * Adds the node to the passed graph
     */
    public void SetUpNode(ref GraphOfMap graph)
    {       
        _successors = new List<Node>();
        Node newNode = graph.nodeWith(this);  

        if (newNode.GetX() == 0)
            Debug.Log("15 setup");
    }    

    /**
     * Adds this node horizontal neighbour as it successors
     */
    public void AddNeighbour(float direction, ref GraphOfMap graph)
    {
        for(int i = 0; i < graph.ReturnGraph().Count; i++)
        {
            Node node = graph.ReturnGraph().ElementAt(i);

            if (node.GetX() == 15)
                Debug.Log("node 15 found");
            
            if (node.GetX() == GetX() - direction && node.GetY() == GetY())
            {
                AddSuccessor(node, ref graph);  
                Debug.Log("successor " + node.GetX() +", " + node.GetY());
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

    /**
     * If this node is the position of a rising platform, add the node above it as its horizontal sucecssor's successor
     * If the node is in the position of a platform, add the successors to noed above to allow movement across it.
     */
    void OnTriggerStay2D(Collider2D col)
    {
        float nodeDistance = GetComponentInParent<NodeGenerator>().NodeDistance;
        GraphOfMap graph = _gameMap.GetComponent<NodeGenerator>().ReturnGeneratedGraph();
    
        if (col.gameObject.layer == 10 && col.gameObject.tag == "RisingPlatform" && !_addedRising)
        {            
            //Debug.Log("node is " + GetX() + ", " + GetY());
            Node risingNode = FindPlatformNode(nodeDistance,ref graph);
            
            AddSuccessorForRising(risingNode,ref graph, nodeDistance);

            _addedRising = false;                
        }

        if (col.gameObject.layer == 10 && col.gameObject.tag == "Platform")
        {
            Node platformNode = FindPlatformNode(nodeDistance, ref graph);

            platformNode.AddNeighbour(nodeDistance, ref graph);
            platformNode.AddNeighbour(-nodeDistance, ref graph);
        }
        
    }

    /**
     * Finds the node that is above the current node
     */
    private Node FindPlatformNode(float nodeDistance, ref GraphOfMap graph)
    {
        Node platformNode = null;        

        for (int i = 0; i < graph.ReturnGraph().Count; i++)
        {
            Node node = graph.ReturnGraph().ElementAt(i);

            if (node.GetX() == this.GetX() &&
                node.GetY() == this.GetY() + nodeDistance)
            {                
                platformNode = node;
                return platformNode;
            }
        }

        return platformNode;
    }

    /*
     * Creates a jumping route
     * Neighbours of the current node become sucessor of the passed node (which should have higher Y coordinates)
     * Also add successors the other way
     */
    private void AddSuccessorForRising(Node platformNode, ref GraphOfMap graph, float nodeDistance)
    {
        for (int j = 0; j < graph.ReturnGraph().Count; j++)
        {
            Node node = graph.ReturnGraph().ElementAt(j);

            if ((node.GetX() == this.GetX() - nodeDistance || node.GetX() == this.GetX() + nodeDistance) && node.GetY() == this.GetY() && !node.gameObject.GetComponent<Collider2D>().IsTouchingLayers(_enviLayer))
            {
                node.AddSuccessor(platformNode, ref graph);
                platformNode.AddSuccessor(node, ref graph);

                DeleteSuccessor(node, ref graph);
                node.DeleteSuccessor(this, ref graph);
            }
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
