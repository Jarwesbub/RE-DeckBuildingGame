using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MansionControl : MonoBehaviourPun
{
    public GameObject MansionCard, MansionDoor, MansionCardTextFile, MansionHandCardPrefab;
    public GameObject LeftMenuControl, MansionDoor_btnUI, BossEncounterObj;
    public GameObject MansionActionButtons, OtherActionButtons, HandCardStats;
    public TMP_Text mansionActionTMP, mansionDeckCountTMP, exploreCountTMP, bossCountTMP;
    public int MansionDeckCount, MansionExploreCount;
    [SerializeField] private string enemyName, currentPlayerName;
    [SerializeField] private List<string> mansionDeck; //All the mansion cards are here (works in a harmony with mansionDeckArrayHolder)
    private string[] mansionDeckArrayHolder; //Holds mansionDeck cards and helps as a "randomizer"
    private string currentMansionCard;
    private string mansionTxt;
    private bool activeOtherActionBtn, doorKnobLock, isBottomCard, isBossEncounter, isButtonLock;
    private int mansionBossCount;
    [SerializeField] private Color inactiveColor; //Color indication for mansion card when player has won/lost etc.
    private Image image;
    private PhotonView view;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        image = MansionCard.GetComponent<Image>();
    }
    private void Start()
    {
        mansionTxt = "";
        mansionActionTMP.text = mansionTxt;
        MansionExploreCount = 0;
        exploreCountTMP.text = "Explores this turn: " + 0;
        mansionBossCount = 0; isBossEncounter = false;
        BossEncounterObj.GetComponent<MansionBossEncounter>().SetBossCounterActive(false);
        MansionDoor.SetActive(true);
        MansionDoor_btnUI.SetActive(false);
        activeOtherActionBtn = false;
        MansionActionButtons.SetActive(false);
        OtherActionButtons.SetActive(false);

        currentPlayerName = PhotonNetwork.MasterClient.NickName;
        ResetMansionAnimation();

        if (PhotonNetwork.IsMasterClient)
        {
            mansionDeckArrayHolder = SetMansionCardsFromTextList();
            ShuffleMansionDeck();
            StartCoroutine(AddDelay_SendMansionDeckToClients());
        }
        isButtonLock = false;
    }

    private string[] SetMansionCardsFromTextList()
    {
        int listCount = MansionCardTextFile.GetComponent<TextFileToList>().RemoveFirstLineFromList(); //Removes Mansion deck's name (returns list count)

        string[] cardList = new string[listCount];
        for (int i = 0; i < listCount; i++)
        {
            string card = MansionCardTextFile.GetComponent<TextFileToList>().GetStringFromTextByNumber(i);
            cardList[i] = card;
        }

        return cardList;
    }
    IEnumerator AddDelay_SendMansionDeckToClients() // Gives clients more time to do actions - 500 action limit/second ! -
    {
        GetComponent<GameControl>().onButtonLock = true;
        yield return new WaitForSeconds(1f);
        view.RPC("RPC_SendMansionDeck", RpcTarget.AllBuffered, (object)mansionDeckArrayHolder);
        yield return new WaitForSeconds(1f);
        GetComponent<GameControl>().onButtonLock = false;
    }
    [PunRPC] public void RPC_SendMansionDeck(string[] deck)
    {
        foreach(string m in deck)
        {
            mansionDeck.Remove(m);
        }
        mansionDeckArrayHolder = deck;
        mansionBossCount = 0;

        foreach (string s in deck)
        {
            mansionDeck.Add(s);

            //Check if card is BOSS -card
            if (s == "2_ma-010_premier_uroboros_aheri1" || s == "2_ma-029_alliance_albert_wesker1" ||
                s == "2_ma-041_outbreak_tyrant_t-002_infection_mod" || s == "2_ma-054_nightmare_osmund_saddler")
                mansionBossCount++;
        }
        MansionDeckCount = mansionDeck.Count;
        mansionDeckCountTMP.text = "Mansion card count: " + MansionDeckCount;
        UpdateMansionBossCount();
    }
    private void UpdateMansionBossCount()
    {
        bossCountTMP.text = "Boss count: " + mansionBossCount;
    }
    private void MansionBossIsBeaten(bool isBeaten)
    {
        if (isBeaten)
        {
            mansionBossCount--;
            UpdateMansionBossCount();

            if (mansionBossCount > 0)
                BossEncounterObj.GetComponent<MansionBossEncounter>().SetBossCounterActive(false);
            else
                BossEncounterObj.GetComponent<MansionBossEncounter>().AllBossesDefeated();
        }
        else
        {
            BossEncounterObj.GetComponent<MansionBossEncounter>().SetBossCounterActive(false);
        }

        isBossEncounter = false;
    }
    private bool CheckIfMansionCardIsBoss(string s)
    {
        if (s == "2_ma-010_premier_uroboros_aheri1" || s == "2_ma-029_alliance_albert_wesker1" ||
        s == "2_ma-041_outbreak_tyrant_t-002_infection_mod" || s == "2_ma-054_nightmare_osmund_saddler")
        {
            //Debug.Log("IsBossCard = "+s);
            return true;
        }
        else
            return false;

    }
    private void ShuffleMansionDeck() //
    {
        int count = mansionDeckArrayHolder.Length;
        for (int i = 0; i < count; i++)
        {
            string temp = mansionDeckArrayHolder[i];
            int randomIndex = Random.Range(i, mansionDeckArrayHolder.Length);
            mansionDeckArrayHolder[i] = mansionDeckArrayHolder[randomIndex];
            mansionDeckArrayHolder[randomIndex] = temp;
        }
    }
    [PunRPC] public void RPC_ShuffleMansionDeck(string[] deck) //Shuffle
    {
        foreach (string m in deck)
        {
            mansionDeck.Remove(m);
        }
        mansionDeckArrayHolder = deck;

        foreach (string s in deck)
        {
            mansionDeck.Add(s);
        }
        MansionDeckCount = mansionDeck.Count;
        mansionDeckCountTMP.text = "Mansion card count: " + MansionDeckCount;
        UpdateMansionBossCount();
    }

    public bool IsButtonLockActive()
    {
        return isButtonLock;
    }
    public void MansionDoorReset()
    {
        OnClickSetMansionTextEmpty();
        MansionActionButtons.SetActive(false);
        OtherActionButtons.SetActive(false);
        isBottomCard = false;
        doorKnobLock = false;
        isButtonLock = false;
    }

    public void DoorAnimationEnds()
    {
        isButtonLock = false;

        if (view.IsMine)
        {
            MansionActionButtons.SetActive(true);
            MansionExploreCount++;
            exploreCountTMP.text = "Explores this turn: " + MansionExploreCount;
        }
    }
    [PunRPC] void RPC_SetMansionAnimation()
    {
        MansionDoor.SetActive(true);

    }

    public void MansionSetForNextPlayer(string name) //GameControl.cs
    {
        currentPlayerName = name;
        MansionExploreCount = 0;
        exploreCountTMP.text = "Explores this turn: " + MansionExploreCount;
    }

    public void ResetMansionAnimation() //GameControl.cs
    {
        if (view.IsMine)
        {
            view.RPC("RPC_SetMansionAnimation", RpcTarget.AllBuffered);
        }
    }

    ////////////////////


    public void ClickEnterMansion(int clickValue) //ClickableObject.cs
    {
        if (view.IsMine && !doorKnobLock)
        {
            if (MansionDeckCount > 0)
            {
                isButtonLock = true;
                doorKnobLock = true;

                if (clickValue == 1) //Mouse1 button
                {
                    isBottomCard = false;
                    currentMansionCard = mansionDeck[0];
                    GetComponent<StatsMansionCards>().CheckMansionCardStats(view.OwnerActorNr, currentMansionCard);
                    bool isBossCard = CheckIfMansionCardIsBoss(currentMansionCard);
                    view.RPC("RPC_ClickEnterMansion", RpcTarget.AllBuffered, 0, isBossCard); //Normal

                }
                else if (clickValue == 2) //Mouse2 button
                {
                    isBottomCard = true;
                    MansionDoor_btnUI.SetActive(true);
                }
            }
            else
            {
                StartCoroutine(MansionDoorOpening("MANSION EMPTY!"));
            }

        }
    }
    public void OnClickChooseBottomCard(bool chooseBottomCard) //Unity UI
    {
        if (chooseBottomCard) //Choose bottom card
        {
            int value = MansionDeckCount-1;
            currentMansionCard = mansionDeck[value];
            GetComponent<StatsMansionCards>().CheckMansionCardStats(view.OwnerActorNr, currentMansionCard);
            bool isBossCard = CheckIfMansionCardIsBoss(currentMansionCard);
            view.RPC("RPC_ClickEnterMansion", RpcTarget.AllBuffered, value, isBossCard); 
        }
        MansionDoor_btnUI.SetActive(false);

    }
    [PunRPC] void RPC_ClickEnterMansion(int value, bool isBossCard)
    {
        if (value!=0)
            StartCoroutine(MansionDoorOpening("Bottom card"));

        MansionCard.GetComponent<SpriteFromAtlas>().SetMansionCardSprite(mansionDeck[value]);
        MansionDoor.GetComponent<MansionDoor>().OpenMansionDoor();

        isBossEncounter = isBossCard;
        BossEncounterObj.GetComponent<MansionBossEncounter>().SetBossCounterActive(isBossCard);
        
    }

    ///////////////////////////
    
    public void OnClickDeleteCard() //WIN button
    {
        if (view.IsMine)
        {
            /*GameObject mansionHandCard = Instantiate(MansionHandCardPrefab);
            mansionHandCard.GetComponent<Image>().sprite = image.sprite;
            mansionHandCard.transform.SetParent(MansionGridContent.transform);
            mansionHandCard.transform.localScale = new Vector3(1f, 1f, 1f);*/
            LeftMenuControl.GetComponent<LeftMenuControl>().AddNewMansionCard(image.sprite);
            view.RPC("RPC_SendCardToPlayer", RpcTarget.AllBuffered, isBottomCard);
            int points = CurrentMansionCard.GetPoints();
            view.RPC("RPC_SetMainTextPlayer", RpcTarget.AllBuffered, currentPlayerName, true, points, "");
            MansionActionButtons.SetActive(false); OtherActionButtons.SetActive(true);
            LeftMenuControl.GetComponent<LeftMenuControl>().SetPlus1Text();
            HandCardStats.GetComponent<HandCardStatsControl>().MansionSetPlayerWins(true);


        }
    }
    [PunRPC] public void RPC_SendCardToPlayer(bool chooseBottomCard)
    {
        int value = 0;
        if (chooseBottomCard)
            value = MansionDeckCount-1;

        if (isBossEncounter)
            MansionBossIsBeaten(true);

        //GetComponent<StatsControl>().UpdatePlayerPoints(id, cardName);
        GetComponent<StatsMansionCards>().PlayerGetsPointsFromCurrentMansionCard();
        mansionDeck.Remove(mansionDeck[value]);
        MansionDeckCount = mansionDeck.Count;
        mansionDeckCountTMP.text = "Mansion card count: " + MansionDeckCount;
    }
    ///////////////////////////
    public void OnClickSendCardBottomOfTheDeck() //LOSE btn
    {
        if (view.IsMine)
        {
            view.RPC("RPC_SendCardBottomOfTheDeck", RpcTarget.AllBuffered, isBottomCard);
            int dmg = CurrentMansionCard.GetDMG();
            string cardInfo = CurrentMansionCard.GetCardInfo();
            if (cardInfo == "shuffle")
                ShuffleDeck(dmg);
            else
                view.RPC("RPC_SetMainTextPlayer", RpcTarget.AllBuffered, currentPlayerName, false, dmg, "");

            MansionActionButtons.SetActive(false); OtherActionButtons.SetActive(true);
            HandCardStats.GetComponent<HandCardStatsControl>().MansionSetPlayerWins(true);
        }
    }
    [PunRPC] private void RPC_SendCardBottomOfTheDeck(bool chooseBottomCard)
    {
        if(!chooseBottomCard)
        {
            mansionDeck.Add(mansionDeck[0]);
            mansionDeck.Remove(mansionDeck[0]);
        }
        if (isBossEncounter)
            MansionBossIsBeaten(false);
    }
    ///////////////////////////
    private void ShuffleDeck(int dmg) //SHUFFLE btn
    {
        if (view.IsMine)
        {
            mansionDeckArrayHolder = mansionDeck.ToArray();
            ShuffleMansionDeck();      
            view.RPC("RPC_ShuffleMansionDeck", RpcTarget.AllBuffered, (object)mansionDeckArrayHolder);
            //string txt = "Mansion deck SHUFFLE";
            view.RPC("RPC_SetMainTextPlayer", RpcTarget.AllBuffered, currentPlayerName, false, dmg, "Deck shuffled");
            //view.RPC("RPC_SetMainTextManually", RpcTarget.AllBuffered, txt);
        }
    }

    [PunRPC] private void RPC_SetMainTextPlayer(string name, bool wins, int value, string info)
    {
        StartCoroutine(Main_PlayerWins(name, wins,value, info));
    }
    [PunRPC] private void RPC_SetMainTextManually(string txt)
    {
        StartCoroutine(Main_OtherText(txt));
    }

    public void OnClickSetMansionTextEmpty() // CLICK???
    {
        string txt = "";
        view.RPC("RPC_SetMainTextManually", RpcTarget.AllBuffered, txt);

    }

    IEnumerator MansionDoorOpening(string doorText)
    {
        mansionActionTMP.text = doorText;
        yield return new WaitForSeconds(4f);
        mansionActionTMP.text = "";
    }
    IEnumerator Main_OtherText(string txt)
    {
        mansionActionTMP.text = txt;
        yield return new WaitForSeconds(5f);
        mansionActionTMP.text = "";
    }
    IEnumerator Main_PlayerWins(string name, bool wins, int value, string info)
    {
        MansionCard.GetComponent<Image>().color = inactiveColor;
        if (wins)
        {
            mansionActionTMP.text = "WIN\n";
            if(value > 0)
                mansionActionTMP.text += name + " got "+ value + " points";
            else
                mansionActionTMP.text += name + " didn't get any points";
        }
        else
        {
            mansionActionTMP.text = "LOSE\n";
            //int dmg = CurrentMansionCard.GetDMG();
            if(value > 0)
                mansionActionTMP.text += name + " takes "+ value + " damage";
            else
                mansionActionTMP.text += name + " takes no damage";
        }
        mansionActionTMP.text += "\n" + info;
        yield return new WaitForSeconds(5f);
        
        mansionActionTMP.text = "";
    }
}