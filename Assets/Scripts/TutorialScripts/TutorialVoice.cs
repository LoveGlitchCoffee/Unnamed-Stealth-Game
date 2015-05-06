using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialVoice : MonoBehaviour
{

    private Text _descriptionBox;
    private string _narrative1 = "We must leave";
    private string _narrative2 = "Steal the key, listen to my voice";
    private string _instructions1 = "Click on the guard, I will tell you his sin";
    private bool _instructing = true;

	void Start ()
	{
	    _descriptionBox = GetComponent<Text>();
	    StartCoroutine(WriteTutorial());
	}

    private IEnumerator WriteTutorial()
    {
        while (_instructing)
        {
                
        }



        yield return null;
    }
}
