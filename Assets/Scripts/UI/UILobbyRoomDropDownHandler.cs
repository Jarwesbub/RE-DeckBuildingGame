using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyRoomDropDownHandler : MonoBehaviour
{
    [SerializeField] bool isMansionType;
    private int value;

    void Start()
    {
        if (!isMansionType)
            CharacterType();
        else
            MansionType();

    }
    private void MansionType()
    {
        var dropdown = transform.GetComponent<Dropdown>();
        int cardCount = GameStats.MansionDeckCount;
        dropdown.options.Clear();

        List<string> items = new List<string>();

        for (int i = 1; i <= cardCount; i++)
        {
            items.Add("Custom Deck "+i);
        }


        foreach (var item in items)
        {
            dropdown.options.Add(new Dropdown.OptionData() { text = item });
        }

        dropdown.value = value;
        dropdown.onValueChanged.AddListener(delegate { MansionDropDownItemSelected(dropdown); });
    }

    private void CharacterType()
    {
        if (PlayerPrefs.HasKey("CharacterType")) //0 = Original; 1 = Custom
            value = PlayerPrefs.GetInt("CharacterType");
        else
        {
            PlayerPrefs.SetInt("CharacterType", 0);
            value = 0;
        }
        var dropdown = transform.GetComponent<Dropdown>();
        dropdown.options.Clear();

        List<string> items = new List<string>();
        items.Add("Original");
        items.Add("Custom");

        foreach (var item in items)
        {
            dropdown.options.Add(new Dropdown.OptionData() { text = item });
        }

        dropdown.value = value;
        dropdown.onValueChanged.AddListener(delegate { CharacterDropDownItemSelected(dropdown); });

    }

    private void CharacterDropDownItemSelected(Dropdown dropdown)
    {
        int index = dropdown.value;
        PlayerPrefs.SetInt("CharacterType", index);
    }
    private void MansionDropDownItemSelected(Dropdown dropdown)
    {
        int index = dropdown.value;
        GameStats.CharacterDeckValue = index;
    }
}
