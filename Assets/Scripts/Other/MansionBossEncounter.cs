using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MansionBossEncounter : MonoBehaviour
{
    public TMP_Text encounterTMP;
    public GameObject EndScreen;

    private void OnEnable()
    {
        
    }
    private void Start()
    {
        encounterTMP.text = "Boss Encounter";
    }

    public void AllBossesDefeated()
    {
        encounterTMP.text = "All Bosses Defeated!";
        EndScreen.SetActive(true);
    }

}
