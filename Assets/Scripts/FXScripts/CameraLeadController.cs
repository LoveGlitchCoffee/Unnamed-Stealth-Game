using System.Collections;
using UnityEngine;

public class CameraLeadController : MonoBehaviour
{

    private float DistanceFromPlayerX = 3.5f;
    private const float DistanceFromPlayerY = 1f;
    private Transform _playerPosition;    
    private bool _showing = false;
    [HideInInspector] public bool ContactWall = false;    
    private CameraController _cameraFX;

    
	void Start ()
	{
	    GameObject player = GameObject.FindGameObjectWithTag("Player");
	    _playerPosition = player.transform;
	    _cameraFX = GameObject.FindGameObjectWithTag("CameraGroup").GetComponent<CameraController>();
	}

    /*
     * Lerps all camera after player
     * Done by lerping a 'lead' to a certain position and just have camera follow on it
     */
    void Update ()
    {

        if (!_showing && !ContactWall)
            transform.position = new Vector3(_playerPosition.position.x + DistanceFromPlayerX,
                _playerPosition.position.y + DistanceFromPlayerY);
    }

    /*
     * Set direction to which player is facing
     * controls which way camera lerps
     */
    public void SetDirection()
    {
        DistanceFromPlayerX *= -1f;
    }

    /*
     * Manually lerp camera lead to certain position, camera follow,
     * use to show part of game to draw attention to, works with zooming
     */
    public IEnumerator MoveToPosition(GameObject objectToMoveTo)
    {
        _cameraFX.ZoomCameras(true);
        
        while (objectToMoveTo != null && transform.position != new Vector3(objectToMoveTo.transform.position.x,objectToMoveTo.transform.position.y + 1f, objectToMoveTo.transform.position.z) && _showing)
        {            
            transform.position = Vector3.Lerp(transform.position, new Vector3(objectToMoveTo.transform.position.x,objectToMoveTo.transform.position.y + 1f, objectToMoveTo.transform.position.z), Time.deltaTime * 3f);
            yield return null;
        }
    }

   

    /*
     * Indicates if game is showing something of interest
     * stops regular follow-player lerp
     */
    public void IsShowing(bool showing)
    {        
        this._showing = showing;        
    }

    /*
     * If collide to wall, will stop lerping
     */
    void OnCollisionEnter2D()
    {    
        ContactWall = true;
    }

}
