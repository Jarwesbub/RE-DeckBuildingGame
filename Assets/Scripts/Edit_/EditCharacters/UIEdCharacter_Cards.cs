using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIEdCharacter_Cards : MonoBehaviour
{
    private GameObject EdCharacterControl, ShowCaseCard;
    private Image img;
    [SerializeField] TMP_Text supportedText;

    private void Start()
    {
        EdCharacterControl = GameObject.FindWithTag("EditMansionControl");
        ShowCaseCard = GameObject.FindWithTag("UICardShowcase");
        img = GetComponent<Image>();

        CharacterCardsList cardsList = new();
        string name = img.sprite.name;
        name = name.Replace("(Clone)", "");
        if (cardsList.CheckIfCardIsSupported(name))
        {
            supportedText.text = "Supported";
            supportedText.color = Color.green;
        } 
        else
        {
            supportedText.text = "Not supported";
            supportedText.color = Color.red;
        }

    }


    public void OnClickDeleteCard()
    {
        int index = transform.GetSiblingIndex();
        EdCharacterControl.GetComponent<EditCharacterControl>().OnUIDeleteCard(index);
        Destroy(gameObject);
    }

    public void OnClickAddCard()
    {
        string name = img.sprite.name;
        name = name.Replace("(Clone)", "");
        Debug.Log(name + " ADDED!");
        EdCharacterControl.GetComponent<EditCharacterControl>().OnUIAddCard(name);
    }

    public void OnClickChangeCardShowcaseImage()
    {
        ShowCaseCard.GetComponent<Image>().sprite = img.sprite;

    }
}
