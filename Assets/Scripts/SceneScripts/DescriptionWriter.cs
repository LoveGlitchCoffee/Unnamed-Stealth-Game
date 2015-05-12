using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DescriptionWriter : MonoBehaviour
{

    private Text _descriptionBox;

    void Awake()
    {
        _descriptionBox = transform.GetChild(0).GetComponent<Text>();
    }

    public IEnumerator WriteNarration(string narration)
    {
        _descriptionBox.text = "";
        int letterCount = 0;        

        while (letterCount < narration.Length)
        {            
            _descriptionBox.text += narration[letterCount];
            letterCount++;            
            yield return new WaitForSeconds(0.04f);
        }
        
        yield return new WaitForSeconds(2f);
    }
}
