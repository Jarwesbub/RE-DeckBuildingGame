using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.Events;

public class GameUIControl : MonoBehaviourPun
{
    public GameObject PlayerTurnUI, MainCanvas; //Shows current player's deck and buttons
    public TMP_Text whoIsPlaying;
    public bool isLocalPlayerDead;
    public GameObject LocalCharacterCard, OtherCharacterCard, OtherHPInfo;
    public GameObject ShopMenuButton, DeckCountCardsTMPs;
    [SerializeField] string playerName;
    public int playerID;

    void Awake()
    {
        OtherCharacterCard.SetActive(true);
        playerID =PhotonNetwork.LocalPlayer.ActorNumber;
        isLocalPlayerDead = false;
        whoIsPlaying.text = "";
        DeckCountCardsTMPs.SetActive(false);
        playerName = PhotonNetwork.CurrentRoom.GetPlayer(1).NickName;
    }
    /*
    private void Start()
    {
        if (playerID == 1)
        {
            UIMyTurnStart();
            StartCoroutine(ShowMyTurnInUI());
        }
        else
        {       
            string name = PhotonNetwork.PlayerList[0].NickName;
            UIOtherTurnStart(name);
            StartCoroutine(ShowOtherTurnInUI());
        }
    }*/
    public void UIHostStartGame(bool isHost)
    {
        if (isHost)
        {
            PlayerTurnUI.SetActive(true);
            LocalCharacterCard.SetActive(true);
            OtherCharacterCard.SetActive(true);
            OtherHPInfo.SetActive(false);
            ShopMenuButton.SetActive(true);
            DeckCountCardsTMPs.SetActive(true);
            MainCanvas.GetComponent<GameControl>().ShowLocalPlayerHP();
            StartCoroutine(ShowMyTurnInUI());
            OtherCharacterCard.GetComponent<CharacterControl>().GetAllSpritesFromPIMaster();
        }
        else
        {
            LocalCharacterCard.SetActive(true);
            PlayerTurnUI.SetActive(false);
            OtherCharacterCard.SetActive(true);
            OtherHPInfo.SetActive(true);
            ShopMenuButton.SetActive(false);
            DeckCountCardsTMPs.SetActive(false);
            OtherCharacterCard.GetComponent<CharacterControl>().GetAllSpritesFromPIMaster();
            OtherCharacterCard.GetComponent<CharacterControl>().SetOtherCharacterSprite(1);
            
            StartCoroutine(ShowOtherTurnInUI());
        }
    }


    public void UIMyTurnStart()
    {
        if (!isLocalPlayerDead)
        {
            PlayerTurnUI.SetActive(true);
            LocalCharacterCard.SetActive(true);
            OtherCharacterCard.SetActive(false);
            OtherHPInfo.SetActive(false);
            ShopMenuButton.SetActive(true);
            DeckCountCardsTMPs.SetActive(true);
            MainCanvas.GetComponent<GameControl>().ShowLocalPlayerHP();
            StartCoroutine(ShowMyTurnInUI());

        }
        else
        {
            MainCanvas.GetComponent<GameControl>().onButtonLock = true;
            MainCanvas.GetComponent<GameControl>().ShowLocalPlayerHP();
            StartCoroutine(PlayerIsDead());
        }

    }
    public void UIOtherTurnStart(string name, int otherID)
    {
        playerName = name;
        LocalCharacterCard.SetActive(true);
        PlayerTurnUI.SetActive(false);
        OtherCharacterCard.SetActive(true);
        OtherHPInfo.SetActive(true);
        ShopMenuButton.SetActive(false);
        DeckCountCardsTMPs.SetActive(false);
        OtherCharacterCard.GetComponent<CharacterControl>().SetOtherCharacterSprite(otherID);
        StartCoroutine(ShowOtherTurnInUI());
    }

    public void PlayerLeftRoomUI(string name)
    {
        StartCoroutine(ShowPlayerLeftInUI(name));

    }

    IEnumerator ShowMyTurnInUI()
    {
        whoIsPlaying.text = "Your turn!";
        OtherCharacterCard.SetActive(false);
        OtherHPInfo.SetActive(false);
        yield return new WaitForSeconds(2f);
        whoIsPlaying.text = "";
    }
    IEnumerator ShowOtherTurnInUI()
    {
        whoIsPlaying.text = playerName + "'s turn!";
        
        yield return new WaitForSeconds(2f);
        whoIsPlaying.text = "";
    }

    IEnumerator ShowPlayerLeftInUI(string name)
    {
        whoIsPlaying.text = name + " left";

        yield return new WaitForSeconds(2f);
        whoIsPlaying.text = "";
    }

    IEnumerator PlayerIsDead()
    {
        whoIsPlaying.color = Color.red;
        whoIsPlaying.text = "YOU ARE DEAD!";
        yield return new WaitForSeconds(2f);
        MainCanvas.GetComponent<GameControl>().OnClickEndMyTurn();
        //MainCanvas.GetComponent<GameControl>().TransferOwnership();

    }
}
