using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIEdMansionLoadDropDownHandler : MonoBehaviour
{
    private int customCount;
    private Dropdown dropdown;

    private void Start()
    {
        dropdown = transform.GetComponent<Dropdown>();
        dropdown.options.Clear();

        List<string> items = new List<string>();

        customCount = GameStats.MansionDeckCount;

        if(customCount>0)
            for (int i = 1; i <= customCount; i++)
            {
                string textFilePath = Application.persistentDataPath + "/Custom_data/MansionCards" + i + ".txt";
                string name = File.ReadLines(textFilePath).First();
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
        //PlayerPrefs.SetInt("MansionType", index);
        GameStats.MansionDeckValue = index+1;

    }

    public void ChangeDropItemNameByIndex(int index, string name)
    {
        //if (index <= customCount)
        {
            Dropdown.OptionData newItem = new Dropdown.OptionData(name);
            this.dropdown.options.RemoveAt(index);
            this.dropdown.options.Insert(index, newItem);
        }
    }
}

