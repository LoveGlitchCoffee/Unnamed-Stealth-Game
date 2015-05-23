using UnityEngine;
using System.Collections;
using UnityEngineInternal;

public class Spritehandler : MonoBehaviour
{

        
    private Animator _anim;
    [HideInInspector] public bool SpriteLeft { get; set; }

	
	void Awake ()
	{	
	    _anim = GetComponent<Animator>();
	    SpriteLeft = false;
	}
	

    /*
     * Flips the sprite
     */
    public void FlipSprite()
    {        
        Vector3 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        gameObject.transform.localScale = localScale;
        SpriteLeft = !SpriteLeft;
    }

    public void PlayAnimation(string anim)
    {
        _anim.SetBool(anim, true);
    }

    public void StopAnimation(string anim)
    {
        _anim.SetBool(anim, false);
    }
}
