using UnityEngine;
using UnityEngine.UI;

public class GuardDescriptor : MonoBehaviour {

    private Text _descriptionBox;
    private IBehaviour _behaviour;
    private GuardSoundHandler _soundHandler;

    void Start()
    {
        _descriptionBox = GameObject.FindGameObjectWithTag("DescriptionBox").GetComponent<Text>(); 
        _behaviour = GetComponent<IBehaviour>();
        _soundHandler = GetComponent<GuardSoundHandler>();
    }

    void OnMouseDown()
    {
        if (!enabled)
            return;

        _descriptionBox.text = _behaviour.ReturnBehaviourDescription();
        _soundHandler.PlaySound("Select", 0.8f);
    }
}
