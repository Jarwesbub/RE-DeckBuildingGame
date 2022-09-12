using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using System.Linq;

public class MansionCards : MonoBehaviourPun
{
    public GameObject MansionCard, MansionDoor, MansionCardPrefab, MansionGridContent, MansionHandCardPrefab;
    public GameObject LeftMenuControl, MansionDoor_btnUI;
    private Image image;
    PhotonView view;
    public GameObject MansionActionButtons, OtherActionButtons, ToggleOtherActions;
    public TMP_Text mansionActionTMP, mansionDeckCountTMP, exploreCountTMP;
    public int MansionDeckCount, MansionExploreCount;
    [SerializeField] private string enemyName;
    public List<string> mansionDeck;
    private string[] mansionDeckStringList;
    private string mansionTxt;
    private bool activeOtherActionBtn, doorKnobLock, isBottomCard;
    private int clickMansionDoorValue;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        image = MansionCard.GetComponent<Image>();
    }
    private void Start()
    {
        mansionTxt = "";
        mansionActionTMP.text = mansionTxt;
        MansionExploreCount = 0;
        exploreCountTMP.text = "Explores this turn: " + 0;
        
        MansionDoor.SetActive(true);
        MansionDoor_btnUI.SetActive(false);
        activeOtherActionBtn = false;
        MansionActionButtons.SetActive(false);
        OtherActionButtons.SetActive(false);
        ToggleOtherActions.SetActive(false);

        SetMansionAnimation();

        if (PhotonNetwork.IsMasterClient)
        {
            mansionDeckStringList = SetMansionCardsList();
            ShuffleMansionDeck();
            StartCoroutine(AddDelay_SendMansionDeckToClients());
        }

    }

    private string[] SetMansionCardsList()
    {
        int listCount = MansionCardPrefab.GetComponent<TextFileToList>().textListCount;

        string[] cardList = new string[listCount];
        for (int i = 0; i < listCount; i++)
        {
            string card = MansionCardPrefab.GetComponent<TextFileToList>().GetStringFromTextByNumber(i);
            cardList[i] = card;
        }

        return cardList;
    }
    IEnumerator AddDelay_SendMansionDeckToClients() // Gives clients more time to do actions - 500 action limit/second ! -
    {
        GetComponent<GameControl>().onButtonLock = true;
        yield return new WaitForSeconds(1f);
        view.RPC("RPC_SendMansionDeck", RpcTarget.AllBuffered, (object)mansionDeckStringList);
        yield return new WaitForSeconds(1f);
        GetComponent<GameControl>().onButtonLock = false;
    }
    [PunRPC]
    public void RPC_SendMansionDeck(string[] deck)
    {
        foreach(string m in deck)
        {
            mansionDeck.Remove(m);
        }
        mansionDeckStringList = deck;

        foreach(string s in deck)
        {
            mansionDeck.Add(s);
        }
        MansionDeckCount = mansionDeck.Count;
        mansionDeckCountTMP.text = "Mansion card count: " + MansionDeckCount;
    }

    private void ShuffleMansionDeck() //
    {
        int count = mansionDeckStringList.Length;
        for (int i = 0; i < count; i++)
        {
            string temp = mansionDeckStringList[i];
            int randomIndex = Random.Range(i, mansionDeckStringList.Length);
            mansionDeckStringList[i] = mansionDeckStringList[randomIndex];
            mansionDeckStringList[randomIndex] = temp;
        }
    }
    public void MansionDoorReset()
    {
        OnClickSetMansionTextEmpty();
        MansionActionButtons.SetActive(false);
        ToggleOtherActions.SetActive(false);
        OtherActionButtons.SetActive(false);
        isBottomCard = false;
        doorKnobLock = false;
    }

    public void DoorAnimationEnds()
    {
        if (view.IsMine)
        {
            MansionActionButtons.SetActive(true);
            MansionExploreAdd(true);
        }
    }
    public void MansionExploreAdd(bool add)
    {
        if(add)
            MansionExploreCount++;
        else
            MansionExploreCount = 0;

        exploreCountTMP.text = "Explores this turn: " + MansionExploreCount;
    }

    public void SetMansionAnimation()
    {
        if(view.IsMine)
            view.RPC("RPC_SetMansionAnimation", RpcTarget.AllBuffered);

    }
    [PunRPC]
    void RPC_SetMansionAnimation()
    {
        MansionDoor.SetActive(true);
        //MansionMainButton.SetActive(false);
    }

    ////////////////////

    public void ClickEnterMansion(int clickValue)
    {
        if (view.IsMine && !doorKnobLock)
        {      
            if (clickValue == 1) //Mouse1 button
            {
                doorKnobLock = true;
                isBottomCard = false;
                view.RPC("RPC_ClickEnterMansion", RpcTarget.AllBuffered, 0); //Normal

            }
            else if (clickValue == 2) //Mouse2 button
            {
                doorKnobLock = true;
                isBottomCard = true;
                clickMansionDoorValue = clickValue;
                MansionDoor_btnUI.SetActive(true);
            }
            

        }
    }
    public void OnClickChooseBottomCard(bool chooseBottomCard)
    {

        if (chooseBottomCard) //Choose bottom card
        {
            int value = MansionDeckCount-1;          
            view.RPC("RPC_ClickEnterMansion", RpcTarget.AllBuffered, value); 
        }

        clickMansionDoorValue = 0;
        MansionDoor_btnUI.SetActive(false);
        //doorKnobPressed = false;
    }
    [PunRPC]
    void RPC_ClickEnterMansion(int value)
    {
        if (value!=0)
            StartCoroutine(MansionDoorOpening("Bottom card"));

        MansionCard.GetComponent<SpriteFromAtlas>().SetMansionCardSprite(mansionDeck[value]);
        MansionDoor.GetComponent<MansionDoor>().OpenMansionDoor();
        
    }

    ///////////////////////////
    
    public void OnClickOtherButton()
    {
        if(activeOtherActionBtn)
        {
            ToggleOtherActions.SetActive(false);
            activeOtherActionBtn = false;
        }
        else
        {
            ToggleOtherActions.SetActive(true);
            activeOtherActionBtn = true;
        }

    }
    
    public void OnClickDeleteCard() //WIN button
    {
        int i = GetComponent<GameControl>().currentPlayerID - 1;
        string nickName = PhotonNetwork.PlayerList[i].NickName;

        if (view.IsMine)
        {
            GameObject mansionHandCard = Instantiate(MansionHandCardPrefab);
            mansionHandCard.GetComponent<Image>().sprite = image.sprite;
            //mansionHandCard.transform.parent = MansionGridContent.transform;
            mansionHandCard.transform.SetParent(MansionGridContent.transform);
            mansionHandCard.transform.localScale = new Vector3(1f, 1f, 1f);

            view.RPC("RPC_DeleteCard", RpcTarget.AllBuffered, isBottomCard);
            string txt = nickName + " WINS" + "\n" + "\n" + "Player's Mansion deck updated";
            view.RPC("SetMansionActionText", RpcTarget.AllBuffered, txt);
            MansionActionButtons.SetActive(false); OtherActionButtons.SetActive(true);
            LeftMenuControl.GetComponent<LeftMenuControl>().SetPlus1Text();
        }
    }
    [PunRPC]
    public void RPC_DeleteCard(bool chooseBottomCard)
    {
        int value = 0;
        if (chooseBottomCard)
            value = MansionDeckCount-1;
        
        mansionDeck.Remove(mansionDeck[value]);
        MansionDeckCount = mansionDeck.Count;
        mansionDeckCountTMP.text = "Mansion card count: " + MansionDeckCount;
    }
    ///////////////////////////
    public void OnClickSendCardBottomOfTheDeck() //LOSE btn
    {
        int i = GetComponent<GameControl>().currentPlayerID - 1;
        string nickName = PhotonNetwork.PlayerList[i].NickName;

        if (view.IsMine)
        {
            view.RPC("RPC_SendCardBottomOfTheDeck", RpcTarget.AllBuffered, isBottomCard);
            clickMansionDoorValue = 0;
            string txt = nickName + " LOSE" +"\n"+ "\n" + "Player takes damage";
            view.RPC("SetMansionActionText", RpcTarget.AllBuffered, txt);
            MansionActionButtons.SetActive(false); OtherActionButtons.SetActive(true);
        }
    }
    [PunRPC]
    private void RPC_SendCardBottomOfTheDeck(bool chooseBottomCard)
    {
        if(!chooseBottomCard)
        {
            mansionDeck.Add(mansionDeck[0]);
            mansionDeck.Remove(mansionDeck[0]);
        }
    }
    ///////////////////////////
    public void OnClickShuffleDeck() //SHUFFLE btn
    {
        if (view.IsMine)
        {
            int i = GetComponent<GameControl>().currentPlayerID - 1;
            string nickName = PhotonNetwork.PlayerList[i].NickName;
            mansionDeckStringList = mansionDeck.ToArray();
            ShuffleMansionDeck();
            
            view.RPC("RPC_SendMansionDeck", RpcTarget.AllBuffered, (object)mansionDeckStringList);
            string txt = nickName+ " LOSE" + "\n" + "\n" + "Mansion deck shuffled";
            view.RPC("SetMansionActionText", RpcTarget.AllBuffered, txt);
        }
    }

    [PunRPC]
    private void SetMansionActionText(string txt)
    {
        mansionTxt = txt;
        mansionActionTMP.text = mansionTxt;
        //StartCoroutine(ShowMansionText());
    }


    public void OnClickSetMansionTextEmpty()
    {
        string txt = "";
        view.RPC("SetMansionActionText", RpcTarget.AllBuffered, txt);

    }

    IEnumerator MansionDoorOpening(string doorText)
    {
        mansionActionTMP.text = doorText;
        yield return new WaitForSeconds(4f);
        mansionActionTMP.text = "";
    }
}
