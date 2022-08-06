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
    public GameObject LeftMenuControl;
    private Image image;
    PhotonView view;
    public GameObject MansionActionButtons, OtherActionButtons;
    public TMP_Text mansionActionTMP;
    [SerializeField] private string enemyName;
    public List<string> mansionDeck;
    private string[] mansionDeckStringList;
    private string mansionTxt;
    private bool activeOtherActionBtn;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        image = MansionCard.GetComponent<Image>();
    }
    private void Start()
    {
        mansionTxt = "";
        mansionActionTMP.text = mansionTxt;
        
        MansionDoor.SetActive(true);
        activeOtherActionBtn = false;
        MansionActionButtons.SetActive(false);
        OtherActionButtons.SetActive(false);

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
    }

    public void DoorAnimationEnds()
    {
        if(view.IsMine)
            MansionActionButtons.SetActive(true);
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

    public void OnClickEnterMansion()
    {
        if (view.IsMine)
            view.RPC("RPC_OnClickEnterMansion", RpcTarget.AllBuffered);
    }
    [PunRPC]
    void RPC_OnClickEnterMansion()
    {
        MansionCard.GetComponent<SpriteFromAtlas>().SetMansionCardSprite(mansionDeck[0]);
        MansionDoor.GetComponent<MansionDoor>().OpenMansionDoor();
        
    }


    ///////////////////////////
    public void OnClickOtherButton()
    {
        if(activeOtherActionBtn)
        {
            OtherActionButtons.SetActive(false);
            activeOtherActionBtn = false;
        }
        else
        {
            OtherActionButtons.SetActive(true);
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
            mansionHandCard.transform.parent = MansionGridContent.transform;
            mansionHandCard.transform.localScale = new Vector3(1f, 1f, 1f);

            view.RPC("RPC_DeleteCard", RpcTarget.AllBuffered);
            string txt = nickName + " WINS" + "\n" + "\n" + "Player's Mansion deck updated";
            view.RPC("SetMansionActionText", RpcTarget.AllBuffered, txt);
            MansionActionButtons.SetActive(false);
            LeftMenuControl.GetComponent<LeftMenuControl>().SetPlus1Text();
        }
    }
    [PunRPC]
    public void RPC_DeleteCard()
    {
        mansionDeck.Remove(mansionDeck[0]);
    }
    ///////////////////////////
    public void OnClickSendCardBottomOfTheDeck() //LOSE btn
    {
        int i = GetComponent<GameControl>().currentPlayerID - 1;
        string nickName = PhotonNetwork.PlayerList[i].NickName;

        if (view.IsMine)
        //if(view.OwnerActorNr== currentPlayer)
        {
            view.RPC("RPC_SendCardBottomOfTheDeck", RpcTarget.AllBuffered);
            string txt = nickName + " LOSE" +"\n"+ "\n" + "Player takes damage";
            view.RPC("SetMansionActionText", RpcTarget.AllBuffered, txt);
            MansionActionButtons.SetActive(false);
        }
    }
    [PunRPC]
    private void RPC_SendCardBottomOfTheDeck()
    {
        mansionDeck.Add(mansionDeck[0]);
        mansionDeck.Remove(mansionDeck[0]);
    }
    ///////////////////////////
    public void OnClickShuffleDeck() //SHUFFLE btn
    {
        int i = GetComponent<GameControl>().currentPlayerID - 1;
        string nickName = PhotonNetwork.PlayerList[i].NickName;
        if (view.IsMine)
        {       
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

}