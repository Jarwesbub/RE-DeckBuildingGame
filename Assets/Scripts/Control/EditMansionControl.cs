using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class EditMansionControl : MonoBehaviour
{
    public GameObject AllMansionCards, MansionBaseCardPrefab, MansionGridContent;
    public GameObject UI_CurrentDeck, UI_AddCards, currentDeck_btn, addCards_btn;
    public GameObject UI_EasyTier, UI_NormalTier, UI_HardTier, easy_btn, normal_btn, hard_btn;
    public GameObject UI_Items, items_btn, AddCardsPrefab, ShowCaseCard1, ShowCaseCard2;
    public TMP_Text mainTitleText, loadSaveInfo, consoleTxt;
    public Dropdown MansionDropDownHandler;
    public InputField EditName_inputf;
    public TMP_Text _cardCount, _lowCount, _midCount, _topCount;
    [SerializeField] private int cardCount, lowCount, midCount, topCount;
    public int mansionValue;
    private bool isButtonLock, mansionCardsLoaded;
    private Image img;

    [SerializeField] private string currentCustomDeck, customDeckName;
    [SerializeField] private List<string> CustomDeckCardsList;
    [SerializeField] private string[] LowTierCardsList, MidTierCardsList, HighTierCardsList, ItemCardsList;
    public int textListCount;
    [SerializeField] private Color32 colorON;

    private void Awake()
    {
        int mansionCount = 1;
        while (File.Exists(Application.persistentDataPath + "/Custom_data/MansionCards" + mansionCount + ".txt"))
        {
            mansionCount++;
        }
        mansionCount--;
        GameStats.MansionDeckCount = mansionCount;

    }
    private void Start()
    {
        consoleTxt.text = "";
        loadSaveInfo.text = "Load your Custom deck first";
        EditName_inputf.text = ""; customDeckName = "";
        UpdateCardsCount(true);
        img = GetComponent<Image>();
        OnClickChooseCurrentDeck();
        CreateEnemyTierCards();

        UI_EasyTier.SetActive(false); UI_NormalTier.SetActive(false); UI_HardTier.SetActive(false);
        UI_Items.SetActive(false);
        easy_btn.GetComponent<Image>().color = Color.black;
        normal_btn.GetComponent<Image>().color = Color.black;
        hard_btn.GetComponent<Image>().color = Color.black;
        items_btn.GetComponent<Image>().color = Color.black;

        //Choose random card for the showcase image
        int length = LowTierCardsList.Length - 1; int rand = Random.Range(0, length); string s = LowTierCardsList[rand];
        GetComponent<SpriteFromAtlas>().SetMansionCardSprite(s);
        ShowCaseCard1.GetComponent<Image>().sprite = img.sprite;

        length = HighTierCardsList.Length - 1; rand = Random.Range(0, length); s = HighTierCardsList[rand];
        GetComponent<SpriteFromAtlas>().SetMansionCardSprite(s);
        ShowCaseCard2.GetComponent<Image>().sprite = img.sprite;

        StartCoroutine(ShowInConsole("Mansion deck builder loaded"));
    }

    public void OnClickChooseCurrentDeck()
    {
        mainTitleText.text = "Mansion Deck Builder";
        UI_CurrentDeck.SetActive(true);
        UI_AddCards.SetActive(false);
        currentDeck_btn.GetComponent<Image>().color = colorON;
        addCards_btn.GetComponent<Image>().color = Color.black;
    }
    public void OnClickChooseAddCards()
    {
        if(mansionCardsLoaded)
        {
            mainTitleText.text = "Add Enemy Cards";
            UI_CurrentDeck.SetActive(false);
            UI_AddCards.SetActive(true);
            currentDeck_btn.GetComponent<Image>().color = Color.black;
            addCards_btn.GetComponent<Image>().color = colorON;
        }
        else
        {
            StartCoroutine(ShowInConsole("Load your Custom Deck first!"));
        }
    }

    private void CreateEnemyTierCards() //START
    {
        LowTierCardsList = AllMansionCards.GetComponent<AllMansionCards>().GetLowTierMansionCards();
        MidTierCardsList = AllMansionCards.GetComponent<AllMansionCards>().GetMidTierMansionCards();
        HighTierCardsList = AllMansionCards.GetComponent<AllMansionCards>().GetTopTierMansionCards();
        ItemCardsList = AllMansionCards.GetComponent<AllMansionCards>().GetItemMansionCards();

        CreateMansionCards(UI_EasyTier, LowTierCardsList);
        CreateMansionCards(UI_NormalTier, MidTierCardsList);
        CreateMansionCards(UI_HardTier, HighTierCardsList);
        CreateMansionCards(UI_Items, ItemCardsList);


    }
    private void CreateMansionCards(GameObject UI_TierObj,string[] array) //START
    {
        GameObject gridContent = UI_TierObj.transform.GetChild(0).GetChild(0).gameObject;

        foreach (string s in array)
        {
            GetComponent<SpriteFromAtlas>().SetMansionCardSprite(s);

            GameObject card = Instantiate(AddCardsPrefab);
            card.GetComponent<Image>().sprite = img.sprite;
            card.transform.SetParent(gridContent.transform);
            card.transform.localScale = new Vector3(1f, 1f, 1f);
        }

    }

    public void OnClickSelectEnemyTier(int value) //0 = easy, 1 = normal, 2 = hard
    {
        if (!isButtonLock && mansionCardsLoaded)
        {
            if (value == 0) //EASY
            {
                UI_EasyTier.SetActive(true);
                UI_NormalTier.SetActive(false);
                UI_HardTier.SetActive(false);
                UI_Items.SetActive(false);

                easy_btn.GetComponent<Image>().color = colorON;
                normal_btn.GetComponent<Image>().color = Color.black;
                hard_btn.GetComponent<Image>().color = Color.black;
                items_btn.GetComponent<Image>().color = Color.black;
            }
            else if (value == 1) //NORMAL
            {
                UI_EasyTier.SetActive(false);
                UI_NormalTier.SetActive(true);
                UI_HardTier.SetActive(false);
                UI_Items.SetActive(false);

                easy_btn.GetComponent<Image>().color = Color.black;
                normal_btn.GetComponent<Image>().color = colorON;
                hard_btn.GetComponent<Image>().color = Color.black;
                items_btn.GetComponent<Image>().color = Color.black;
            }
            else if (value == 2)  //2 = HARD
            {
                UI_EasyTier.SetActive(false);
                UI_NormalTier.SetActive(false);
                UI_HardTier.SetActive(true);
                UI_Items.SetActive(false);

                easy_btn.GetComponent<Image>().color = Color.black;
                normal_btn.GetComponent<Image>().color = Color.black;
                hard_btn.GetComponent<Image>().color = colorON;
                items_btn.GetComponent<Image>().color = Color.black;
            }
            else // 3 = MANSION ITEMS
            {
                UI_EasyTier.SetActive(false);
                UI_NormalTier.SetActive(false);
                UI_HardTier.SetActive(false);
                UI_Items.SetActive(true);

                easy_btn.GetComponent<Image>().color = Color.black;
                normal_btn.GetComponent<Image>().color = Color.black;
                hard_btn.GetComponent<Image>().color = Color.black;
                items_btn.GetComponent<Image>().color = colorON;
            }

        }
            
    }
    public void OnClickClearCustomMansion()
    {
        if (!isButtonLock)
        {
            CustomDeckCardsList.Clear();

            foreach (Transform child in MansionGridContent.transform)
            {
                Destroy(child.gameObject);
            }
            UpdateCardsCount(true); //reset = true

            if (mansionValue != 0)
                StartCoroutine(ShowInConsole("Custom Deck " + mansionValue + " was cleared!"));
        }
    }
    public void OnClickLoadCustomMansion()
    {
        if (!isButtonLock)
        {
            isButtonLock = true;
            //mansionValue = PlayerPrefs.GetInt("MansionType") +1;
            mansionValue = GameStats.MansionDeckValue; // TESTING 12.11.2022
            if (mansionValue == 0) mansionValue++;
            string file = "MansionCards"+mansionValue;;
            LoadTextFileByName(file);

            cardCount = CustomDeckCardsList.Count;
            lowCount = 0; midCount = 0; topCount = 0;
            foreach (string s in CustomDeckCardsList)
            {
                char c = s[0];

                if (c == '0')//Card tiers 0-2 (low-mid-top)
                    lowCount++;
                else if (c == '1')
                    midCount++;
                else
                    topCount++;

                Debug.Log("Char value = " + c);
            }
            UpdateCardsCount(false);

            StartCoroutine(LoadAndSave(true)); //Load
            
        }
    }
    private void LoadTextFileByName(string name)
    {
        string readFromFilePath = Application.persistentDataPath + "/Custom_data/" + name + ".txt";
        List<string> fileLines = File.ReadAllLines(readFromFilePath).ToList();
        if (fileLines.Any()) //Checks if list is not empty
        {
            customDeckName = fileLines[0]; //First row = card name
            fileLines.RemoveAt(0); //Remove card name from custom list
            Debug.Log("FILE IS NOT EMPTY");
        }
        else
            customDeckName = name;

        CustomDeckCardsList = fileLines;
        textListCount = CustomDeckCardsList.Count;
        currentCustomDeck = name;
        EditName_inputf.text = customDeckName;
        MansionDropDownHandler.captionText.text = customDeckName;
        Debug.Log("File loaded: " + name + ".txt");
    }
    
    public void OnClickSaveCustomMansion()
    {
        if (!isButtonLock)
        {
            isButtonLock = true;

            string writeToFilePath = Application.persistentDataPath + "/Custom_data/" + currentCustomDeck + ".txt";
            CustomDeckCardsList.Sort();
            customDeckName = EditName_inputf.text;
            MansionDropDownHandler.captionText.text = customDeckName;
            MansionDropDownHandler.GetComponent<UIEdMansionLoadDropDownHandler>().ChangeDropItemNameByIndex(mansionValue-1, customDeckName);
            List<string> list = new();
            list.Add(customDeckName);
            foreach(string s in CustomDeckCardsList)
            {
                list.Add(s);
            }

            File.WriteAllLines(writeToFilePath, list);
            StartCoroutine(LoadAndSave(false)); //Save
            
        }
    }

    public void OnUIDeleteCard(int childIndex)
    {
        string name = CustomDeckCardsList[childIndex];
        Debug.Log(name + " got deleted!");
        CustomDeckCardsList.Remove(name);
        UpdateCardTierCountByName(name, false);
        StartCoroutine(ShowInConsole("Card was deleted from the Custom Deck "+mansionValue));

    }
    public void OnUIAddCard(string name)
    {
        CustomDeckCardsList.Add(name);
        GetComponent<SpriteFromAtlas>().SetMansionCardSprite(name);

        GameObject card = Instantiate(MansionBaseCardPrefab);
        card.GetComponent<Image>().sprite = img.sprite;
        card.transform.SetParent(MansionGridContent.transform);
        card.transform.localScale = new Vector3(1f, 1f, 1f);
        StartCoroutine(ShowInConsole("'" +name+ "'" + " was added to Custom Deck " + mansionValue));
        Debug.Log(name + " added!");
        UpdateCardTierCountByName(name, true);
    }
    private void UpdateCardTierCountByName(string name, bool add)
    {
        char c = name[0];
        int val = -1;
        if (add)
            val = 1;

        if (c == '0')//Card tiers 0-2 (low-mid-top)
            lowCount += val;
        else if (c == '1')
            midCount += val;
        else
            topCount += val;

        UpdateCardsCount(false); //reset = false
    }
    private void UpdateCardsCount(bool reset)
    {
        if (reset)
        {
            cardCount = 0; lowCount = 0; midCount = 0; topCount = 0;
        }
        cardCount = CustomDeckCardsList.Count;

        _cardCount.text = "Cards count: " + cardCount;
        _lowCount.text = "Low: " + lowCount;
        _midCount.text = "Mid: " + midCount;
        _topCount.text = "Top: " + topCount;
    }

    private void InstantiateToHandler()
    {
        foreach(Transform child in MansionGridContent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach(string s in CustomDeckCardsList)
        {
            GetComponent<SpriteFromAtlas>().SetMansionCardSprite(s);

            GameObject card = Instantiate(MansionBaseCardPrefab);
            card.GetComponent<Image>().sprite = img.sprite;
            card.transform.SetParent(MansionGridContent.transform);
            card.transform.localScale = new Vector3(1f, 1f, 1f);

        }
    }

    private IEnumerator LoadAndSave(bool load) //load = true, save = false
    {
        if (load)
        {
            loadSaveInfo.text = "Loading...";
            yield return new WaitForSeconds(1f);
            InstantiateToHandler();
            mansionCardsLoaded = true;
            //currentDeckName.text = "Custom Deck " + mansionValue;
            loadSaveInfo.text = "";
            isButtonLock = false;
            StartCoroutine(ShowInConsole("Custom Deck " + mansionValue + " loaded succesfully!"));
        }
        else //Save
        {
            loadSaveInfo.text = "Saving...";
            yield return new WaitForSeconds(1f);
            loadSaveInfo.text = "";
            isButtonLock = false;
            StartCoroutine(ShowInConsole("Custom Deck " + mansionValue + " saved succesfully!"));
        }
    }

    private IEnumerator ShowInConsole(string s)
    {
        consoleTxt.text = s;
        yield return new WaitForSeconds(4f);
        consoleTxt.text = "";
       
    }
}
