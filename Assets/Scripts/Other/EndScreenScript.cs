using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScreenScript : MonoBehaviour
{
    public GameObject PhotonRoomMaster, WinnerCharacterCard, HostButtons;
    private GameObject Child;
    public TMP_Text winnerMainTMP;
    public GameObject Shop_btn, Mansion_btn, endTurn_btn;

    void OnEnable()
    {
        Child = transform.GetChild(0).gameObject;
        Child.SetActive(false);
        Shop_btn.SetActive(false);
        Mansion_btn.SetActive(false);
        Shop_btn.SetActive(false);

        bool isMaster = PhotonRoomMaster.GetComponent<PhotonRoomMaster>().CheckIfIsMasterClient();
        HostButtons.SetActive(isMaster);

        StartCoroutine(ShowEndScreen());
    }

    IEnumerator ShowEndScreen()
    {
        yield return new WaitForSeconds(3f);
        Child.SetActive(true);
        EndScreen();
    }
    private void EndScreen()
    {
        winnerMainTMP.text = "Winner is:\n";
        string name = PhotonRoomMaster.GetComponent<PlayerListControl>().GetPlayerWithMostPoints();
        winnerMainTMP.text += name;

        int id = PhotonRoomMaster.GetComponent<PlayerListControl>().GetWinnerIDNumber();
        string characterCard = "";
        foreach (DictionaryEntry info in GameStats.playerInfos)
        {
            int _id = (int)info.Key;

            if (_id == id)
            {
                characterCard = (string)info.Value;
                break;
            }
        }

        WinnerCharacterCard.GetComponent<SpriteFromAtlas>().ChangeCardSprite(characterCard);
    }
}
