using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// ShopBaseControl.cs abstract class must be inherited!
// All the lists are located in ShopBaseControl

public class ShopControl : ShopBaseControl   //
{
    private GameObject SpawnCards;
    public TMP_Text Sold, BuysCounttxt, BuysCount2txt;
    public GameObject LeftMenuControl;
    public GameObject ShopScrollBar, Shop_Items;
    private float scrollBarValue;
    [SerializeField] private List<GameObject> allButtonsList; //List of all objects. Can be accessed by list number! (used in [PunRPC])
    protected List<string> hostNameHolder; // Saves the card names for the first load -> is send to clients afterwards
    //PhotonView view;
    public int buysCount; //Reset to 0 from "GameControl.cs" when player turn changes
    private bool isZoomed, isMaster, isRandomCardActive; //Tells if current card is in bigger- or normal size

    private int boughtCardListIndex;
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
        BuysCount2txt.text = "Buys (B) = " + buysCount;

        SetCountValuesList();


        if (isMaster)
        {
            StartCoroutine(SetAllShopCardSprites(hostNameHolder.ToArray()));
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
            hostNameHolder = new List<string>();
            hostNameHolder.Add("Ammo10");
            hostNameHolder.Add("Ammo20");
            hostNameHolder.Add("Ammo30");
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
            hostNameHolder.Add(cardList[rand]);
        }
        else if (isMaster && listCount == 0)
            hostNameHolder.Add("");
        //buttonObject.GetComponent<SpriteFromAtlas>().ChangeCardSprite(cardList[0]); //Set first card in list to SPRITE
        allButtonsList.Add(buttonObject); //List of gameobjects in order for PunRPC (same order as count_Values list)
        return cardList;
    }

    IEnumerator SetAllShopCardSprites(string[] array)
    {
        yield return new WaitForSeconds(2f);
        view.RPC("Pun_SetAllShopCardSprites", RpcTarget.AllBuffered, (object)hostNameHolder.ToArray());
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
            BuysCount2txt.text = "Buys (B) = " + buysCount;

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
    protected override void CheckShopCard(GameObject cardObj, int cardCount)
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
    protected override void BuyShopCard(GameObject cardObj, int cardCount, string cardName, int listIndex) //cardName MUST be original card sprite name!
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

    protected override void Add_LM_HandDeck(Sprite sprt) //LM = Left menu 
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
