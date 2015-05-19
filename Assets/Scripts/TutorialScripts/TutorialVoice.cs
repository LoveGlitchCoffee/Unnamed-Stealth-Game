using System.Collections;
using UnityEngine;

public class TutorialVoice : MonoBehaviour
{
    
    private string[] _narration = new string[3];
    private bool _instructing = true;
    private DescriptionWriter _writer;
    private GameObject _guard;
    private MoveLead _leadMover;

	void Start ()
	{
	    _guard = GameObject.FindGameObjectWithTag("Guard");
	    _leadMover = GameObject.FindGameObjectWithTag("CameraLead").GetComponent<MoveLead>();
        _writer = GetComponentInParent<DescriptionWriter>();

	    //StartCoroutine(WriteTutorial());	    
	    StartCoroutine(ShowGuard());
	}


    private IEnumerator ShowGuard()
    {        
        yield return new WaitForSeconds(0f); //initiall 3 sconds, for test purpose

        _leadMover.IsShowing(true);
        StartCoroutine(_leadMover.MoveToPosition(_guard));
        StartCoroutine(_writer.WriteNarration("Click on the guard, I will tell you his sin"));
    }


    /*private IEnumerator WriteTutorial()
    {
        int narrationCount = 0;
        
        TutorialCursorSwitch();

        while (_instructing)
        {
            if (narrationCount == _narration.Length)
            {
                _instructing = false;
            }
            else
            {                     
                yield return StartCoroutine(_writer.WriteNarration(_narration[narrationCount]));
                narrationCount++;            
            }                        
        }
        
        TutorialCursorSwitch();
    }*/

    

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
