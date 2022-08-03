using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;


public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField createInput;
    public InputField joinInput;
    private GameObject PlayerInfoMaster;
    public GameObject RoomInfoListPrefab;
    public GameObject PlayerInfoPrefab;
    public GameObject LobbyRoomControlPrefab;
    public GameObject PlayerListTMP;
    [SerializeField] private bool isMaster;
    private string currentScene;

    private PhotonView view;

    void Awake()
    {
        PlayerInfoMaster = GameObject.FindWithTag("PlayerInfoMaster");
        view = GetComponent<PhotonView>();
        currentScene = SceneManager.GetActiveScene().name;
        RoomInfoListPrefab = GameObject.FindWithTag("RoomInfoList"); //FIND SINGLETON
    }
    void Start()
    {
        if(view.IsMine)
        {
            string nickName = PlayerPrefs.GetString("NickName");
            PhotonNetwork.NickName = nickName;

        }
        isMaster = PhotonNetwork.IsMasterClient;

        if (currentScene == "Game")
            PhotonNetwork.CurrentRoom.IsOpen=false;


    }
    public void OnClickCreateRoom()
    {
        PhotonNetwork.CreateRoom(createInput.text);
    }

    public void OnClickJoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }
    public override void OnCreatedRoom() //When Host makes room
    {
        Debug.Log("OnCreatedRoom");

        UpdateJoinedPlayerList();
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("LobbyRoom");

    }

    public override void OnPlayerEnteredRoom(Player newPlayer) //When others join your room!
    {
        Debug.Log(newPlayer +" OnPlayerEnteredRoom");

            if(PhotonNetwork.IsMasterClient)
                UpdateJoinedPlayerList();


    }

    private void UpdateJoinedPlayerList()
    {
        //New
        if (PlayerListTMP != null)
            PlayerListTMP.GetComponent<PlayerListTMP>().UpdatePlayerList();


        //if (view!=null)
            //view.RPC("RPCUpdateJoinedPlayers", RpcTarget.AllBuffered);

    }
    [PunRPC]
    private void RPCUpdateJoinedPlayers()
    {
        if (PlayerListTMP != null)
            PlayerListTMP.GetComponent<PlayerListTMP>().UpdatePlayerList();

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer + " Left Room");

        UpdateJoinedPlayerList();

    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        //SceneManager.LoadScene("Loading");
        SceneManager.LoadScene("StartMenu");
    }

    public void OnClickReturnToLobby()
    {
        if (isMaster)
        {
            view.RPC("RPC_OnClickReturnToLobby", RpcTarget.AllBuffered);
            PhotonNetwork.Destroy(PlayerInfoMaster);
            PhotonNetwork.LoadLevel("LobbyRoom");
        }
    }
    [PunRPC]
    public void RPC_OnClickReturnToLobby()
    {
        PlayerInfoMaster.GetComponent<PlayerInfoMaster>().DestroyAllChilds();
        //SceneManager.LoadScene("LobbyRoom");
    }


    public void OnClickReturnToMainMenu()
    {
        PlayerInfoMaster.GetComponent<PlayerInfoMaster>().DestroyAllChilds();
        Destroy(PlayerInfoMaster);
        //PhotonNetwork.LeaveRoom();
        //PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();
        
    }
    public void OnClickQuitGame()
    {
        Application.Quit();
    }
}
