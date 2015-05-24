using System.Collections;
using UnityEngine;

public class DragDropTutorial : MonoBehaviour
{

    private InventoryLogic _inventory;
    private Tool _firstCoin;
    private DescriptionWriter _writer;
    private TutorialVoice _tutorial;

    private GameObject _key;
    private CommonTutorialFunctions _tutFunc;
    private CameraLeadController _leadMover;
    private CameraController _cameraFx;
    private FirstClick _firstClick;

	void Start ()
	{
	    _inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryLogic>();
	    _firstCoin = _inventory.ReturnToolDb().ToolDatabase[1];
	    _writer = GetComponentInParent<DescriptionWriter>();
	    _tutorial = GetComponent<TutorialVoice>();
	    _leadMover = GameObject.FindGameObjectWithTag("CameraLead").GetComponent<CameraLeadController>();
	    _firstClick = GameObject.FindGameObjectWithTag("Guard").GetComponent<FirstClick>();
	    _cameraFx = GameObject.FindGameObjectWithTag("CameraGroup").GetComponent<CameraController>();

        _tutFunc = new CommonTutorialFunctions();
	    _key = _tutFunc.GetLootables("key");
	}

    void Update()
    {
        if (_inventory.IndexOf(_firstCoin) != -1)
        {
            StartCoroutine(DragAndDropTutorial());
            enabled = false;
        }
        
    }

    private IEnumerator DragAndDropTutorial()
    {
        _firstClick.StopAllCoroutines();        
        StartCoroutine(_writer.WriteNarration(""));
        _leadMover.IsShowing(true);
        _key.transform.GetChild(0).GetComponent<Light>().enabled = true;

        StartCoroutine(_writer.WriteNarration("Now get the key, don't let the guard see you"));        
        StartCoroutine(_leadMover.MoveToPosition(_key));
        
        yield return new WaitForSeconds(3f);
        _cameraFx.ZoomCameras(false);
        StartCoroutine(_writer.WriteNarration("Drag and Drop the coin near the guard when you need to"));

        yield return new WaitForSeconds(1f);
        
        _leadMover.StopAllCoroutines();
        _leadMover.IsShowing(false);                              
    }
}
