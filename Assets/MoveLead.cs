using System.Collections;
using UnityEngine;

public class MoveLead : MonoBehaviour
{

    private float DistanceFromPlayerX = 3.5f;
    private const float DistanceFromPlayerY = 1f;
    private Transform _playerPosition;
    private Movement _playerMovement;
    private bool showing = false;

	void Start ()
	{
	    GameObject player = GameObject.FindGameObjectWithTag("Player");
	    _playerPosition = player.transform;
	    _playerMovement = player.GetComponent<Movement>();
	}
	
	
	void Update () {

        if (!showing)
	        transform.position = Vector3.Lerp(transform.position, new Vector3(_playerPosition.position.x + DistanceFromPlayerX,_playerPosition.position.y + DistanceFromPlayerY),Time.deltaTime*3f);
	}

    public void SetDirection()
    {
        DistanceFromPlayerX *= -1f;
    }

    public IEnumerator MoveToPosition(GameObject objectToMoveTo)
    {        
        while (objectToMoveTo != null && transform.position != objectToMoveTo.transform.position)
        {            
            transform.position = Vector3.Lerp(transform.position, objectToMoveTo.transform.position, Time.deltaTime * 2f);
            yield return null;
        }
    }

    public void IsShowing(bool showing)
    {
        this.showing = showing;        
    }
}
