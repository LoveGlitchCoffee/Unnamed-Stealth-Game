using System;
using UnityEngine;

public class Movement : MonoBehaviour {

    private const KeyCode GoLeft = KeyCode.A;
    private const KeyCode GoRight = KeyCode.D;
    
  
    private Animator _anim;
    private const float MoveForce = 150f;
    private const float MaxSpeed = 1f;
    private const float JumpSpeed = 300f;
    private const float MaxRunSpeed = 7f;

    private bool _onGround = true;
    private bool _jump = true;
    private bool _goingRight;

    private Rigidbody2D _playerRb;
    public Transform GroundCheck;
    
    // Use this for initialization
	void Start ()
	{
	    _anim = gameObject.GetComponent<Animator>();
	    _playerRb = gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
	{

	    _onGround = Physics2D.Linecast(gameObject.transform.position, GroundCheck.position, 1 << LayerMask.NameToLayer("Environment"));
        Debug.DrawLine(gameObject.transform.position, GroundCheck.position);
	    if (Input.GetKeyDown(KeyCode.Space) && _onGround)
	    {
	        _jump = true;
	    }
	}

    void FixedUpdate()
    {
        float axisPress = Input.GetAxisRaw("Horizontal");

        _anim.SetInteger("walkDirection",(int)axisPress);

        if (axisPress*_playerRb.velocity.x < MaxSpeed)
        {
            _playerRb.AddForce(Vector2.right * axisPress * MoveForce);
        }

        if (Math.Abs(_playerRb.velocity.x) > MaxSpeed)
        {
            _playerRb.velocity = new Vector2(Math.Sign(_playerRb.velocity.x) * MaxSpeed, _playerRb.velocity.y);
        }

        if (_jump)
        {
            _playerRb.AddForce(new Vector2(0, JumpSpeed));
            _jump = false;
        }
    }
    
}
