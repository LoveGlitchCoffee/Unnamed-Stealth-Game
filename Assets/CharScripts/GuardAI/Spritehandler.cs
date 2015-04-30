using UnityEngine;
using System.Collections;
using UnityEngineInternal;

public class Spritehandler : MonoBehaviour
{

    private Patrol _Patrol;
    [HideInInspector] public bool GuardIdle { get; set; }
    private Animator _anim;

	// Use this for initialization
	void Awake ()
	{
	    _Patrol = GetComponent<Patrol>();
	    _anim = GetComponent<Animator>();
	}
	

    void FixedUpdate()
    {        
        _anim.SetBool("idle",GuardIdle);
    }

    public void FlipSprite()
    {        
        Vector3 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        gameObject.transform.localScale = localScale;
    }
}
