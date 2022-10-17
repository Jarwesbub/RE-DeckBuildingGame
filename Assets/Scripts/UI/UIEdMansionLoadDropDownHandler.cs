using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEdMansionLoadDropDownHandler : MonoBehaviour
{
    public int customCount;

    private void Start()
    {
        //int value = 0;
        /*
        if (PlayerPrefs.HasKey("MansionType")) //by number: 1-4 Custom Mansion Decks
            value = PlayerPrefs.GetInt("MansionType");
        else
        {
            PlayerPrefs.SetInt("MansionType", 1);
            value = 1;
        }
        if (value == 0)
        {
            value = 1;
            GameStats.MansionDeckValue = value;
        }
        */
        Dropdown dropdown = transform.GetComponent<Dropdown>();
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

        dropdown.value = 0;
        dropdown.RefreshShownValue();
        dropdown.onValueChanged.AddListener(delegate { DropDownItemSelected(dropdown); });

    }

    private void DropDownItemSelected(Dropdown dropdown)
    {
        int index = dropdown.value;
        PlayerPrefs.SetInt("MansionType", index);

    }
}

