using UnityEngine;
using System.Collections;

public class Gate : MonoBehaviour, IInteractive
{

    private AudioSource _audio;
    [HideInInspector]
    public bool CanOpen;

    void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }


    public void PerformPurpose(InventoryLogic inventory)
    {
        if (CanOpen)
            StartCoroutine(Open());
    }

    /*
     * Gate's collider disabled, allow player to progress to certain area of the map
     * Scale adjustments are 'animations'
     */
    public IEnumerator Open()
    {
        _audio.Play();

        GetComponent<BoxCollider2D>().enabled = false;

        while (transform.localScale.y > 0)
        {
            transform.localScale -= new Vector3(0, 0.01f);
            transform.position += new Vector3(0, 0.003f);
            yield return null;
        }
    }
}