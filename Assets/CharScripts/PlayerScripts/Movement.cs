using System;
using UnityEngine;

public class Movement : MonoBehaviour {

    private const KeyCode GoLeft = KeyCode.A;
    private const KeyCode GoRight = KeyCode.D;
    
  
    private Animator _anim;
    private const float MoveForce = 150f;
    private float _speed = 1f;
    private const float JumpSpeed = 650f;
    private const float OriginalSpeed = 0.7f;
    private const float MaxRunSpeed = 2f;

    private bool _onGround = true;
    private bool _jump = true;
    private bool _goingRight = true;
    

    private Rigidbody2D _playerRb;
    public Transform GroundCheck;
    
    // Use this for initialization
	void Awake ()
	{
	    _anim = gameObject.GetComponent<Animator>();
	    _playerRb = gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
	{

	    _onGround = Physics2D.Linecast(gameObject.transform.position, GroundCheck.position, 1 << LayerMask.NameToLayer("Environment"));
        
	    if (Input.GetKeyDown(KeyCode.Space) && _onGround)
	    {
	        _jump = true;
	    }

	    if (Input.GetKeyDown(KeyCode.LeftShift))
	    {
	        _speed = MaxRunSpeed;
	    }
	    else if (Input.GetKeyUp(KeyCode.LeftShift))
	    {
	        _speed = OriginalSpeed;
	    }
	}

    void FixedUpdate()
    {
        float axisPress = Input.GetAxisRaw("Horizontal");

        _anim.SetFloat("Speed", Math.Abs(axisPress));

        if (axisPress*_playerRb.velocity.x < _speed)
        {
            _playerRb.AddForce(Vector2.right * axisPress * MoveForce);
        }

        if (Math.Abs(_playerRb.velocity.x) > _speed)
        {
            _playerRb.velocity = new Vector2(Math.Sign(_playerRb.velocity.x) * _speed, _playerRb.velocity.y);
        }

        if (axisPress < 0 && _goingRight)
        {            
            Flip();
        }
        else if (axisPress > 0 && !_goingRight)
        {
            Flip();
        }

        if (_jump)
        {
            _playerRb.AddForce(new Vector2(0, JumpSpeed));
            _jump = false;
        }
        
    }

    private void Flip()
    {
            _goingRight = !_goingRight;
            Vector3 localScale = gameObject.transform.localScale;
            localScale.x *= -1;
            gameObject.transform.localScale = localScale;

    }
    
}
