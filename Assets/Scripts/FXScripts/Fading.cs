using System.Collections;
using UnityEngine;

public class Fading : MonoBehaviour
{

    
    private Animator _anim;    

    void Awake()
    {
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

    public IEnumerator FadeToNextLevel(float numberOfSeconds,int currentLevel, GameObject player, GameObject cameraLead)
    {
        yield return new WaitForSeconds(numberOfSeconds);
        player.GetComponent<PlayerMapRelation>().SetNodeManually(null);        

        Application.LoadLevel(currentLevel + 1);
        player.GetComponent<PlayerMapRelation>().GetNewMap();        
        player.transform.position = new Vector3(0,0);
        

        cameraLead.GetComponent<MoveLead>().ContactWall = false;
        cameraLead.transform.position = new Vector3(3.5f, 0);
        FadeIn();        
    }
                       
}
