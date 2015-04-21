using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private Tool _itemBeingDragged;
    private Vector3 _startPosition;
    public GameObject InteractiveObj;
    private GameObject _physicalSpawn;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _itemBeingDragged = gameObject.transform.parent.gameObject.GetComponent<ItemUI>().ReturnToolInSlot();
        _startPosition = transform.position;

        _physicalSpawn = Instantiate(InteractiveObj);
        _physicalSpawn.GetComponent<Identifer>().SetIdentity(_itemBeingDragged);
        _physicalSpawn.transform.position.Set(_startPosition.x, _startPosition.y, _startPosition.z);

    }

    public void OnDrag(PointerEventData eventData)
    {
        _physicalSpawn.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    
    }
}
