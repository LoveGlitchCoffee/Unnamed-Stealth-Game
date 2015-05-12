using UnityEngine;

public class InteractTutorial : MonoBehaviour {


    private TutorialVoice _tutorial;
    private DescriptionWriter _writer;
    public string Tutorial;

    void Awake()
    {
        GameObject _description = GameObject.FindGameObjectWithTag("DescriptionBox");
        _tutorial = _description.GetComponent<TutorialVoice>();
        _writer = _description.GetComponentInParent<DescriptionWriter>();        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player" && enabled)
        {
            StartCoroutine(_writer.WriteNarration(Tutorial));
            enabled = false;
        }        
    }
}
