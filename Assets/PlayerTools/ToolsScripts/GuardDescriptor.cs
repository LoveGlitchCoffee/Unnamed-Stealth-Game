using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GuardDescriptor : MonoBehaviour, IPointerEnterHandler {

    private Text _descriptionBox;
    private IBehaviour _behaviour;

    void Start()
    {
        _descriptionBox = GameObject.FindGameObjectWithTag("DescriptionBox").GetComponent<Text>();
        _behaviour = GetComponent<IBehaviour>();
        Debug.Log("descriptor workin");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("in the zone");
        _descriptionBox.text = _behaviour.ReturnBehaviourDescription();
    }
}
