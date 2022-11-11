using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

//Attach this script to object that contains "OverwriteTextFileList.cs" -script

public class LobbyRoomOpen : MonoBehaviourPunCallbacks
{
    public GameObject MainCanvas, CharacterList, OptionsMenu, HostSetup, LeftMenu;
    [SerializeField] private string myCharacterCard;
    private bool isMaster, showSettings;
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
        CreateAllCharacterCards();
    }

    private void CreateAllCharacterCards() //Accessed from OverWriteTextFileList (attached to this main object)
    {
        if (view.IsMine)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false; //Set room unjoinable before creating character cards
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

            int deckType = GameStats.CharacterDeckValue; // 0 = ORIGINAL , 1 = CUSTOM
            string[] characterCardsList = CharacterList.GetComponent<AllCharacterCards>().GetCharacterCardsByType(deckType);

            int cardCount = characterCardsList.Length;
            int[] randomNumbers = new int[playerCount];

            if (playerCount < cardCount)
            {
                for (int i = 0; i < playerCount; i++)
                {
                    int number = Random.Range(0, cardCount);
                    randomNumbers[i] = number;

                    while (!randomNumbers.Contains(number))
                    {
                        number = Random.Range(0, cardCount);
                        randomNumbers[i] = number;
                    };               
                }
            }
            else
            {
                for (int i = 0; i < playerCount; i++)
                {
                    int number = Random.Range(0, cardCount);
                    randomNumbers[i] = number;
                }
            }

            int[] playerIDs = new int[playerCount]; //PhotonNetwork player actor numbers (id)
            string[] cards = new string[playerCount]; //All character cards array

            int index = 0;
            foreach(Player p in PhotonNetwork.PlayerList)
            {
                int id = p.ActorNumber;
                //string card = CharacterList.GetComponent<TextFileToList>().GetStringFromTextByNumber(randomNumbers[index]);
                string card = characterCardsList[randomNumbers[index]];
                playerIDs[index] = id;
                cards[index] = card;
                index++;
                view.RPC("PRC_AddPlayerInfos", RpcTarget.AllBuffered, id, card);
            }

            view.RPC("RPC_GoToGameScene", RpcTarget.AllBuffered);

        }
    }
    [PunRPC] private void PRC_AddPlayerInfos(int id, string card)
    {
        GameStats.playerInfos.Add(id, card);
    }

    [PunRPC] public void RPC_GoToGameScene()
    {
        StartCoroutine(RPC_WaitOtherPlayersBuffer());

    }
    IEnumerator RPC_WaitOtherPlayersBuffer()
    {
        yield return new WaitForSeconds(1.0f);
        PhotonNetwork.LoadLevel("Game");

    }

    public void OnClickShowSettings()
    {
        if (showSettings)
            showSettings = false;
        else
            showSettings = true;
        LeftMenu.SetActive(showSettings);
    }

    public void OnClickLeaveRoom()
    {
        if (isMaster)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            view.RPC("RPC_KickAllPlayers", RpcTarget.OthersBuffered);
        }
            PhotonNetwork.LeaveRoom();
    }
    [PunRPC] void RPC_KickAllPlayers()
    {
        StartCoroutine(ShowHostLeaves());
    }
    IEnumerator ShowHostLeaves()
    {
        MainCanvas.GetComponent<LobbyOptionsMenu>().SetMainInfoText("Host has ended the room\n\nDisconnecting...");
        yield return new WaitForSeconds(2f);
        PhotonNetwork.LeaveRoom();
    }
}
