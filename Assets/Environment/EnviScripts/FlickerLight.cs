using UnityEngine;
using System.Collections;

public class FlickerLight : MonoBehaviour
{
    private bool _canFlicker = true;

    private Light torchLight;

    void Start()
    {
        torchLight = gameObject.GetComponent<Light>();
        Debug.Log(torchLight);

        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (_canFlicker)
        {
            torchLight.intensity = Random.Range(1.5f, 2.5f);
            yield return new WaitForSeconds(0.08f);
        }
    }


}
