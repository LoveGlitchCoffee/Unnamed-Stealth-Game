using System.Collections;
using UnityEngine;

public class Fading : MonoBehaviour
{

    private CanvasGroup _fader;
    private Animator _anim;
    private GameObject _restart;
    private DescriptionWriter _writer;

    void Awake()
    {
        _fader = GetComponent<CanvasGroup>();
        _anim = GetComponent<Animator>();
        _restart = transform.GetChild(0).gameObject;
        _writer = GameObject.FindGameObjectWithTag("DescriptionBox").GetComponentInParent<DescriptionWriter>();
    }

    public void FadeOut()
    {
        _anim.SetBool("EnterScene",false);        
    }

    public void FadeIn()
    {
        _anim.SetBool("EnterScene",true);
    }

    public IEnumerator Restart()
    {                
        yield return StartCoroutine(_writer.WriteNarration("No wonder you were caught in the first place"));
        yield return new WaitForSeconds(1.5f);
        FadeOut();
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        _restart.SetActive(true);
    }

    public void ResetScene()
    {        
        int currentLevel = Application.loadedLevel;
        Application.LoadLevel(currentLevel);
        FadeIn();
    }

    public void ToNextLevel()
    {
        StartCoroutine(FadeToNextLevel());
    }

    
    IEnumerator FadeToNextLevel()
    {
        int currentLevel = Application.loadedLevel;

        float fadeTime = 1f;
        float currentTime = 0f;
        
        while (currentTime < fadeTime)
        {                    
            currentTime += 0.1f;
            yield return null;
        }

        Application.LoadLevel(currentLevel + 1);
    }
    
}
