using UnityEngine;
using System.Collections;

public class PlayerSoundHandler : MonoBehaviour
{

    private AudioSource _audioSource;
	
	void Awake ()
	{
	    _audioSource = GetComponent<AudioSource>();
	}


    public void SetRunningSound()
    {        
        _audioSource.clip = Resources.Load<AudioClip>("Running");        
    }

    public IEnumerator PlayJumpSound()
    {
        _audioSource.clip = Resources.Load<AudioClip>("Jump");        
        _audioSource.Play();
        Debug.Log(_audioSource.clip);

        while (_audioSource.isPlaying)
        {
            yield return null;
        }

        SetRunningSound();
    }

    public void PlayRunningSound()
    {
        if (!_audioSource.isPlaying)
            _audioSource.Play();
    }
}
