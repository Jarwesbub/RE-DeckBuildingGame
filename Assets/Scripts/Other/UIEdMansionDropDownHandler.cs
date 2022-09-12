using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEdMansionDropDownHandler : MonoBehaviour
{
    public int customCount;
    private int value;

    private void Start()
    {
        if (PlayerPrefs.HasKey("MansionType")) //by number: 1-4 Custom Mansion Decks
            value = PlayerPrefs.GetInt("MansionType");
        else
        {
            PlayerPrefs.SetInt("MansionType", 1);
            value = 1;
        }

        var dropdown = transform.GetComponent<Dropdown>();
        dropdown.options.Clear();

        List<string> items = new List<string>();

        if (customCount == 0)
            Debug.Log("NO MansionCards1-? .txt files");
        else
            for (int i = 0; i < customCount; i++)
            {
                int val = i + 1;
                string name = "Custom " + val;
                items.Add(name);
            }

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
        PlayerPrefs.SetInt("MansionType", index);

    }
}

