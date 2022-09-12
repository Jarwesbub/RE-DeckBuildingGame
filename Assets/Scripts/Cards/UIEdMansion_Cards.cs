using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIEdMansion_Cards: MonoBehaviour
{
    private GameObject EditMansionControl, ShowCaseCard;
    Image img;
    private bool isZoomed;
    public TMP_Text newCardAdded;

    private void Start()
    {
        EditMansionControl = GameObject.FindWithTag("EditMansionControl");
        ShowCaseCard = GameObject.FindWithTag("UICardShowcase");
        img = GetComponent<Image>();

        if(newCardAdded!=null)
            newCardAdded.text = "";
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
        EditMansionControl.GetComponent<EditMansionControl>().OnUIÁddCard(name);
    }

    public void OnClickChangeCardShowcaseImage()
    {
        ShowCaseCard.GetComponent<Image>().sprite = img.sprite;

    }

    IEnumerator ShowInfoText()
    {
        newCardAdded.text = "New card added!";
        yield return new WaitForSeconds(2f);
        newCardAdded.text = "";
    }
}
