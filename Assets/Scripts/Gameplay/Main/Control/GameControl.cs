using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI; //DEBUGGING
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

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
    bool isMaster;
    private Player currentPlayer;
    [SerializeField] private int myPlayerID, whoIsPlaying, playerCount; //IN USE
    [SerializeField] private int cardCount;
    private int drawHandCardCount;
    private bool isExtraHandCard;


    void Awake()
    {
        MansionMenuObject.SetActive(true);
        view = GetComponent<PhotonView>();
        isMaster = PhotonNetwork.IsMasterClient;
        myPlayerID = PhotonNetwork.LocalPlayer.ActorNumber;
        currentPlayerID = 1; //HOST
        ShopMenuObject.SetActive(true);     
        EndTurnMenuObject.SetActive(true);
        MainSpawnCards = Instantiate(SpawnCardsPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity); //NEW
        
        shopMenuOpen = false;
        mansionOpen = false;
        currentRound = 0;
    }

    void Start()
    {
        currentPlayer = PhotonNetwork.PlayerList[0];
        string hostName = currentPlayer.NickName;
        GameStats.CurrentPlayerID = currentPlayerID;

        UIGameControl.GetComponent<GameUIControl>().UIHostStartGame(isMaster); //true = is host, false = is not host

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
        {
            ShopMenuObject.SetActive(false); //If player leaves
            MansionMenuObject.SetActive(false); //If player leaves

            if (view.IsMine)
            {
                SendCardsToDiscardPile();
                TransferOwnership();
            }
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
            GetComponent<MansionControl>().ResetMansionAnimation(); //RESET MANSION
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

            currentPlayerID = _player.ActorNumber;
            
            if (view.IsMine)
            {
                view.TransferOwnership(_player);
                Hashtable hash = new Hashtable();
                hash.Add("currentPlayerID", currentPlayerID);
                PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

                
            }
        }
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        int id = (int)changedProps["currentPlayerID"];
        //if(/*!view.IsMine && */targetPlayer == view.Owner)
        //if(id!=currentPlayerID)
        {
            Player player = PhotonNetwork.CurrentRoom.GetPlayer(id);
            SetNextPlayer(player);
        }
    }
    private void SetNextPlayer(Player player)
    {
        currentPlayer = player;
        currentPlayerID = player.ActorNumber;
        GameStats.CurrentPlayerID = currentPlayerID; //TESTING
        string name = player.NickName;
        playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        Debug.Log("View transfered to: " + player);
        ShowCurrentPlayer(name);

        Debug.Log("Current player: " + currentPlayerID + " myID = " + myPlayerID);
        if (currentPlayerID == myPlayerID)
            UIGameControl.GetComponent<GameUIControl>().UIMyTurnStart();
        else
            UIGameControl.GetComponent<GameUIControl>().UIOtherTurnStart(name, currentPlayerID);

        GetComponent<ShopControl>().UpdateAndResetBuysCount(true);
        GetComponent<MansionControl>().MansionSetForNextPlayer(name);
        /*
        if (playerCount > 1)
        {   //BUG Index out of range error when HOST ends turn and loads spritedata from "unenabled object" (Development build only)
            //OtherCharacterControl.GetComponent<CharacterControl>().SetOtherCharacterSprite(currentPlayerID);
        }*/
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
                GetComponent<ShopControl>().SetAllCardsToNormalSize();
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
        if (view.IsMine && !GetComponent<MansionControl>().IsButtonLockActive())
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
        GetComponent<MansionControl>().ResetMansionAnimation();
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

    public void ShowDeleteCardInfo()
    {
        view.RPC("PRC_ShowDeleteCardInfo", RpcTarget.AllBuffered);
    }
    [PunRPC] void PRC_ShowDeleteCardInfo()
    {
        UIGameControl.GetComponent<DeleteCardsControl>().ShowDeletedCardInfo();
    }


    /*
    private void OnDestroy()
    {
        if (view.IsMine)
            PhotonNetwork.Destroy(view);
    }*/
}
