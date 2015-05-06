using UnityEngine;
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
        if (!enabled)
            return;

        _descriptionBox.text = _behaviour.ReturnBehaviourDescription();
    }
}
