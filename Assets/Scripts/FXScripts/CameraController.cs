using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private Transform _camerasPosition;
    private GameObject _cameraLead;
    
    private Camera[] _cameras;
    private const int NoOfcameras = 4;

    void Awake()
    {
        _camerasPosition = gameObject.transform;
        _cameraLead = GameObject.FindGameObjectWithTag("CameraLead");

        _cameras = new Camera[NoOfcameras];

	    for (int i = 0; i < NoOfcameras; i++)
	    {
	        _cameras[i] = transform.GetChild(i).GetComponent<Camera>();
        }
    }
		
    /*
     * lerps camera to lead
     */
	void Update ()
	{
	    _camerasPosition.position = Vector3.Lerp(_camerasPosition.position, _cameraLead.transform.position, Time.deltaTime*6f);
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
     * Zooms all cameras in or out depending on request
     */
    public void ZoomCameras(bool zoomIn)
    {
        for (int i = 0; i < NoOfcameras; i++)
        {
            StartCoroutine(zoomIn ? ZoomIn(_cameras[i]) : ZoomOut(_cameras[i]));
        }
    }
}
