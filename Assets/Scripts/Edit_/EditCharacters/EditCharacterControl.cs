using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class EditCharacterControl : MonoBehaviour
{
    public GameObject CharacterBaseCardPrefab, CharacterGridContent;
    public GameObject UI_CurrentDeck, UI_AddCards, currentDeck_btn, addCards_btn;
    public GameObject UI_Normal, UI_Custom, normal_btn, custom_btn;
    public GameObject UI_Items, AddCardsPrefab, ShowCaseCard1, ShowCaseCard2;
    public TMP_Text mainTitleText, loadSaveInfo, consoleTxt;
    public Dropdown CharactersDropDownHandler;
    public InputField EditName_inputf;
    public TMP_Text _cardCount;
    [SerializeField] private int cardCount;
    public int characterValue;
    private bool isButtonLock, characterCardsLoaded;
    private Image img;

    [SerializeField] private string currentCustomDeck, customDeckName;
    [SerializeField] private List<string> CustomDeckCardsList;
    [SerializeField] private string[] normalCardsList, customCardsList;
    public int textListCount;
    [SerializeField] private Color32 colorON, colorOFF;

    private void Awake()
    {
        int characterCount = 1;
        while (File.Exists(Application.persistentDataPath + "/Custom_data/CharacterCards" + characterCount + ".txt"))
        {
            characterCount++;
        }
        characterCount--;
        GameStats.CharacterDeckCount = characterCount;

    }

    private void Start()
    {
        consoleTxt.text = "";
        loadSaveInfo.text = "Load your Custom deck first";
        EditName_inputf.text = ""; customDeckName = "";
        UpdateCardsCount(true);
        img = GetComponent<Image>();
        OnClickChooseCurrentDeck();

        CreateCharacterCardLists();

        UI_Normal.SetActive(false); UI_Custom.SetActive(false);
        UI_Items.SetActive(false);
        normal_btn.GetComponent<Image>().color = Color.black;
        custom_btn.GetComponent<Image>().color = Color.black;

        //Choose random card for the showcase image
        int length = normalCardsList.Length - 1; int rand = Random.Range(0, length); string s = normalCardsList[rand];
        Debug.Log("LEngth=" + length);
        GetComponent<SpriteFromAtlas>().SetMansionCardSprite(s);
        ShowCaseCard1.GetComponent<Image>().sprite = img.sprite;

    }

    private void UpdateCardsCount(bool reset)
    {
        if (reset)
        {
            cardCount = 0;
        }
        cardCount = CustomDeckCardsList.Count;

        _cardCount.text = "Cards count: " + cardCount;
    }

    public void OnClickChooseCurrentDeck()
    {
        mainTitleText.text = "Mansion Deck Builder";
        UI_CurrentDeck.SetActive(true);
        UI_AddCards.SetActive(false);
        currentDeck_btn.GetComponent<Image>().color = colorON;
        addCards_btn.GetComponent<Image>().color = colorOFF;
    }
    public void OnClickChooseAddCards()
    {
        if (characterCardsLoaded)
        {
            mainTitleText.text = "Add Character Cards";
            UI_CurrentDeck.SetActive(false);
            UI_AddCards.SetActive(true);
            currentDeck_btn.GetComponent<Image>().color = colorOFF;
            addCards_btn.GetComponent<Image>().color = colorON;
        }
        else
        {
            StartCoroutine(ShowInConsole("Load your Custom Deck first"));
        }
    }

    private void CreateCharacterCardLists()
    {
        CharacterCardsList cards = new();
        normalCardsList = cards.GetAllNormalCharacterCards();
        customCardsList = cards.GetAllCustomCharacterCards();

        CreateCharacterCards(UI_Normal, normalCardsList);
        CreateCharacterCards(UI_Custom, customCardsList);
    }

    private void CreateCharacterCards(GameObject UI_TypeObj, string[] array) //START
    {
        GameObject gridContent = UI_TypeObj.transform.GetChild(0).GetChild(0).gameObject;

        foreach (string s in array)
        {
            GetComponent<SpriteFromAtlas>().SetMansionCardSprite(s);

            GameObject card = Instantiate(AddCardsPrefab);
            card.GetComponent<Image>().sprite = img.sprite;
            card.transform.SetParent(gridContent.transform);
            card.transform.localScale = new Vector3(1f, 1f, 1f);
        }

    }

    public void OnClickSelectNormalType(bool isNormalCard) //0 = normal, 1 = custom
    {
        if (!isButtonLock && characterCardsLoaded)
        {
            if (isNormalCard) // NORMAL CARDS
            {
                UI_Normal.SetActive(true);
                UI_Custom.SetActive(false);

                normal_btn.GetComponent<Image>().color = colorON;
                custom_btn.GetComponent<Image>().color = colorOFF;
            }
            else // CUSTOM CARDS
            {
                UI_Normal.SetActive(false);
                UI_Custom.SetActive(true);

                normal_btn.GetComponent<Image>().color = colorOFF;
                custom_btn.GetComponent<Image>().color = colorON;
            }

        }

    }
    public void OnClickClearCustomDeck()
    {
        if (!isButtonLock)
        {
            CustomDeckCardsList.Clear();

            foreach (Transform child in CharacterGridContent.transform)
            {
                Destroy(child.gameObject);
            }
            UpdateCardsCount(true); //reset = true

            if (characterValue != 0)
                StartCoroutine(ShowInConsole("Custom Deck " + characterValue + " was cleared"));
        }
    }
    public void OnClickLoadData()
    {
        if (!isButtonLock)
        {
            isButtonLock = true;
            //mansionValue = PlayerPrefs.GetInt("MansionType") +1;
            characterValue = GameStats.CharacterDeckValue; // TESTING 12.11.2022
            if (characterValue == 0) characterValue++;
            string file = "CharacterCards" + characterValue;
            LoadTextFileByName(file);

            cardCount = CustomDeckCardsList.Count;

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
        CharactersDropDownHandler.captionText.text = customDeckName;
        Debug.Log("File loaded: " + name + ".txt");
    }

    public void OnClickSaveData()
    {
        if (!isButtonLock)
        {
            isButtonLock = true;

            string writeToFilePath = Application.persistentDataPath + "/Custom_data/" + currentCustomDeck + ".txt";
            CustomDeckCardsList.Sort();
            customDeckName = EditName_inputf.text;
            CharactersDropDownHandler.captionText.text = customDeckName;
            CharactersDropDownHandler.GetComponent<UIEdCharacterLoadDropDownHandler>().ChangeDropItemNameByIndex(characterValue - 1, customDeckName);
            List<string> list = new();
            list.Add(customDeckName);
            foreach (string s in CustomDeckCardsList)
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
        UpdateCardsCount(false); //reset = false
        StartCoroutine(ShowInConsole("Card was deleted from the Custom Deck " + characterValue));

    }
    public void OnUIAddCard(string name)
    {
        CustomDeckCardsList.Add(name);
        GetComponent<SpriteFromAtlas>().SetMansionCardSprite(name);

        GameObject card = Instantiate(CharacterBaseCardPrefab);
        card.GetComponent<Image>().sprite = img.sprite;
        card.transform.SetParent(CharacterGridContent.transform);
        card.transform.localScale = new Vector3(1f, 1f, 1f);
        StartCoroutine(ShowInConsole("'" + name + "'" + " was added to the Custom Deck " + characterValue));
        Debug.Log(name + " added!");
        UpdateCardsCount(false); //reset = false
    }


    private void InstantiateToHandler()
    {
        foreach (Transform child in CharacterGridContent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (string s in CustomDeckCardsList)
        {
            GetComponent<SpriteFromAtlas>().SetMansionCardSprite(s);

            GameObject card = Instantiate(CharacterBaseCardPrefab);
            card.GetComponent<Image>().sprite = img.sprite;
            card.transform.SetParent(CharacterGridContent.transform);
            card.transform.localScale = new Vector3(1f, 1f, 1f);

        }
    }


    private IEnumerator LoadAndSave(bool load) //load = true, save = false
    {
        if (load)
        {
            loadSaveInfo.text = "Loading...";
            yield return new WaitForSeconds(1f);
            InstantiateToHandler(); //!!!!!
            characterCardsLoaded = true;

            loadSaveInfo.text = "";
            isButtonLock = false;
            OnClickSelectNormalType(true);
            StartCoroutine(ShowInConsole("Deck " + characterValue + " loaded successfully"));
        }
        else //Save
        {
            loadSaveInfo.text = "Saving...";
            yield return new WaitForSeconds(1f);
            loadSaveInfo.text = "";
            isButtonLock = false;
            StartCoroutine(ShowInConsole("Deck " + characterValue + " saved successfully"));
        }
    }

    private IEnumerator ShowInConsole(string s)
    {
        consoleTxt.text = s;
        yield return new WaitForSeconds(4f);
        consoleTxt.text = "";

    }

}
