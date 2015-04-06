using UnityEngine;
using System.Collections;

public class PlayerDetection : MonoBehaviour, IDetection
{

    private PursuePlayer _pursue;
    private GuardAI _regularAi;
    private GameObject _player;

   
    private const int LayerLiving = 11;
    private const int LayerEnvi = 10;
    private LayerMask _detectLiving;
    private LayerMask _detectEnvi;
    private LayerMask _detectLayerMask;

    public string PlayerTag = "Player";


	Ray2D _lineOfSight;
    private int _sightDistance;

	// Use this for initialization
    /**
     * Ray cast only collides with player or environment that can block vision
     */
	void Start () {

	    _detectLiving = 1 << LayerLiving;
	    _detectEnvi = 1 << LayerEnvi;

	    _detectLayerMask = _detectLiving | _detectEnvi;

	    _pursue = gameObject.GetComponent<PursuePlayer>();
	    _regularAi = gameObject.GetComponent<GuardAI>();
	    _player = GameObject.FindGameObjectWithTag(PlayerTag);

        _sightDistance = 10;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
			CheckLineOfSight();
	}

    /**
     * Calculates the direction to cast ray, should be direction towards player
     */
    private Vector2 CalculateDirection()
    {
        return new Vector2(_player.transform.position.x - gameObject.transform.position.x, _player.transform.position.y - gameObject.transform.position.y);
    }

    /**
     * Ray cast towards player if within vision cone
     * If ray cast hits player, i.e. no obstacles between them, then enable guard's pursue behaviour, turning off patrol behaviour
     */
    public void CheckLineOfSight()
	{
        _lineOfSight = new Ray2D(new Vector2(gameObject.transform.position.x + 0.9f, gameObject.transform.position.y + 0.1f), CalculateDirection());
        RaycastHit2D detectPlayer = Physics2D.Raycast(_lineOfSight.origin, _lineOfSight.direction, _sightDistance, _detectLayerMask); // distance is x distance

        if (detectPlayer.collider != null && detectPlayer.collider.tag == PlayerTag)
        {
            _pursue.enabled = true;
            _regularAi.enabled = false;
        }
	}
}
