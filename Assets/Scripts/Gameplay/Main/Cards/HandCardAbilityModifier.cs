using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCardAbilityModifier: MonoBehaviour
{
    public GameObject HPControl, UIControl;
    private List<GameObject> trashObjectsThisTurn;
    
    private void Start()
    {
        trashObjectsThisTurn = new List<GameObject>();
    }
    public void PlayerTurnEnds()
    {
        TrashAllTheCardsInTheList();
        UIControl.GetComponent<DeleteCardsControl>().ResetStats();
    }

    public void AddOrRemovePlayerHP(int value)
    {
        HPControl.GetComponent<HPContol>().AddOrRemoveHP(value);
    }

    public void TrashACardInstantly(GameObject deleteCard)
    {
        Debug.Log("TrashACard: "+deleteCard.name);
        UIControl.GetComponent<DeleteCardsControl>().DeleteCardManually(deleteCard);
    }

    public void TrashACardWhenTurnEnds(GameObject trashCard) //Trash all the cards in the list when turn is over
    {
        trashObjectsThisTurn.Add(trashCard);
    }
    private void TrashAllTheCardsInTheList()
    {
        foreach(GameObject o in trashObjectsThisTurn)
        {
            Debug.Log("TrashACard: " + o.name);
            UIControl.GetComponent<DeleteCardsControl>().DeleteCardManually(o);
        }
        trashObjectsThisTurn.Clear();
    }

}
