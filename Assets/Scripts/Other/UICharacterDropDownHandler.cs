using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterDropDownHandler : MonoBehaviour
{
    int value;

    void Start()
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
        dropdown.onValueChanged.AddListener(delegate { DropDownItemSelected(dropdown); });
    }


    private void DropDownItemSelected(Dropdown dropdown)
    {
        int index = dropdown.value;
        PlayerPrefs.SetInt("CharacterType", index);

    }
}
