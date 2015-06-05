using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour, IInteractive
{

    private GameObject _gate;
    public string GateName;
    private Animator _anim;

    //extendible to anything, not just opening gates, keep <IInteractive>
    void Awake()
    {
        GameObject[] gates = GameObject.FindGameObjectsWithTag("Gates");

        for (int i = 0; i < gates.Length; i++)
        {
            if (gates[i].name == GateName)
            {
                _gate = gates[i];
                break;
            }
        }

        _anim = GetComponent<Animator>();
    }


    /**haven't thought of way around this
     */
    public void PerformPurpose(InventoryLogic inventory)
    {
        _anim.SetBool("Activated", true);
        _gate.GetComponent<Gate>().CanOpen = true;
        _gate.GetComponent<IInteractive>().PerformPurpose(inventory);

    }

}