using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EdShopCardCountScript : MonoBehaviour
{
    //myShopCardIndex values:
    //0 = Ammo;     1 = HPItems     2 = Knives      3 = Handguns        4 = Shotguns      5 = Machineguns       6 = rifles      7 = Explosives
    //8 = Action1   9 = Action2     10 = Action3     11 = Action 4       12 = Action 5     13 = Action 6         14 = Action 7
    //15 = Extra1   16 = Extra2 (random)

    [SerializeField] int myShopCardIndex, myLineIndex;
    [SerializeField] Button addButton, removeButton;
    [SerializeField] TMP_Text myCountTMP;
    [SerializeField] int myCardCount;
    private GameObject EditShopControl, ShowCaseCard;
    Image img;

    public void SetMyCardData(int shopCardIndex, int lineIndex, int value)
    {
        myShopCardIndex = shopCardIndex;
        myLineIndex = lineIndex;
        myCardCount = value;

    }
    void Start()
    {
        EditShopControl = GameObject.FindWithTag("EditMansionControl");
        ShowCaseCard = GameObject.FindWithTag("UICardShowcase");
        img = GetComponent<Image>();
        //EditShopControl.GetComponent<EditShopControl>().GetShopCardCount(myShopCardIndex, 0);
        UpdateMyCount();
    }

    public void OnClickAddToMyCounts() //PLUS
    {
        if(myCardCount<50)
            myCardCount++;

        UpdateMyCount();

    }

    public void OnClickRemoveFromMyCounts() //MINUS
    {
        if (myCardCount > 0)
            myCardCount--;

        UpdateMyCount();

    }

    public void ResetMyCountToZero()
    {
        myCardCount = 0;
        UpdateMyCount();
    }



    public void OnClickChangeCardShowcaseImage()
    {
        ShowCaseCard.GetComponent<Image>().sprite = img.sprite;

    }

    private void UpdateMyCount()
    {
        EditShopControl.GetComponent<EditShopControl>().UpdateShopCardArrayValue(myShopCardIndex, myLineIndex, myCardCount);
        myCountTMP.text = "Count: " + myCardCount;
    }

    public int GetMyCardCount()
    {
        return myCardCount;
    }

}
