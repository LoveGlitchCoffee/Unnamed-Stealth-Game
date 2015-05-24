using System.Collections;
using UnityEngine;

public class MoveLead : MonoBehaviour
{

    private float DistanceFromPlayerX = 3.5f;
    private const float DistanceFromPlayerY = 1f;
    private Transform _playerPosition;    
    private bool _showing = false;
    [HideInInspector] public bool ContactWall = false;
    private GameObject _cameraGroup;
    private Camera[] _cameras;
    private const int NoOfcameras = 4;

    
	void Start ()
	{
	    GameObject player = GameObject.FindGameObjectWithTag("Player");
	    _playerPosition = player.transform;	    
	    _cameraGroup = GameObject.FindGameObjectWithTag("CameraGroup");

        _cameras = new Camera[NoOfcameras];

	    for (int i = 0; i < NoOfcameras; i++)
	    {
	        _cameras[i] = _cameraGroup.transform.GetChild(i).GetComponent<Camera>();
	    }
	}

    /*
     * Lerps all camera after player
     * Done by lerping a 'lead' to a certain position and just have camera follow on it
     */
    void Update () {

        if (!_showing && !ContactWall)
	        transform.position = Vector3.Lerp(transform.position, new Vector3(_playerPosition.position.x + DistanceFromPlayerX,_playerPosition.position.y + DistanceFromPlayerY),Time.deltaTime*6f);
	}


    public void SetDirection()
    {
        DistanceFromPlayerX *= -1f;
    }

    /*
     * Manually lerps camera to a certain position, use to show part of game to draw attention to, works with zooming
     */
    public IEnumerator MoveToPosition(GameObject objectToMoveTo)
    {
        for (int i = 0; i < NoOfcameras; i++)
        {
            StartCoroutine(ZoomIn(_cameras[i]));
        }
        
        while (objectToMoveTo != null && transform.position != new Vector3(objectToMoveTo.transform.position.x,objectToMoveTo.transform.position.y + 1f, objectToMoveTo.transform.position.z) && _showing)
        {            
            transform.position = Vector3.Lerp(transform.position, new Vector3(objectToMoveTo.transform.position.x,objectToMoveTo.transform.position.y + 1f, objectToMoveTo.transform.position.z), Time.deltaTime * 3f);
            yield return null;
        }
    }

    /*
     * Zooms all camera in, size decrease gradually
     */
    IEnumerator ZoomIn(Camera camera)
    {        
        float zoomedSize = 2.5f;
        

        while (camera.orthographicSize > zoomedSize)
        {
            camera.orthographicSize -= 0.1f;
            yield return null;
        }
    }

    /*
     * Zooms a specified camera out, size increase graudally
     */
    IEnumerator ZoomOut(Camera camera)
    {
        float originalSize = 5f;

        while (camera.orthographicSize < originalSize)
        {
            camera.orthographicSize += 0.1f;
            yield return null;
        }
    }

    /*
     * Zooms all cameras out
     */
    public void ZoomCamerasOut()
    {
        for (int i = 0; i < NoOfcameras; i++)
        {
            StartCoroutine(ZoomOut(_cameras[i]));
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
