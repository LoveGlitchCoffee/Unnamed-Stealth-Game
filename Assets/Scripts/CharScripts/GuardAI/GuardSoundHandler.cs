using UnityEngine;

public class GuardSoundHandler : MonoBehaviour
{


    private AudioSource _audioSource;

	void Awake ()
	{
	    _audioSource = GetComponent<AudioSource>();
	}
	
	
    public void SelectSound()
    {
        _audioSource.clip = Resources.Load<AudioClip>("Select");
        _audioSource.Play();
    }
}
