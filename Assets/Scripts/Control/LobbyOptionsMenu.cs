using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using UnityEditor;

public class LobbyOptionsMenu : MonoBehaviourPun
{
    public GameObject characterTypeDropDown, mansionTypeDropDown, OptionsMenu;
    public TMP_Text mainInfoText;
    private bool gameStarts, isMaster, optionsMenuOn;
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
        mainInfoText.text = "";
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
            //int mansionValue = mansionTypeDropDown.GetComponent<Dropdown>().value; //1-4 = MansionCards1-4.txt file
            //PlayerPrefs.SetInt("CharacterType", characterValue);
            //Debug.Log("CharacterType = " + characterValue);

            view.RPC("Pun_StartGetLobbyOptions", RpcTarget.AllBuffered);
            gameStarts = true;
        }
    }

    [PunRPC] void Pun_StartGetLobbyOptions()
    {
        SetMainInfoText("Game is starting...");
    }



    public void OnClickOpenOptionsMenu()
    {
        if (optionsMenuOn)
        {
            OptionsMenu.SetActive(false);
            optionsMenuOn = false;
        }
        else
        {
            OptionsMenu.SetActive(true);
            optionsMenuOn = true;
        }
    }

    public void SetMainInfoText(string txt)
    {
        mainInfoText.text = txt;
    }
}
