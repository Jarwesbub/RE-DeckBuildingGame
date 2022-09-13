using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EditMansionMenu : MonoBehaviour
{
    public GameObject MenuObject;

    void Start()
    {
        MenuObject.SetActive(false);
    }

    public void OnClickShowMenu(bool show)
    {
        MenuObject.SetActive(show);
    }

    public void OnClickReturnToStartMenu()
    {
        SceneManager.LoadScene("StartMenu");

    }
}
