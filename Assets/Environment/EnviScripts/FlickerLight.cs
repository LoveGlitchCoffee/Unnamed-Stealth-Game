using UnityEngine;
using System.Collections;

public class FlickerLight : MonoBehaviour
{
    private bool _canFlicker = true;

    private Light torchLight;

    void Start()
    {
        torchLight = gameObject.GetComponent<Light>();        

        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (_canFlicker)
        {
            torchLight.intensity = Random.Range(1.3f, 2.3f);
            yield return new WaitForSeconds(0.08f);
        }
    }


}
