using UnityEngine;

public class Movement : MonoBehaviour {

    private const KeyCode GoLeft = KeyCode.A;
    private const KeyCode GoRight = KeyCode.D;
  
    private Animator _anim;
    private const int Speed = 3;
    private const int RunSpeed = 5;

    private bool _onGround;
    private const string GroundTag = "Ground";
    private const string PlatformTag = "Platform";
    private const string RisingPlatformTag = "RisingPlatform";

    // Use this for initialization
	void Start ()
	{
	    _anim = gameObject.GetComponent<Animator>();
	    _onGround = true;	    
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (Input.GetKey(GoLeft))
        {
            rigidbody2D.AddForce(new Vector2(-Speed, 0));
            _anim.SetInteger("walkDirection", 2);
        }
        else if (Input.GetKey(GoRight))
        {
            rigidbody2D.AddForce(new Vector2(Speed, 0));
            _anim.SetInteger("walkDirection", 1);
        }
        else if (Input.GetKeyUp(GoRight))
        {
            _anim.SetInteger("walkDirection", -1);
        }
        else if (Input.GetKeyUp(GoLeft))
        {
            _anim.SetInteger("walkDirection", -2);
        }

        if (Input.GetKeyDown(KeyCode.Space) && _onGround == true)
        {
            rigidbody2D.AddForce(new Vector2(0, 320));
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKey(GoLeft))
            {
                rigidbody2D.AddForce(new Vector2(-RunSpeed, 0));
                _anim.SetInteger("walkDirection", 2);
            }
            else if (Input.GetKey(GoRight))
            {
                rigidbody2D.AddForce(new Vector2(RunSpeed, 0));
                _anim.SetInteger("walkDirection", 1);
            }
        }
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        string collisionTag = col.gameObject.tag;
        Debug.Log(collisionTag);
        if ((collisionTag == GroundTag || collisionTag == PlatformTag || collisionTag == RisingPlatformTag) && col.gameObject.transform.position.y < gameObject.transform.position.y)            
        _onGround = true;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        string collisionTag = col.gameObject.tag;
        if (collisionTag == GroundTag || collisionTag == PlatformTag || collisionTag == RisingPlatformTag)
            _onGround = false;
    }
}
