using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Photon.Pun;
//using Photon.Realtime;

public class CharacterCard : MonoBehaviour
{
    public List<string> CurrentCharactersList;


    public void AddCharacterToList(string name)
    {
        CurrentCharactersList.Add(name);
        //Debug.Log(name + " added!");

    }

}
