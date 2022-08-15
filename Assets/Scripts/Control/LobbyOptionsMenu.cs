using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LobbyOptionsMenu : MonoBehaviourPun
{
    public GameObject CharacterCardList, characterTypeDropDown, CharacterControl;
    private bool gameStarts, isMaster;
    PhotonView view;

    private void Start()
    {
        isMaster = PhotonNetwork.IsMasterClient;
        view = GetComponent<PhotonView>();

        if (isMaster)
            characterTypeDropDown.SetActive(true);
        else
            characterTypeDropDown.SetActive(false);

    }

    public void OnClickStartGetLobbyOptions()
    {
        
        if (!gameStarts && isMaster)
        {
            int characterValue = characterTypeDropDown.GetComponent<Dropdown>().value; //0 = Original; 1 = Custom
            PlayerPrefs.SetInt("CharacterType", characterValue);
            Debug.Log("CharacterType = " + characterValue);

            view.RPC("Pun_StartGetLobbyOptions", RpcTarget.AllBuffered, characterValue);
            gameStarts = true;
        }
    }
    [PunRPC] void Pun_StartGetLobbyOptions(int charValue)
    {
        if (charValue==0)
        {
            CharacterCardList.GetComponent<TextFileToList>().LoadTextFileByName("CharacterList");
        }
        else //Value == 1
        {
            CharacterCardList.GetComponent<TextFileToList>().LoadTextFileByName("CharacterCustomlist");
        }
    }

}
