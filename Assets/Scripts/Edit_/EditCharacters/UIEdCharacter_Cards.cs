using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIEdCharacter_Cards : MonoBehaviour
{
    private GameObject EdCharacterControl, ShowCaseCard;
    Image img;

    private void Start()
    {
        EdCharacterControl = GameObject.FindWithTag("EditMansionControl");
        ShowCaseCard = GameObject.FindWithTag("UICardShowcase");
        img = GetComponent<Image>();


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
