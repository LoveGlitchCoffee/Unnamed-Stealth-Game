using UnityEngine;
using System.Collections;

public class EnviAudioController : MonoBehaviour
{

    private AudioSource _audio;

    void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        _audio.Play();
    }
}
