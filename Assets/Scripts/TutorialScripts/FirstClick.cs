using UnityEngine;
using System.Collections;

public class FirstClick : MonoBehaviour
{

    private TutorialVoice tutorial;
    private GameObject _player;

	void Start ()
	{
	    tutorial = GameObject.FindGameObjectWithTag("DescriptionBox").GetComponent<TutorialVoice>();
        _player = GameObject.FindGameObjectWithTag("Player");
	}

    void OnMouseDown()
    {
        if (!enabled)
            return;

        tutorial.StopAllCoroutines();
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

        yield return StartCoroutine(tutorial.WriteNarration("Lucky you with a coin under your blanket, feed his greed"));
        yield return StartCoroutine(tutorial.WriteNarration("Go to your bed and pick up the coin"));
        _player.GetComponent<Movement>().enabled = true;
        yield return StartCoroutine(tutorial.WriteNarration("W,A,S,D to move, Space to jump, and hold Shift to run"));        
    }


}
