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
    public GameObject AddCardsPrefab, currentDeck_btn, addCards_btn;
    public Scrollbar currDeckScrollBar;
    private Image img;
    public TMP_Text mainTitleText, loadingText, cardCountText;
    bool shopCardTypeIsChosen, buttonLock;
    [SerializeField] int[] ammoCounts, hpItemCounts, knifeCounts, handgunCounts, shotgunCounts, machinegunCounts, rifleCounts, explosiveCounts;
    [SerializeField] int[] action1Counts, action2Counts, action3Counts, action4Counts, action5Counts, action6Counts, action7Counts;
    [SerializeField] int[] extra1Counts, extra2Counts;
    private int shopDataCount, shopDataNumber;
    private LinkedList<int[]> shopCardsLinkedList;

    [SerializeField] Color colorON;

    void Start()
    {
        shopDataNumber = 1;
        img = GetComponent<Image>();
        CardTypeDropdown.GetComponent<Dropdown>().interactable = false;
        shopCardTypeIsChosen = false;
        buttonLock = true;
        loadingText.text = "";
        shopDataCount = GameStats.ShopDeckDataCount;
        CreateLinkedShopLists();

    }
    private void CreateLinkedShopLists()
    {
        shopCardsLinkedList = new LinkedList<int[]>();
        shopCardsLinkedList.AddLast(ammoCounts);        //0
        shopCardsLinkedList.AddLast(hpItemCounts);      //1
        shopCardsLinkedList.AddLast(knifeCounts);       //2
        shopCardsLinkedList.AddLast(handgunCounts);     //3
        shopCardsLinkedList.AddLast(shotgunCounts);     //4
        shopCardsLinkedList.AddLast(machinegunCounts);  //5
        shopCardsLinkedList.AddLast(rifleCounts);       //6 
        shopCardsLinkedList.AddLast(explosiveCounts);   //7
        shopCardsLinkedList.AddLast(action1Counts);     //8
        shopCardsLinkedList.AddLast(action2Counts);     //9
        shopCardsLinkedList.AddLast(action3Counts);     //10
        shopCardsLinkedList.AddLast(action4Counts);     //11
        shopCardsLinkedList.AddLast(action5Counts);     //12
        shopCardsLinkedList.AddLast(action6Counts);     //13
        shopCardsLinkedList.AddLast(action7Counts);     //14
        shopCardsLinkedList.AddLast(extra1Counts);      //15
        shopCardsLinkedList.AddLast(extra2Counts);      //16

        foreach (int[] s in shopCardsLinkedList)
        {
            Debug.Log(s+" added to linked list");

        }

        Debug.Log("All arrays added to linked list");
    }

    public void UpdateShopDeckType(int indexValue)
    {
        if (!buttonLock)
        {
            string[] gridContent;

            switch (indexValue)
            {
                case 0: //Ammo
                    gridContent = AllShopCards.GetComponent<AllShopCards>().GetAmmoCards();
                    CreateShopCards(indexValue,gridContent);
                    break;
                case 1: //Health items
                    gridContent = AllShopCards.GetComponent<AllShopCards>().GetHealthItems();
                    CreateShopCards(indexValue, gridContent);
                    break;
                case 2: //Knives
                    gridContent = AllShopCards.GetComponent<AllShopCards>().GetKnifeCards();
                    CreateShopCards(indexValue, gridContent);
                    break;
                case 3: //Handguns
                    gridContent = AllShopCards.GetComponent<AllShopCards>().GetHandguns();
                    CreateShopCards(indexValue, gridContent);
                    break;
                case 4: //Shotguns
                    gridContent = AllShopCards.GetComponent<AllShopCards>().GetShotguns();
                    CreateShopCards(indexValue, gridContent);
                    break;
                case 5: //Machine guns etc.
                    gridContent = AllShopCards.GetComponent<AllShopCards>().GetMachineguns();
                    CreateShopCards(indexValue, gridContent);
                    break;
                case 6: //Rifles
                    gridContent = AllShopCards.GetComponent<AllShopCards>().GetRifles();
                    CreateShopCards(indexValue, gridContent);
                    break;
                case 7: //Explosives
                    gridContent = AllShopCards.GetComponent<AllShopCards>().GetExplosives();
                    CreateShopCards(indexValue, gridContent);
                    break;
                case 8: //Action 1
                    gridContent = AllShopCards.GetComponent<AllShopCards>().GetActionCards();
                    CreateShopCards(indexValue, gridContent);
                    break;
                case 9: //Action 2
                    gridContent = AllShopCards.GetComponent<AllShopCards>().GetActionCards();
                    CreateShopCards(indexValue, gridContent);
                    break;
                case 10://Action 3
                    gridContent = AllShopCards.GetComponent<AllShopCards>().GetActionCards();
                    CreateShopCards(indexValue, gridContent);
                    break;
                case 11://Action 4
                    gridContent = AllShopCards.GetComponent<AllShopCards>().GetActionCards();
                    CreateShopCards(indexValue, gridContent);
                    break;
                case 12://Action 5
                    gridContent = AllShopCards.GetComponent<AllShopCards>().GetActionCards();
                    CreateShopCards(indexValue, gridContent);
                    break;
                case 13://Action 6
                    gridContent = AllShopCards.GetComponent<AllShopCards>().GetActionCards();
                    CreateShopCards(indexValue, gridContent);
                    break;
                case 14://Action 7
                    gridContent = AllShopCards.GetComponent<AllShopCards>().GetActionCards();
                    CreateShopCards(indexValue, gridContent);
                    break;
                case 15://Extra sloth (Extra1)
                    gridContent = AllShopCards.GetComponent<AllShopCards>().GetAllNonActionCards();
                    CreateShopCards(indexValue, gridContent);
                    break;
                case 16: //Random sloth (Extra2)
                    gridContent = AllShopCards.GetComponent<AllShopCards>().GetAllNonActionCards();
                    CreateShopCards(indexValue, gridContent);
                    break;

            }

        }
    }
    private void CreateShopCards(int cardIndex,string[] array) //START
    {
        StartCoroutine(LoadAndSave(true)); //load text
        ClearAllShopCards();
        GameObject gridContent = LoadGridContent.transform.gameObject; //WRONG OBJECT TESTING

        int lineIndex = 0;
        foreach (string s in array)
        {
            int count = GetShopCardCount(cardIndex, lineIndex);
            GetComponent<SpriteFromAtlas>().SetMansionCardSprite(s);
            GameObject card = Instantiate(AddCardsPrefab);
            card.GetComponent<Image>().sprite = img.sprite;
            card.transform.SetParent(gridContent.transform);
            card.transform.localScale = new Vector3(1f, 1f, 1f);
            card.GetComponent<EdShopCardCountScript>().SetMyCardData(cardIndex, lineIndex, count);

            lineIndex++;
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

    public void LoadNewShopData(int dropdownIndexValue)
    {
        shopDataNumber = dropdownIndexValue+1;
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

    //cardIndex values:
    //0 = Ammo;     1 = HPItems     2 = Knives      3 = Handguns        4 = Shotguns      5 = Machineguns       6 = rifles      7 = Explosives
    //8 = Action1   9 = Action2     10 = Action3     11 = Action 4       12 = Action 5     13 = Action 6         14 = Action 7
    //15 = Extra1   16 = Extra2 (random)
    
    private int GetShopCardCount(int cardTypeIndex, int lineIndex) //Shop card prefab
    {
        int count = 0;
        switch(cardTypeIndex)
        {
            case 0:
                count = ammoCounts[lineIndex];
                break;
            case 1:
                count = hpItemCounts[lineIndex];
                break;
            case 2:
                count = knifeCounts[lineIndex];
                break;
            case 3:
                count = handgunCounts[lineIndex];
                break;
            case 4:
                count = shotgunCounts[lineIndex];
                break;
            case 5:
                count = machinegunCounts[lineIndex];
                break;
            case 6:
                count = rifleCounts[lineIndex];
                break;
            case 7:
                count = explosiveCounts[lineIndex];
                break;
            case 8:
                count = action1Counts[lineIndex];
                break;
            case 9:
                count = action2Counts[lineIndex];
                break;
            case 10:
                count = action3Counts[lineIndex];
                break;
            case 11:
                count = action4Counts[lineIndex];
                break;
            case 12:
                count = action5Counts[lineIndex];
                break;
            case 13:
                count = action6Counts[lineIndex];
                break;
            case 14:
                count = action7Counts[lineIndex];
                break;
            case 15:
                count = extra1Counts[lineIndex];
                break;
            case 16:
                count = extra2Counts[lineIndex];
                break;

        }

        return count;
    }
    
    public void UpdateShopCardArrayValue(int cardTypeIndex, int row, int value)
    {
        switch (cardTypeIndex)
        {
            case 0:
                ammoCounts[row] = value;
                break;
            case 1:
                hpItemCounts[row] = value;
                break;
            case 2:
                knifeCounts[row] = value;
                break;
            case 3:
                handgunCounts[row] = value;
                break;
            case 4:
                shotgunCounts[row] = value;
                break;
            case 5:
                machinegunCounts[row] = value;
                break;
            case 6:
                rifleCounts[row] = value;
                break;
            case 7:
                explosiveCounts[row] = value;
                break;
            case 8:
                action1Counts[row] = value;
                break;
            case 9:
                action2Counts[row] = value;
                break;
            case 10:
                action3Counts[row] = value;
                break;
            case 11:
                action4Counts[row] = value;
                break;
            case 12:
                action5Counts[row] = value;
                break;
            case 13:
                action6Counts[row] = value;
                break;
            case 14:
                action7Counts[row] = value;
                break;
            case 15:
                extra1Counts[row] = value;
                break;
            case 16:
                extra2Counts[row] = value;
                break;

        }


    }


    public void OnClickLoad()
    {
        string textFilePath = Application.persistentDataPath + "/Custom_data/ShopCardsData" + shopDataNumber + ".txt";
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
        extra2Counts = SetStringNumberValues(fileLines[10]);

        if (buttonLock)
        {
            CardTypeDropdown.GetComponent<Dropdown>().interactable = true;
            buttonLock = false;
            UpdateShopDeckType(0);
        }

    }

    public void OnClickSaveAll()
    {
        string[] writeLine = new string[11];

        writeLine[0] = ConvertIntArrayToString(ammoCounts);
        writeLine[1] = ConvertIntArrayToString(hpItemCounts);
        writeLine[2] = ConvertIntArrayToString(knifeCounts);
        writeLine[3] = ConvertIntArrayToString(handgunCounts);
        writeLine[4] = ConvertIntArrayToString(shotgunCounts);
        writeLine[5] = ConvertIntArrayToString(machinegunCounts);
        writeLine[6] = ConvertIntArrayToString(rifleCounts);
        writeLine[7] = ConvertIntArrayToString(explosiveCounts);
        writeLine[8] = ConvertIntArrayToString(action1Counts); //action cards 1-7
        writeLine[9] = ConvertIntArrayToString(extra1Counts);
        writeLine[10] = ConvertIntArrayToString(extra2Counts);


        string writeToFilePath = Application.persistentDataPath + "/Custom_data/ShopCardsData" + shopDataNumber + ".txt";
        File.WriteAllLines(writeToFilePath, writeLine);
        Debug.Log("Overwriting to: " + writeToFilePath);

        StartCoroutine(LoadAndSave(false)); //save text
    }
    private string ConvertIntArrayToString(int[] array)
    {
        int length = array.Length;
        string result="";
        for (int i = 0; i < length; i++)
        {
            int value = array[i];

            if (value < 10) //Set zeros to 1 digit numbers
                result += "0" + value;
            else
                result += value;
        }
        return result;
    }

    private IEnumerator LoadAndSave(bool load) //load = true, save = false
    {
        buttonLock = true;
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
        buttonLock = false;
    }
}

