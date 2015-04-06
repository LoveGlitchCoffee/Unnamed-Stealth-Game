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

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.layer == 12)
        {
            _nodeAt = _gameMap.GetComponent<GenerateNodes>().ReturnGeneratedGraph().nodeWith(col.gameObject.GetComponent<Node>());
            Debug.Log(_nodeAt);
        }
    }

    public Node ReturnNodePlayerAt()
    {
        return _nodeAt;
    }
}
