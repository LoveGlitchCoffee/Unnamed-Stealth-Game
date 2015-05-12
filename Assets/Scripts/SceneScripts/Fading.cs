using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
        yield return new WaitForSeconds(1f);
        FadeOut();
        StartCoroutine(_writer.WriteNarration("No wonder you were caught in the first place"));
        yield return new WaitForSeconds(1.5f);
        _restart.SetActive(true);
    }

    public void ResetScene()
    {
        Application.LoadLevel(0);
    }
}
