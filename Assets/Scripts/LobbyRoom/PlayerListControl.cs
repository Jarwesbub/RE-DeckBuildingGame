using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
//Attach this script with same object as "PhotonRoomMaster.cs"

public class PlayerListControl : MonoBehaviourPun
{
    public GameObject PlayerListPrefab;
    PhotonView view;
    private bool isMaster;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        isMaster = PhotonNetwork.IsMasterClient;

        //if (isMaster)
            //NewPlayerJoined(1); //Host joins first
    }

    public void NewPlayerJoined(int playerID) //
    {
        Debug.Log("UpdateJoinedPlayerList id: " + playerID);

        if(isMaster)
            view.RPC("RPC_UpdatePlayerList", RpcTarget.AllBuffered);

    }
    [PunRPC]
    private void RPC_UpdatePlayerList()
    {
        PlayerListPrefab.GetComponent<PlayerListTMP>().SetPlayerList();

    }

    public void PlayerLeft(int playerID)
    {
        Debug.Log("PlayerLeft id: " + playerID);

        if (isMaster || view.OwnerActorNr == PhotonNetwork.PlayerList[0].ActorNumber) //If host leaves -> get next player
            view.RPC("RPC_RemovePlayer", RpcTarget.AllBuffered, playerID);

    }
    [PunRPC]private void RPC_RemovePlayer(int id)
    {
        Debug.Log("RPC_RemovePlayer " + id);
        PlayerListPrefab.GetComponent<PlayerListTMP>().RemovePlayerByID(id);
    }

    public void UpdateCurrentPlayerPoints(int points)
    {
        int id = PhotonNetwork.LocalPlayer.ActorNumber;

        if (id == GameStats.CurrentPlayerID)
            view.RPC("RPC_UpdatePlayerPoints", RpcTarget.AllBuffered, id, points);

    }
    [PunRPC]
    private void RPC_UpdatePlayerPoints(int id, int points)
    {
        //Debug.Log("PunRPC id: " + id + " points: " + points);
        PlayerListPrefab.GetComponent<PlayerListTMP>().UpdatePlayerPoints(id,points);

    }

    public string GetPlayerWithMostPoints()
    {
        string name = PlayerListPrefab.GetComponent<PlayerListTMP>().GetPlayerWithMostPoints();
        return name;
    }
    public int GetWinnerIDNumber()
    {
        int id = PlayerListPrefab.GetComponent<PlayerListTMP>().GetWinnerCharacterCardName();
        return id;
    }

}
