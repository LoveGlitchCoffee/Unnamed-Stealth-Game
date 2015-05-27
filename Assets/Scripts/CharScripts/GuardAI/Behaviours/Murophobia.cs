using UnityEngine;
using System.Collections;

public class Murophobia : IBehaviour
{

    private RatCollecting _ratCollect;

    void Awake()
    {
        base.AssignComponents();
        WeaknessItem = "rat";
        _ratCollect = GetComponent<RatCollecting>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {        
        base.ReactToWeakness(col);
    }
    

    
    /*
     * This is behavour toward rats
     * Guard takes short moment to 'realise' there is a rat in sight
     * then all movement and detection is disabled indefinitely until rat is removed from detection
     */
    protected override IEnumerator ActivateBehaviour(GameObject item, Node nodeItemIn)
    {
        const float realiseTime = 3f;
        float timer = 0f;

        while (timer < realiseTime)
        {
            timer += 1f;
            yield return null;
        }
        

        GetComponentInParent<Rigidbody2D>().AddForce(new Vector2(0, 200f));
        _soundHandler.PlaySound("Scared", 0.5f);
        _spriteHandler.PlayAnimation("Scared");

        timer = 0f;
        float jumpTIme = 2f;

        while (timer < jumpTIme)
        {
            timer += 0.3f;
            yield return null;
        }

        yield return StartCoroutine(_ratCollect.RatGuardCollect(item,nodeItemIn));

        _spriteHandler.StopAnimation("Scared");
        StartCoroutine(ResumeCoroutines());      
        
    }

    public override string ReturnBehaviourDescription()
    {
        return "swarmed by rats when he was a child, scarred him for life";
    }
}
