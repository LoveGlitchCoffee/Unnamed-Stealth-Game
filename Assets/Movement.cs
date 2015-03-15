using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    KeyCode goLeft = KeyCode.A;
    KeyCode goRight = KeyCode.D;
    int speed = 3;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (Input.GetKey(goLeft))
        {
            rigidbody2D.AddForce(new Vector2(-speed, 0));
        }
        else if (Input.GetKey(goRight))
        {
            rigidbody2D.AddForce(new Vector2(speed, 0));
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigidbody2D.AddForce(new Vector2(0, 230));
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {

        }
	}
}
