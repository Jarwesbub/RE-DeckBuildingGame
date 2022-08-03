using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
public class RoomControl : MonoBehaviourPunCallbacks
{
    public GameObject PhotonPlayerPrefab, PlayerInfoPrefab;
    private GameObject PlayerListTMP;
    private string NickName;

    void Awake()
    {
        PlayerListTMP = GameObject.FindWithTag("PlayerListTMP");
        NickName = PlayerPrefs.GetString("NickName");
    }
    void Start()
    {
        

    }

}
