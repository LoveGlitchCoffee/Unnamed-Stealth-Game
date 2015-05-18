using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DescriptionWriter : MonoBehaviour
{

    private Text _descriptionBox;
    private AudioSource _scribbles;

    void Awake()
    {
        _descriptionBox = transform.GetChild(0).GetComponent<Text>();
        _scribbles = GetComponent<AudioSource>();
    }

    public IEnumerator WriteNarration(string narration)
    {
        StopAllCoroutines();
        _descriptionBox.text = "";
        int letterCount = 0;        
        _scribbles.Play();

        while (letterCount < narration.Length)
        {            
            _descriptionBox.text += narration[letterCount];
            letterCount++;            
            yield return new WaitForSeconds(0.04f);
        }
        
        yield return new WaitForSeconds(2f);
    }
}
