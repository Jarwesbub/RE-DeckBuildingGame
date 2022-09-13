using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEditor;

public class LobbyOptionsMenu : MonoBehaviourPun
{
    public GameObject CharacterCardList, characterTypeDropDown, CharacterControl, mansionTypeDropDown;
    private bool gameStarts, isMaster;
    PhotonView view;

    private void Start()
    {
        isMaster = PhotonNetwork.IsMasterClient;
        view = GetComponent<PhotonView>();

        if (isMaster)
        {
            characterTypeDropDown.SetActive(true);
            mansionTypeDropDown.SetActive(true);
        }
        else
        {
            characterTypeDropDown.SetActive(false);
            mansionTypeDropDown.SetActive(false);
        }
    }
    public void OnClickShowCustomdataPath()
    {
        string appdatapath = Application.persistentDataPath;
        //EditorUtility.RevealInFinder(appdatapath + "/Custom_data/");
        System.Diagnostics.Process.Start(appdatapath + "/Custom_data/");
    }

    public void OnClickStartGetLobbyOptions()
    {
        
        if (!gameStarts && isMaster)
        {
            int characterValue = characterTypeDropDown.GetComponent<Dropdown>().value; //0 = Original; 1 = Custom
            int mansionValue = mansionTypeDropDown.GetComponent<Dropdown>().value; //1-4 = MansionCards1-4.txt file
            PlayerPrefs.SetInt("CharacterType", characterValue);
            Debug.Log("CharacterType = " + characterValue);

            view.RPC("Pun_StartGetLobbyOptions", RpcTarget.AllBuffered, characterValue, mansionValue);
            gameStarts = true;
        }
    }
    [PunRPC] void Pun_StartGetLobbyOptions(int charValue, int mansionValue)
    {
        if (charValue==0)
        {
            CharacterCardList.GetComponent<TextFileToList>().LoadTextFileByName("CharacterList");
        }
        else //Value == 1
        {
            CharacterCardList.GetComponent<TextFileToList>().LoadTextFileByName("CharacterCustomlist");
        }
        if (mansionValue == 0)
            mansionValue = 1;

        GameStats.MansionDeckValue = mansionValue;
        //GameStats.CharacterDeckValue = charValue;
    }

}
