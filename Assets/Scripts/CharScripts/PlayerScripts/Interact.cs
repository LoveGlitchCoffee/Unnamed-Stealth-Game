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

    /*
     * If player interacts with an interact-able object, performs the object's purpose
     */
    void Update()
    {
        if (_canInteract && Input.GetKeyDown(engage))
        {            
            _interactObj.PerformPurpose(_inventory);
        }
    }
    
    /*
     * Allows interaction under circumstance that collider's object has an interactable script
     */
    void OnTriggerStay2D(Collider2D col) // find a more fficient way
    {
        if (col.tag == interactTag)
        {
            //Debug.Log(col.gameObject.GetComponent<IInteractive>());
            _canInteract = true;
            _interactObj = col.gameObject.GetComponent<IInteractive>();
        }
    }

    /*
     * Disables interaction if leaves object
     */
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == interactTag)
        {            
            _canInteract = false;
            _interactObj = null;
        }
    }

}
