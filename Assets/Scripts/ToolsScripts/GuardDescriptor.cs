using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GuardDescriptor : MonoBehaviour {

    private Text _descriptionBox;
    private IBehaviour _behaviour;

    void Start()
    {
        _descriptionBox = GameObject.FindGameObjectWithTag("DescriptionBox").GetComponent<Text>();
        _behaviour = GetComponent<IBehaviour>();        
    }

    void OnMouseDown()
    {
        _descriptionBox.text = _behaviour.ReturnBehaviourDescription();
    }
}
