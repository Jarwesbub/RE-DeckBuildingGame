using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeleteCardsControl : MonoBehaviour
{
    public GameObject MainCanvas, ShowDeleteActions, LeftMenuControl, HandCardStatsControl;
    public TMP_Text showDeleteTxt;
    public Button DeleteCard_btn, DontDeleteCard_btn;
    private GameObject currentDeleteCard;
    private int gameMode, handgunsDeleteCount;
    [SerializeField] List<string> AcceptedTrashableCardsList;
    [SerializeField] List<GameObject> EndTurnTrashCardsList;

    private void Start()
    {
        gameMode = GameStats.GameMode;
        AcceptedTrashableCardsList = new List<string>();
        EndTurnTrashCardsList = new List<GameObject>();
        showDeleteTxt.text = "";
        handgunsDeleteCount = 0;
        ShowDeleteActions.SetActive(false);
        
    }


    public void AskDeleteAction(GameObject deleteCard)
    {
        if (gameMode < 2)
        {
            currentDeleteCard = deleteCard;
            ShowDeleteActions.SetActive(true);
        }
        else if (AcceptedTrashListContainsCard(deleteCard.GetComponent<HandCard>().currentCard)) //GameMode 2 -> If card is located in the TrashCards list
        {
            currentDeleteCard = deleteCard;
            ShowDeleteActions.SetActive(true);
        }
        else //Game Mode 2
        {
            currentDeleteCard = deleteCard;
            StartCoroutine(ShowDeletedCardText("Card cannot be deleted!"));
            OnClickDontDeleteCard();
        }
    }

    public void DeleteCardManually(GameObject deleteCard) //onPlayArea -> decrease stats
    {
        string sprtName = deleteCard.GetComponent<Image>().sprite.name;
        deleteCard.GetComponent<HandCard>().DeleteThisCardCompletely(); // Card deletes itself in it's own script
        LeftMenuControl.GetComponent<LeftMenuControl>().DeleteHandCard(sprtName); // Remove card from the left menu
        MainCanvas.GetComponent<GameControl>().ShowDeleteCardInfo(); // Shows deletion info to others

        string cardName = deleteCard.GetComponent<HandCard>().currentCard;
        HandCardStatsControl.GetComponent<HandCardStatsControl>().RemoveCardByName(cardName); // Updates stats
        
    }

    public void OnClickDeleteCard() //Trash a Card platform
    {
        if (currentDeleteCard != null)
        {
            currentDeleteCard.GetComponent<HandCard>().DeleteThisCardCompletely();
            string name = currentDeleteCard.GetComponent<Image>().sprite.name;
            CheckDeletedCardType(name);
            LeftMenuControl.GetComponent<LeftMenuControl>().DeleteHandCard(name);
            MainCanvas.GetComponent<GameControl>().ShowDeleteCardInfo();
            //StartCoroutine(ShowDeletedCardText());
        }
    }
    public void OnClickDontDeleteCard()
    {
        currentDeleteCard.GetComponent<Drag>().JumpBackToLastPosition();
        ShowDeleteActions.SetActive(false);
    }

    public void ShowCardDeletedSuccesfully()
    {
        StartCoroutine(ShowDeletedCardText("Card deleted!"));

    }

    private void CheckDeletedCardType(string name)
    {
        string type = name.Substring(0, 3);
        if (name == "hg_")
        {
            handgunsDeleteCount++;
        }
    }
    public void ResetStats()
    {
        handgunsDeleteCount = 0;
    }

    IEnumerator ShowDeletedCardText(string txt)
    {
        ShowDeleteActions.SetActive(false);
        showDeleteTxt.text = txt;
        yield return new WaitForSeconds(2f);
        showDeleteTxt.text = "";
    }

    public void AcceptedTrashListInsert(string name) // GAME MODE 2 ONLY -> only cards located in array can be trashed
    {
        AcceptedTrashableCardsList.Add(name);
    }
    public void AcceptedTrashListInsertMultiple(string[] names)
    {
        foreach (string name in names)
        {
            AcceptedTrashableCardsList.Add(name);
        }
    }
    public bool AcceptedTrashListContainsCard(string name)
    {
        Debug.Log("Check if TrashListContainsCards: "+name+" "+ AcceptedTrashableCardsList.Contains(name));
        return AcceptedTrashableCardsList.Contains(name);
    }
    public bool AcceptedTrashListContainsMultipleCards(string[] names)
    {
        foreach (string name in names)
        {
            if (AcceptedTrashableCardsList.Contains(name))
                return true;
        }
        return false;
    }
    public void AcceptedTrashListRemove(string name) // GAME MODE 2 ONLY
    {
        if(AcceptedTrashableCardsList.Contains(name))
            AcceptedTrashableCardsList.Remove(name);
    }

    public void AddToEndTurnTrashList(GameObject obj)
    {
        EndTurnTrashCardsList.Add(obj);
    }

    public void EndTurnTrashListActivate()
    {
        foreach (GameObject o in EndTurnTrashCardsList)
        {
            currentDeleteCard = o;
            OnClickDeleteCard();
            
        }
        EndTurnTrashCardsList.Clear();
        AcceptedTrashableCardsList.Clear();
    }
}
