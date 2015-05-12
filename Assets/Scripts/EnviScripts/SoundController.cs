using UnityEngine;

public class SoundController : MonoBehaviour
{

    private AudioSource _sound;

    void Awake()
    {
        _sound = GetComponent<AudioSource>();
    }

    void Start()
    {
            
    }

}
