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
    public GameObject AddCardsPrefab, AllMansionCardsObj;
    public Text dropdownText;
    public TMP_Text currentDeckName, loadSaveInfo;
    public int mansionValue;
    private bool isButtonPressed, mansionCardsLoaded;
    private Image img;

    [SerializeField] private string textFileName;
    [SerializeField] private List<string> TextList;
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
        MansionDropDownHandler.GetComponent<UIEdMansionDropDownHandler>().customCount = value;

        Debug.Log("MansionCards count = " + value);
    }
    private void Start()
    {
        loadSaveInfo.text = "Load your Custom deck first";
        dropdownText.text = "";
        img = GetComponent<Image>();
        OnClickChooseCurrentDeck();
        SetEnemyTierCards();
    }
    public void OnClickChooseCurrentDeck()
    {
        UI_CurrentDeck.SetActive(true);
        UI_AddCards.SetActive(false);

        //Color color = colorON;
        currentDeck_btn.GetComponent<Image>().color = colorON;
        addCards_btn.GetComponent<Image>().color = Color.black;
    }
    public void OnClickChooseAddCards()
    {
        UI_CurrentDeck.SetActive(false);
        UI_AddCards.SetActive(true);

        //Color color = colorON;
        currentDeck_btn.GetComponent<Image>().color = Color.black;
        addCards_btn.GetComponent<Image>().color = colorON;
    }
    private void SetEnemyTierCards()
    {
        GameObject gridContent = UI_EasyTier.transform.GetChild(0).GetChild(0).gameObject;
        

    }



    public void OnClickSelectEnemyTier(int value) //0 = easy, 1 = normal, 2 = hard
    {
        if (!isButtonPressed && mansionCardsLoaded)
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

    public void OnClickLoadCustomMansion()
    {
        if (!isButtonPressed)
        {
            isButtonPressed = true;
            mansionValue = PlayerPrefs.GetInt("MansionType") +1;
            string file = "MansionCards"+mansionValue;
            LoadTextFileByName(file);
            
            StartCoroutine(LoadAndSave(true)); //Load
        }
    }
    public void OnClickSaveCustomMansion()
    {
        if (!isButtonPressed)
        {
            isButtonPressed = true;

            StartCoroutine(LoadAndSave(false)); //Save
        }
    }
    public void OnUIDeleteCard(int childIndex)
    {
        Debug.Log(childIndex + " got deleted!");
        TextList.Remove(TextList[childIndex]);

    }
    public void OnUIÁddCard(string name)
    {
        TextList.Add(name);
        Debug.Log(name + " added!");
    }

    private void LoadTextFileByName(string name)
    {
        string readFromFilePath = Application.persistentDataPath + "/Custom_data/" + name + ".txt";
        List<string> fileLines = File.ReadAllLines(readFromFilePath).ToList();
        TextList = fileLines;
        textListCount = TextList.Count;
        textFileName = name;
        Debug.Log("File loaded: " + name+".txt");
    }

    private void InstantiateToHandler()
    {
        foreach(Transform child in MansionGridContent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach(string s in TextList)
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
            currentDeckName.text = "Custom Deck " + mansionValue;
            loadSaveInfo.text = "";
            isButtonPressed = false;
        }
        else //Save
        {
            loadSaveInfo.text = "Saving...";
            yield return new WaitForSeconds(1f);
            loadSaveInfo.text = "";
            isButtonPressed = false;

        }
    }
}
