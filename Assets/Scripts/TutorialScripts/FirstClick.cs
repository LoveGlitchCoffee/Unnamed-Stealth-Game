using UnityEngine;
using System.Collections;

public class FirstClick : MonoBehaviour
{

    private TutorialVoice _tutorial;
    private DescriptionWriter _writer;
    private GameObject _player;
    private GameObject _coin;
    private CommonTutorialFunctions _tutFunc;

	void Start ()
	{
	    GameObject _description = GameObject.FindGameObjectWithTag("DescriptionBox");
        _tutorial = _description.GetComponent<TutorialVoice>();
	    _writer = _description.GetComponentInParent<DescriptionWriter>();

        _player = GameObject.FindGameObjectWithTag("Player");	 

        _tutFunc = new CommonTutorialFunctions();
	    _coin = _tutFunc.GetLootables("coin");
	}

    void OnMouseDown()
    {
        if (!enabled)
            return;

        //_writer.StopAllCoroutines();
        StartCoroutine(WaitToRead());                
        enabled = false;
    }

    private IEnumerator WaitToRead()
    {
        float count = 0f;
        float maxTime = 2.5f;

        while (count < maxTime)
        {
            count += 0.01f;
            yield return null;
        }

        _tutorial.TutorialCursorSwitch();

        yield return StartCoroutine(_writer.WriteNarration("Lucky you with a coin under your blanket, feed his greed"));
        _coin.transform.GetChild(0).GetComponent<Light>().enabled = true;
        yield return StartCoroutine(_writer.WriteNarration("Go to your bed and pick up the coin"));        
        yield return StartCoroutine(_writer.WriteNarration("W,A,S,D to move, Space to jump, and hold Shift to run"));
        _player.GetComponent<Movement>().enabled = true;
    }


}
