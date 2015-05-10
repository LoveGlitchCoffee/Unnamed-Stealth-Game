using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fading : MonoBehaviour
{

    private CanvasGroup _fader;
    private Animator _anim;
    private GameObject _restart;

    void Awake()
    {
        _fader = GetComponent<CanvasGroup>();
        _anim = GetComponent<Animator>();
        _restart = transform.GetChild(0).gameObject;
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
        yield return new WaitForSeconds(1.5f);
        _restart.SetActive(true);
    }

    public void ResetScene()
    {
        Application.LoadLevel(0);
    }
}
