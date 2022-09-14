using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyRoomOpen : MonoBehaviourPunCallbacks
{
    //public GameObject PlayerInfoPrefab;
    public GameObject CharacterList, OptionsMenu, HostSetup;
    [SerializeField] private GameObject[] playerInfoList;
    //[SerializeField] private int myID;
    [SerializeField] private string myCharacterCard;
    private bool isMaster, readyForGameRoom, optionsMenuOn;
    Vector2 playerInfoPos = new Vector2(-6.9f, -1.6f);
    PhotonView view;
    Hashtable hash;
    

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true; //Works with PhotonNetwork.LoadLevel();
        view = GetComponent<PhotonView>();
        //myID = view.OwnerActorNr;
        isMaster = PhotonNetwork.IsMasterClient;
        GameStats.playerInfos = new Hashtable();

        optionsMenuOn = false;

        

            if (isMaster)
                HostSetup.SetActive(true);
            else
                HostSetup.SetActive(false);

        OptionsMenu.SetActive(false);
    }

    public void ReadyForGameScene() //Accessed from OverWriteTextFileList (attached to this main object)
    {


        if (view.IsMine)
        {
            int count = PhotonNetwork.CurrentRoom.PlayerCount;
            int[] playerIDs = new int[count];
            string[] cards = new string[count];

            int i = 0;
            foreach(Player p in PhotonNetwork.PlayerList)
            {
                int id = p.ActorNumber;
                string card = CharacterList.GetComponent<TextFileToList>().GetRandomLineFromTextFile();

                playerIDs[i] = id;
                cards[i] = card;
                i++;
                view.RPC("PRC_AddPlayerInfos", RpcTarget.AllBuffered, id, card);
            }
            view.RPC("RPC_GoToGameScene", RpcTarget.AllBuffered);
            //view.RPC("RPC_OnClickGoToGameScene", RpcTarget.AllBuffered);
        }
    }
    [PunRPC]
    private void PRC_AddPlayerInfos(int id, string card)
    {
        GameStats.playerInfos.Add(id, card);
    }


    [PunRPC]
    public void RPC_OnClickGoToGameScene()
    {
        myCharacterCard = CharacterList.GetComponent<TextFileToList>().GetRandomLineFromTextFile();
        PlayerPrefs.SetString("myCharacterCard", myCharacterCard);

        object[] myCustomInitData = new object[]
        {
                myCharacterCard

        };
        //PhotonNetwork.Instantiate(PlayerInfoPrefab.name, playerInfoPos, Quaternion.identity, 0, myCustomInitData);

        //if (view.IsMine)
            //view.RPC("GoToGameScene", RpcTarget.AllBuffered);

    }

    public void PlayerInfoOrderReady() //Call from PlayerInfo script when all actions are done
    {
        if (view.IsMine)
            view.RPC("RPC_GoToGameScene", RpcTarget.AllBuffered);
    }



    [PunRPC]
    public void RPC_GoToGameScene()
    {
        StartCoroutine(RPC_WaitOtherPlayersBuffer());

    }
    IEnumerator RPC_WaitOtherPlayersBuffer()
    {

        yield return new WaitForSeconds(1.0f);
        //SceneManager.LoadScene("Game");
        PhotonNetwork.LoadLevel("Game");
    }

    public void OnClickOpenOptionsMenu()
    {
        if(optionsMenuOn)
        {
            OptionsMenu.SetActive(false);
            optionsMenuOn = false;
        }
        else
        {
            OptionsMenu.SetActive(true);
            optionsMenuOn = true;
        }
    }
}
