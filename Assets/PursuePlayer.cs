using UnityEngine;
using System.Collections;
using UnityEditor.VersionControl;

public class PursuePlayer : MonoBehaviour
{

    private GameObject _player;
    private int _speed = 6; // speed will be on average 5 (less than player's 6)
	// Use this for initialization
	void Start ()
	{
	    gameObject.GetComponent<PursuePlayer>().enabled = false;
	    _player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
        float xDiff = CalculatePositionDifference(transform.position.x, _player.transform.position.x);
        //CalculatePositionDifference(transform.position.y, player.transform.position.y);

        GoToPlayerX(xDiff);
	}

    private float CalculatePositionDifference(float _guardPos, float _playerPos)
    {
        return (_guardPos - _playerPos);
    }

    private void GoToPlayerX(float xDiff)
    {
        if (xDiff > 0)
        gameObject.rigidbody2D.AddForce(new Vector2(-_speed, 0));
        else
        {
            gameObject.rigidbody2D.AddForce(new Vector2(_speed, 0));
        }
    }

 



}
