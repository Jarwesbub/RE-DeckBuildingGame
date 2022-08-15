using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Photon.Pun;

public class OverwriteTextFileList : MonoBehaviourPun
{
    private bool isMaster,buttonIsPressed;
    PhotonView view;
    public GameObject LoadingTextObj;
    //private string[] Shop_Cards;

    private void Start()
    {
        isMaster = PhotonNetwork.IsMasterClient;
        buttonIsPressed = false;
        view = GetComponent<PhotonView>();
        Debug.Log("isMaster=" + isMaster);
    }

    public void OnClickOverwriteNewFiles() //
    {
        if (isMaster && !buttonIsPressed)
        {
            
            buttonIsPressed = true;
            string[] shop_cards = new string[18]; //+1 //ADD HERE ONE MORE!
            shop_cards[0] = "ActionCardsList1_AllCounted"; shop_cards[1] = "ActionCardsList2_AllCounted"; shop_cards[2] = "ActionCardsList3_AllCounted";
            shop_cards[3] = "ActionCardsList4_AllCounted"; shop_cards[4] = "ActionCardsList5_AllCounted"; shop_cards[5] = "AmmoCountList";
            shop_cards[6] = "AR_SG_List_AllCounted"; shop_cards[7] = "CharacterList"; shop_cards[8] = "GrenadeList_AllCounted";
            shop_cards[9] = "HandgunsList_AllCounted"; shop_cards[10] = "HPItemsList_AllCounted"; shop_cards[11] = "KnifeList_AllCounted";
            shop_cards[12] = "RifleList_AllCounted"; shop_cards[13] = "ShotgunList_AllCounted"; shop_cards[14] = "StartingDeckList";
            shop_cards[15] = "MansionCards1_AllCounted"; shop_cards[16] = "Extra1List_AllCounted"; shop_cards[17] = "CharacterCustomlist";

            view.RPC("Pun_OverwriteNewFiles", RpcTarget.AllBuffered, (object)shop_cards);
            //Pun_OverwriteNewFiles(shop_cards);
        }      
    }

    [PunRPC]
    public void Pun_OverwriteNewFiles(string[] shop_cards) //Overwrite from client's Custom_data folder to Game_data folder
    {
        LoadingTextObj.SetActive(true);
        //Shop_Cards = shop_cards;
        foreach (string textFile in shop_cards)
        {
            string readFromFilePath = Application.persistentDataPath + "/Custom_data/" + textFile + ".txt";
            string[] fileLines = File.ReadAllLines(readFromFilePath).ToArray();

            string writeToFilePath = Application.persistentDataPath + "/Game_data/" + textFile + ".txt";
            File.WriteAllLines(writeToFilePath, fileLines);

        }

        StartCoroutine(WaitTime());
        Debug.Log("Overwriting done from Client");
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<LobbyRoomOpen>().ReadyForGameScene();
    }
    
}
