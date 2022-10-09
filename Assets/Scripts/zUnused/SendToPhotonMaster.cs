using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;


public class SendToPhotonMaster : MonoBehaviourPunCallbacks
{
    public GameObject PhotonRoomMaster;
    public InputField createInput;
    public InputField joinInput;

    public void OnClickCreateRoom()
    {
        PhotonRoomMaster.GetComponent<PhotonRoomMaster>().CreateRoomByPassword(createInput.text);
    }

    public void OnClickJoinRoom()
    {
        PhotonRoomMaster.GetComponent<PhotonRoomMaster>().JoinRoomByPassword(joinInput.text);
    }

}
