using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class UILobbyRoomDropDownHandler : MonoBehaviour
{
    [SerializeField] int dropdownType;
    [SerializeField] GameObject LobbyRoomControl;

    void Start()
    {
        if (dropdownType == 0)
            MansionType();
        else if(dropdownType == 1)
            ShopType();
        else
            CharacterType();

    }
    private void MansionType()
    {
        var dropdown = transform.GetComponent<Dropdown>();
        int cardCount = GameStats.MansionDeckCount;
        dropdown.options.Clear();

        List<string> items = new List<string>();

        for (int i = 1; i <= cardCount; i++)
        {
            string textFilePath = Application.persistentDataPath + "/Custom_data/MansionCards" + i + ".txt";
            string name = File.ReadLines(textFilePath).First();
            items.Add(name);
        }


        foreach (var item in items)
        {
            dropdown.options.Add(new Dropdown.OptionData() { text = item });
        }

        int value = GameStats.MansionDeckValue;
        if (value > 0) value--;
        dropdown.value = value;
        dropdown.RefreshShownValue();
        dropdown.onValueChanged.AddListener(delegate { MansionDropDownItemSelected(dropdown); });
    }
    private void MansionDropDownItemSelected(Dropdown dropdown)
    {
        int index = dropdown.value + 1;
        GameStats.MansionDeckValue = index;
    }

    ////////////////////////////

    private void ShopType()
    {
        var dropdown = transform.GetComponent<Dropdown>();
        int cardCount = GameStats.ShopDeckDataCount;
        dropdown.options.Clear();
        if (cardCount == 0) cardCount=1;

        List<string> items = new List<string>();

        for (int i = 1; i <= cardCount; i++)
        {
            string textFilePath = Application.persistentDataPath + "/Custom_data/ShopCardsData" + i + ".txt";
            string name = File.ReadLines(textFilePath).First();
            items.Add(name);
        }


        foreach (var item in items)
        {
            dropdown.options.Add(new Dropdown.OptionData() { text = item });
        }

        int value = GameStats.ShopDeckDataValue;
        if (value > 0) value--;

        dropdown.value = value;
        dropdown.RefreshShownValue();
        dropdown.onValueChanged.AddListener(delegate { ShopDropDownItemSelected(dropdown); });
    }
    private void ShopDropDownItemSelected(Dropdown dropdown)
    {
        int index = dropdown.value + 1;
        //LobbyRoomControl.GetComponent<OverwriteTextFileList>().Shop_CreateDataCards(index);
        GameStats.ShopDeckDataValue = index;
    }

    ////////////////////////////
    
    private void CharacterType()
    {

        var dropdown = transform.GetComponent<Dropdown>();
        int cardCount = GameStats.CharacterDeckCount;
        dropdown.options.Clear();
        if (cardCount == 0) cardCount=1;

        List<string> items = new List<string>();

        for (int i = 1; i <= cardCount; i++)
        {
            string textFilePath = Application.persistentDataPath + "/Custom_data/CharacterCards" + i + ".txt";
            string name = File.ReadLines(textFilePath).First();
            items.Add(name);
        }


        foreach (var item in items)
        {
            dropdown.options.Add(new Dropdown.OptionData() { text = item });
        }

        int value = GameStats.CharacterDeckValue;
        if (value > 0) value--;

        dropdown.value = value;
        dropdown.RefreshShownValue();
        dropdown.onValueChanged.AddListener(delegate { CharacterDropDownItemSelected(dropdown); });

    }

    private void CharacterDropDownItemSelected(Dropdown dropdown)
    {
        int index = dropdown.value + 1;
        GameStats.CharacterDeckValue = index;
    }

}
