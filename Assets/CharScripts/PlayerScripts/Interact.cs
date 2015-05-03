using System.Runtime.InteropServices;
using UnityEngine;

public class Interact : MonoBehaviour
{

    private IInteractive _interactObj;
    private string interactTag = "Interactive";
    private bool _canInteract;
    private const KeyCode engage = KeyCode.E;
    private InventoryLogic _inventory;

    void Awake()
    {
        _inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryLogic>();
    }

    void Update()
    {
        if (_canInteract && Input.GetKeyDown(engage))
        {
            Debug.Log("interact");
            _interactObj.PerformPurpose(_inventory);
        }

    }
    

    void OnTriggerStay2D(Collider2D col) // find a more fficient way
    {
        if (col.tag == interactTag)
        {
            //Debug.Log(col.gameObject.GetComponent<IInteractive>());
            _canInteract = true;
            _interactObj = col.gameObject.GetComponent<IInteractive>();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == interactTag)
        {            
            _canInteract = false;
            _interactObj = null;
        }
    }

}
