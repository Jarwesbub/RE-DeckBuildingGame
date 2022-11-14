using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class LobbyControl : MonoBehaviour
{
    public GameObject NameEntry, Lobby, SaveMaster;
    public GameObject CreateRoom, JoinRoom, Back_btn;
    public InputField setNickName;
    public Text errorMessage;
    public TMP_Text infoText;
    public int currentMenu; //0 = Name Entry, 1 = Create/Join lobby, 2 = Create Room, 3 = Join Room
    private int maxNameLength = 16;

    void Start()
    {
        Back_btn.SetActive(false);
        NameEntry.SetActive(true);
        Lobby.SetActive(false);
        CreateRoom.SetActive(false);
        JoinRoom.SetActive(false);
        currentMenu = 0;
        string name = SaveMaster.GetComponent<SaveMaster>().GetNickName();
        if (name != null)
            setNickName.text = name;

        infoText.text = "Give your online nickname.";
    }



    public void OnClickGoToLobby()
    {
        currentMenu = 0;
        errorMessage.text = "";
        string name = setNickName.text;

        if (name == "" || name == " ")
        {
            errorMessage.text = "Enter name!";

        }
        else if (name.Length >= maxNameLength)
        {
            errorMessage.text = "Name too long!";
        }
        else if (name == "jarkko")
        {
            errorMessage.text = "Ei apinoita!";
        }
        else
        {
            SaveMaster.GetComponent<SaveMaster>().SetNickName(name);
            infoText.text = "Host or join online game.";
            currentMenu = 1;
            NameEntry.SetActive(false);
            Lobby.SetActive(true);
            CreateRoom.SetActive(false);
            JoinRoom.SetActive(false);
            Back_btn.SetActive(true);
        }
    }
    public void OnClickMenuCreateRoom()
    {
        currentMenu = 2;
        infoText.text = "Create password for other players to join your game.";
        CreateRoom.SetActive(true);
        JoinRoom.SetActive(false);
        Lobby.SetActive(false);
    }
    public void OnClickMenuJoinRoom()
    {
        currentMenu = 3;
        infoText.text = "Join online game by given password.";
        CreateRoom.SetActive(false);
        JoinRoom.SetActive(true);
        Lobby.SetActive(false);
    }

    public void OnClickMenuBack()
    {
        if (currentMenu <= 1)
            SceneManager.LoadScene("Lobby");
        else if (currentMenu >= 2)
        {
            NameEntry.SetActive(false);
            Lobby.SetActive(true);
            CreateRoom.SetActive(false);
            JoinRoom.SetActive(false);
            currentMenu = 1;
        }
    }

    public void OnClickBackToMainMenu()
    {
        PhotonNetwork.Disconnect();
        //SceneManager.LoadScene("StartMenu");
    }
}
