using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerListTMP : MonoBehaviourPun
{
    //private GameObject MainCanvas;
    private TextMeshProUGUI playerListTMP;
    public List<string> JoinedPlayerList;
    private string JoinedPlayers;
    //private PhotonView view;
    public bool destroyOnLoad;

    void Awake()
    {
        //MainCanvas = GameObject.FindWithTag("MainCanvas");
        playerListTMP = GetComponent<TextMeshProUGUI>();
        if(!destroyOnLoad)
            DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        UpdatePlayerList();
        //view = GetComponent<PhotonView>();

    }
    public void UpdatePlayerList() //Updated from CreateAndJoinRooms.cs
    {
        if (playerListTMP == null)
            playerListTMP = GetComponent<TextMeshProUGUI>();

        playerListTMP.text = "Joined Players:" + "\n";

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            string list = p.NickName.ToString();
            JoinedPlayerList.Add(list);
            playerListTMP.text += list +"\n";
            Debug.Log("PlayerListTMP: added: " + list);

        }
        
        //RoomInfoList.RI.JoinedPlayerList = JoinedPlayerList;

    }



}
