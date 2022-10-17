using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEdShopLoadDropDownHandler : MonoBehaviour
{
    public GameObject EditShopControl;
    private int shopDataCount;
    Dropdown dropdown;

    private void Start()
    {
        dropdown = transform.GetComponent<Dropdown>();

        shopDataCount = GameStats.ShopDeckDataCount;
        if (shopDataCount == 0)
            shopDataCount++;

        SetLoadDropDown();
    }


    private void SetLoadDropDown()
    {
        dropdown.options.Clear();

        List<string> items = new List<string>();

        //items.Add(" ");

        for (int i = 1; i <= shopDataCount; i++)
        {
            items.Add("Custom-ShopData-" + shopDataCount);
        }


        foreach (var item in items)
        {
            dropdown.options.Add(new Dropdown.OptionData() { text = item });
        }

        dropdown.value = 0;
        dropdown.RefreshShownValue();
        dropdown.onValueChanged.AddListener(delegate { DropDownItemSelected(dropdown); });

    }

    private void DropDownItemSelected(Dropdown dropdown)
    {
        int index = dropdown.value;
        Debug.Log("Load ShopData pressed");
        //if(index!=0)
        EditShopControl.GetComponent<EditShopControl>().LoadNewShopData(index);

    }
}
