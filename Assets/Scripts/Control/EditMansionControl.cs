using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class EditMansionControl : MonoBehaviour
{
    public GameObject MansionDropDownHandler, MansionBaseCardPrefab, MansionGridContent;
    public GameObject UI_CurrentDeck, UI_AddCards, currentDeck_btn, addCards_btn;
    public GameObject UI_EasyTier, UI_NormalTier, UI_HardTier, easy_btn, normal_btn, hard_btn;
    public GameObject AddCardsPrefab, ShowCaseCard1, ShowCaseCard2;
    public Text dropdownText;
    public TMP_Text mainTitleText, loadSaveInfo, consoleTxt;
    public TMP_Text _cardCount, _lowCount, _midCount, _topCount;
    [SerializeField] private int cardCount, lowCount, midCount, topCount;
    public int mansionValue;
    private bool isButtonLock, mansionCardsLoaded;
    private Image img;

    [SerializeField] private string currentCustomDeck;
    [SerializeField] private List<string> CustomDeckCardsList;
    [SerializeField] private string[] LowTierCardsList, MidTierCardsList, HighTierCardsList;
    public int textListCount;
    [SerializeField] private Color32 colorON;

    private void Awake()
    {
        int value = 1;
        string name = "MansionCards"+value;
        
        int maxMansionDecks = 10;
        string readFromFilePath = Application.persistentDataPath + "/Custom_data/MansionCards1.txt";

        if (System.IO.File.Exists(readFromFilePath))
        {
            while (value < maxMansionDecks)
            {
                name = "MansionCards"+value;
                readFromFilePath = Application.persistentDataPath + "/Custom_data/" + name + ".txt";

                if (System.IO.File.Exists(readFromFilePath))
                {
                    value++;
                    Debug.Log(readFromFilePath);
                }
                else
                {
                    value--;
                    break;
                }
                
            }
        }
        MansionDropDownHandler.GetComponent<UIEdMansionLoadDropDownHandler>().customCount = value;
        //Debug.Log("MansionCards count = " + value);
    }
    private void Start()
    {
        consoleTxt.text = ""; dropdownText.text = "";
        loadSaveInfo.text = "Load your Custom deck first";
        UpdateCardsCount(true);
        img = GetComponent<Image>();
        OnClickChooseCurrentDeck();
        CreateEnemyTierCards();

        UI_EasyTier.SetActive(false); UI_NormalTier.SetActive(false); UI_HardTier.SetActive(false);
        easy_btn.GetComponent<Image>().color = Color.black;
        normal_btn.GetComponent<Image>().color = Color.black;
        hard_btn.GetComponent<Image>().color = Color.black;


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
        string readLowTierFile = Application.streamingAssetsPath + "/Base_Data/MansionEnemies_LowTierList.txt";
        LowTierCardsList = File.ReadAllLines(readLowTierFile).ToArray();

        string readMidTierFile = Application.streamingAssetsPath + "/Base_Data/MansionEnemies_MidTierList.txt";
        MidTierCardsList = File.ReadAllLines(readMidTierFile).ToArray();

        string readHighTierFile = Application.streamingAssetsPath + "/Base_Data/MansionEnemies_HighTierList.txt";
        HighTierCardsList = File.ReadAllLines(readHighTierFile).ToArray();

        CreateMansionCards(UI_EasyTier, LowTierCardsList);
        CreateMansionCards(UI_NormalTier, MidTierCardsList);
        CreateMansionCards(UI_HardTier, HighTierCardsList);


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

                easy_btn.GetComponent<Image>().color = colorON;
                normal_btn.GetComponent<Image>().color = Color.black;
                hard_btn.GetComponent<Image>().color = Color.black;
            }
            else if (value == 1) //NORMAL
            {
                UI_EasyTier.SetActive(false);
                UI_NormalTier.SetActive(true);
                UI_HardTier.SetActive(false);

                easy_btn.GetComponent<Image>().color = Color.black;
                normal_btn.GetComponent<Image>().color = colorON;
                hard_btn.GetComponent<Image>().color = Color.black;
            }
            else //2 = HARD
            {
                UI_EasyTier.SetActive(false);
                UI_NormalTier.SetActive(false);
                UI_HardTier.SetActive(true);

                easy_btn.GetComponent<Image>().color = Color.black;
                normal_btn.GetComponent<Image>().color = Color.black;
                hard_btn.GetComponent<Image>().color = colorON;
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
            mansionValue = PlayerPrefs.GetInt("MansionType") +1;
            string file = "MansionCards"+mansionValue;

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
        CustomDeckCardsList = fileLines;
        textListCount = CustomDeckCardsList.Count;
        currentCustomDeck = name;
        Debug.Log("File loaded: " + name + ".txt");
    }
    
    public void OnClickSaveCustomMansion()
    {
        if (!isButtonLock)
        {
            isButtonLock = true;

            string writeToFilePath = Application.persistentDataPath + "/Custom_data/" + currentCustomDeck + ".txt";
            CustomDeckCardsList.Sort();
            File.WriteAllLines(writeToFilePath, CustomDeckCardsList);
            
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
            dropdownText.text = "Custom " + mansionValue;
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
