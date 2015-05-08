using System.Collections;
using UnityEngine;

public class DragDropTutorial : MonoBehaviour
{

    private InventoryLogic _inventory;
    private Tool _firstCoin;
    private TutorialVoice _tutorial;

	void Start ()
	{
	    _inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryLogic>();
	    _firstCoin = _inventory.ReturnToolDb().ToolDatabase[1];
	    _tutorial = GetComponent<TutorialVoice>();
	}

    void Update()
    {
        if (_inventory.PlayerTools.Contains(_firstCoin))
        {
            StartCoroutine(DragAndDropTutorial());
            enabled = false;
        }
        
    }

    private IEnumerator DragAndDropTutorial()
    {
        _tutorial.StopAllCoroutines();
        
        _tutorial.TutorialCursorSwitch();

        yield return StartCoroutine(_tutorial.WriteNarration("You have it? Good"));
        yield return StartCoroutine(_tutorial.WriteNarration("Now get the key, don't let the guard see you"));
        yield return StartCoroutine(_tutorial.WriteNarration("Drag and Drop the coin near the guard when you need to"));
        
        _tutorial.TutorialCursorSwitch();
    }
}
