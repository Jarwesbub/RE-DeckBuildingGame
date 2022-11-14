using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Abstract class of ShopControl.cs -> keeps code cleaner (Unity UI's OnClick buttons are located here)

public abstract class ShopBaseControl: MonoBehaviourPun   //
{
    public GameObject Ammo10, Ammo20, Ammo30, Handgun, Knife, Grenade, HP;   //All buttons in ShopMenu
    public GameObject Shotgun, AR_SG, Rifle;                           //All buttons in ShopMenu
    public GameObject Action1, Action2, Action3, Action4, Action5, Action6, Action7;    //All buttons in ShopMenu
    public GameObject Extra1, Extra2;
    public GameObject AmmoCountListPrefab, HandgunListPrefab, KnifeListPrefab, GrenadeListPrefab, HPListPrefab;   //ShopLists_AllCounted (.text)
    public GameObject ShotgunListPrefab, AR_SG_ListPrefab, RiflesListPrefab;                                    //ShopLists_AllCounted (.text)
    public GameObject Action1ListPrefab, Action2ListPrefab, Action3ListPrefab, Action4ListPrefab, Action5ListPrefab, Action6ListPrefab, Action7ListPrefab;
    public GameObject Extra1ListPrefab, Extra2ListPrefab;

    public PhotonView view;
    protected Vector3 vec_Normal = new Vector3(1, 1, 1);
    protected Vector3 vec_Zoom = new Vector3(1.5f, 1.5f, 1); //OLD = 1.8f
    public bool waitRPC;  //Shows if "wait time" is active while sending data in network
    //private string holdNameRPC; //Holds the name of the next card (accessed in coroutine)
    //public int myPlayerID,currentPlayerID;
    protected int[] ammoCounts; //Max ammount of cards/type -> works in harmony with "activeCardObjectList" (same numbers)
    protected List<string> HandgunList, KnifeList, GrenadeList, HPList, ShotgunList, AR_SG_List, RifleList; //List of all card names by type
    protected List<string> ActionList1, ActionList2, ActionList3, ActionList4, ActionList5, ActionList6, ActionList7;           //List of all card names by type
    protected List<string> ExtraList1, ExtraList2;

    public void OnClickAmmo10() //Stretch button image bigger and show "Buy" button
    {
        if (!waitRPC && view.IsMine)
        {
            SetAllCardsToNormalSize();
            CheckShopCard(Ammo10, ammoCounts[0]); //0 = Ammo10;
        }
    }
    public void OnClickAmmo10Buy() //"Buy" button which sends purchased card to local player's discard pile and deletes it in shop
    {
        if (!waitRPC && view.IsMine)
        {
            Add_LM_HandDeck(Ammo10.GetComponent<Image>().sprite);
            BuyShopCard(Ammo10, ammoCounts[0], "ia_ammo10", 0); //0 = Ammo10;
        }
    }

    public void OnClickAmmo20()
    {
        if (!waitRPC && view.IsMine)
        {
            SetAllCardsToNormalSize();
            CheckShopCard(Ammo20, ammoCounts[1]); //1 = Ammo20;
        }
    }
    public void OnClickAmmo20Buy()
    {
        if (!waitRPC && view.IsMine)
        {
            Add_LM_HandDeck(Ammo20.GetComponent<Image>().sprite);
            BuyShopCard(Ammo20, ammoCounts[1], "ia_ammo20", 1); //1 = Ammo20;
        }
    }

    public void OnClickAmmo30()
    {
        if (!waitRPC && view.IsMine)
        {
            SetAllCardsToNormalSize();
            CheckShopCard(Ammo30, ammoCounts[2]); //2 = Ammo30;
        }

    }
    public void OnClickAmmo30Buy()
    {
        if (!waitRPC && view.IsMine)
        {
            Add_LM_HandDeck(Ammo30.GetComponent<Image>().sprite);
            BuyShopCard(Ammo30, ammoCounts[2], "ia_ammo30", 2); //2 = Ammo30;
        }

    }
    ///////////////////////////////////////
    public void OnClickHandgun()
    {
        if (!waitRPC && view.IsMine)
        {
            SetAllCardsToNormalSize();
            CheckShopCard(Handgun, HandgunList.Count); //3 = Handguns;
        }
    }
    public void OnClickHandgunBuy()
    {
        if (!waitRPC && view.IsMine)
        {
            string name = Handgun.GetComponent<SpriteFromAtlas>().spriteName;
            Add_LM_HandDeck(Handgun.GetComponent<Image>().sprite);
            BuyShopCard(Handgun, HandgunList.Count, name, 3); //3 = Handguns;
        }
    }

    public void OnClickKnife()
    {
        if (!waitRPC && view.IsMine)
        {
            SetAllCardsToNormalSize();
            CheckShopCard(Knife, KnifeList.Count); //4 = Knifes;
        }
    }
    public void OnClickKnifeBuy()
    {
        if (!waitRPC && view.IsMine)
        {
            string name = Knife.GetComponent<SpriteFromAtlas>().spriteName;
            Add_LM_HandDeck(Knife.GetComponent<Image>().sprite);
            BuyShopCard(Knife, KnifeList.Count, name, 4); //4 = Knifes;
        }
    }

    public void OnClickGrenade()
    {
        if (!waitRPC && view.IsMine)
        {
            SetAllCardsToNormalSize();
            CheckShopCard(Grenade, GrenadeList.Count); //5 = Grenades;
        }
    }
    public void OnClickGrenadeBuy()
    {
        if (!waitRPC && view.IsMine)
        {
            string name = Grenade.GetComponent<SpriteFromAtlas>().spriteName;
            Add_LM_HandDeck(Grenade.GetComponent<Image>().sprite);
            BuyShopCard(Grenade, GrenadeList.Count, name, 5); //5 = Grenades;
        }
    }

    public void OnClickHPHerbs()
    {
        if (!waitRPC && view.IsMine)
        {
            SetAllCardsToNormalSize();
            CheckShopCard(HP, HPList.Count); //6 = HP (herbs and first aid);
        }
    }
    public void OnClickHPHerbsBuy()
    {
        if (!waitRPC && view.IsMine)
        {
            string name = HP.GetComponent<SpriteFromAtlas>().spriteName;
            Add_LM_HandDeck(HP.GetComponent<Image>().sprite);
            BuyShopCard(HP, HPList.Count, name, 6); //6 = HP (herbs and first aid);
        }
    }

    ///////////////////////////////////////

    public void OnClickShotgun()
    {
        if (!waitRPC && view.IsMine)
        {
            SetAllCardsToNormalSize();
            CheckShopCard(Shotgun, ShotgunList.Count); //7 = Shotguns;
        }
    }
    public void OnClickShotgunBuy()
    {
        if (!waitRPC && view.IsMine)
        {
            string name = Shotgun.GetComponent<SpriteFromAtlas>().spriteName;
            Add_LM_HandDeck(Shotgun.GetComponent<Image>().sprite);
            BuyShopCard(Shotgun, ShotgunList.Count, name, 7); //7 = Shotguns;
        }
    }

    public void OnClickAR_SG()
    {
        if (!waitRPC && view.IsMine)
        {
            SetAllCardsToNormalSize();
            CheckShopCard(AR_SG, AR_SG_List.Count); //8 = Assault rifles and Submachine guns;
        }
    }
    public void OnClickAR_SGBuy()
    {
        if (!waitRPC && view.IsMine)
        {
            string name = AR_SG.GetComponent<SpriteFromAtlas>().spriteName;
            Add_LM_HandDeck(AR_SG.GetComponent<Image>().sprite);
            BuyShopCard(AR_SG, AR_SG_List.Count, name, 8); //8 = Assault rifles and Submachine guns;
        }
    }

    public void OnClickRifle()
    {
        if (!waitRPC && view.IsMine)
        {
            SetAllCardsToNormalSize();
            CheckShopCard(Rifle, RifleList.Count); //9 = Rifles;
        }
    }
    public void OnClickRifleBuy()
    {
        if (!waitRPC && view.IsMine)
        {
            string name = Rifle.GetComponent<SpriteFromAtlas>().spriteName;
            Add_LM_HandDeck(Rifle.GetComponent<Image>().sprite);
            BuyShopCard(Rifle, RifleList.Count, name, 9); //9 = Rifles;
        }
    }

    ///////////////////////////////////////

    public void OnClickAction1()
    {
        if (!waitRPC && view.IsMine)
        {
            SetAllCardsToNormalSize();
            CheckShopCard(Action1, ActionList1.Count); //10 = Action1;
        }
    }
    public void OnClickAction1Buy()
    {
        if (!waitRPC && view.IsMine)
        {
            string name = Action1.GetComponent<SpriteFromAtlas>().spriteName;
            Add_LM_HandDeck(Action1.GetComponent<Image>().sprite);
            BuyShopCard(Action1, ActionList1.Count, name, 10); //10 = Action1;
        }
    }

    public void OnClickAction2()
    {
        if (!waitRPC && view.IsMine)
        {
            SetAllCardsToNormalSize();
            CheckShopCard(Action2, ActionList2.Count); //11 = Action2;
        }
    }
    public void OnClickAction2Buy()
    {
        if (!waitRPC && view.IsMine)
        {
            string name = Action2.GetComponent<SpriteFromAtlas>().spriteName;
            Add_LM_HandDeck(Action2.GetComponent<Image>().sprite);
            BuyShopCard(Action2, ActionList2.Count, name, 11); //11 = Action2;
        }
    }

    public void OnClickAction3()
    {
        if (!waitRPC && view.IsMine)
        {
            SetAllCardsToNormalSize();
            CheckShopCard(Action3, ActionList3.Count); //12 = Action3;
        }
    }
    public void OnClickAction3Buy()
    {
        if (!waitRPC && view.IsMine)
        {
            string name = Action3.GetComponent<SpriteFromAtlas>().spriteName;
            Add_LM_HandDeck(Action3.GetComponent<Image>().sprite);
            BuyShopCard(Action3, ActionList3.Count, name, 12); //12 = Action3;
        }
    }

    public void OnClickAction4()
    {
        if (!waitRPC && view.IsMine)
        {
            SetAllCardsToNormalSize();
            CheckShopCard(Action4, ActionList4.Count); //13 = Action4;
        }
    }
    public void OnClickAction4Buy()
    {
        if (!waitRPC && view.IsMine)
        {
            string name = Action4.GetComponent<SpriteFromAtlas>().spriteName;
            Add_LM_HandDeck(Action4.GetComponent<Image>().sprite);
            BuyShopCard(Action4, ActionList4.Count, name, 13); //13 = Action4;
        }
    }

    public void OnClickAction5()
    {
        if (!waitRPC && view.IsMine)
        {
            SetAllCardsToNormalSize();
            CheckShopCard(Action5, ActionList5.Count); //14 = Action5;
        }
    }
    public void OnClickAction5Buy()
    {
        if (!waitRPC && view.IsMine)
        {
            string name = Action5.GetComponent<SpriteFromAtlas>().spriteName;
            Add_LM_HandDeck(Action5.GetComponent<Image>().sprite);
            BuyShopCard(Action5, ActionList5.Count, name, 14); //14 = Action5;
        }
    }

    public void OnClickAction6()
    {
        if (!waitRPC && view.IsMine)
        {
            SetAllCardsToNormalSize();
            CheckShopCard(Action6, ActionList6.Count); //15 = Action6;
        }
    }
    public void OnClickAction6Buy()
    {
        if (!waitRPC && view.IsMine)
        {
            string name = Action6.GetComponent<SpriteFromAtlas>().spriteName;
            Add_LM_HandDeck(Action6.GetComponent<Image>().sprite);
            BuyShopCard(Action6, ActionList6.Count, name, 15); //15 = Action6;
        }
    }

    public void OnClickAction7()
    {
        if (!waitRPC && view.IsMine)
        {
            SetAllCardsToNormalSize();
            CheckShopCard(Action7, ActionList7.Count); //16 = Action7;
        }
    }
    public void OnClickAction7Buy()
    {
        if (!waitRPC && view.IsMine)
        {
            string name = Action7.GetComponent<SpriteFromAtlas>().spriteName;
            Add_LM_HandDeck(Action7.GetComponent<Image>().sprite);
            BuyShopCard(Action7, ActionList7.Count, name, 16); //16 = Action7;
        }
    }
    /// <summary>
    public void OnClickExtra1() //Stretch button image bigger and show "Buy" button
    {
        if (!waitRPC && view.IsMine)
        {
            SetAllCardsToNormalSize();
            CheckShopCard(Extra1, ExtraList1.Count); //17 = Extra1;
        }
    }
    public void OnClickExtra1Buy()
    {
        if (!waitRPC && view.IsMine)
        {
            string name = Extra1.GetComponent<SpriteFromAtlas>().spriteName;
            Add_LM_HandDeck(Extra1.GetComponent<Image>().sprite);
            BuyShopCard(Extra1, ExtraList1.Count, name, 17); //17 = Extra1;
        }
    }
    public void OnClickExtra2() //Stretch button image bigger and show "Buy" button
    {
        if (!waitRPC && view.IsMine)
        {
            SetAllCardsToNormalSize();
            CheckShopCard(Extra2, ExtraList2.Count); //18 = Extra2;
        }
    }
    public void OnClickExtra2Buy()
    {
        if (!waitRPC && view.IsMine)
        {
            string name = Extra2.GetComponent<SpriteFromAtlas>().spriteName;
            Add_LM_HandDeck(Extra2.GetComponent<Image>().sprite);
            BuyShopCard(Extra2, ExtraList2.Count, name, 18); //18 = Extra2;
        }
    }

    protected abstract void CheckShopCard(GameObject cardObj, int cardCount);
    protected abstract void Add_LM_HandDeck(Sprite sprt);
    protected abstract void BuyShopCard(GameObject o, int count_value, string name, int indexType_value);

    public void SetAllCardsToNormalSize() //When ShopMenu button is pressed (GameControl.cs)
    {
        Ammo10.transform.localScale = vec_Normal;
        Ammo10.transform.GetChild(0).gameObject.SetActive(false);
        Ammo20.transform.localScale = vec_Normal;
        Ammo20.transform.GetChild(0).gameObject.SetActive(false);
        Ammo30.transform.localScale = vec_Normal;
        Ammo30.transform.GetChild(0).gameObject.SetActive(false);
        Handgun.transform.localScale = vec_Normal;
        Handgun.transform.GetChild(0).gameObject.SetActive(false);
        Knife.transform.localScale = vec_Normal;
        Knife.transform.GetChild(0).gameObject.SetActive(false);
        Grenade.transform.localScale = vec_Normal;
        Grenade.transform.GetChild(0).gameObject.SetActive(false);
        HP.transform.localScale = vec_Normal;
        HP.transform.GetChild(0).gameObject.SetActive(false);
        Shotgun.transform.localScale = vec_Normal;
        Shotgun.transform.GetChild(0).gameObject.SetActive(false);
        AR_SG.transform.localScale = vec_Normal;
        AR_SG.transform.GetChild(0).gameObject.SetActive(false);
        Rifle.transform.localScale = vec_Normal;
        Rifle.transform.GetChild(0).gameObject.SetActive(false);
        Action1.transform.localScale = vec_Normal;
        Action1.transform.GetChild(0).gameObject.SetActive(false);
        Action2.transform.localScale = vec_Normal;
        Action2.transform.GetChild(0).gameObject.SetActive(false);
        Action3.transform.localScale = vec_Normal;
        Action3.transform.GetChild(0).gameObject.SetActive(false);
        Action4.transform.localScale = vec_Normal;
        Action4.transform.GetChild(0).gameObject.SetActive(false);
        Action5.transform.localScale = vec_Normal;
        Action5.transform.GetChild(0).gameObject.SetActive(false);
        Action6.transform.localScale = vec_Normal;
        Action6.transform.GetChild(0).gameObject.SetActive(false);
        Action7.transform.localScale = vec_Normal;
        Action7.transform.GetChild(0).gameObject.SetActive(false);
        Extra1.transform.localScale = vec_Normal;
        Extra1.transform.GetChild(0).gameObject.SetActive(false);
        Extra2.transform.localScale = vec_Normal;
        Extra2.transform.GetChild(0).gameObject.SetActive(false);
    }


}
