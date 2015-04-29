using UnityEngine;
using System.Collections;

public class PlayerMapRelation : MonoBehaviour
{

    private Node _nodeAt;
    private GameObject _gameMap;

	// Use this for initialization
	void Start ()
	{
	    _nodeAt = null;
	    _gameMap = GameObject.FindGameObjectWithTag("Map");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /**
     * tracks the node the player is currently at
     */
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.layer == 12)
        {
            _nodeAt = _gameMap.GetComponent<GenerateNodes>().ReturnGeneratedGraph().nodeWith(col.gameObject.GetComponent<Node>());                 
        }
    }

    /*
     * Returns the node the player is at
     */
    public Node ReturnNodePlayerAt()
    {        
        return _nodeAt;
    }
}
