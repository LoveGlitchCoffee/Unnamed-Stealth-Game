using UnityEngine;
using System.Collections;

public class Gate : MonoBehaviour, IInteractive
{

    private Animator _anim;
    [HideInInspector]
    public bool CanOpen;


    public void PerformPurpose(InventoryLogic inventory)
    {
        if (CanOpen)
            StartCoroutine(Open());
    }

    public IEnumerator Open()
    {

        GetComponent<BoxCollider2D>().enabled = false;

        while (transform.localScale.y > 0)
        {
            transform.localScale -= new Vector3(0, 0.2f);
            transform.position += new Vector3(0, 0.05f);
            yield return null;
        }
    }
}