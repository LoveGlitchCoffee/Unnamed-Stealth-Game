using UnityEngine;
using System.Collections;
using UnityEditor.VersionControl;

public class PursuePlayer : MonoBehaviour
{

    private GameObject _player;
	// Use this for initialization
	void Start ()
	{
	    gameObject.GetComponent<PursuePlayer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    
	}

    public void Chase(GameObject player)
    {
        CalculatePositionDifference(transform.position.x, player.transform.position.x);
        CalculatePositionDifference(transform.position.y, player.transform.position.y);
    }

    private float CalculatePositionDifference(float _guardPos, float _playerPos)
    {
        return (_guardPos - _playerPos);
    }

 



}
