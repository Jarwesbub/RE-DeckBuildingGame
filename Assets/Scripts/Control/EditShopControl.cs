using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Linq;
using System;

public class EditShopControl : MonoBehaviour
{
    public GameObject AllShopCards, CardTypeDropdown, LoadGridContent;
    public GameObject AddCardsPrefab, UI_CurrentDeck, UI_AddCards, currentDeck_btn, addCards_btn;
    public Scrollbar currDeckScrollBar;
    private Image img;
    public TMP_Text mainTitleText, loadingText;
    bool shopCardTypeIsChosen, buttonLock;
    [SerializeField] int[] ammoCounts, hpItemCounts, knifeCounts, handgunCounts, shotgunCounts, machinegunCounts, rifleCounts, explosiveCounts;
    [SerializeField] int[] action1Counts, action2Counts, action3Counts, action4Counts, action5Counts, action6Counts, action7Counts;
    [SerializeField] int[] extra1Counts, extra2Counts;
    private int shopDataCount;

    [SerializeField] Color colorON;

    void Start()
    {
        img = GetComponent<Image>();
        shopCardTypeIsChosen = false;
        buttonLock = false;
        loadingText.text = "";
        shopDataCount = GameStats.ShopDeckDataCount;

        OnClickChooseCurrentDeck();
    }
    public void UpdateShopDeckType(int indexValue)
    {
        {
            string[] gridContent;

            switch (indexValue)
            {
                case 0: //Ammo
                    gridContent = AllShopCards.GetComponent<AllShopCards>().GetAmmoCards();
                    CreateShopCards(gridContent);
                    break;
                case 1: //Health items
                    gridContent = AllShopCards.GetComponent<AllShopCards>().GetHealthItems();
                    CreateShopCards(gridContent);
                    break;
                case 2: //Knives
                    gridContent = AllShopCards.GetComponent<AllShopCards>().GetKnifeCards();
                    CreateShopCards(gridContent);
                    break;
                case 3: //Handguns
                    gridContent = AllShopCards.GetComponent<AllShopCards>().GetHandguns();
                    CreateShopCards(gridContent);
                    break;
                case 4: //Shotguns
                    gridContent = AllShopCards.GetComponent<AllShopCards>().GetShotguns();
                    CreateShopCards(gridContent);
                    break;
                case 5: //Machine guns etc.
                    gridContent = AllShopCards.GetComponent<AllShopCards>().GetMachineguns();
                    CreateShopCards(gridContent);
                    break;
                case 6: //Rifles
                    gridContent = AllShopCards.GetComponent<AllShopCards>().GetRifles();
                    CreateShopCards(gridContent);
                    break;
                case 7: //Explosives
                    gridContent = AllShopCards.GetComponent<AllShopCards>().GetExplosives();
                    CreateShopCards(gridContent);
                    break;
                case 8: //Action 1
                    gridContent = AllShopCards.GetComponent<AllShopCards>().GetActionCards();
                    CreateShopCards(gridContent);
                    break;
                case 9: //Action 2
                    gridContent = AllShopCards.GetComponent<AllShopCards>().GetActionCards();
                    CreateShopCards(gridContent);
                    break;
                case 10://Action 3
                    gridContent = AllShopCards.GetComponent<AllShopCards>().GetActionCards();
                    CreateShopCards(gridContent);
                    break;
                case 11://Action 4
                    gridContent = AllShopCards.GetComponent<AllShopCards>().GetActionCards();
                    CreateShopCards(gridContent);
                    break;
                case 12://Action 5
                    gridContent = AllShopCards.GetComponent<AllShopCards>().GetActionCards();
                    CreateShopCards(gridContent);
                    break;
                case 13://Action 6
                    gridContent = AllShopCards.GetComponent<AllShopCards>().GetActionCards();
                    CreateShopCards(gridContent);
                    break;
                case 14://Action 7
                    gridContent = AllShopCards.GetComponent<AllShopCards>().GetActionCards();
                    CreateShopCards(gridContent);
                    break;
                case 15://Extra sloth (Extra1)
                    gridContent = AllShopCards.GetComponent<AllShopCards>().GetAllNonActionCards();
                    CreateShopCards(gridContent);
                    break;
                case 16: //Random sloth (Extra2)
                    gridContent = AllShopCards.GetComponent<AllShopCards>().GetAllNonActionCards();
                    CreateShopCards(gridContent);
                    break;

            }

        }
    }
    private void CreateShopCards(string[] array) //START
    {
       
        StartCoroutine(LoadAndSave(true)); //load text
        ClearAllShopCards();
        GameObject gridContent = LoadGridContent.transform.gameObject; //WRONG OBJECT TESTING


        foreach (string s in array)
        {
            GetComponent<SpriteFromAtlas>().SetMansionCardSprite(s);

            GameObject card = Instantiate(AddCardsPrefab);
            card.GetComponent<Image>().sprite = img.sprite;
            card.transform.SetParent(gridContent.transform);
            card.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        shopCardTypeIsChosen = true;
    }
    private void ClearAllShopCards()
    {
        foreach(Transform child in LoadGridContent.transform) //WRONG OBJECT TESTING
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void LoadNewShopData(int shopDataNumber)
    {
        string textFilePath = Application.persistentDataPath + "/Custom_data/ShopCardsData"+shopDataNumber+".txt";
        string[] fileLines = File.ReadAllLines(textFilePath).ToArray();

        Debug.Log("Filelines length = " + fileLines.Length);

        ammoCounts = SetStringNumberValues(fileLines[0]);
        hpItemCounts = SetStringNumberValues(fileLines[1]);
        knifeCounts = SetStringNumberValues(fileLines[2]);
        handgunCounts = SetStringNumberValues(fileLines[3]);
        shotgunCounts = SetStringNumberValues(fileLines[4]);
        machinegunCounts = SetStringNumberValues(fileLines[5]);
        rifleCounts = SetStringNumberValues(fileLines[6]);
        explosiveCounts = SetStringNumberValues(fileLines[7]);

        action1Counts = SetStringNumberValues(fileLines[8]);
        action2Counts = action1Counts;
        action3Counts = action1Counts;
        action4Counts = action1Counts;
        action5Counts = action1Counts;
        action6Counts = action1Counts;
        action7Counts = action1Counts;

        extra1Counts = SetStringNumberValues(fileLines[9]);
        extra2Counts = extra1Counts;
    }


    private int[] SetStringNumberValues(string s)
    {
        int index = 0;
        int lineLenght = s.Length;
        int[] cardType = new int[lineLenght/2];
        int typeIndex = -1;

        for (int digit = 0; digit < lineLenght; digit++)
        {
            //Debug.Log("Digit: " + digit);
            if (digit % 2 == 0 || digit == 0) //Even numbers
            {
                char firstCharacter = s[digit];
                typeIndex++;
                cardType[typeIndex] = (int)(firstCharacter - '0') * 10;
                //Debug.Log("Parillinen " + cardType[typeIndex] + " index: " + typeIndex);
            }
            else //UnEven numbers
            {
                char lastCharacter = s[digit];
                cardType[typeIndex] += (int)(lastCharacter - '0');
                //Debug.Log("Pariton " + cardType[typeIndex] + " index: " + typeIndex);
            }
            index++;
        }
        return cardType;
    }
        


    
    



    public void OnClickChooseCurrentDeck()
    {
        mainTitleText.text = "Shop Builder";
        UI_CurrentDeck.SetActive(true);
        UI_AddCards.SetActive(false);
        currentDeck_btn.GetComponent<Image>().color = colorON;
        addCards_btn.GetComponent<Image>().color = Color.black;
    }
    public void OnClickChooseAddCards()
    {
        if (shopCardTypeIsChosen)
        {
            mainTitleText.text = "Add Shop Cards";
            UI_CurrentDeck.SetActive(false);
            UI_AddCards.SetActive(true);
            currentDeck_btn.GetComponent<Image>().color = Color.black;
            addCards_btn.GetComponent<Image>().color = colorON;
        }
        else
        {
           
        }
    }

    private IEnumerator LoadAndSave(bool load) //load = true, save = false
    {
        if (load)
        {
            CardTypeDropdown.GetComponent<Dropdown>().interactable = false;
            loadingText.text = "Loading...";
            yield return new WaitForSeconds(1f);
            CardTypeDropdown.GetComponent<Dropdown>().interactable = true;
        }
        else //Save
        {
            loadingText.text = "Saving...";
            yield return new WaitForSeconds(1f);
        }
        loadingText.text = "";
        
    }
}

