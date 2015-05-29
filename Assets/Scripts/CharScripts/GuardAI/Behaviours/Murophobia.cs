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
     * all movement and detection is disabled indefinitely until rat is collected by owning guard
     */
    protected override IEnumerator ActivateBehaviour(GameObject item, Node nodeItemIn, RaycastHit2D detectItem)
    {
        GetComponent<PolygonCollider2D>().enabled = false;


        _soundHandler.PlaySound("Scared", 0.5f);
        _spriteHandler.PlayAnimation("Scared");

        StartCoroutine(ScaredJumpPhysics());
        
        yield return StartCoroutine(_ratCollect.RatGuardCollect(item,detectItem));
        
        _spriteHandler.StopAnimation("Scared");
        StartCoroutine(ResumeCoroutines());
        GetComponent<PolygonCollider2D>().enabled = true;

    }

    IEnumerator ScaredJumpPhysics()
    {
        float timer = 0f;
        float jumpTime = 3f;
        float fallSpeed = 0.5f;
        Vector3 initialPosition = transform.parent.position;
        Vector3 goalHeight = new Vector3(initialPosition.x,initialPosition.y + 2f);

        while (timer < jumpTime)
        {
            transform.parent.position = Vector3.Lerp(transform.parent.position, goalHeight, (timer/jumpTime));
            timer += 0.3f;
            yield return null;
        }

        while (transform.parent.position.y > initialPosition.y)
        {
            transform.parent.position = Vector3.MoveTowards(transform.parent.position, initialPosition, fallSpeed);
            yield return null;
        }

        transform.parent.position = initialPosition;
    }


    public override string ReturnBehaviourDescription()
    {
        return "swarmed by rats when he was a child, scarred him for life";
    }
}
