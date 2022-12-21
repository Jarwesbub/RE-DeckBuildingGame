using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class LeftMenuControl : MonoBehaviourPun
{
    public GameObject LeftMenu, OtherSettingsMenu, LM_HandDeck, LM_MansionDeck, LM_HandButton, LM_MansionButton;
    public GameObject LM_HandCardPrefab, LM_HandCardGridContent, LM_MansionCardGridContent;
    public Text plus1Main, plus1Hand,plus1Mansion;
    public TMP_Text cardsCountText;
    private bool isVisible, isSettingsVisible, isButtonLock;
    [SerializeField] private List<GameObject> HandCardsList, MansionCardsList;
    [SerializeField] private List<Sprite> HandCardSprites;
    private bool showHandCards;
    private Color32 colorON;

    private void Start()
    {
        isButtonLock = true;
        LeftMenu.SetActive(false);
        OtherSettingsMenu.SetActive(false);
        isVisible = false;
        isSettingsVisible = false;
        showHandCards = true;
        plus1Main.text = "";
        colorON = new Color32(120, 30, 30, 255);
        LM_MansionButton.GetComponent<Image>().color = Color.black;
        GetStarterHandCards();
        StartCoroutine(GameStartWait());
    }

    public void GetStarterHandCards()
    {
        foreach (Transform child in LM_HandCardGridContent.transform)
        {
            HandCardsList.Add(child.gameObject);
            HandCardSprites.Add(child.gameObject.GetComponent<Image>().sprite);
        }
        UpdateCardsCount(HandCardsList.Count);
    }


    public void InstantiateNewHandCard(Sprite sprt)
    {
        GameObject handCard = Instantiate(LM_HandCardPrefab);
        handCard.GetComponent<Image>().sprite = sprt;
        handCard.transform.parent = LM_HandCardGridContent.transform;
        handCard.transform.localScale = new Vector3(1f, 1f, 1f);
        HandCardsList.Add(handCard);
        HandCardSprites.Add(sprt);
    }
    public void DeleteHandCard(string name)
    {
        Debug.Log(name);
        int count = HandCardSprites.Count;
        for (int i = 0; i < count; i++)
        {
            if (HandCardSprites[i].name == name)
            {
                Debug.Log(name +" deleted!");
                GameObject destroy = HandCardsList[i].gameObject;
                HandCardsList.Remove(HandCardsList[i]);
                Destroy(destroy);
                HandCardSprites.Remove(HandCardSprites[i]);
                break;
            }

        }

    }
    public void AddNewMansionCard(Sprite sprt)
    {
        GameObject mansionCard = Instantiate(LM_HandCardPrefab);
        mansionCard.GetComponent<Image>().sprite = sprt;
        mansionCard.transform.parent = LM_MansionCardGridContent.transform;
        mansionCard.transform.localScale = new Vector3(1f, 1f, 1f);
        MansionCardsList.Add(mansionCard);
    }


    public void SetPlus1Text()
    {
        plus1Main.text = "+1";
    }
    public void UpdateCardsCount(int count)
    {
        cardsCountText.text = "Cards count: "+count;
    }

    public void OnClickLeftMenuButton()
    {
        plus1Main.text = "";
        if (isVisible)
        {
            LeftMenu.SetActive(false);
            isVisible = false;
        }
        else
        {
            LeftMenu.SetActive(true);
            isVisible = true;
            if(showHandCards)
                UpdateCardsCount(HandCardsList.Count);
            else //showMansionCards
                UpdateCardsCount(MansionCardsList.Count);
        }

    }
    public void OnClickOtherSettingsButton()
    {
        if(!isButtonLock)
        if (isSettingsVisible)
        {
            OtherSettingsMenu.SetActive(false);
            isSettingsVisible = false;
        }
        else
        {
            OtherSettingsMenu.SetActive(true);
            isSettingsVisible = true;
        }

    }

    public void OnClickShowHandCardsList()
    {
        showHandCards = true;
        LM_HandDeck.SetActive(true);
        LM_MansionDeck.SetActive(false);
        plus1Hand.text = "";
        UpdateCardsCount(HandCardsList.Count);
        LM_HandButton.GetComponent<Image>().color = colorON;
        LM_MansionButton.GetComponent<Image>().color = Color.black;
    }
    public void OnClickShowMansionCardsList()
    {
        showHandCards = false;
        LM_MansionDeck.SetActive(true);
        LM_HandDeck.SetActive(false);
        plus1Mansion.text = "";
        UpdateCardsCount(MansionCardsList.Count);
        LM_MansionButton.GetComponent<Image>().color = colorON;
        LM_HandButton.GetComponent<Image>().color = Color.black;
    }

    IEnumerator GameStartWait()
    {
        yield return new WaitForSeconds(2f);
        isButtonLock = false;
    }
}
