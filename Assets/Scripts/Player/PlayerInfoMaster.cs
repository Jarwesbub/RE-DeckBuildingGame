using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInfoMaster : MonoBehaviourPun
{
    private static PlayerInfoMaster master;

    private GameObject LobbyRoomOpen;
    public List<GameObject> ChildList;
    public List<int> ChildIDList;
    public int playerCount, childCount;
    //PhotonView view;

    private void Awake()
    {
        
        if (master != null && master != this)
        {
            Destroy(this);
            //view = GetComponent<PhotonView>();
            //PhotonNetwork.Destroy(view);
            //PhotonNetwork.Destroy(this.view);
            Debug.Log("PlayerInfoMaster DESTROYED");
        }
        else
        {
            master = this;
            //view = GetComponent<PhotonView>();          
            DontDestroyOnLoad(this);
            LobbyRoomOpen = GameObject.FindWithTag("LobbyRoomControl");
            playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            DestroyAllChilds();
        }

    }
    public void LobbyRoomIsLoaded()
    {
        LobbyRoomOpen = GameObject.FindWithTag("LobbyRoomControl");
        ChildList.Clear();
        ChildIDList.Clear();
        DestroyAllChilds();
    }


    public void DestroyAllChilds()
    {
        foreach (Transform child in gameObject.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        //PhotonNetwork.Destroy(view.gameObject);
    }
    /*
    public void OnDestroy()
    {
        foreach (Transform child in gameObject.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

    }
    */

    public void PlayerInfoAddNewChild(int playerID, GameObject child)
    {
        ChildList.Add(child);
        ChildIDList.Add(playerID);
        playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        childCount = ChildList.Count;
        //if (view.IsMine)
            if (playerCount == childCount)
                ChangeChildOrder();
            //view.RPC("ChangeChildOrder", RpcTarget.AllBuffered);

    }
    //[PunRPC]
    private void ChangeChildOrder()
    {
        //playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        int count = 1;
        int siblingIndex = 0;
        int i = 0;
        do
        {
            //Debug.Log("ChangeChildOrder.count = " + count+" .i = " +i);
            if (ChildIDList[i] == count)
            {
                ChildList[i].transform.SetSiblingIndex(siblingIndex);
                siblingIndex++;
                count++;
                i = 0;
                //Debug.Log("Found " + i);
            }
            else
                i++;

            if (i > childCount)
            {
                Debug.Log("ERROR: i is bigger than childcount!");
                break;
            }

        }
        while (count < playerCount);

        if (LobbyRoomOpen == null)
            LobbyRoomOpen = GameObject.FindWithTag("LobbyRoomControl");
        LobbyRoomOpen.GetComponent<LobbyRoomOpen>().PlayerInfoOrderReady();
    }

}
