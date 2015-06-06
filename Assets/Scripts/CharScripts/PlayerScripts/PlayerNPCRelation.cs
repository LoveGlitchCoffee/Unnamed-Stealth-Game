using UnityEngine;
using System.Collections;

public class PlayerNPCRelation : MonoBehaviour
{

    private const string GuardTag = "Guard";
    private Animator _anim;
    private Movement _playerMovement;
    private SceneHandler _sceneHandler;
    private Rigidbody2D _rbody;
    public bool CanDie = false;

    [HideInInspector] public bool dead { get; set; }

	// Use this for initialization
	void Start ()
	{
	    _anim = gameObject.GetComponent<Animator>();
	    _playerMovement = gameObject.GetComponent<Movement>();
	    _sceneHandler = GameObject.FindGameObjectWithTag("SceneFade").GetComponent<SceneHandler>();
	    _rbody = GetComponent<Rigidbody2D>();
	}

    /*
     * If touch guard, player dies
     */
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == GuardTag && CanDie)
        {                                   
            _anim.SetBool("dead",true);
            _playerMovement.enabled = false;
            _rbody.isKinematic = false;
            GetComponent<CircleCollider2D>().enabled = false;
            dead = true;
            gameObject.layer = 9;
            StartCoroutine(_sceneHandler.Restart());
        }
    }
   
}
