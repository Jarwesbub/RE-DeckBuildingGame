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
    public int currentPlayerID;
    //public Text currentPlayer; //DEBUGGING
    public TMP_Text currentPlayer,drawHandCards_tmp;
    private string currentPlayerName;
    public bool onButtonLock; // Adds more time for clients to create MansionDeck - 500 action limit/second! -
    bool isMaster;
    [SerializeField] private int whoIsPlaying, playerCount; //IN USE
    [SerializeField] private int cardCount;
    private int drawHandCardCount;
    private bool isExtraHandCard;

    // Start is called before the first frame update
    void Awake()
    {
        view = GetComponent<PhotonView>();
        isMaster = PhotonNetwork.IsMasterClient;
        currentPlayerID = view.OwnerActorNr;
        ShopMenuObject.SetActive(true);
        MansionMenuObject.SetActive(true);
        EndTurnMenuObject.SetActive(true);
        //if(MainSpawnCards == null)
        //MainSpawnCards = PhotonNetwork.Instantiate(SpawnCardsPrefab.name, new Vector3(0f,0f,0f), Quaternion.identity); //OLD
        MainSpawnCards = Instantiate(SpawnCardsPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity); //NEW
        
        shopMenuOpen = false;
        mansionOpen = false;
    }

    void Start()
    {
        if (isMaster)
        {
            currentPlayerName = PhotonNetwork.NickName;
            //ShowCurrentPlayer(currentPlayerName);
        }
        string hostName = PhotonNetwork.PlayerList[0].NickName;
        UIGameControl.GetComponent<GameUIControl>().UIPlayerNextTurnStart(hostName, currentPlayerID);
        drawHandCardCount = 5;
        isExtraHandCard = false;
        ShowCurrentPlayer(hostName);
        ShopMenuObject.SetActive(false);
        MansionMenuObject.SetActive(false);
        cardCount = MainSpawnCards.GetComponent<SpawnCards>().cardCount;
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
    public void OnClickCardsToDiscardPile()
    {
        if (!onButtonLock)
            if (!shopMenuOpen && !mansionOpen)
            if (view.IsMine)
            {
                GetComponent<MansionCards>().SetMansionAnimation(); //RESET MANSION
                MainSpawnCards.GetComponent<SpawnCards>().PutHandCardsToDiscardPile();
            }
        
        cardCount = MainSpawnCards.GetComponent<SpawnCards>().handCards;
        HPControl.GetComponent<HPContol>().ResetMyHPDifferenceText();
        
    }

    
    public void OnClickTransferOwnershipToNextPlayer() //Important
    {
        if (!onButtonLock)
        {
            if (!shopMenuOpen && !mansionOpen)
            {
                currentPlayerID = view.OwnerActorNr;

                if (view.IsMine)
                {
                    drawHandCards_tmp.text = "Draw " + drawHandCardCount + " cards";
                    view.RPC("PunTransferOwnershipToNextPlayer", RpcTarget.AllBuffered);
                }
            }
        }
    }
    [PunRPC]
    public void PunTransferOwnershipToNextPlayer()
    {
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        if (currentPlayerID >= playerCount)
            currentPlayerID = 1;
        else
            currentPlayerID++;

        Player _player = PhotonNetwork.PlayerList[currentPlayerID - 1];
        

        view.TransferOwnership(_player);
        //Debug.Log("View transfered to: " + _player + "    Player count: " + playerCount);

        currentPlayerName = _player.NickName;
        ShowCurrentPlayer(currentPlayerName);

        UIGameControl.GetComponent<GameUIControl>().UIPlayerNextTurnStart(currentPlayerName, currentPlayerID);

        if (playerCount > 1)
        {
            OtherCharacterControl.GetComponent<CharacterControl>().SetOtherCharacterSprite(_player);
            GetComponent<ShopCards>().UpdateAndResetBuysCount(true);
            //ShopMenuObject.GetComponent<ShopCards>().GetCurrentPlayerFromGameControl(currentPlayerID);
        }
        drawHandCardCount = 5; isExtraHandCard = false;
        drawHandCards_tmp.text = "Draw " + drawHandCardCount + " cards";
    }
    
    public void ShowCurrentPlayer(string name)//
    {
        currentPlayer.text = "Now Playing: " +name;
        
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
