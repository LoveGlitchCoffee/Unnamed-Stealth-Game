using System.Collections;
using UnityEngine;

public class ShowGateOpened : MonoBehaviour {

    private CameraLeadController _leadMover;
    private CameraController _cameraFx;

    void Awake()
    {
        _leadMover = GameObject.FindGameObjectWithTag("CameraLead").GetComponent<CameraLeadController>();
        _cameraFx = GameObject.FindGameObjectWithTag("CameraGroup").GetComponent<CameraController>();
    }

    public IEnumerator MoveCamera(GameObject gate)
    {
        yield return new WaitForSeconds(1.2f);

        _leadMover.IsShowing(true);
        StartCoroutine(_leadMover.MoveToPosition(gate));
        yield return new WaitForSeconds(3f);

        _cameraFx.ZoomCameras(false);
        _leadMover.IsShowing(false);       
    }
}
