using UnityEngine;
using System.Collections;

public class FirstClick : MonoBehaviour
{

    private TutorialVoice _tutorial;
    private DescriptionWriter _writer;
    private GameObject _player;
    private GameObject _coin;
    private CommonTutorialFunctions _tutFunc;
    private MoveLead _leadMover;    

	void Start ()
	{
	    GameObject _description = GameObject.FindGameObjectWithTag("DescriptionBox");
        _tutorial = _description.GetComponent<TutorialVoice>();
	    _writer = _description.GetComponentInParent<DescriptionWriter>();
	    _leadMover = GameObject.FindGameObjectWithTag("CameraLead").GetComponent<MoveLead>();

        _player = GameObject.FindGameObjectWithTag("Player");	 

        _tutFunc = new CommonTutorialFunctions();
	    _coin = _tutFunc.GetLootables("coin");
	}

    void OnMouseDown()
    {
        if (!enabled)
            return;
        
        StartCoroutine(WaitToRead());                
        enabled = false;
    }

    private IEnumerator WaitToRead()
    {
                
        _tutorial.StopAllCoroutines();
        yield return new WaitForSeconds(2.5f);

        /*_tutorial.TutorialCursorSwitch();

        yield return StartCoroutine(_writer.WriteNarration("Lucky you with a coin under your blanket, feed his greed"));        
        yield return StartCoroutine(_writer.WriteNarration("Go to your bed and pick up the coin"));        
        yield return StartCoroutine(_writer.WriteNarration("W,A,S,D to move, Space to jump, and hold Shift to run"));
        _player.GetComponent<Movement>().enabled = true;*/

        _coin.transform.GetChild(0).GetComponent<Light>().enabled = true;
        _coin.GetComponent<CircleCollider2D>().enabled = true;
        _player.GetComponent<Loot>().enabled = true;
        _player.GetComponent<Interact>().enabled = true;
        yield return StartCoroutine(_leadMover.MoveToPosition(_coin));

        yield return new WaitForSeconds(0.5f);
        _leadMover.IsShowing(false);
    }


}
