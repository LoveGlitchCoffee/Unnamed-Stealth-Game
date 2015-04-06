using UnityEngine;
using System.Collections;

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

	    for (int i = StartX; i < _mapLength; i++)
	    {
	        for (int j = StartY; j < _mapHeight; j++)
	        {
	            GameObject newPos = (GameObject)Instantiate(Node, new Vector3(i*_nodeSize, j*_nodeSize), Node.transform.rotation);
	            newPos.transform.parent = gameObject.transform;
	            newPos.layer = 12;

	            newPos.AddComponent<Node>();
	            newPos.GetComponent<Node>().SetUpNode(_graph); // pass by ref?
	        }
	    }

	    /*foreach (Transform nodes in gameObject.transform)
	    {
	        nodes.gameObject.GetComponent<Node>().AddNeighbour(-2,ref _graph);
            nodes.gameObject.GetComponent<Node>().AddNeighbour(2,ref _graph);
	    }*/

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public GraphOfMap ReturnGraph()
    {
        return _graph;
    }
}
