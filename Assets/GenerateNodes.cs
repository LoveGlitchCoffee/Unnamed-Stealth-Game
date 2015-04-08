using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GenerateNodes : MonoBehaviour
{

    public GameObject Node;
    private float _nodeSize;

    public int StartX;
    public int StartY;

    private int _mapLength;
    private int _mapHeight;    

    private GraphOfMap _graph;

	// Use this for initialization
	void Start ()
	{
	    _mapHeight = 3;
	    _mapLength = 10;

	    _nodeSize = Node.GetComponent<CircleCollider2D>().radius * 2;

        _graph = new GraphOfMap(); // where map is
        _graph._abstractMap = new List<Node>();

	    for (int i = StartX; i < _mapLength; i++)
	    {
	        for (int j = StartY; j < _mapHeight; j++)
	        {
	            GameObject newPos = (GameObject)Instantiate(Node, new Vector2(i * _nodeSize, j * _nodeSize), Node.transform.rotation);
	            newPos.transform.parent = gameObject.transform;
	            newPos.layer = 12;

	            newPos.AddComponent<Node>();
	            newPos.GetComponent<Node>().SetUpNode(ref _graph);
	        }
	    }

	    for (int j  = 0; j < _graph.ReturnGraph().Count; j++)
	    {
             //Debug.Log("node " + j + ": " + _graph.ReturnGraph().ElementAt(j).GetX() + ", " + _graph.ReturnGraph().ElementAt(j).GetY());
            _graph.ReturnGraph().ElementAt(j).AddNeighbour(-2,ref _graph);
            _graph.ReturnGraph().ElementAt(j).AddNeighbour(2,ref _graph);

	    }

	   

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public GraphOfMap ReturnGeneratedGraph()
    {
        return _graph;
    }
}
