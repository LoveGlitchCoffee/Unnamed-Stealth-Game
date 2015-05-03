using UnityEngine;
using System.Collections;

public class PlayerNPCRelation : MonoBehaviour
{

    private const string GuardTag = "Guard";
    private Animator _anim;
    private Movement _playerMovement;

    [HideInInspector] public bool dead { get; set; }

	// Use this for initialization
	void Start ()
	{
	    _anim = gameObject.GetComponent<Animator>();
	    _playerMovement = gameObject.GetComponent<Movement>();
	}


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == GuardTag)
        {                                   
            _anim.SetBool("dead",true);
            _playerMovement.enabled = false;
            dead = true;
            gameObject.layer = 9;
        }
    }
}
