using UnityEngine;
using System.Collections;

public class MoveLead : MonoBehaviour
{

    private float DistanceFromPlayerX = 3.5f;
    private const float DistanceFromPlayerY = 1f;
    private Transform _playerPosition;
    private Movement _playerMovement;

	void Start ()
	{
	    GameObject player = GameObject.FindGameObjectWithTag("Player");
	    _playerPosition = player.transform;
	    _playerMovement = player.GetComponent<Movement>();
	}
	
	
	void Update () {
	    transform.position = Vector3.Lerp(transform.position, new Vector3(_playerPosition.position.x + DistanceFromPlayerX,_playerPosition.position.y + DistanceFromPlayerY),Time.deltaTime*2.5f);
	}

    public void SetDirection()
    {
        DistanceFromPlayerX *= -1f;
    }
}
