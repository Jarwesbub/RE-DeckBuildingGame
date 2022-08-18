using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI; //DEBUGGING
using TMPro;

public class GameControl : MonoBehaviourPunCallbacks
{
    PhotonView view;
    public GameObject SpawnCardsPrefab, UIGameControl, OtherCharacterControl, HPControl;
    private GameObject MainSpawnCards;
    public GameObject EndTurnMenuObject, ShopMenuObject, MansionMenuObject;
    private bool shopMenuOpen, mansionOpen;
    public int currentPlayerID, currentRound;
    //public Text currentPlayer; //DEBUGGING
    public TMP_Text currentPlayerTxt,drawHandCards_tmp, currentRoundTxt;
    //[SerializeField] private List<string> playerNamesList;
    //private string currentPlayerName;
    public bool onButtonLock; // Adds more time for clients to create MansionDeck - 500 action limit/second! -
    bool isMaster, isMyView;
    private Player currentPlayer;
    [SerializeField] private int myPlayerID, whoIsPlaying, playerCount; //IN USE
    [SerializeField] private int cardCount;
    private int drawHandCardCount;
    private bool isExtraHandCard;

    // Start is called before the first frame update
    void Awake()
    {
        MansionMenuObject.SetActive(true);
        view = GetComponent<PhotonView>();
        isMaster = PhotonNetwork.IsMasterClient;
        isMyView = view.IsMine;
        myPlayerID = PhotonNetwork.LocalPlayer.ActorNumber;
        currentPlayerID = 1; //HOST
        ShopMenuObject.SetActive(true);     
        EndTurnMenuObject.SetActive(true);
        //if(MainSpawnCards == null)
        //MainSpawnCards = PhotonNetwork.Instantiate(SpawnCardsPrefab.name, new Vector3(0f,0f,0f), Quaternion.identity); //OLD
        MainSpawnCards = Instantiate(SpawnCardsPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity); //NEW
        
        shopMenuOpen = false;
        mansionOpen = false;
        currentRound = 0;
    }

    void Start()
    {
        currentPlayer = PhotonNetwork.PlayerList[0];
        string hostName = currentPlayer.NickName;
        if (isMaster)
        {
            UIGameControl.GetComponent<GameUIControl>().UIHostStartGame();
            //currentPlayerName = PhotonNetwork.NickName;
            //ShowCurrentPlayer(currentPlayerName);
        }
        else
        {
            UIGameControl.GetComponent<GameUIControl>().UIOtherTurnStart(hostName);
        }
        drawHandCardCount = 5;
        isExtraHandCard = false;
        ShowCurrentPlayer(hostName);
        ShopMenuObject.SetActive(false);
        MansionMenuObject.SetActive(false);
        cardCount = MainSpawnCards.GetComponent<SpawnCards>().cardCount;

        RoundChanges();
    }
    public void NewHandCardSpawned()
    {
        if (!isExtraHandCard)
        {
            cardCount--;
            drawHandCardCount--;
            if (drawHandCardCount > 0)
            {
                drawHandCards_tmp.text = "Draw " + drawHandCardCount + " cards";
            }
            else
            {
                drawHandCards_tmp.text = "";
                isExtraHandCard = true;
            }
        }
        else
        {
            cardCount--;
            drawHandCardCount++;
            drawHandCards_tmp.text = "Extra cards: " + drawHandCardCount;
        }
    }
    public void DeckIsShuffled()
    {
        drawHandCards_tmp.text = "Deck shuffled";
    }

