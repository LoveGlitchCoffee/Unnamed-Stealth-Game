using System.Collections;
using UnityEngine;

public class CoinLover : IBehaviour
{
    
    void Awake()
    {
        base.AssignComponents();
        WeaknessItem = "coin";
    }

    void OnTriggerStay2D(Collider2D col)
    {
        base.ReactToWeakness(col);
    }

    /*
     * The behaviour is reaction to coin
     * Guard navigates to coin's position
     * All movement and detection is disabled for a set period of time
     */
    override protected IEnumerator ActivateBehaviour(GameObject item)
    {
        const float maxTime = 7f;
        float timer = 0f;
                                
        Destroy(item);
        _soundHandler.PlaySound("Confused", 0.5f);
        _spriteHandler.PlayAnimation("idle");
        
        while (timer < maxTime)
        {            
            timer+= 0.03f;
            yield return null;
        }

        _spriteHandler.StopAnimation("idle");

        StartCoroutine(ResumeCoroutines());
    }


    public override string ReturnBehaviourDescription()
    {
        return "Greedy and love coins, if he sees one, he'll forget the world just to look at it";
    }
}
