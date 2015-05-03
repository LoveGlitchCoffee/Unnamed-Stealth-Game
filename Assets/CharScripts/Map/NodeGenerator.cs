using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeGenerator : MonoBehaviour
{

    public GameObject Node;
    private float _nodeSize;
    [HideInInspector] public float NodeDistance;

    /*private Patrol[] _guardsPatrol;
    private PlayerMapRelation _playerMap;*/

    public int StartX;
    public int StartY;

    private int _mapLength;
    private int _mapHeight;    

    private GraphOfMap _graph;
	
    /*
     * creates a map, consisting of nodes which can be use for determining coordinates/location
     * Once map has been generated, add neighbours (horizontal only) to each node, add null pointer otherwise
     */
	void Awake ()
	{
	    _mapHeight = 3;
	    _mapLength = 9;

	    _nodeSize = Node.GetComponent<CircleCollider2D>().radius * 2;
	    const float nodeGap = 0.5f;
	    NodeDistance = _nodeSize + nodeGap;

        //was trying to delete node game objects
	    /*GameObject[] guards = GameObject.FindGameObjectsWithTag("Guard");
        _guardsPatrol = new Patrol[guards.Length];
	    for (int i = 0; i < guards.Length; i++)
	    {
	        _guardsPatrol[i] = guards[i].GetComponent<Patrol>();
	    }

	    _playerMap = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMapRelation>();*/


        _graph = new GraphOfMap(); // where map is
        _graph.AbstractMap = new List<Node>();

	    GenerateNodes();
	}

    /*
     * Generates all nodes according to the length and height of the map
     * Then, for each node, add their neighbours as successors
     */
    private void GenerateNodes()
    {
        for (int i = StartX; i < _mapLength; i++)
        {
            for (int j = StartY; j < _mapHeight; j++)
            {
                GameObject newPos =
                    (GameObject) Instantiate(Node, new Vector2(i*(NodeDistance), j*(NodeDistance)), Node.transform.rotation);
                newPos.transform.parent = gameObject.transform;
                newPos.layer = 12;

                newPos.AddComponent<Node>();
                newPos.GetComponent<Node>().SetUpNode(ref _graph);
            }
        }

        for (int j = 0; j < _graph.ReturnGraph().Count; j++)
        {
            Node node = _graph.ReturnGraph().ElementAt(j);

            if (node.GetY() < NodeDistance)
            {
                //Debug.Log("node is " + node.GetX() + ", " + node.GetY());
                node.AddNeighbour(-NodeDistance, ref _graph);
                node.AddNeighbour(NodeDistance, ref _graph);
            }
        }

        StartCoroutine(WaitForDeletion());        
    }

    /*
     * Waits before deletion to allow time for adding platform nodes and others
     * as they use triggers
     */
    IEnumerator WaitForDeletion()
    {
        float delayTime = 1f;
        float delayCounter = 0;

        while (delayCounter < delayTime)
        {
            delayCounter += 0.5f;
            yield return 0;
        }

        DeleteWaypoints();

    }

    /**
     * returns the object that contains the graph
     */
    public GraphOfMap ReturnGeneratedGraph()
    {
        return _graph;
    }

    /*
     * Currently only deletes the circle collider, deleting game object creates complication for startup
     */
    public void DeleteWaypoints()
    {
        for (int j = 0; j < _graph.ReturnGraph().Count; j++)
        {
            Node node = _graph.ReturnGraph().ElementAt(j);

            if (node.GetSuccessors().Count == 0)
            {                
   
                /*if (_playerMap.ReturnNodePlayerAt() == _graph.nodeWith(node))
                {
                    _playerMap.SetNodeManually(_graph.ReturnGraph()[0]);
                }

                for (int i = 0; i < _guardsPatrol.Length; i++)
                {
                    if (_guardsPatrol[i].ReturnNodeGuardAt() == _graph.nodeWith(node))
                    {
                        _guardsPatrol[i].SetNodeAtManually(_graph.ReturnGraph()[0]);
                    }
                }*/

                Destroy(node.GetComponent<CircleCollider2D>());
                
            }
        }	   
    }
}