    public void OnClickDrawCard()
    {
        if (!onButtonLock)
        {
            if (view.IsMine)
                MainSpawnCards.GetComponent<SpawnCards>().DrawCard();
        }
    }
    public void OnClickEndMyTurn() //End turn-, Exit to main menu- and Quit game buttons
    {
        if (!onButtonLock)
        if (view.IsMine)
        {
            SendCardsToDiscardPile();
            TransferOwnership();
        }
    }
    public void PlayerLeftRoom(Player player)
    {
        if (currentPlayer == player)
            OnClickEndMyTurn();

        UIGameControl.GetComponent<GameUIControl>().PlayerLeftRoomUI(player.NickName);
    }
    private void SendCardsToDiscardPile()
    {
        if (!shopMenuOpen && !mansionOpen)
        {
            GetComponent<MansionCards>().SetMansionAnimation(); //RESET MANSION
            MainSpawnCards.GetComponent<SpawnCards>().PutHandCardsToDiscardPile();
        }
        
        cardCount = MainSpawnCards.GetComponent<SpawnCards>().handCards;
        HPControl.GetComponent<HPContol>().ResetMyHPDifferenceText();
        
    }

    
    private void TransferOwnership() //Important
    {
        if (!shopMenuOpen && !mansionOpen)
        {
                drawHandCards_tmp.text = "Draw " + drawHandCardCount + " cards";
                Player _player = view.Owner.GetNext();

                if (_player == null)
                    _player = PhotonNetwork.PlayerList[0]; //HOST

                view.TransferOwnership(_player);
                view.RPC("Pun_TransferOwnership", RpcTarget.AllBuffered, _player);
        }
    }
    [PunRPC]
    private void Pun_TransferOwnership(Player player)
    {
        currentPlayer = player;
        currentPlayerID = player.ActorNumber;
        string currentPlayerName = player.NickName;
        playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        Debug.Log("View transfered to: "+player);
        //currentPlayerName = playerNamesList[currentPlayerID - 1];
        ShowCurrentPlayer(currentPlayerName);

        Debug.Log("Current player: " + currentPlayerID + " myID = " + myPlayerID);
        if (currentPlayerID == myPlayerID)
            UIGameControl.GetComponent<GameUIControl>().UIMyTurnStart();
        else
            UIGameControl.GetComponent<GameUIControl>().UIOtherTurnStart(currentPlayerName);

        //UIGameControl.GetComponent<GameUIControl>().UIPlayerNextTurnStart(currentPlayerName, currentPlayerID);
        GetComponent<ShopCards>().UpdateAndResetBuysCount(true);

        if (playerCount > 1)
        {   //BUG Index out of range error when HOST ends turn and loads spritedata from "unenabled object" (Development build only)
            OtherCharacterControl.GetComponent<CharacterControl>().SetOtherCharacterSprite(currentPlayerID);
        }
        if (currentPlayerID == 1)
            RoundChanges();
        drawHandCardCount = 5; isExtraHandCard = false;
        drawHandCards_tmp.text = "Draw " + drawHandCardCount + " cards";
    }
    
    public void ShowCurrentPlayer(string name)//
    {
        currentPlayerTxt.text = "Now Playing: " +name;
        
    }
    private void RoundChanges()
    {
        currentRound++;
        currentRoundTxt.text = "Round: " + currentRound;
    }


    public void OnClickShopMenuButton()
    {
        if (!onButtonLock)
        if (view.IsMine && !mansionOpen)
        {
            if (shopMenuOpen)
            {
                GetComponent<ShopCards>().SetAllCardsToNormalSize();
                EndTurnMenuObject.SetActive(true);
                shopMenuOpen = false;
            }
            else
            {
                EndTurnMenuObject.SetActive(false);
                shopMenuOpen = true;
            }
            
            view.RPC("RPC_OnClickShopMenuButton", RpcTarget.AllBuffered, shopMenuOpen);
        }
    }


    [PunRPC]
    public void RPC_OnClickShopMenuButton(bool isGettingIn)
    {
        if (!isGettingIn)
        {
            shopMenuOpen = false;
            ShopMenuObject.SetActive(false);
        }
        else
        {
            shopMenuOpen = true;
            ShopMenuObject.SetActive(true);
        }
    }

    public void OnClickOpenMansionMenu()
    {
        if (!onButtonLock)
        if (view.IsMine)
        {
            if(mansionOpen)
                EndTurnMenuObject.SetActive(true);
            else
                EndTurnMenuObject.SetActive(false);

            view.RPC("Pun_OnClickOpenMansionMenu", RpcTarget.AllBuffered);
        }
    }
    [PunRPC]
    public void Pun_OnClickOpenMansionMenu()
    {
        if(!shopMenuOpen)
        if (mansionOpen)
        {
            MansionMenuObject.SetActive(false);
            mansionOpen = false;
        }
        else
        {
            MansionMenuObject.SetActive(true);
            mansionOpen = true;
        }
    }
    public void OnClickResetMansion()
    {
        GetComponent<MansionCards>().SetMansionAnimation();
    }


    public void ShowLocalPlayerHP()
    {
        //if (view.IsMine)
        {
            float hp = HPControl.GetComponent<HPContol>().myHP;
            float maxhp = HPControl.GetComponent<HPContol>().myMaxHP;

            Debug.Log("ShowPlayer:" + view.OwnerActorNr + " HP = "+hp+" MaxHP"+maxhp);
            view.RPC("RPC_ShowLocalPlayerHP", RpcTarget.AllBuffered,hp,maxhp);
        }
    }
    [PunRPC]
    void RPC_ShowLocalPlayerHP(float hp, float maxhp)
    {
        HPControl.GetComponent<HPContol>().ShowHPToOthers(hp, maxhp);

    }

    private void OnDestroy()
    {
        if (view.IsMine)
            PhotonNetwork.Destroy(view);
    }
}
