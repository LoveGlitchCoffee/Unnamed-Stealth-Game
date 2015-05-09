using UnityEngine;
using System.Collections;

public class FirstClick : MonoBehaviour
{

    private TutorialVoice tutorial;
    private GameObject _player;
    private GameObject coin;
    private GameObject[] lootables;
    private Identifer[] identities;

	void Start ()
	{
	    tutorial = GameObject.FindGameObjectWithTag("DescriptionBox").GetComponent<TutorialVoice>();
        _player = GameObject.FindGameObjectWithTag("Player");

	    lootables = GameObject.FindGameObjectsWithTag("Lootable");
        identities = new Identifer[lootables.Length];
        
	    for (int i = 0; i < lootables.Length; i++)
	    {
	        identities[i] = lootables[i].GetComponent<Identifer>();

	        if (identities[i].ReturnIdentity() == "coin")
	            coin = identities[i].gameObject;
	    }

        
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
        coin.transform.GetChild(0).GetComponent<Light>().enabled = true;
        yield return StartCoroutine(tutorial.WriteNarration("Go to your bed and pick up the coin"));
        _player.GetComponent<Movement>().enabled = true;
        yield return StartCoroutine(tutorial.WriteNarration("W,A,S,D to move, Space to jump, and hold Shift to run"));        
    }


}
