using UnityEngine;
using System.Collections;

public class ShineEffect : MonoBehaviour
{

    private Light _shineLight;
    private float normalLighting = 0f;
    private float highLighting = 3.5f;

    void Awake()
    {
        _shineLight = GetComponent<Light>();
    }

	void Start ()
	{
	    StartCoroutine(ShineLight());
	}

    private IEnumerator ShineLight()
    {
        bool shine = true;
        float timeStamp = Time.time;
        
        float secondLighting = normalLighting;
        float firstLighting = highLighting;

        while (shine)
        {
            if (Time.time - timeStamp > 1f)
                timeStamp = Time.time;

            _shineLight.intensity = Mathf.Lerp(firstLighting, secondLighting, (Time.time-timeStamp)/1.7f);

            if (_shineLight.intensity == highLighting)
            {
                firstLighting = highLighting;
                secondLighting = normalLighting;
            }
            else if (_shineLight.intensity == normalLighting)
            {
                firstLighting = normalLighting;
                secondLighting = highLighting;
            }

            yield return null;
        }
           
            
            
        
    }
}
