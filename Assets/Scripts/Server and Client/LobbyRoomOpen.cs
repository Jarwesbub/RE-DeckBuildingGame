using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
//using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

//Attach this script to object that contains "OverwriteTextFileList.cs" -script

public class LobbyRoomOpen : MonoBehaviourPunCallbacks
{
    public GameObject CharacterList, OptionsMenu, HostSetup;
    [SerializeField] private string myCharacterCard;
    private bool isMaster;
    PhotonView view;

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true; //Works with PhotonNetwork.LoadLevel();
        view = GetComponent<PhotonView>();
        isMaster = PhotonNetwork.IsMasterClient;
        GameStats.playerInfos = new Hashtable(); //Set new (empty) Hashtable in GameStats script

        if (isMaster)
                HostSetup.SetActive(true);
            else
                HostSetup.SetActive(false);

        OptionsMenu.SetActive(false);

    }
    public void TextFilesAreLoadedToOthers()
    {
        StartCoroutine(TextFileIsReadyDelay());
    }
    IEnumerator TextFileIsReadyDelay()
    {
        yield return new WaitForSeconds(1f);
        ReadyForGameScene();
    }

    private void ReadyForGameScene() //Accessed from OverWriteTextFileList (attached to this main object)
    {
        if (view.IsMine)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false; //Set room unjoinable before creating character cards

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

    }

    [PunRPC]
    public void RPC_GoToGameScene()
    {
        StartCoroutine(RPC_WaitOtherPlayersBuffer());

    }
    IEnumerator RPC_WaitOtherPlayersBuffer()
    {
        yield return new WaitForSeconds(1.0f);
        PhotonNetwork.LoadLevel("Game");

    }

}
