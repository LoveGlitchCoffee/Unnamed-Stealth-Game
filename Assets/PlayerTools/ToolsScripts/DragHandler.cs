using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private Tool _itemBeingDragged;
    private Vector3 _startPosition;
    public GameObject InteractiveObj;
    private GameObject _physicalSpawn;
    private InventoryLogic _inventory;
    private GameObject _slotIn;

    private Vector3 screenPoint, offset;

    void Start()
    {
        _slotIn = transform.parent.gameObject;
        _inventory = _slotIn.transform.parent.GetComponent<InventoryLogic>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _itemBeingDragged = gameObject.transform.parent.gameObject.GetComponent<ItemUI>().ReturnToolInSlot();

        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        /*offset = (gameObject.transform.position -
                  Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z))); */ //for 3d

        _physicalSpawn = Instantiate(InteractiveObj);
        _physicalSpawn.GetComponent<Identifer>().SetIdentity(_itemBeingDragged);
        _physicalSpawn.layer = 14;        
        _physicalSpawn.transform.position =
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 itemPosition =
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z)); 
        _physicalSpawn.transform.position = itemPosition;
        //Debug.Log(_physicalSpawn.transform.position);
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        _physicalSpawn.GetComponent<BoxCollider2D>().enabled = true;
        _physicalSpawn.GetComponent<CircleCollider2D>().enabled = true;
        _physicalSpawn.GetComponent<Rigidbody2D>().isKinematic = false;
        int indexOfDestoryed = _slotIn.GetComponent<ItemUI>().ItemSlotNumber;
        _inventory.RemoveItem(indexOfDestoryed);
    }
}
