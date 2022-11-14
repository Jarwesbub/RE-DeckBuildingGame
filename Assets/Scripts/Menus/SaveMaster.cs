using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveMaster : MonoBehaviour
{

    public void SetNickName(string name)
    {
        PlayerPrefs.SetString("NickName", name);
        PlayerPrefs.Save();
        Debug.Log("NickName saved!");
    }
    public string GetNickName()
    {

        string nickname = PlayerPrefs.GetString("NickName");
        return nickname;

    }

}
