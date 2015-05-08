using UnityEngine;

public class InteractTutorial : MonoBehaviour {


    private TutorialVoice _tutorial;
    public string Tutorial;

    void Awake()
    {
        _tutorial = GameObject.FindGameObjectWithTag("DescriptionBox").GetComponent<TutorialVoice>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player" && enabled)
        {
            StartCoroutine(_tutorial.WriteNarration(Tutorial));
            enabled = false;
        }
    }
}
