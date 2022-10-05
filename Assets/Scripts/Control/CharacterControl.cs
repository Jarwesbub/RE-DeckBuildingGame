using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class CharacterControl : MonoBehaviourPun
{
    public GameObject HPControl;
    [SerializeField] private List<int> PlayerIDs;
    [SerializeField] private List<string> SpriteNames;
    private int roomPlayerCount;
    float normalScale = 1.25f, zoomedScale = 2.45f;
    private bool isZoomed;
    [SerializeField] private bool isLocalPlayer;
    [SerializeField] private int playerID, otherPlayerID;

    void Awake()
    {
        playerID = PhotonNetwork.LocalPlayer.ActorNumber;
        gameObject.tag = "Player" + playerID.ToString();
        gameObject.SetActive(false);
        otherPlayerID = 1; //Host starts!
        roomPlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        CreateAllCharacterSprites();
    }

    void Start()
    {
        if (isLocalPlayer)
        {
            SetLocalCharacterSprite();
        }
        else if (playerID != 1 || roomPlayerCount <= 1) //Is other than HOST
        {
            string name = SpriteNames[0]; //Show player 1 character card
            ChangeSpriteByName(name);
            
        }
        else if(roomPlayerCount>1) // Is the HOST -> set second player to OtherCharacterSprite
        {
            int id = PhotonNetwork.PlayerList[1].ActorNumber;
            int index = PlayerIDs.IndexOf(id);
            string name = SpriteNames[index]; //Host always starts the game so set his sprite!
            ChangeSpriteByName(name);
        }
        GetComponent<SpriteFromAtlas>().CharacterCardStart(); //Set "Original" or "Custom" Character cards

    }

    public void CreateAllCharacterSprites()
    {      
        if (SpriteNames.Count!=0)
        {
            SpriteNames.Clear();
            PlayerIDs.Clear();
        }

        foreach (DictionaryEntry info in GameStats.playerInfos)
        {
            int id = (int)info.Key;
            string card = (string)info.Value;
            PlayerIDs.Add(id);
            SpriteNames.Add(card);
            Debug.Log("GameStats.playerInfos: Key = " + id + " Value = " + card);
        }

    }

    private void SetLocalCharacterSprite()
    {
        int index = PlayerIDs.IndexOf(playerID);
        string name = SpriteNames[index];

        ChangeSpriteByName(name);
    }

    public void SetOtherCharacterSprite(int playerID)
    {
        otherPlayerID = playerID;

        int value = PlayerIDs.IndexOf(playerID);
        string name = SpriteNames[value];
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
