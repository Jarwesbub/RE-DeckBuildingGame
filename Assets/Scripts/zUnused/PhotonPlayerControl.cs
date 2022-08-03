using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonPlayerControl : MonoBehaviourPunCallbacks //NOT IN USE
{
    //public static PhotonPlayerControl PPC { get; private set; }

    public int PlayerID;
    private GameObject MainCamera;

    PhotonView view;

    void Awake()
    {
        /*
        if (PPC != null && PPC != this)
        {
            Destroy(this);
        }
        else
        {
            PPC = this;
        }
        */
    }

    void OnEnable()
    {
        MainCamera = GameObject.FindWithTag("MainCamera");

        view = MainCamera.GetComponent<PhotonView>();

    }

    void Start()
    {
        PlayerID = PhotonNetwork.LocalPlayer.ActorNumber;


    }
}
