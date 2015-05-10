using UnityEngine;
using System.Collections;

public class FirstClick : MonoBehaviour
{

    private TutorialVoice _tutorial;
    private GameObject _player;
    private GameObject _coin;
    private CommonTutorialFunctions _tutFunc;

	void Start ()
	{
	    _tutorial = GameObject.FindGameObjectWithTag("DescriptionBox").GetComponent<TutorialVoice>();
        _player = GameObject.FindGameObjectWithTag("Player");	 

        _tutFunc = new CommonTutorialFunctions();
	    _coin = _tutFunc.GetLootables("coin");
	}

    void OnMouseDown()
    {
        if (!enabled)
            return;

        _tutorial.StopAllCoroutines();
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

        yield return StartCoroutine(_tutorial.WriteNarration("Lucky you with a coin under your blanket, feed his greed"));
        _coin.transform.GetChild(0).GetComponent<Light>().enabled = true;
        yield return StartCoroutine(_tutorial.WriteNarration("Go to your bed and pick up the coin"));
        _player.GetComponent<Movement>().enabled = true;
        yield return StartCoroutine(_tutorial.WriteNarration("W,A,S,D to move, Space to jump, and hold Shift to run"));        
    }


}
