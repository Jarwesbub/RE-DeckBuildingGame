using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIEdMansion_Cards: MonoBehaviour
{
    private GameObject EditMansionControl, ShowCaseCard;
    Image img;

    private void Start()
    {
        EditMansionControl = GameObject.FindWithTag("EditMansionControl");
        ShowCaseCard = GameObject.FindWithTag("UICardShowcase");
        img = GetComponent<Image>();


    }


    public void OnClickDeleteCard()
    {
        int index = transform.GetSiblingIndex();
        EditMansionControl.GetComponent<EditMansionControl>().OnUIDeleteCard(index);
        Destroy(gameObject);
    }

    public void OnClickAddCard()
    {
        string name = img.sprite.name;
        name = name.Replace("(Clone)", "");
        Debug.Log(name+" ADDED!");
        EditMansionControl.GetComponent<EditMansionControl>().OnUIAddCard(name);
    }

    public void OnClickChangeCardShowcaseImage()
    {
        ShowCaseCard.GetComponent<Image>().sprite = img.sprite;

    }
}
