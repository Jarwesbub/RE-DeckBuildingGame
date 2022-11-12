using UnityEngine;
using TMPro;
using Photon.Pun;

//LobbyRoom.scene script

public class LobbyOptionsMenu : MonoBehaviour
{
    public GameObject characterTypeDropDown, mansionTypeDropDown, OptionsMenu;
    public TMP_Text mainInfoText;
    private bool isMaster, optionsMenuOn;

    private void Start()
    {
        isMaster = PhotonNetwork.IsMasterClient;

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
        System.Diagnostics.Process.Start(appdatapath + "/Custom_data/");
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

    public void SetMainInfoText(string txt) //Info when Start button is pressed (Game is starting... -> located in OverwriteTextFileList.cs)
    {
        mainInfoText.text = txt;
    }
}
