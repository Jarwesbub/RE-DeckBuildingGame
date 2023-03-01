using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBlackedGridContent : MonoBehaviour
{
    public string[] cardsList;
    public Sprite[] chosenSpriteList;
    private List<string> chosenCardsList;
    public GameObject UIBlackedCardPrefab, MainCanvas;
    public GameObject UIControl, PlayerHandCards, GridContentItems, selectButton;
    public TMP_Text selectBtnTMP;
    [SerializeField]private GameObject SpawnCards;
    private bool isMultipleSelection;
    int rule = 0; // 0 = remove card
    int selectBtnCount = 0;
    int ammo, dmg;

    private void OnEnable()
    {
        selectBtnCount = 0;
        SetSelectButtonText(selectBtnCount);
    }

    private void Start()
    {
        SpawnCards = GameObject.FindWithTag("Respawn");
    }

    private void SetSelectButtonText(int value)
    {
        selectBtnTMP.text = "Choose ("+value+") cards";
    }

    public void UISetActive(string text, int _rule, bool multipleSelection)
    {
        rule = _rule;
        isMultipleSelection = multipleSelection;
        selectButton.SetActive(multipleSelection);
        chosenCardsList = new List<string>();
        int length = chosenSpriteList.Length;
        for (int i = 0; i < length; i++)
        {
            GameObject cardObj = Instantiate(UIBlackedCardPrefab, new Vector3(0f,0f,0f), Quaternion.identity);
            cardObj.GetComponent<Image>().sprite = chosenSpriteList[i];
            cardObj.GetComponent<BlackedHandCardPrefab>().SetCardName(cardsList[i]);
            cardObj.transform.parent = GridContentItems.transform;
            cardObj.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        GetComponent<UIBlackedMain>().SetMainInfoText(text);
        gameObject.SetActive(true);
    }
    public void UISetInActive()
    {
        foreach (Transform child in GridContentItems.transform)
        {
            Destroy(child.gameObject);
        }
        GetComponent<UIBlackedMain>().SetMainInfoText("");
        gameObject.SetActive(false);
    }

    public void AddHandCardNameAndSprite(string currentCardName,string[] names, Sprite[] sprt)
    {
        cardsList = names;
        chosenSpriteList = sprt;
        int[] extraCardStats = CheckCardExtraStats(currentCardName);
        ammo = extraCardStats[0];
        dmg = extraCardStats[1];
    }
    private int[] CheckCardExtraStats(string name)
    {
        int[] values = new int[2];

        switch (name)
        {
            case "hg_polizei"://"Get +10 damage for each selected 'Pistol' card and move them in to the Discardpile"
                values[0] = 0; //Extra Ammo requirement
                values[1] = 10; //Extra Damage
                break;

        }
        return values;
    }

    public void CardIsChosenFromTheGridContent(string name, bool isChosen)
    {
        if(!isMultipleSelection)
            SuccessfullFinish(name);
        else if(!isChosen)
        {
            selectBtnCount++;
            SetSelectButtonText(selectBtnCount);
            chosenCardsList.Add(name);
        }
        else if (isChosen)
        {
            selectBtnCount--;
            SetSelectButtonText(selectBtnCount);
            chosenCardsList.Remove(name);
        }
    }
    public void OnClickAllTheCardsSelected()
    {
        int count = chosenCardsList.Count;

        if (count == 0)
        {
            UISetInActive();
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                SuccessfullFinish(chosenCardsList[i]);
            }
            chosenCardsList.Clear();
        }
    }



    private void SuccessfullFinish(string name)
    {
        Debug.Log("SuccesfullFinish -> "+name);
        if (rule == 0) //Trash a card
        {
            Debug.Log("Removing card...");
            foreach (Transform child in PlayerHandCards.transform)
            {
                if (child.GetComponent<HandCard>().currentCard == name)
                {
                    Debug.Log("Deleting card: " + name);
                    UIControl.GetComponent<DeleteCardsControl>().DeleteCardManually(child.gameObject);
                    break;
                }
            }
        }
        else if (rule == 1) //Send card to discard pile
        {
            foreach (Transform child in PlayerHandCards.transform)
            {
                if (child.GetComponent<HandCard>().currentCard == name)
                {
                    Debug.Log("Adding a card in discardpile: " + name);
                    SpawnCards.GetComponent<SpawnCards>().PutHandCardToDiscardPile(child.gameObject);
                    if(dmg!=0)
                        MainCanvas.GetComponent<HandCardStatsUI>().AddToWeaponStats(ammo, dmg);
                    break;
                }
            }
          
        }
        

        UISetInActive();
    }

    public void OnClickExit()
    {
        UISetInActive();
    }
}
