using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    KeyCode goLeft = KeyCode.A;
    KeyCode goRight = KeyCode.D;
  
    public Animator anim;
    int speed = 3;
    int runSpeed = 5;

    private bool _onGround;
    private string _groundTag;

	// Use this for initialization
	void Start ()
	{
	    _onGround = true;
	    _groundTag = "Ground";
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (Input.GetKey(goLeft))
        {
            rigidbody2D.AddForce(new Vector2(-speed, 0));
            anim.SetInteger("walkDirection", 2);
        }
        else if (Input.GetKey(goRight))
        {
            rigidbody2D.AddForce(new Vector2(speed, 0));
            anim.SetInteger("walkDirection", 1);
        }
        else if (Input.GetKeyUp(goRight))
        {
            anim.SetInteger("walkDirection", -1);
        }
        else if (Input.GetKeyUp(goLeft))
        {
            anim.SetInteger("walkDirection", -2);
        }

        if (Input.GetKeyDown(KeyCode.Space) && _onGround == true)
        {
            rigidbody2D.AddForce(new Vector2(0, 300));
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKey(goLeft))
            {
                rigidbody2D.AddForce(new Vector2(-runSpeed, 0));
                anim.SetInteger("walkDirection", 0);
            }
            else if (Input.GetKey(goRight))
            {
                rigidbody2D.AddForce(new Vector2(runSpeed, 0));
                anim.SetInteger("walkDirection", 1);
            }
        }
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == _groundTag)
        _onGround = true;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == _groundTag)
            _onGround = false;
    }
}
