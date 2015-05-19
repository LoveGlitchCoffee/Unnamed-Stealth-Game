using UnityEngine;
using System.Collections;

public class PlayerMapRelation : MonoBehaviour
{

    private Node _nodeAt;
    private GameObject _gameMap;
	
	void Start ()
	{
	    _nodeAt = null;
	    GetNewMap();
	}

    public void GetNewMap()
    {
        _gameMap = GameObject.FindGameObjectWithTag("Map");        
    }
	

    /**
     * tracks the node the player is currently at
     */
    void OnTriggerStay2D(Collider2D col)
    {        
        if (col.gameObject.layer == 12 && _gameMap != null)
        {            
            _nodeAt = _gameMap.GetComponent<NodeGenerator>().ReturnGeneratedGraph().nodeWith(col.gameObject.GetComponent<Node>());                 
        }
    }

    /*
     * Returns the node the player is at
     */
    public Node ReturnNodePlayerAt()
    {        
        return _nodeAt;
    }

    public void SetNodeManually(Node node)
    {
        _nodeAt = node;
    }
}
