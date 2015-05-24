using UnityEngine;
using System.Collections;

public class Murophobia : IBehaviour {
    
    void Awake()
    {
        base.AssignComponents();
        WeaknessItem = "rat";
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        base.ReactToWeakness(col);
    }
    
    /*
     * Guard only resumes patrolling if the rat is out of sight
     */
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == LootTag && col.GetComponent<Identifer>().ReturnIdentity() == WeaknessItem)
        {
            StartCoroutine(ResumeCoroutines());            
        }
    }
    
    /*
     * This is behavour toward rats
     * Guard takes short moment to 'realise' there is a rat in sight
     * then all movement and detection is disabled indefinitely until rat is removed from detection
     */
    protected override IEnumerator ActivateBehaviour(GameObject item)
    {
        const float realiseTime = 3f;
        float timer = 0f;

        while (timer < realiseTime)
        {
            timer += 1f;
            yield return null;
        }

        _soundHandler.PlaySound("Confused", 0.5f);               
    }

    public override string ReturnBehaviourDescription()
    {
        return "swarmed by rats when he was a child, scarred him for life";
    }
}
