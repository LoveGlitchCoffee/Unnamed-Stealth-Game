using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour, IInteractive
{

    private GameObject _gate;
    public string GateName;
    private Animator _anim;    
    private ShowGateOpened _showGate;
    private EnviAudioController _audio;

    /*
     * Currently open gates
     * Lever is connected to a certain gate
     * extendible to anything, not just opening gates, keep <IInteractive>
     */
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
        _showGate = GetComponent<ShowGateOpened>();
        _audio = GetComponent<EnviAudioController>();
    }


    /*
     * Purpose is to activate gate opening
     * Currenly only applicable to gate
     */
    public void PerformPurpose(InventoryLogic inventory)
    {
        _anim.SetBool("Activated", true);
        _audio.PlaySound();        
        StartCoroutine(_showGate.MoveCamera(_gate));
        _gate.GetComponent<Gate>().CanOpen = true;
        _gate.GetComponent<IInteractive>().PerformPurpose(inventory);
    }

}