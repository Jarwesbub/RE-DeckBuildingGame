using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class LobbyRoomOpen : MonoBehaviourPunCallbacks
{
    public GameObject PlayerInfoPrefab;
    public GameObject CharacterList;
    [SerializeField] private GameObject[] playerInfoList;
    [SerializeField] private int myID;
    [SerializeField] private string myCharacterCard;
    public GameObject startGame_btn;
    private bool isMaster, readyForGameRoom;
    Vector2 playerInfoPos = new Vector2(-6.9f, -1.6f);
    PhotonView view;

    

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true; //Works with PhotonNetwork.LoadLevel();
        view = GetComponent<PhotonView>();
        myID = view.OwnerActorNr;
        isMaster = PhotonNetwork.IsMasterClient;

            if(view.IsMine)
                startGame_btn.SetActive(true);
            else
                startGame_btn.SetActive(false);
    }

    public void ReadyForGameScene() //Accessed from OverWriteTextFileList (attached to this main object)
    {
        if(view.IsMine)
        view.RPC("RPC_OnClickGoToGameScene", RpcTarget.AllBuffered);

    }
    [PunRPC]
    public void RPC_OnClickGoToGameScene()
    {
        myCharacterCard = CharacterList.GetComponent<TextFileToList>().GetRandomizedCharacterName();
        PlayerPrefs.SetString("myCharacterCard", myCharacterCard);

        object[] myCustomInitData = new object[]
        {
                myCharacterCard

        };
        PhotonNetwork.Instantiate(PlayerInfoPrefab.name, playerInfoPos, Quaternion.identity, 0, myCustomInitData);

        //if (view.IsMine)
            //view.RPC("GoToGameScene", RpcTarget.AllBuffered);

    }

    public void PlayerInfoOrderReady() //Call from PlayerInfo script when all actions are done
    {
        if (view.IsMine)
            view.RPC("GoToGameScene", RpcTarget.AllBuffered);
    }



    [PunRPC]
    public void GoToGameScene()
    {
        StartCoroutine(RPC_WaitOtherPlayersBuffer());

    }
    IEnumerator RPC_WaitOtherPlayersBuffer()
    {

        yield return new WaitForSeconds(1.0f);
        //SceneManager.LoadScene("Game");
        PhotonNetwork.LoadLevel("Game");
    }
}
