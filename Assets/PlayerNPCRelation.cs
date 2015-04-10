using UnityEngine;
using System.Collections;

public class PlayerNPCRelation : MonoBehaviour
{

    private const string GuardTag = "Guard";
    private Animator _anim;
    private Movement _playerMovement;

	// Use this for initialization
	void Start ()
	{
	    _anim = gameObject.GetComponent<Animator>();
	    _playerMovement = gameObject.GetComponent<Movement>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == GuardTag)
        {
            Debug.Log("guard killed me");
            _anim.SetInteger("walkDirection", 0);
            Debug.Log(_anim.GetInteger("walkDirection"));
            _anim.SetBool("dead",true);
            _playerMovement.enabled = false;
            Debug.Log(_anim.GetBool("dead"));
        }
    }
}
