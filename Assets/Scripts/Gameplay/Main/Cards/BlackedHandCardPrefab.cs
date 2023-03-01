using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackedHandCardPrefab : MonoBehaviour
{
    public string cardName;
    private bool isChosen;
    [SerializeField]private GameObject UIBlacked, chosenImage;

    private void Start()
    {
        isChosen = false;
        SetChosenImageActive();
        UIBlacked = GameObject.FindWithTag("UIBlackedMain");
    }

    public void SetCardName(string name)
    {
        cardName = name;
    }

    public void OnClickChooseThis()
    {
        UIBlacked.GetComponent<UIBlackedGridContent>().CardIsChosenFromTheGridContent(cardName, isChosen);
        isChosen = !isChosen;
        SetChosenImageActive();
    }

    private void SetChosenImageActive()
    {
        chosenImage.SetActive(isChosen);
    }
}
