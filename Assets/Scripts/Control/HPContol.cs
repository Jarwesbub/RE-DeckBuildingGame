using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using System;

public class HPContol : MonoBehaviour
{
    public GameObject GameUIControl, localCharacterCard, OtherCharacterCard;
    public TMP_Text localHpTxt, otherHpTxt, localMaxHpTxt, otherMaxHpTxt;
    public TMP_Text hpPlus, hpMinus;
    //private GameObject PlayerInfoMaster;
    public Slider slider;
    public int playerID, myMaxHP, myFullHP;
    private int myHPDiff;
    public float myHP;
    //[SerializeField] private List<string> CharacterCardList;
    [SerializeField] private List<int> CharacterNumberList;
    [SerializeField] private List<int> CharacterHPList;

    void Start()
    {
        playerID = PhotonNetwork.LocalPlayer.ActorNumber;
        //PlayerInfoMaster = GameObject.FindWithTag("PlayerInfoMaster");
        MakeCharacterCardLists();
        int get = playerID - 1;

        myMaxHP = GetCharacterHPInfo(CharacterNumberList[get]);
        slider.maxValue = myMaxHP;
        myFullHP = myMaxHP;

        myHP = slider.value;
        localHpTxt.text = "HP : " + myHP.ToString();
        localMaxHpTxt.text = "Max HP : " + myMaxHP.ToString();
        ResetMyHPDifferenceText();

        GameObject MainCanvas = GameObject.FindWithTag("MainCanvas");
        MainCanvas.GetComponent<GameControl>().ShowLocalPlayerHP();
    }

    private void MakeCharacterCardLists()
    {
        foreach (DictionaryEntry info in GameStats.playerInfos)
        {
            //int id = (int)info.Key;
            string cardName = (string)info.Value;
            int number = Int32.Parse(cardName.Substring(3, 3)); //Row 3 -> next 3 letters
            CharacterNumberList.Add(number);

        }

        /* OLD DELETE
        int count = PlayerInfoMaster.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            GameObject child = PlayerInfoMaster.transform.GetChild(i).gameObject;
            string characterName = child.GetComponent<PlayerInfo>().myCharacterCard;
            //CharacterCardList.Add(characterName);
            int number = Int32.Parse(characterName.Substring(3, 3)); //Row 3 -> next 3 letters
            CharacterNumberList.Add(number);

            int hp = GetCharacterHPInfo(number);

            CharacterHPList.Add(hp);
        }
        */
    }

    private int GetCharacterHPInfo(int v)
    {
        int HP=80;

        //HP70 = 3 6 13 15 21 26 27 29 43 (promo rebecca = 101)
        //HP80 = 2 4 7 9 12 17 18 19 20 22 23 25 36 37 38 39 40 42
        //HP90 = 5 11 14 16 24 28 30 34 45 46 48 50 (promo hunk = 100)
        //HP100 = 1 35 44
        //HP110 = 41 49 51
        //HP120 = 8 47

        if (v == 8 || v == 47)
            HP = 120;
        else if (v == 41 || v == 49 || v == 51)
            HP = 110;
        else if (v == 1 || v == 35 || v == 44)
            HP = 100;

        else if (v == 3 || v == 6 || v == 13 || v == 15 || v == 21 || v == 26 || v == 27 || v == 29
            || v == 43 || v == 101) //HP70
            HP = 70;

        else if (v == 5 || v == 11 || v == 14 || v == 16 || v == 24 || v == 28 || v == 30 || v == 34 //HP90
             || v == 45 || v == 46 || v == 48 || v == 50 || v == 100)
            HP = 90;
        else
            HP = 80;

        
        return HP;
    }

    public void OnClickAddHP()
    {
        if (slider.value < myFullHP)
        {
            slider.value += 5;
            HPChange();
            ShowHPDiff(true);
        }
    }
    public void OnClickTakeHP()
    {
        if (myMaxHP > 5)
        {
            slider.value -= 5;
            HPChange();
            ShowHPDiff(false);
        }
    }

    public void HPChange()
    {
        if(slider.value<0.1f)
        {
            myMaxHP -= 20;
            myHP = myFullHP;
            slider.value = myHP;
        }
        if (myMaxHP > 0)
        {
            myHP = slider.value;
            localHpTxt.text = "HP : " + myHP.ToString();
            localMaxHpTxt.text = "Max HP : " + myMaxHP.ToString();
        }
        else
        {
            myHP = 0f;
            slider.value = myHP;
            localHpTxt.text = "HP : " + "0";
            localMaxHpTxt.text = "YOU ARE DEAD";
            GameUIControl.GetComponent<GameUIControl>().isLocalPlayerDead = true;
        }
    }

    public void ShowHPToOthers(float hp, float maxhp)
    {
        otherHpTxt.text = "HP : " + hp;
        otherMaxHpTxt.text = "Max HP : " + maxhp;

        if(maxhp > 0)
            otherMaxHpTxt.text = "Max HP : " + maxhp;
        else
            otherMaxHpTxt.text = "YOU ARE DEAD";

    }
    private void ShowHPDiff(bool addHP)
    {
        if(!addHP) //If you lose hp
            myHPDiff -= 5;
        else if (myMaxHP > 0) //If you get hp but are not dead
            myHPDiff += 5;

        if(myHPDiff==0)
        {
            hpPlus.text = ""; hpMinus.text = "";
        }
        else if(myHPDiff<0)
        {
            hpMinus.text = myHPDiff.ToString();
            hpPlus.text = "";
        }
        else if (myHPDiff>0)
        {
            hpPlus.text = "+" + myHPDiff;
            hpMinus.text = "";
        }
    }


    public void ResetMyHPDifferenceText()
    {
        hpPlus.text = ""; hpMinus.text = ""; myHPDiff = 0;
        myHPDiff = 0;
    }
}
