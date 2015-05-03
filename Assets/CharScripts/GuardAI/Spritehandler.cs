using UnityEngine;
using System.Collections;
using UnityEngineInternal;

public class Spritehandler : MonoBehaviour
{

    private Patrol _Patrol;
    [HideInInspector] public bool GuardIdle { get; set; }
    private Animator _anim;

	
	void Awake ()
	{
	    _Patrol = GetComponent<Patrol>();
	    _anim = GetComponent<Animator>();
	}
	
    /*
     * Changes guard animation to idle accordingly
     */
    void FixedUpdate()
    {        
        _anim.SetBool("idle",GuardIdle);
    }

    /*
     * Flips the sprite
     */
    public void FlipSprite()
    {        
        Vector3 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        gameObject.transform.localScale = localScale;
    }
}
