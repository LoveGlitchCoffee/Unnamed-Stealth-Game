using System.Collections;
using UnityEngine;

public class TutorialVoice : MonoBehaviour
{
    
    private string[] _narration = new string[3];
    private bool _instructing = true;
    private DescriptionWriter _writer;
    private GameObject _guard;
    private CameraLeadController _leadMover;

	void Start ()
	{
	    _guard = GameObject.FindGameObjectWithTag("Guard");
	    _leadMover = GameObject.FindGameObjectWithTag("CameraLead").GetComponent<CameraLeadController>();
        _writer = GetComponentInParent<DescriptionWriter>();

	    //StartCoroutine(WriteTutorial());	    
	    StartCoroutine(ShowGuard());
	}


    private IEnumerator ShowGuard()
    {        
        yield return new WaitForSeconds(3f);

        _leadMover.IsShowing(true);
        StartCoroutine(_leadMover.MoveToPosition(_guard));
        StartCoroutine(_writer.WriteNarration("Click on the guard, I will tell you his sin"));
    }

    public void TutorialCursorSwitch()
    {
        if (Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;    
        }
        else
        {            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }        
    }

   
}
