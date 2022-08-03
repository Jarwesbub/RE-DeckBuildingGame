using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
//using Photon.Realtime;


public class PlayerInfo : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    public GameObject CharacterCardPrefab;
    private GameObject PIMaster;
    public int playerID; //PhotonNetwork ActorNumber
    //public int myCharacterCard;
    public string myCharacterCard;
    public GameObject[] PlayerInfoList;
    private SpriteRenderer rend;


    [SerializeField] private bool isMaster;
    //public List<Sprite> allCharacters;
    PhotonView view;

    private void Awake()
    {

        view = GetComponent<PhotonView>();
        playerID = view.OwnerActorNr;
        isMaster = PhotonNetwork.IsMasterClient;

        rend = GetComponent<SpriteRenderer>();
        rend.enabled = false;

        PIMaster = GameObject.FindWithTag("PlayerInfoMaster");
        transform.parent = PIMaster.transform;
        PIMaster.GetComponent<PlayerInfoMaster>().PlayerInfoAddNewChild(playerID, gameObject);
    }


    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] instantiationData = info.photonView.InstantiationData;
        myCharacterCard = (string)instantiationData[0];

        //myCharacterCard = PlayerPrefs.GetString("myCharacterCard");
        if (view.IsMine)
            view.RPC("SendMyCharacterCardToOthers", RpcTarget.OthersBuffered, myCharacterCard);

    }
    [PunRPC]
    void SendMyCharacterCardToOthers(string cardName)
    {
        myCharacterCard = cardName;
    }
    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        if (scene.name == "Game")
        {
           
            Debug.Log("GAME SCENE LOADED IN U FACE");
        }
    }
}
