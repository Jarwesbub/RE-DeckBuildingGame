using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class UIEdShopLoadDropDownHandler : MonoBehaviour
{
    public GameObject EditShopControl;
    private int shopDataCount;
    Dropdown dropdown;
    private List<string> dropItems;
    bool isFirstLoad;

    private void Start()
    {
        dropdown = transform.GetComponent<Dropdown>();
        isFirstLoad = true;
        shopDataCount = GameStats.ShopDeckDataCount;
        if (shopDataCount == 0)
            shopDataCount++;

        SetLoadDropDown();
    }


    private void SetLoadDropDown()
    {
        dropdown.options.Clear();


        //items.Add(" ");
        dropItems = new();
        for (int i = 1; i <= shopDataCount; i++)
        {
            string textFilePath = Application.persistentDataPath + "/Custom_data/ShopCardsData" + i + ".txt";
            string name = File.ReadLines(textFilePath).First();
            dropItems.Add(name);
        }

        foreach (var item in dropItems)
        {
            dropdown.options.Add(new Dropdown.OptionData() { text = item });
        }
        dropdown.options.Add(new Dropdown.OptionData() { text = "" }); //Add empty for first load

        dropdown.value = dropItems.Count;
        dropdown.RefreshShownValue();
        dropdown.onValueChanged.AddListener(delegate { DropDownItemSelected(dropdown); });

    }

    private void DropDownItemSelected(Dropdown dropdown)
    {
        int index = dropdown.value;
        if (isFirstLoad)
        {
            isFirstLoad = false;
            this.dropdown.options.RemoveAt(this.dropdown.options.Count-1); //Remove last one (empty item)
        }


        EditShopControl.GetComponent<EditShopControl>().LoadNewShopData(index);

    }

    public void ChangeDropItemNameByIndex(int index, string name)
    {
        if (index <= shopDataCount)
        {
            Dropdown.OptionData newItem = new Dropdown.OptionData(name);
            this.dropdown.options.RemoveAt(index);
            this.dropdown.options.Insert(index, newItem);
        }
    }
}
