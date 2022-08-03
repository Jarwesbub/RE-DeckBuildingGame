using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyControl : MonoBehaviour
{
    public GameObject NameEntry, JoinLobby, SaveMaster;
    public InputField setNickName;
    public Text errorMessage;

    void Start()
    {
        NameEntry.SetActive(true);
        JoinLobby.SetActive(false);

        string name = SaveMaster.GetComponent<SaveMaster>().GetNickName();
        if (name != null)
            setNickName.text = name;

    }



    public void GoToLobby()
    {
        errorMessage.text = "";
        string name = setNickName.text;

        if (name == "" || name == " ")
        {
            errorMessage.text = "Enter name!";

        }
        else if (name.Length >= 16)
        {
            errorMessage.text = "Name too long!";
        }
        else if (name == "jarkko")
        {
            errorMessage.text = "Ei apinoita!";
        }
        else
        {
            SaveMaster.GetComponent<SaveMaster>().SetNickName(name);

            NameEntry.SetActive(false);
            JoinLobby.SetActive(true);
        }
    }
}
