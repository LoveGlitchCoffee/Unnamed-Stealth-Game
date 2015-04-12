using UnityEngine;

public class Movement : MonoBehaviour {

    private const KeyCode GoLeft = KeyCode.A;
    private const KeyCode GoRight = KeyCode.D;
  
    private Animator _anim;
    private const float Speed = 1.7f;
    private const float RunSpeed = 2.7f;

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
            transform.Translate(new Vector3(-Speed*Time.deltaTime, 0,transform.position.z));
            _anim.SetInteger("walkDirection", 2);
        }
        else if (Input.GetKey(GoRight))
        {
            transform.Translate(new Vector3(Speed * Time.deltaTime, 0, transform.position.z));            
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
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 320));
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKey(GoLeft))
            {
                transform.Translate(new Vector3(-RunSpeed * Time.deltaTime, 0, transform.position.z));                            
                _anim.SetInteger("walkDirection", 2);
            }
            else if (Input.GetKey(GoRight))
            {
                transform.Translate(new Vector3(RunSpeed * Time.deltaTime, 0, transform.position.z));                                            
                _anim.SetInteger("walkDirection", 1);
            }
        }
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        float spriteSize = GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        string collisionTag = col.gameObject.tag;
        //Debug.Log(collisionTag);
        if ((collisionTag == GroundTag || collisionTag == PlatformTag || collisionTag == RisingPlatformTag) && ((gameObject.transform.position.y - col.transform.position.y) >= spriteSize/2f)) 
        {
            //Debug.Log("can jump");
            _onGround = true;
        }

        
    }

    void OnCollisionExit2D(Collision2D col)
    {
        string collisionTag = col.gameObject.tag;
        if (collisionTag == GroundTag || collisionTag == PlatformTag || collisionTag == RisingPlatformTag)
        {
            _onGround = false;
            //Debug.Log("cannot jump");
        }
            
    }
}
