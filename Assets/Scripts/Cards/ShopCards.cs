using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopCards : MonoBehaviourPun   //
{
    private GameObject SpawnCards;
    public TMP_Text Sold;
    public GameObject Ammo10,Ammo20,Ammo30, Handgun,Knife,Grenade,HP;   //All buttons in ShopMenu
    public GameObject Shotgun, AR_SG, Rifle;                           //All buttons in ShopMenu
    public GameObject Action1, Action2, Action3, Action4, Action5;    //All buttons in ShopMenu
    public GameObject AmmoCountListPrefab, HandgunListPrefab, KnifeListPrefab,GrenadeListPrefab, HPListPrefab;   //ShopLists_AllCounted (.text)
    public GameObject ShotgunListPrefab, AR_SG_ListPrefab, RiflesListPrefab;                                    //ShopLists_AllCounted (.text)
    public GameObject Action1ListPrefab, Action2ListPrefab, Action3ListPrefab, Action4ListPrefab, Action5ListPrefab;
    [SerializeField] private List<GameObject> activeCardObjectList; //List of all objects. Can be accessed by list number! (used in [PunRPC])
    PhotonView view;
    private bool isZoomed; //Tells if current card in bigger or normal size
    Vector3 vec_Normal = new Vector3(1, 1, 1);
    Vector3 vec_Zoom = new Vector3(1.8f, 1.8f, 1);
    [SerializeField] private bool waitRPC;  //Shows if "wait time" is active while sending data in network
    private string holdNameRPC; //Holds the name of the next card (accessed in coroutine)
    //public int myPlayerID,currentPlayerID;
    [SerializeField] private List<int> count_Values; //Max ammount of cards/type -> works in harmony with "activeCardObjectList" (same numbers)
    private int currentCardObject;
    public string[] HandgunList,KnifeList,GrenadeList,HPList, ShotgunList,AR_SG_List,RifleList; //List of all card names by type
    public string[] ActionList1, ActionList2, ActionList3, ActionList4, ActionList5;           //List of all card names by type
    private int count_RandomNumber; //Random number for the next card

    private void Start()
    {
        //currentPlayerID = 1; //MasterClient
        view = GetComponent<PhotonView>();
        SpawnCards = GameObject.FindWithTag("Respawn");
        //myPlayerID = view.OwnerActorNr;

        waitRPC = false;
        isZoomed = false;
        Sold.text = "";
        SetCountValuesList();
        
    }

    private void SetCountValuesList()
    {
        int[] ammoList = new int[3]; // Ammo10, -20 and -30 cards
        for (int i = 0; i < 3; i++)
        {
            string ammo = AmmoCountListPrefab.GetComponent<TextFileToList>().GetStringFromTextByNumber(i);
            ammoList[i] = int.Parse(ammo);
            count_Values.Add(ammoList[i]);
            Debug.Log("ammoList count" + i);
        }

        activeCardObjectList.Add(Ammo10); // 0 //GameObject numbered in a list (used in PUN_BuyCard(...) "[PunRPC] function")
        activeCardObjectList.Add(Ammo20); // 1
        activeCardObjectList.Add(Ammo30); // 2

        HandgunList = AddAllCardsToList(Handgun, HandgunListPrefab);    //3
        KnifeList = AddAllCardsToList(Knife, KnifeListPrefab);          //4
        GrenadeList = AddAllCardsToList(Grenade, GrenadeListPrefab);    //5
        HPList = AddAllCardsToList(HP, HPListPrefab);                   //6
        ShotgunList = AddAllCardsToList(Shotgun, ShotgunListPrefab);    //7
        AR_SG_List = AddAllCardsToList(AR_SG, AR_SG_ListPrefab);        //8
        RifleList = AddAllCardsToList(Rifle, RiflesListPrefab);         //9
        ActionList1 = AddAllCardsToList(Action1, Action1ListPrefab);    //10
        ActionList2 = AddAllCardsToList(Action2, Action2ListPrefab);    //11
        ActionList3 = AddAllCardsToList(Action3, Action3ListPrefab);    //12
        ActionList4 = AddAllCardsToList(Action4, Action4ListPrefab);    //13
        ActionList5 = AddAllCardsToList(Action5, Action5ListPrefab);    //14

    }

    private string[] AddAllCardsToList(GameObject buttonObject, GameObject allCountedPrefab) //Takes all the .text files and sends them to objects SpriteFromAtlas.cs
    {                                                                                        //Returns finished string value
        int listCount = allCountedPrefab.GetComponent<TextFileToList>().textListCount;

        string[] cardList = new string[listCount];
        for (int i = 0; i < listCount; i++)
        {
            string card = allCountedPrefab.GetComponent<TextFileToList>().GetStringFromTextByNumber(i);
            cardList[i] = card;
        }
        count_Values.Add(listCount);
        buttonObject.GetComponent<SpriteFromAtlas>().ChangeCardSprite(cardList[0]);

        activeCardObjectList.Add(buttonObject); //List of gameobjects in order for PunRPC (same order as count_Values list)

        return cardList;
    }

    ///////////////////////////////////////
    public void OnClickAmmo10() //Stretch button image bigger and show "Buy" button
    {
        if (!waitRPC && view.IsMine)
        {
            SetAllCardsToNormalSize();
            CheckShopCard(Ammo10, count_Values[0]); //0 = Ammo10;
        }
    }
    public void OnClickBuyAmmo10() //"Buy" button which sends purchased card to local player's discard pile and deletes it in shop
    {
        if (!waitRPC && view.IsMine)
            BuyShopCard(Ammo10, count_Values[0], "ia_ammo10",0); //0 = Ammo10;
    }
    
    public void OnClickAmmo20() 
    {
        if (!waitRPC && view.IsMine)
        {
            SetAllCardsToNormalSize();
            CheckShopCard(Ammo20, count_Values[1]); //1 = Ammo20;
        }
    }
    public void OnClickBuyAmmo20() 
    {
        if (!waitRPC && view.IsMine)
            BuyShopCard(Ammo20, count_Values[1], "ia_ammo20",1); //1 = Ammo20;

    }

    public void OnClickAmmo30() 
    {
        if (!waitRPC && view.IsMine)
        {
            SetAllCardsToNormalSize();
            CheckShopCard(Ammo30, count_Values[2]); //2 = Ammo30;
        }

    }
    public void OnClickBuyAmmo30() 
    {
        if (!waitRPC && view.IsMine)
            BuyShopCard(Ammo30, count_Values[2], "ia_ammo30", 2); //2 = Ammo30;

    }
    ///////////////////////////////////////
    public void OnClickHandgun()
    {
        if (!waitRPC && view.IsMine)
        {
            SetAllCardsToNormalSize();
            CheckShopCard(Handgun, count_Values[3]); //3 = Handguns;
        }
    }
    public void OnClickBuyHandgun()
    {
        if (!waitRPC && view.IsMine)
        {
            string name = Handgun.GetComponent<SpriteFromAtlas>().spriteName;
            BuyShopCard(Handgun, count_Values[3], name, 3); //3 = Handguns;
        }
    }

    public void OnClickKnife()
    {
        if (!waitRPC && view.IsMine)
        {
            SetAllCardsToNormalSize();
            CheckShopCard(Knife, count_Values[4]); //4 = Knifes;
        }
    }
    public void OnClickBuyKnife()
    {
        if (!waitRPC && view.IsMine)
        {
            string name = Knife.GetComponent<SpriteFromAtlas>().spriteName;
            BuyShopCard(Knife, count_Values[4], name, 4); //4 = Knifes;
        }
    }

    public void OnClickGrenade()
    {
        if (!waitRPC && view.IsMine)
        {
            SetAllCardsToNormalSize();
            CheckShopCard(Grenade, count_Values[5]); //5 = Grenades;
        }
    }
    public void OnClickBuyGrenade()
    {
        if (!waitRPC && view.IsMine)
        {
            string name = Grenade.GetComponent<SpriteFromAtlas>().spriteName;
            BuyShopCard(Grenade, count_Values[5], name, 5); //5 = Grenades;
        }
    }

    public void OnClickHPHerbs()
    {
        if (!waitRPC && view.IsMine)
        {
            SetAllCardsToNormalSize();
            CheckShopCard(HP, count_Values[6]); //6 = HP (herbs and first aid);
        }
    }
    public void OnClickBuyHPHerbs()
    {
        if (!waitRPC && view.IsMine)
        {
            string name = HP.GetComponent<SpriteFromAtlas>().spriteName;
            BuyShopCard(HP, count_Values[6], name, 6); //6 = HP (herbs and first aid);
        }
    }

    ///////////////////////////////////////

    public void OnClickShotgun()
    {
        if (!waitRPC && view.IsMine)
        {
            SetAllCardsToNormalSize();
            CheckShopCard(Shotgun, count_Values[7]); //7 = Shotguns;
        }
    }
    public void OnClickBuyShotgun()
    {
        if (!waitRPC && view.IsMine)
        {
            string name = Shotgun.GetComponent<SpriteFromAtlas>().spriteName;
            BuyShopCard(Shotgun, count_Values[7], name, 7); //7 = Shotguns;
        }
    }

    public void OnClickAR_SG()
    {
        if (!waitRPC && view.IsMine)
        {
            SetAllCardsToNormalSize();
            CheckShopCard(AR_SG, count_Values[8]); //8 = Assault rifles and Submachine guns;
        }
    }
    public void OnClickBuyAR_SG()
    {
        if (!waitRPC && view.IsMine)
        {
            string name = AR_SG.GetComponent<SpriteFromAtlas>().spriteName;
            BuyShopCard(AR_SG, count_Values[8], name, 8); //8 = Assault rifles and Submachine guns;
        }
    }

    public void OnClickRifle()
    {
        if (!waitRPC && view.IsMine)
        {
            SetAllCardsToNormalSize();
            CheckShopCard(Rifle, count_Values[9]); //9 = Rifles;
        }
    }
    public void OnClickBuyRifle()
    {
        if (!waitRPC && view.IsMine)
        {
            string name = Rifle.GetComponent<SpriteFromAtlas>().spriteName;
            BuyShopCard(Rifle, count_Values[9], name, 9); //9 = Rifles;
        }
    }

    ///////////////////////////////////////
    
    public void OnClickAction1()
    {
        if (!waitRPC && view.IsMine)
        {
            SetAllCardsToNormalSize();
            CheckShopCard(Action1, count_Values[10]); //10 = Action1;
        }
    }
    public void OnClickBuyAction1()
    {
        if (!waitRPC && view.IsMine)
        {
            string name = Action1.GetComponent<SpriteFromAtlas>().spriteName;
            BuyShopCard(Action1, count_Values[10], name, 10); //10 = Action1;
        }
    }

    public void OnClickAction2()
    {
        if (!waitRPC && view.IsMine)
        {
            SetAllCardsToNormalSize();
            CheckShopCard(Action2, count_Values[11]); //11 = Action2;
        }
    }
    public void OnClickBuyAction2()
    {
        if (!waitRPC && view.IsMine)
        {
            string name = Action2.GetComponent<SpriteFromAtlas>().spriteName;
            BuyShopCard(Action2, count_Values[11], name, 11); //11 = Action2;
        }
    }
    
    public void OnClickAction3()
    {
        if (!waitRPC && view.IsMine)
        {
            SetAllCardsToNormalSize();
            CheckShopCard(Action3, count_Values[12]); //12 = Action3;
        }
    }
    public void OnClickBuyAction3()
    {
        if (!waitRPC && view.IsMine)
        {
            string name = Action3.GetComponent<SpriteFromAtlas>().spriteName;
            BuyShopCard(Action3, count_Values[12], name, 12); //12 = Action3;
        }
    }
    
    public void OnClickAction4()
    {
        if (!waitRPC && view.IsMine)
        {
            SetAllCardsToNormalSize();
            CheckShopCard(Action4, count_Values[13]); //13 = Action4;
        }
    }
    public void OnClickBuyAction4()
    {
        if (!waitRPC && view.IsMine)
        {
            string name = Action4.GetComponent<SpriteFromAtlas>().spriteName;
            BuyShopCard(Action4, count_Values[13], name, 13); //13 = Action4;
        }
    }
    
    public void OnClickAction5()
    {
        if (!waitRPC && view.IsMine)
        {
            SetAllCardsToNormalSize();
            CheckShopCard(Action5, count_Values[14]); //14 = Action5;
        }
    }
    public void OnClickBuyAction5()
    {
        if (!waitRPC && view.IsMine)
        {
            string name = Action5.GetComponent<SpriteFromAtlas>().spriteName;
            BuyShopCard(Action5, count_Values[14], name, 14); //14 = Action5;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void CheckShopCard(GameObject cardObj, int cardCount)
    {
                if (isZoomed)
                {
                    cardObj.transform.localScale = vec_Normal;
                    isZoomed = false;
                    cardObj.transform.GetChild(0).gameObject.SetActive(false);
                }
                else
                {
                    cardObj.transform.SetAsLastSibling();
                    cardObj.transform.localScale = vec_Zoom;
                    isZoomed = true;
                    cardObj.transform.GetChild(0).gameObject.SetActive(true);
                    cardObj.transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = cardCount.ToString() + " pcs";
                }


    }
    private void BuyShopCard(GameObject cardObj, int cardCount, string cardName, int count_Value) //cardName MUST be original card sprite name!
    {                                                                                             //count_Value=card number in list (GameObject)
            if (cardCount > 1)
            {
                SpawnCards.GetComponent<SpawnCards>().SHOP_AddCardToDiscardPile(cardName);
                cardCount--;
                cardObj.transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = cardCount.ToString()+" pcs";
 
                count_RandomNumber = 0;
            if (count_Value > 2) //Not ammo card
            {
                count_RandomNumber = Random.Range(0, cardCount);
            }
                view.RPC("PUN_BuyCard", RpcTarget.AllBuffered, cardCount, count_Value, count_RandomNumber);
            }
            else
            {
                cardObj.SetActive(false);
            }
    }
    [PunRPC]
    public void PUN_BuyCard(int cardCount,int count_Value, int randomNumber)
    {
        currentCardObject = count_Value;
        count_Values[currentCardObject] = cardCount;
        Sold.transform.position = activeCardObjectList[currentCardObject].transform.position;
        Sold.text = "Card bought!"+"\n"+" ("+cardCount.ToString()+" pcs left)";

        //if (count_Value != 0) //Not ammo card
        {
            count_RandomNumber = randomNumber;

            switch (count_Value)
            {
                case 0: //Ammo10
                    holdNameRPC = "ia_ammo10";
                    break;
                case 1: ////Ammo20
                    holdNameRPC = "ia_ammo20";
                    break;
                case 2: ////Ammo30
                    holdNameRPC = "ia_ammo30";
                    break;
                case 3: //Handguns
                    holdNameRPC = HandgunList[count_RandomNumber];
                    break;
                case 4: //Knives
                    holdNameRPC = KnifeList[count_RandomNumber];
                    break;
                case 5: //Grenades
                    holdNameRPC = GrenadeList[count_RandomNumber];
                    break;
                case 6: //HP
                    holdNameRPC = HPList[count_RandomNumber];
                    break;
                case 7: //Shotguns
                    holdNameRPC = ShotgunList[count_RandomNumber];
                    break;
                case 8: //Assault rifles and submachine guns
                    holdNameRPC = AR_SG_List[count_RandomNumber];
                    break;
                case 9: //Rifles
                    holdNameRPC = RifleList[count_RandomNumber];
                    break;
                case 10: //Action cards1
                    holdNameRPC = ActionList1[count_RandomNumber];
                    break;
                case 11: //Action cards2
                    holdNameRPC = ActionList2[count_RandomNumber];
                    break;
                case 12: //Action cards3
                    holdNameRPC = ActionList3[count_RandomNumber];
                    break;
                case 13: //Action cards4
                    holdNameRPC = ActionList4[count_RandomNumber];
                    break;
                case 14: //Action cards5
                    holdNameRPC = ActionList5[count_RandomNumber];
                    break;
            }
        }


        StartCoroutine(WaitSoldText());
    }
    private IEnumerator WaitSoldText()
    {
        waitRPC = true;
        yield return new WaitForSeconds(2f);
        if (holdNameRPC != "")
        {
            activeCardObjectList[currentCardObject].GetComponent<SpriteFromAtlas>().ChangeCardSprite(holdNameRPC);
        }
        isZoomed = false;
        activeCardObjectList[currentCardObject].transform.localScale = vec_Normal;
        activeCardObjectList[currentCardObject].transform.GetChild(0).gameObject.SetActive(false);
        Sold.text = "";
        waitRPC = false;
    }

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
    }
}
