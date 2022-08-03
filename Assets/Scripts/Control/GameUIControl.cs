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
        playerID=PhotonNetwork.LocalPlayer.ActorNumber;
        isLocalPlayerDead = false;
        whoIsPlaying.text = "";
    }
    public void UIPlayerNextTurnStart(string name, int id)
    {
        playerName = name;
        if (playerID == id)
        {
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

            StartCoroutine(ShowTurnInUI());
        }
    }
    IEnumerator ShowTurnInUI()
    {
        whoIsPlaying.text = playerName + "'s turn!";

        yield return new WaitForSeconds(2f);
        whoIsPlaying.text = "";
    }

    IEnumerator PlayerIsDead()
    {
        whoIsPlaying.color = Color.red;
        whoIsPlaying.text = "YOU ARE DEAD!";
        yield return new WaitForSeconds(2f);
        MainCanvas.GetComponent<GameControl>().OnClickTransferOwnershipToNextPlayer();

    }
}
