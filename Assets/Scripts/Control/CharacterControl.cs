using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class CharacterControl : MonoBehaviourPun
{
    public GameObject PIMaster, HPControl;
    //public GameObject[] PlayerInfoList;
    //public List<Sprite> CharacterSprites;
    [SerializeField] private List<string> SpriteNames;
    private int roomPlayerCount;
    float normalScale = 1.25f, zoomedScale = 2.45f;
    private bool isZoomed;
    //private PhotonView view;
    [SerializeField] private bool isLocalPlayer;
    [SerializeField] private int playerID, otherPlayerID;

    void Awake()
    {
        PIMaster = GameObject.FindGameObjectWithTag("PlayerInfoMaster");
        //view = GetComponent<PhotonView>();
        playerID = PhotonNetwork.LocalPlayer.ActorNumber;
        gameObject.tag = "Player" + playerID.ToString();
        gameObject.SetActive(false);
        otherPlayerID = 1; //Host starts!
    }

    void Start()
    {
        roomPlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        GetAllSpritesFromPIMaster();

        if (isLocalPlayer)
        {
            SetLocalCharacterSprite();
        }
        else if (playerID != 1 || roomPlayerCount <= 1) //Is other than HOST
        {
            string name = SpriteNames[0]; //Host always starts the game so set his sprite!
            ChangeSpriteByName(name);
            
        }
        else if(roomPlayerCount>1) // Is the HOST -> set second player to OtherCharacterSprite
        {
            string name = SpriteNames[1]; //Host always starts the game so set his sprite!
            ChangeSpriteByName(name);
            //gameObject.SetActive(false);
        }

        GetComponent<SpriteFromAtlas>().CharacterCardStart(); //Set "Original" or "Custom" Character cards


    }
    public void GetAllSpritesFromPIMaster()
    {
        if (SpriteNames.Count < roomPlayerCount)
            SpriteNames.Clear();

        int count = PIMaster.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            GameObject child = PIMaster.transform.GetChild(i).gameObject;
            string characterName = child.GetComponent<PlayerInfo>().myCharacterCard;
            SpriteNames.Add(characterName);

        }
;
    }

    private void SetLocalCharacterSprite()
    {
        string name = SpriteNames[playerID - 1];
        ChangeSpriteByName(name);
    }

    public void SetOtherCharacterSprite(int playerID)
    {//BUG Index out of range error when HOST ends turn and loads spritedata from "unenabled object" (Development build only)
        otherPlayerID = playerID;
        /*
        if (otherPlayerID > PhotonNetwork.LocalPlayer.ActorNumber)
            otherPlayerID -= 1;*/
        int value = otherPlayerID - 1;
        string name = SpriteNames[value];
        //Debug.Log("OtherCharacterSprite name = " + name);
        ChangeSpriteByName(name);

    }

    public void ChangeSpriteByName(string name)
    {
        GetComponent<SpriteFromAtlas>().ChangeCardSprite(name);
    }

    public void OnClickZoomCard()
    {
        if (isZoomed)
        {
            transform.localScale = new Vector2(normalScale, normalScale);
            isZoomed = false;
        }
        else
        {
            transform.localScale = new Vector2(zoomedScale, zoomedScale);
            isZoomed = true;
        }
    }
}
