using UnityEngine;
using System.Collections;

public class DragSoundHandler : MonoBehaviour {

    public IEnumerator SetSoundProperties(GameObject physicalSpawn)
    {            
        physicalSpawn.GetComponent<Identifer>().SetDropSound();
        physicalSpawn.GetComponent<Identifer>().PlaySound();

        while (physicalSpawn.GetComponent<AudioSource>().isPlaying)
        {
            yield return null;
        }

        physicalSpawn.GetComponent<Identifer>().SetPickUpSound();
    }


    public void PlayDragSound()
    {
        transform.parent.parent.GetComponent<AudioSource>().Play();
    }
}
