using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class TutorialVoice : MonoBehaviour
{

    private Text _descriptionBox;
    private string[] _narration = new string[3];
    private bool _instructing = true;
    

	void Start ()
	{
        _narration[0] = "We must leave";
        _narration[1] = "Steal the key, listen to my voice";	    
        _narration[2] = "Click on the guard, I will tell you his sin";

	    _descriptionBox = GetComponent<Text>();	    
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
                yield return StartCoroutine(WriteNarration(_narration[narrationCount]));
                narrationCount++;            
            }                        
        }
        
        TutorialCursorSwitch();


    }

    public IEnumerator WriteNarration(string narration)
    {
        _descriptionBox.text = "";
        int letterCount = 0;        

        while (letterCount < narration.Length)
        {
            _descriptionBox.text += narration[letterCount];
            letterCount++;
            yield return new WaitForSeconds(0.04f);
        }
        
        yield return new WaitForSeconds(2f);
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
