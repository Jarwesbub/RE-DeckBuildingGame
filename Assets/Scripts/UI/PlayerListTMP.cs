using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerListTMP : MonoBehaviourPun
{
    [SerializeField]private TMP_Text playerListTMP, playerPointsTMP;
    [SerializeField] private List<int> JoinedPlayerIDList, playerPointsList;
    private bool showPoints;
    private int WinnerIDNumber;

    private void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "LobbyRoom" || currentScene == "Game")
        {
            showPoints = currentScene == "Game";
        }
        SetPlayerList();
    }

    public void SetPlayerList() //
    {
        Debug.Log("PlayerList RESET");
        JoinedPlayerIDList.Clear();
        playerPointsList.Clear();
        playerListTMP.text = "Players:" + "\n";

        if (showPoints)
        {
            playerPointsTMP.text = "Points:" + "\n";
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                JoinedPlayerIDList.Add(p.ActorNumber);
                playerPointsList.Add(0);
                playerListTMP.text += p.NickName + "\n";
                playerPointsTMP.text += 0 + "\n";
            }
        }
        else
        {
            playerPointsTMP.text = "";
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                JoinedPlayerIDList.Add(p.ActorNumber);
                playerPointsList.Add(0);
                playerListTMP.text += p.NickName + "\n";
            }
        }
    }
    public void RemovePlayerByID(int id)
    {
        if (JoinedPlayerIDList.Contains(id))
        {
            int indx = JoinedPlayerIDList.IndexOf(id);
            JoinedPlayerIDList.Remove(JoinedPlayerIDList[indx]);
            playerPointsList.Remove(playerPointsList[indx]);
            Debug.Log("Player removed - id: " + id+" index: "+ indx);
            playerListTMP.text = "Players:" + "\n";
            playerPointsTMP.text = "Points:" + "\n";

            if (showPoints)
            {
                foreach (int listID in JoinedPlayerIDList)
                {
                    //Debug.Log("JoinedPlayerIDList listID= " + listID);
                    if (listID != id)
                    {
                        string _name = PhotonNetwork.CurrentRoom.GetPlayer(listID).NickName;
                        int _index = JoinedPlayerIDList.IndexOf(listID);
                        playerListTMP.text += _name + "\n";
                        playerPointsTMP.text += playerPointsList[_index] + "\n";
                    }
                }
            }
            else
            {
                foreach (int listID in JoinedPlayerIDList)
                {
                    if (listID != id)
                    {
                        string _name = PhotonNetwork.CurrentRoom.GetPlayer(listID).NickName;
                        playerListTMP.text += _name + "\n";
                    }
                }
            }
        }
    }

    public void UpdatePlayerPoints(int id, int points)
    {
        //Debug.Log(" Wanted id: " + id+" Points: "+points);
        playerPointsTMP.text = "Points:" + "\n";
        
        foreach(int i in JoinedPlayerIDList)
        {
            int index = JoinedPlayerIDList.IndexOf(i);

            if (i == id)
            {
                playerPointsList[index] += points;
            }

            playerPointsTMP.text += playerPointsList[index] + "\n";
        }

    }

    public string GetPlayerWithMostPoints()
    {
        int playerIndex = 0; int count = playerPointsList.Count;
        int points =0;

        for (int i = 0; i < count; i++)
        {
            if (playerPointsList[i] > points)
            {
                points = playerPointsList[i];
                playerIndex = i;
            }
        }
        
        WinnerIDNumber = JoinedPlayerIDList[playerIndex];
        string playerName = PhotonNetwork.CurrentRoom.GetPlayer(WinnerIDNumber).NickName;
        for (int i = 0; i < count; i++) //Check if there are player with same amount of points
        {
            if (points == playerPointsList[i] && i != playerIndex)
            {
                playerName += " and " + PhotonNetwork.CurrentRoom.GetPlayer(JoinedPlayerIDList[i]).NickName;
            }
        }

        return playerName;
    }
    public int GetWinnerCharacterCardName()
    {
        return WinnerIDNumber;
    }
}
