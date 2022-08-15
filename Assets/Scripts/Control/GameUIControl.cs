using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class GameUIControl : MonoBehaviourPun
{
    public GameObject PlayerTurnUI, MainCanvas; //Shows current player's deck and buttons
    public TMP_Text whoIsPlaying;
    public bool isLocalPlayerDead;
    public GameObject LocalCharacterCard, OtherCharacterCard;
    public GameObject ShopMenuButton;
    [SerializeField] string playerName;
    public int playerID;

    void Awake()
    {
        OtherCharacterCard.SetActive(true);
        playerID =PhotonNetwork.LocalPlayer.ActorNumber;
        isLocalPlayerDead = false;
        whoIsPlaying.text = "";
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
    public void UIHostStartGame()
    {
        PlayerTurnUI.SetActive(true);
        LocalCharacterCard.SetActive(true);
        OtherCharacterCard.SetActive(true);
        ShopMenuButton.SetActive(true);
        MainCanvas.GetComponent<GameControl>().ShowLocalPlayerHP();
        StartCoroutine(ShowMyTurnInUI());

    }


    public void UIMyTurnStart()
    {
        if (!isLocalPlayerDead)
        {
            PlayerTurnUI.SetActive(true);
            LocalCharacterCard.SetActive(true);
            OtherCharacterCard.SetActive(false);
            ShopMenuButton.SetActive(true);
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
    public void UIOtherTurnStart(string name)
    {
        playerName = name;
        LocalCharacterCard.SetActive(true);
        PlayerTurnUI.SetActive(false);
        OtherCharacterCard.SetActive(true);
        ShopMenuButton.SetActive(false);

        StartCoroutine(ShowOtherTurnInUI());
    }

    /*
    public void UIPlayerNextTurnStart(string name, int id) //OLD
    {
        playerName = name;
        if (playerID == id)
        {
            Debug.Log("GameUIControl player = " + name);
            if (!isLocalPlayerDead)
            {
                PlayerTurnUI.SetActive(true);
                LocalCharacterCard.SetActive(true);
                OtherCharacterCard.SetActive(false);
                ShopMenuButton.SetActive(true);
                MainCanvas.GetComponent<GameControl>().ShowLocalPlayerHP();
            }
            else
            {
                MainCanvas.GetComponent<GameControl>().ShowLocalPlayerHP();
                StartCoroutine(PlayerIsDead());
            }
        }
        else
        {
            LocalCharacterCard.SetActive(true);
            PlayerTurnUI.SetActive(false);
            OtherCharacterCard.SetActive(true);
            ShopMenuButton.SetActive(false);

            StartCoroutine(ShowOtherTurnInUI());
        }
    }*/

    public void PlayerLeftRoomUI(string name)
    {
        StartCoroutine(ShowPlayerLeftInUI(name));

    }

    IEnumerator ShowMyTurnInUI()
    {
        whoIsPlaying.text = "Your turn!";

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
