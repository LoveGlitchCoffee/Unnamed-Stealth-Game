using System.Collections;
using UnityEngine;

public class Fading : MonoBehaviour
{

    private CanvasGroup _fader;
    private Animator _anim;

    void Awake()
    {
        _fader = GetComponent<CanvasGroup>();
        _anim = GetComponent<Animator>();
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
        FadeOut();
        yield return new WaitForSeconds(1.5f);

    }
}
