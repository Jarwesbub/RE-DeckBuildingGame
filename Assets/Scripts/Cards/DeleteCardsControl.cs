using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class DeleteCardsControl : MonoBehaviour
{
    public GameObject ShowDeleteActions, LeftMenuControl;
    public TMP_Text showDeleteTxt;
    public Button DeleteCard_btn, DontDeleteCard_btn;
    private GameObject currentDeleteCard;

    private void Start()
    {
        showDeleteTxt.text = "";
        ShowDeleteActions.SetActive(false);
    }


    public void AskDeleteAction(GameObject deleteCard)
    {
        currentDeleteCard = deleteCard;
        ShowDeleteActions.SetActive(true);
    }

    public void OnClickDeleteCard()
    {
        currentDeleteCard.GetComponent<HandCard>().DeleteThisCardCompletely();
        string name = currentDeleteCard.GetComponent<Image>().sprite.name;
        LeftMenuControl.GetComponent<LeftMenuControl>().DeleteHandCard(name);
        StartCoroutine(ShowDeletedCardText());
    }
    public void OnClickDontDeleteCard()
    {
        currentDeleteCard.GetComponent<Drag>().JumpBackToLastPosition();
        ShowDeleteActions.SetActive(false);
    }


    IEnumerator ShowDeletedCardText()
    {
        ShowDeleteActions.SetActive(false);
        showDeleteTxt.text = "Card deleted";
        yield return new WaitForSeconds(2f);
        showDeleteTxt.text = "";
    }

}
