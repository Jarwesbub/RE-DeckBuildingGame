using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

//Attach this script with same object as "PlayerListControl.cs"

public class PhotonRoomMaster : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject MainCanvas;
    //[SerializeField] private GameObject PlayerListTMP;
    private bool isMaster, isGameScene;
    private string currentScene;
    //private PhotonView view;

    void Awake()
    {
        //view = GetComponent<PhotonView>();
        currentScene = SceneManager.GetActiveScene().name;

    }
    void Start()
    {
        //if (view.IsMine)
        {
            string nickName = PlayerPrefs.GetString("NickName");
            //PhotonNetwork.NickName = nickName;
            PhotonNetwork.LocalPlayer.NickName = nickName;
        }
        isMaster = PhotonNetwork.IsMasterClient;

        if (currentScene == "Game")
        {
            SetCurrentRoomOpen(false);
            isGameScene = true;
        }
        else if (currentScene == "LobbyRoom")
        {
            SetCurrentRoomOpen(true);
            isGameScene = false;
        }
    }
    private void SetCurrentRoomOpen(bool value)
    {
        PhotonNetwork.CurrentRoom.IsOpen = value;
    }

    public void CreateRoomByPassword(string password)
    {
        PhotonNetwork.CreateRoom(password);
    }

    public void JoinRoomByPassword(string password)
    {
        PhotonNetwork.JoinRoom(password);
    }

    public override void OnCreatedRoom() //When Host makes room
    {
        Debug.Log("OnCreatedRoom");

        //PhotonNetwork.LoadLevel("LobbyRoom");

        //GetComponent<PlayerListControl>().UpdateJoinedPlayerList(false,1); //bool = playerLeft, int = id

    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("LobbyRoom");

    }

    public override void OnPlayerEnteredRoom(Player newPlayer) //When others join your room!
    {
        Debug.Log(newPlayer + " OnPlayerEnteredRoom");

        if (isMaster)
            GetComponent<PlayerListControl>().NewPlayerJoined(newPlayer.ActorNumber); //bool = playerLeft, int = id

    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (isGameScene)
            MainCanvas.GetComponent<GameControl>().PlayerLeftRoom(otherPlayer);

        GetComponent<PlayerListControl>().PlayerLeft(otherPlayer.ActorNumber); //bool = playerLeft, int = id

    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        SceneManager.LoadScene("MainMenu");
        

    }
    
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.LoadScene("Lobby");
    }
    
    public void OnClickReturnToLobby() //Works only if new players are not joining
    {
        if (isMaster/* && view.IsMine*/)
        {
            PhotonNetwork.DestroyAll();
            PhotonNetwork.LoadLevel("LobbyRoom");
        }
    }

    public void OnClickReturnToMainMenu()
    {

        PhotonNetwork.Disconnect();

    }
    public void OnClickQuitGame()
    {
        PhotonNetwork.Disconnect();
        Application.Quit();
    }
}

