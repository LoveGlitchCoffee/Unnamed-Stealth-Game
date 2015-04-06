using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GuardAI : MonoBehaviour {

    private bool _goingLeft;
    private bool _outOfPatrolArea;

    private PlayerDetection _playerDetection;

    private const string PatrolAreaTag = "PatrolRegion";
    private const string PlayerTag = "Player";

    private const int WalkSpeed = 3;
    private float _waitTime = 0;

    private List<Vector2> _leftVision = new List<Vector2>();
    private List<Vector2> _rightVision = new List<Vector2>();
    private EdgeCollider2D _visionCone;

    /**
     * Guard Patrol pattern
     * Always in patrol region going left
     * Also defines vectors for vision cone
     */

    private void Start()
    {
        _goingLeft = true;
        _outOfPatrolArea = false;


        _playerDetection = gameObject.GetComponent<PlayerDetection>();
        _playerDetection.enabled = false;

        
        _leftVision.Add(new Vector2(-0.2f, 0.7f));
        _leftVision.Add(new Vector2(-5f, 2.5f));
        _leftVision.Add(new Vector2(-5f, -1f));
        _leftVision.Add(new Vector2(-0.2f, 0.6f));

        for (int i = 0; i < 4; i++)
        {
            var rightEye = Vector2.Scale(_leftVision.ElementAt(i), new Vector2(-1f, 1f));
            _rightVision.Add(rightEye);
        }

        _visionCone = GetComponent<EdgeCollider2D>();
    }

    // Update is called once per frame
    /**
     * Whilst in Patrol region, walks left and right
     * once out of patrol region, turns around and continues patrol
     */
	void Update () {

        if (!_outOfPatrolArea)
        {
            gameObject.rigidbody2D.AddForce(_goingLeft ? new Vector2(-WalkSpeed, 0) : new Vector2(WalkSpeed, 0));
        }
        else
        {
            TurnAround();
        }
	}

    /**
     * Waits for 5 seconds and turns around according to previous walking direction, then continue patrol
     * changes vision cone direction as well
     */
    private void TurnAround()
    {
        Wait();

        if (_waitTime >= 5)
        {
            _goingLeft = !_goingLeft;
            _waitTime = 0f;
            _outOfPatrolArea = false;
            SetVisionCone();
        }
    }


    /**
     * Wait 5 seconds
     */
    private void Wait()
    {
        if (_waitTime < 5f)
        {
            _waitTime += 1f * Time.deltaTime;
        }
    }

    /**
     * Sets the vision cone to the direction guard walking
     */
    private void SetVisionCone()
    {
        _visionCone.points = _goingLeft ? _leftVision.ToArray() : _rightVision.ToArray();
    }


    /**
     * If vision cone no longer detects player, turns of player Detection ray cast, should turn on prob calc
     */
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == PatrolAreaTag)
        {
            _outOfPatrolArea = true;
        }

        if (col.tag == "Player")
        {
            _playerDetection.enabled = false;
            gameObject.GetComponent<PursuePlayer>().enabled = false;
        }
        
    }

    
    /**
     * Whilst vision cone can 'see' player, turn on ray cast player detection
     */
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            _playerDetection.enabled = true;
        }
    }

   

}
