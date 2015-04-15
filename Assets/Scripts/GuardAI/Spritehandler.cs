using UnityEngine;
using System.Collections;

public class Spritehandler : MonoBehaviour
{

    private GuardAI _guardAi;
    private bool _guardIdle;
    private Animator _anim;

	// Use this for initialization
	void Awake ()
	{
	    _guardAi = GetComponent<GuardAI>();
	    _anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    _guardIdle = _guardAi.OutOfPatrolArea; // logic m8        
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
