using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenerateNodes : MonoBehaviour
{

    public GameObject Node;
    private float _nodeSize;
    [HideInInspector] public float NodeDistance;

    public int StartX;
    public int StartY;

    private int _mapLength;
    private int _mapHeight;    

    private GraphOfMap _graph;

	// Use this for initialization
    /*
     * creates a map, consisting of nodes which can be use for determining coordinates/location
     * Once map has been generated, add neighbour (horizontal only) to each node, as null pointer otherwise
     */
	void Awake ()
	{
	    _mapHeight = 3;
	    _mapLength = 10;

	    _nodeSize = Node.GetComponent<CircleCollider2D>().radius * 2;
	    const float nodeGap = 0.5f;
	    NodeDistance = _nodeSize + nodeGap;        


        _graph = new GraphOfMap(); // where map is
        _graph.AbstractMap = new List<Node>();

	    for (int i = StartX; i < _mapLength; i++)
	    {
	        for (int j = StartY; j < _mapHeight; j++)
	        {
	            GameObject newPos = (GameObject)Instantiate(Node, new Vector2(i * (NodeDistance), j * (NodeDistance)), Node.transform.rotation);
	            newPos.transform.parent = gameObject.transform;
	            newPos.layer = 12;

	            newPos.AddComponent<Node>();
	            newPos.GetComponent<Node>().SetUpNode(ref _graph);
	        }
	    }

	    for (int j  = 0; j < _graph.ReturnGraph().Count; j++)
	    {
	        Node node = _graph.ReturnGraph().ElementAt(j);             
            node.AddNeighbour(-NodeDistance,ref _graph);
            node.AddNeighbour(NodeDistance,ref _graph);
	    }	   
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /**
     * returns the object that contains the graph
     */
    public GraphOfMap ReturnGeneratedGraph()
    {
        return _graph;
    }
}
