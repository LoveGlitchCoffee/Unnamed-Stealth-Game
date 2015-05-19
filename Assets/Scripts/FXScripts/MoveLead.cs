using System.Collections;
using UnityEngine;

public class MoveLead : MonoBehaviour
{

    private float DistanceFromPlayerX = 3.5f;
    private const float DistanceFromPlayerY = 1f;
    private Transform _playerPosition;
    private Movement _playerMovement;
    private bool _showing = false;
    [HideInInspector] public bool ContactWall = false;
    private GameObject _cameraGroup;
    private Camera[] _cameras;
    private const int NoOfcameras = 4;

	void Start ()
	{
	    GameObject player = GameObject.FindGameObjectWithTag("Player");
	    _playerPosition = player.transform;
	    _playerMovement = player.GetComponent<Movement>();
	    _cameraGroup = GameObject.FindGameObjectWithTag("CameraGroup");

        _cameras = new Camera[NoOfcameras];

	    for (int i = 0; i < NoOfcameras; i++)
	    {
	        _cameras[i] = _cameraGroup.transform.GetChild(i).GetComponent<Camera>();
	    }
	}
	
	
	void Update () {

        if (!_showing && !ContactWall)
	        transform.position = Vector3.Lerp(transform.position, new Vector3(_playerPosition.position.x + DistanceFromPlayerX,_playerPosition.position.y + DistanceFromPlayerY),Time.deltaTime*6f);
	}

    public void SetDirection()
    {
        DistanceFromPlayerX *= -1f;
    }

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

    IEnumerator ZoomIn(Camera camera)
    {        
        float zoomedSize = 2.5f;
        

        while (camera.orthographicSize > zoomedSize)
        {
            camera.orthographicSize -= 0.1f;
            yield return null;
        }
    }

    IEnumerator ZoomOut(Camera camera)
    {
        float originalSize = 5f;

        while (camera.orthographicSize < originalSize)
        {
            camera.orthographicSize += 0.1f;
            yield return null;
        }
    }

    public void ZoomCamerasOut()
    {
        for (int i = 0; i < NoOfcameras; i++)
        {
            StartCoroutine(ZoomOut(_cameras[i]));
        }
    }

    public void IsShowing(bool showing)
    {        
        this._showing = showing;        
    }

    void OnCollisionEnter2D()
    {    
        ContactWall = true;
    }

}
