using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class HandCardStatsUI : MonoBehaviourPun
{
    public TMP_Text playAreaStats, turnTitle;
    int cardCount=0, ammo=0, dmg=0, gold=0;
    int buys=0, maxBuys=1;
    float waitTime = 0f;
    int gameMode;
    PhotonView view;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    private void Start()
    {
        gameMode = GameStats.GameMode;
        DrawPlayAreaText();
    }

    public void SetPlayArea(int _ammo, int _dmg, int _gold, bool add)
    {
        view.RPC("Pun_SetPlayArea", RpcTarget.AllBuffered, _ammo, _dmg, _gold, add);
    }
    [PunRPC]public void Pun_SetPlayArea(int _ammo, int _dmg, int _gold, bool add)
    {
        if (add)
        {
            cardCount++;
            ammo += _ammo;
            dmg += _dmg;
            gold += _gold;
        }
        else
        {
            cardCount--;
            ammo -= _ammo;
            dmg -= _dmg;
            gold -= _gold;
        }
        DrawPlayAreaText();
    }
    public void AddToWeaponStats(int _ammo, int _dmg)
    {
        view.RPC("Pun_AddToWeaponStats", RpcTarget.AllBuffered, _ammo, _dmg);
    }
    [PunRPC] public void Pun_AddToWeaponStats(int _ammo, int _dmg)
    {
        ammo += _ammo;
        dmg += _dmg;
        DrawPlayAreaText();
    }

    public void AddBuyToUI()
    {
        view.RPC("Rpc_AddBuyToUI", RpcTarget.AllBuffered);
    }
    [PunRPC] public void Rpc_AddBuyToUI()
    {
        buys++;
        DrawPlayAreaText();
    }
    public int GetBuysCount()
    {
        return buys;
    }
    public bool CheckIfEnoughBuys() //ShopControl.cs
    {
        if (gameMode == 2)
            return buys < maxBuys;
        else
            return true;
    }

    public void AddMaxBuysToUI(int _maxBuys)
    {
        view.RPC("Pun_AddMaxBuysToUI", RpcTarget.AllBuffered, _maxBuys);
    }
    [PunRPC] public void Pun_AddMaxBuysToUI(int _maxBuys)
    {
        maxBuys += _maxBuys;
        DrawPlayAreaText();
    }

    public void ResetPlayArea() // End Turn -button
    {
        view.RPC("Pun_ResetPlayArea", RpcTarget.AllBuffered);
    }
    [PunRPC] public void Pun_ResetPlayArea()
    {
        cardCount = 0;
        ammo = 0;
        dmg = 0;
        gold = 0;
        buys = 0; maxBuys = 1;
        DrawPlayAreaText();
    }

    private void DrawPlayAreaText()
    {
        playAreaStats.text = "\n" + "Ammo:\t" + ammo + "\n" + "Damage:\t" + dmg + "\n" + "Gold:\t\t" + gold + "\n" +
        "Buys:\t\t" + buys + "/" + maxBuys;
    }

    public string GetShopInfo() //ShopControl.cs
    {
        return "Gold:\t" + gold + "\t" + "Buys:\t" + buys + "/" + maxBuys;
    }

    public void SetPlayerTurnTitle(string txt, float time)
    {
        if(time==0)
            turnTitle.text = txt;
        else
            StartCoroutine(SetTurnText(txt,time));
    }
    public void ResetPlayerTurnTitle()
    {
        turnTitle.text = "";
    }

    public bool CheckIfWeaponHasEnoughAmmo(int _ammo)
    {
        if ((_ammo+ammo)>=0)
            return true;
        else
            return false;
    }

    public int GetPlayerAmmoValue()
    {
        return ammo;
    }

    IEnumerator SetTurnText(string txt, float time)
    {
        turnTitle.text = txt;
        if (time < 0)
            waitTime = 0;
        else
            waitTime += time;
        yield return new WaitForSeconds(waitTime);
        turnTitle.text = "";
    }
}
