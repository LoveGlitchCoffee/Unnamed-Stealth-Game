using System.Collections;
using UnityEngine;

public class TutorialVoice : MonoBehaviour
{
    
    private string[] _narration = new string[3];
    private bool _instructing = true;
    private DescriptionWriter _writer;

	void Start ()
	{
        _narration[0] = "We must leave";
        _narration[1] = "Steal the key, listen to my voice";	    
        _narration[2] = "Click on the guard, I will tell you his sin";

	    _writer = GetComponentInParent<DescriptionWriter>();

	    StartCoroutine(WriteTutorial());
	}

    private IEnumerator WriteTutorial()
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
