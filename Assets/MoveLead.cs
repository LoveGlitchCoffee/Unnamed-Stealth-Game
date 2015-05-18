using System.Collections;
using UnityEngine;

public class MoveLead : MonoBehaviour
{

    private float DistanceFromPlayerX = 3.5f;
    private const float DistanceFromPlayerY = 1f;
    private Transform _playerPosition;
    private Movement _playerMovement;
    private bool _showing = false;
    private bool _contactWall = false;

	void Start ()
	{
	    GameObject player = GameObject.FindGameObjectWithTag("Player");
	    _playerPosition = player.transform;
	    _playerMovement = player.GetComponent<Movement>();
	}
	
	
	void Update () {

        if (!_showing && !_contactWall)
	        transform.position = Vector3.Lerp(transform.position, new Vector3(_playerPosition.position.x + DistanceFromPlayerX,_playerPosition.position.y + DistanceFromPlayerY),Time.deltaTime*5f);
	}

    public void SetDirection()
    {
        DistanceFromPlayerX *= -1f;
    }

    public IEnumerator MoveToPosition(GameObject objectToMoveTo)
    {        
        while (objectToMoveTo != null && transform.position != objectToMoveTo.transform.position && _showing)
        {            
            transform.position = Vector3.Lerp(transform.position, objectToMoveTo.transform.position, Time.deltaTime * 3f);
            yield return null;
        }
    }

    public void IsShowing(bool showing)
    {        
        this._showing = showing;        
    }

    void OnCollisionEnter2D()
    {    
        _contactWall = true;
    }

}
