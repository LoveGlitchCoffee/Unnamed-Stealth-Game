using UnityEngine;
using System.Collections;

public class Spritehandler : MonoBehaviour
{

    private Patrol _Patrol;
    private bool _guardIdle;
    private Animator _anim;

	// Use this for initialization
	void Awake ()
	{
	    _Patrol = GetComponent<Patrol>();
	    _anim = GetComponent<Animator>();
	}
	

	void Update ()
	{
	    //_guardIdle = _Patrol.OutOfPatrolArea;      
	}

    void FixedUpdate()
    {        
        _anim.SetBool("idle",_guardIdle);
    }

    public void FlipSprite()
    {        
        Vector3 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        gameObject.transform.localScale = localScale;
    }
}
