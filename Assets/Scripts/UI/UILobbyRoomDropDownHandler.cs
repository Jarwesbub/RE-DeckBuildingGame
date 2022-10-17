using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyRoomDropDownHandler : MonoBehaviour
{
    [SerializeField] int dropdownType;
    [SerializeField] GameObject LobbyRoomControl;
    //private int value;

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
            items.Add("Custom Deck "+i);
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
        PlayerPrefs.SetInt("MansionDeckValue", index);
        GameStats.MansionDeckValue = index;
    }

    ////////////////////////////

    private void ShopType()
    {
        var dropdown = transform.GetComponent<Dropdown>();
        int cardCount = GameStats.ShopDeckDataCount;
        dropdown.options.Clear();
        if (cardCount == 0)
            cardCount++;

        List<string> items = new List<string>();

        for (int i = 1; i <= cardCount; i++)
        {
            items.Add("Shop cards: " + i);
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
        /*
        if (PlayerPrefs.HasKey("CharacterType")) //0 = Original; 1 = Custom
            value = PlayerPrefs.GetInt("CharacterType");
        else
        {
            PlayerPrefs.SetInt("CharacterType", 0);
            value = 0;
        }*/

        int value = GameStats.CharacterDeckValue; //0 = Original; 1 = Custom

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
        //dropdown.Select();
        dropdown.RefreshShownValue();
        dropdown.onValueChanged.AddListener(delegate { CharacterDropDownItemSelected(dropdown); });

    }

    private void CharacterDropDownItemSelected(Dropdown dropdown)
    {
        int index = dropdown.value; //0 = Original; 1 = Custom
        PlayerPrefs.SetInt("CharacterType", index);
        GameStats.CharacterDeckValue = index;
    }

}
