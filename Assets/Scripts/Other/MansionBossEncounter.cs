using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MansionBossEncounter : MonoBehaviour
{
    public TMP_Text encounterTMP;
    public GameObject EndScreen;
    private CanvasGroup encounter;

    private void Awake()
    {
        encounter = encounterTMP.GetComponent<CanvasGroup>();
    }

    public void SetBossCounterActive(bool active)
    {
        encounter.alpha = 0;
        if (active)
        {
            encounter.alpha = 0;
            StartCoroutine(ShowEncounter());
        }
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
    IEnumerator ShowEncounter()
    {
        yield return new WaitForSeconds(4.5f);
        LeanTween.alphaCanvas(encounter, 1f, 1f);
    }
}
