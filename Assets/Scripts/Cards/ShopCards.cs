using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopCards : MonoBehaviourPun   //
{
    private GameObject SpawnCards;
    public TMP_Text Sold, BuysCounttxt;
    public GameObject Ammo10,Ammo20,Ammo30, Handgun,Knife,Grenade,HP;   //All buttons in ShopMenu
    public GameObject Shotgun, AR_SG, Rifle;                           //All buttons in ShopMenu
    public GameObject Action1, Action2, Action3, Action4, Action5, Action6, Action7;    //All buttons in ShopMenu
    public GameObject Extra1, Extra2;
    public GameObject AmmoCountListPrefab, HandgunListPrefab, KnifeListPrefab,GrenadeListPrefab, HPListPrefab;   //ShopLists_AllCounted (.text)
    public GameObject ShotgunListPrefab, AR_SG_ListPrefab, RiflesListPrefab;                                    //ShopLists_AllCounted (.text)
    public GameObject Action1ListPrefab, Action2ListPrefab, Action3ListPrefab, Action4ListPrefab, Action5ListPrefab, Action6ListPrefab, Action7ListPrefab;
    public GameObject Extra1ListPrefab, Extra2ListPrefab;
    public GameObject LeftMenuControl;
    public GameObject ShopScrollBar, Shop_Items;
    private float scrollBarValue;
    [SerializeField] private List<GameObject> allButtonsList; //List of all objects. Can be accessed by list number! (used in [PunRPC])  
    PhotonView view;
    public int buysCount; //Reset to 0 from "GameControl.cs" when player turn changes
    private bool isZoomed, isMaster, isRandomCardActive; //Tells if current card is in bigger- or normal size
    Vector3 vec_Normal = new Vector3(1, 1, 1);
    Vector3 vec_Zoom = new Vector3(1.5f, 1.5f, 1); //OLD = 1.8f
    [SerializeField] private bool waitRPC;  //Shows if "wait time" is active while sending data in network
    //private string holdNameRPC; //Holds the name of the next card (accessed in coroutine)
    //public int myPlayerID,currentPlayerID;
    [SerializeField] int[] ammoCounts; //Max ammount of cards/type -> works in harmony with "activeCardObjectList" (same numbers)
    private int boughtCardListIndex;
    [SerializeField] List<string> HandgunList,KnifeList,GrenadeList,HPList, ShotgunList,AR_SG_List,RifleList; //List of all card names by type
    [SerializeField] List<string> ActionList1, ActionList2, ActionList3, ActionList4, ActionList5, ActionList6, ActionList7;           //List of all card names by type
    [SerializeField] List<string> ExtraList1, ExtraList2;
    [SerializeField] List<string> firstLoadShopCardNames;
    private int count_RandomNumber; //Random number for the next card

    private void Start()
    {
        isMaster = PhotonNetwork.IsMasterClient;
        view = GetComponent<PhotonView>();
        SpawnCards = GameObject.FindWithTag("Respawn");
        waitRPC = false;
        isZoomed = false;
        isRandomCardActive = true;
        Sold.text = "";
        buysCount = 0;
        BuysCounttxt.text = "Buys (B) = "+buysCount;

        SetCountValuesList();


        if (isMaster)
        {
            StartCoroutine(SetAllShopCardSprites(firstLoadShopCardNames.ToArray()));
        }


    }

    private void SetCountValuesList()
    {
        ammoCounts = new int[3]; // Ammo10, -20 and -30 cards
        for (int i = 0; i < 3; i++)
        {
            string ammo = AmmoCountListPrefab.GetComponent<TextFileToList>().GetStringFromTextByNumber(i);
            ammoCounts[i] = int.Parse(ammo);
        }

        if(isMaster)
        {
            firstLoadShopCardNames = new List<string>();
            firstLoadShopCardNames.Add("Ammo10");
            firstLoadShopCardNames.Add("Ammo20");
            firstLoadShopCardNames.Add("Ammo30");
        }

                                    
        allButtonsList.Add(Ammo10); // 0 //GameObject numbered in a list (used in PUN_BuyCard(...))
        allButtonsList.Add(Ammo20); // 1
        allButtonsList.Add(Ammo30); // 2
        HandgunList = CreateCardList(Handgun, HandgunListPrefab);    //3
        KnifeList = CreateCardList(Knife, KnifeListPrefab);          //4
        GrenadeList = CreateCardList(Grenade, GrenadeListPrefab);    //5
        HPList = CreateCardList(HP, HPListPrefab);                   //6
        ShotgunList = CreateCardList(Shotgun, ShotgunListPrefab);    //7
        AR_SG_List = CreateCardList(AR_SG, AR_SG_ListPrefab);        //8
        RifleList = CreateCardList(Rifle, RiflesListPrefab);         //9
        ActionList1 = CreateCardList(Action1, Action1ListPrefab);    //10
        ActionList2 = CreateCardList(Action2, Action2ListPrefab);    //11
        ActionList3 = CreateCardList(Action3, Action3ListPrefab);    //12
        ActionList4 = CreateCardList(Action4, Action4ListPrefab);    //13
        ActionList5 = CreateCardList(Action5, Action5ListPrefab);    //14
        ActionList6 = CreateCardList(Action6, Action6ListPrefab);    //15
        ActionList7 = CreateCardList(Action7, Action7ListPrefab);    //16
        ExtraList1 = CreateCardList(Extra1, Extra1ListPrefab);     //17
        ExtraList2 = CreateCardList(Extra2, Extra2ListPrefab);     //18

    }
    /// When adding/deleting new SHOP_buttons remember to add changes in text file arrays! (FirstBootTextFile.cs and OverwriteTextFileList.cs)
    /// Before game start remember to delete everything in "Game_data" -folder" (all txt files are overwritten if "CharactersList_AllCounted" is missing)
    /// </summary>
    /// <returns></returns>
    /// 

    private List<string> CreateCardList(GameObject buttonObject, GameObject allCountedPrefab) //Takes all the .text files and sends them to objects SpriteFromAtlas.cs
    {                                                                                        //Returns finished string value
        int listCount = allCountedPrefab.GetComponent<TextFileToList>().GetTextListCount();

        List<string> cardList = new();
        for (int i = 0; i < listCount; i++)
        {
            string card = allCountedPrefab.GetComponent<TextFileToList>().GetStringFromTextByNumber(i);
            //cardList[i] = card;
            cardList.Add(card);
        }
        if(isMaster && listCount>0)
        {
            int rand = Random.Range(0, cardList.Count);
            firstLoadShopCardNames.Add(cardList[rand]);
        }
        else if (listCount==0)
            firstLoadShopCardNames.Add("");
        //buttonObject.GetComponent<SpriteFromAtlas>().ChangeCardSprite(cardList[0]); //Set first card in list to SPRITE
        allButtonsList.Add(buttonObject); //List of gameobjects in order for PunRPC (same order as count_Values list)
        return cardList;
    }

    IEnumerator SetAllShopCardSprites(string[] array)
    {
        yield return new WaitForSeconds(2f);
        view.RPC("Pun_SetAllShopCardSprites", RpcTarget.AllBuffered, (object)firstLoadShopCardNames.ToArray());
    }

    [PunRPC] void Pun_SetAllShopCardSprites(string[] array)
    {
        int index = 0;

        foreach (GameObject o in allButtonsList)
        {
            if (index > 2 && array[index] != "") //Skip Ammo Cards
                o.GetComponent<SpriteFromAtlas>().ChangeCardSprite(array[index]); //Set first card in list to SPRITE
            else if (array[index] == "")
            {
                if (index == array.Length-1) //If card is last one -> Random card! (extra2)
                    isRandomCardActive = false;
                o.SetActive(false);
            }
            index++;
        }

    }
    ///////////////////////////////////////
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

    public void UpdateAndResetBuysCount(bool reset) //Buys counter on the top left corner in SHOP
    {
        if (reset)
        {
            buysCount = 0;
            //EXTRA2 RANDOM CARD ->
            if (view.IsMine && isRandomCardActive)
            {
                StartCoroutine(ExtraRandomCard());
            }
        }
        else
            buysCount++;

            BuysCounttxt.text = "Buys (B) = " + buysCount;

    }
    IEnumerator ExtraRandomCard()
    {
        int count = ExtraList2.Count;
        int newValue = Random.Range(0, count);
        yield return new WaitForSeconds(1f);
        view.RPC("SetNewExtra2RandomCard", RpcTarget.AllBuffered, ExtraList2[newValue]);
    }
    [PunRPC]//EXTRA2 RANDOM CARD ->
    private void SetNewExtra2RandomCard(string name)
    {
        Extra2.GetComponent<SpriteFromAtlas>().ChangeCardSprite(name);

    }



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
                    //cardObj.transform.SetAsLastSibling(); // Change object order to first
                    cardObj.transform.localScale = vec_Zoom;
                    isZoomed = true;
                    cardObj.transform.GetChild(0).gameObject.SetActive(true);
                    cardObj.transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = cardCount.ToString() + " pcs";
                }


    }
    private void BuyShopCard(GameObject cardObj, int cardCount, string cardName, int listIndex) //cardName MUST be original card sprite name!
    {                                                                                             //count_Value=card number in list (GameObject)
            if (cardCount > 1)
            {
                SpawnCards.GetComponent<SpawnCards>().SHOP_AddCardToDiscardPile(cardName);
                cardCount--;
                //cardObj.transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = cardCount-1+" pcs";
                
                int random = 0;
                if (listIndex > 2) //Not ammo card
                {
                    random = Random.Range(0, cardCount);
                }
                view.RPC("PUN_BuyCard", RpcTarget.AllBuffered, cardName, listIndex, random);
            }       
            else //Delete card
            {
            view.RPC("PUN_LastCard", RpcTarget.AllBuffered, listIndex);
            //cardObj.SetActive(false);
            }
    }


    private void Add_LM_HandDeck(Sprite sprt) //LM = Left menu 
    {
        LeftMenuControl.GetComponent<LeftMenuControl>().InstantiateNewHandCard(sprt);
    }



    [PunRPC]
    public void PUN_BuyCard(string cardName, int listIndex, int randomNumber)
    {
        boughtCardListIndex = listIndex;
        //count_Values[boughtCardListIndex] = cardCount;
        int count = 0;
        UpdateAndResetBuysCount(false); //Update don't reset
        string newCardName = "";

        count_RandomNumber = randomNumber;

            switch (listIndex)
            {
            case 0: //Ammo10
                newCardName = "ia_ammo10";
                ammoCounts[0]--;
                count = ammoCounts[0];
                break;
            case 1: ////Ammo20
                newCardName = "ia_ammo20";
                ammoCounts[1]--;
                count = ammoCounts[1];
                break;
            case 2: ////Ammo30
                newCardName = "ia_ammo30";
                ammoCounts[2]--;
                count = ammoCounts[2];
                break;
            case 3: //Handguns
                HandgunList.Remove(cardName);
                count = HandgunList.Count;
                newCardName = HandgunList[count_RandomNumber];
                break;
           case 4: //Knives
                KnifeList.Remove(cardName);
                count = KnifeList.Count;
                newCardName = KnifeList[count_RandomNumber];
                break;
            case 5: //Grenades
                GrenadeList.Remove(cardName);
                count = GrenadeList.Count;
                newCardName = GrenadeList[count_RandomNumber];
                break;
            case 6: //HP
                HPList.Remove(cardName);
                count = HPList.Count;
                newCardName = HPList[count_RandomNumber];
                break;
            case 7: //Shotguns
                ShotgunList.Remove(cardName);
                count = ShotgunList.Count;
                newCardName = ShotgunList[count_RandomNumber];
                break;
            case 8: //Assault rifles and submachine guns
                AR_SG_List.Remove(cardName);
                count = AR_SG_List.Count;
                newCardName = AR_SG_List[count_RandomNumber];
                break;
            case 9: //Rifles
                RifleList.Remove(cardName);
                count = RifleList.Count;
                newCardName = RifleList[count_RandomNumber];
                break;
            case 10: //Action cards1
                ActionList1.Remove(cardName);
                count = ActionList1.Count;
                newCardName = ActionList1[count_RandomNumber];
                break;
            case 11: //Action cards2
                ActionList2.Remove(cardName);
                count = ActionList2.Count;
                newCardName = ActionList2[count_RandomNumber];
                break;
                case 12: //Action cards3
                ActionList3.Remove(cardName);
                count = ActionList3.Count;
                newCardName = ActionList3[count_RandomNumber];
                break;
            case 13: //Action cards4
                ActionList4.Remove(cardName);
                count = ActionList4.Count;
                newCardName = ActionList4[count_RandomNumber];
                break;
            case 14: //Action cards5
                ActionList5.Remove(cardName);
                count = ActionList5.Count;
                newCardName = ActionList5[count_RandomNumber];
                break;
            case 15: //Action cards6
                ActionList6.Remove(cardName);
                count = ActionList6.Count;
                newCardName = ActionList6[count_RandomNumber];
                break;
            case 16: //Action cards7
                ActionList7.Remove(cardName);
                count = ActionList7.Count;
                newCardName = ActionList7[count_RandomNumber];
                break;
            case 17: //Extra1 cards
                ExtraList1.Remove(cardName);
                count = ExtraList1.Count;
                newCardName = ExtraList1[count_RandomNumber];
                break;
            case 18: //Extra2 cards
                ExtraList2.Remove(cardName);
                count = ExtraList2.Count;
                newCardName = ExtraList2[count_RandomNumber];
                break;
            }


        Sold.transform.position = allButtonsList[boughtCardListIndex].transform.position;
        Sold.text = "Card bought!" + "\n" + " (" + count.ToString() + " pcs left)";
        StartCoroutine(WaitSoldText(newCardName));
    }

    [PunRPC]
    void PUN_LastCard(int listIndex)
    {
        GameObject hideThis = allButtonsList[listIndex];

        Sold.transform.position = hideThis.transform.position;
        Sold.text = "Card bought!" + "\n" + " (Deck is now empty!)";
        StartCoroutine(HideButtonAndWait(hideThis));

        
    }
    private IEnumerator WaitSoldText(string cardName)
    {
        waitRPC = true;

        ScrollRect rect = Shop_Items.GetComponent<ScrollRect>();
        rect = Shop_Items.GetComponent<ScrollRect>();
        rect.StopMovement();
        rect.enabled = false;

        yield return new WaitForSeconds(2f);
        if (cardName != "")
        {
            allButtonsList[boughtCardListIndex].GetComponent<SpriteFromAtlas>().ChangeCardSprite(cardName);
        }
        else
        {

        }

        isZoomed = false;
        allButtonsList[boughtCardListIndex].transform.localScale = vec_Normal;
        allButtonsList[boughtCardListIndex].transform.GetChild(0).gameObject.SetActive(false);
        Sold.text = "";
        waitRPC = false;
        rect.enabled = true;
    }

    IEnumerator HideButtonAndWait(GameObject buttonObject)
    {
        
            waitRPC = true;

            ScrollRect rect = Shop_Items.GetComponent<ScrollRect>();
            rect = Shop_Items.GetComponent<ScrollRect>();
            rect.StopMovement();
            rect.enabled = false;

            yield return new WaitForSeconds(2f);

            isZoomed = false;
            allButtonsList[boughtCardListIndex].transform.localScale = vec_Normal;
            allButtonsList[boughtCardListIndex].transform.GetChild(0).gameObject.SetActive(false);
            Sold.text = "";
            waitRPC = false;
            rect.enabled = true;
            buttonObject.SetActive(false);
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
        Action6.transform.localScale = vec_Normal;
        Action6.transform.GetChild(0).gameObject.SetActive(false);
        Action7.transform.localScale = vec_Normal;
        Action7.transform.GetChild(0).gameObject.SetActive(false);
        Extra1.transform.localScale = vec_Normal;
        Extra1.transform.GetChild(0).gameObject.SetActive(false);
        Extra2.transform.localScale = vec_Normal;
        Extra2.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void OnScrollValueChanged()
    {
        if (!waitRPC)
        {
            if (view.IsMine)
            {
                scrollBarValue = ShopScrollBar.GetComponent<Scrollbar>().value;

                view.RPC("ScrollValueChanged", RpcTarget.AllBuffered, scrollBarValue);
            }
        }


    }
    [PunRPC] private void ScrollValueChanged(float value)
    {
        ShopScrollBar.GetComponent<Scrollbar>().value = value;

    }
}
