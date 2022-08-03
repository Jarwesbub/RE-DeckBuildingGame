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
    public List<Sprite> CharacterSprites;
    public List<string> SpriteNames;
    private int playerCount, roomPlayerCount;
    float normalScale = 1.25f;
    private bool isZoomed;
    private PhotonView view;
    [SerializeField] private bool isLocalPlayer;
    [SerializeField] private int playerID, otherPlayerID;

    void Awake()
    {
        PIMaster = GameObject.FindGameObjectWithTag("PlayerInfoMaster");
        view = GetComponent<PhotonView>();
        playerID = PhotonNetwork.LocalPlayer.ActorNumber;
        gameObject.tag = "Player" + playerID.ToString();
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        roomPlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        if (isLocalPlayer)
        {
            GetAllSpritesFromPIMaster();
            SetLocalCharacterSprite();
        }
        else if (playerID != 1) //Is other than HOST
        {
            GetAllSpritesFromPIMaster();
            string name = SpriteNames[0]; //Host always starts the game so set his sprite!
            ChangeSpriteByName(name);
            
        }
        else //Is the HOST
        {
            GetAllSpritesFromPIMaster();
            string name = SpriteNames[1]; //Host always starts the game so set his sprite!
            ChangeSpriteByName(name);
            
        }

    }
    private void GetAllSpritesFromPIMaster()
    {
        if (SpriteNames.Count < playerCount)
            SpriteNames.Clear();

        int count = PIMaster.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            GameObject child = PIMaster.transform.GetChild(i).gameObject;
            string characterName = child.GetComponent<PlayerInfo>().myCharacterCard;
            SpriteNames.Add(characterName);


        }

    }

    private void SetLocalCharacterSprite()
    {
        string name = SpriteNames[playerID - 1];
        ChangeSpriteByName(name);
    }

    public void SetOtherCharacterSprite(Player _player)
    {
        otherPlayerID = _player.ActorNumber;
        if(otherPlayerID > view.ViewID)
            otherPlayerID -= 1;
        string name = SpriteNames[otherPlayerID - 1]; //TEST
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
            transform.localScale = new Vector2(2.2f, 2.2f);
            isZoomed = true;
        }
    }
}
