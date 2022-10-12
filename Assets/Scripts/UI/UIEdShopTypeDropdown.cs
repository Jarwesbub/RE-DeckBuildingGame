using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEdShopTypeDropdown : MonoBehaviour
{
    public GameObject EditShopControl;
    private Dropdown dropdown;

    private void Start()
    {
        dropdown = transform.GetComponent<Dropdown>();
        dropdown.options.Clear();

        List<string> items = new List<string>();

        items.Add("Ammunation");
        items.Add("Health items");
        items.Add("Knives");
        items.Add("Handguns");
        items.Add("Shotguns");
        items.Add("Machine guns");
        items.Add("Rifles");
        items.Add("Explosives");
        items.Add("Action 1");
        items.Add("Action 2");
        items.Add("Action 3");
        items.Add("Action 4");
        items.Add("Action 5");
        items.Add("Action 6");
        items.Add("Action 7");
        items.Add("Extra sloth");
        items.Add("Random sloth");


        foreach (var item in items)
        {
            dropdown.options.Add(new Dropdown.OptionData() { text = item });
        }

        //dropdown.value = items.Count;
        dropdown.onValueChanged.AddListener(delegate { DropDownItemSelected(dropdown); });

    }
    public void FirstLoadDone()
    {
        EditShopControl.GetComponent<EditShopControl>().UpdateShopDeckType(0);
    }

    private void DropDownItemSelected(Dropdown dropdown)
    {
        int index = dropdown.value;
        EditShopControl.GetComponent<EditShopControl>().UpdateShopDeckType(index);

    }
}
