using UnityEngine;
using System.Collections;

public class ShowLever : MonoBehaviour
{

    private CameraLeadController _leadMover;
    private CameraController _cameraFx;

	
	void Awake ()
	{
	    _leadMover = GameObject.FindGameObjectWithTag("CameraLead").GetComponent<CameraLeadController>();
        _cameraFx = GameObject.FindGameObjectWithTag("CameraGroup").GetComponent<CameraController>();
	}

    void Start()
    {        
        StartCoroutine(MoveLeadToLever());
    }

    private IEnumerator MoveLeadToLever()
    {
        yield return new WaitForSeconds(2f);

        _leadMover.IsShowing(true);
        StartCoroutine(_leadMover.MoveToPosition(gameObject));
        yield return new WaitForSeconds(3f);

        _cameraFx.ZoomCameras(false);
        _leadMover.IsShowing(false);        

    }

	
}
