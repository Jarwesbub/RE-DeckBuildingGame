using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandCardStatsControl : MonoBehaviour
{
    public GameObject MainCanvas, UIControl, UIBlackedMain;
    [SerializeField] private GameObject CurrentCardObj, PlayerHandCardsParent;
    [SerializeField] private List<string> PlayAreaCards, cardAbilityHolder, boughtCardsList;
    [SerializeField] private List<GameObject> PlayAreaObjects;
    [SerializeField] private List<int> cardAbilityValues;
    private List<int[]> statsList;
    private int playerDeckSize, playerDiscardPileSize;
    private int knifeCount, grenadeCount, handgunCount, shotgunCount, rifleCount, biggunCount, machinegunCount, actionCount;
    [SerializeField] private int handCardCount, maxHandCards, handCardBaseValue; //Counting handcards -> base value is the value when turn starts (5)
    private int playerActions;
    [SerializeField] int gameMode; //0=No restrictions, 1=Show stats only, 2=full restrictions
    bool playerWins, actionsLocked;

    private void Awake()
    {
        gameMode = 2; //TESTING
        GameStats.GameMode = gameMode;
        handCardBaseValue = 5;
        maxHandCards = handCardBaseValue;
        handCardCount = 0;
        UIBlackedMain.SetActive(false);
    }

    private void Start()
    {
        playerWins = false;
        actionsLocked = false;
        PlayAreaCards = new List<string>();
        PlayAreaObjects = new List<GameObject>();
        statsList = new List<int[]>();
        cardAbilityHolder = new List<string>();
        boughtCardsList = new List<string>();
        cardAbilityValues = new List<int>();
        if (gameMode == 0)
        {
            MainCanvas.SetActive(false);
        }
    }

    public void OnClickEndTurnButton() //RESET STATS
    {
        if (gameMode > 0)
        {
            knifeCount = 0; grenadeCount = 0; handgunCount = 0; shotgunCount = 0; rifleCount = 0; machinegunCount = 0; biggunCount = 0;
            actionCount = 0; playerActions = 0; handCardCount = 0;
            maxHandCards = handCardBaseValue; //Reset handcard ammount
            actionsLocked = false;
            PlayAreaCards.Clear();
            PlayAreaObjects.Clear();
            cardAbilityHolder.Clear();
            boughtCardsList.Clear();
            cardAbilityValues.Clear();
            GetComponent<HandCardAbilityModifier>().PlayerTurnEnds();
            MainCanvas.GetComponent<HandCardStatsUI>().ResetPlayArea();
            UIControl.GetComponent<DeleteCardsControl>().EndTurnTrashListActivate(); //Clear the trash cards list when turn ends
        }
    }
    public bool PlayerDrawsACard()
    {
        if (gameMode < 2)
        {
            return true;
        }
        else if (handCardCount < maxHandCards)
        {
            handCardCount++;
            Debug.Log("Handcards drawn: "+handCardCount);
            return true;
        }
        else
        {
            Debug.Log("No more draws left!");
            return false;
        }

    }

    public void PlayerBuysACard(string name)
    {
        boughtCardsList.Add(name);
    }

    public void SetPlayerDeckSizes(int deckSize, int discardPileSize)
    {
        playerDeckSize = deckSize;
        playerDiscardPileSize = discardPileSize;
    }

    public void SetCardByName(string name, GameObject cardObj)
    {
        if (gameMode > 0) //
        {
            Debug.Log("HandCard added: " + name);
            CurrentCardObj = cardObj;
            int[] stats = FindHandCardStats(name);
            if (stats != null)
            {
                PlayAreaCards.Add(name);
                PlayAreaObjects.Add(cardObj);
                statsList.Add(stats);
                MainCanvas.GetComponent<HandCardStatsUI>().SetPlayArea(stats[0], stats[1], stats[2], true);
                CheckAbilityHolder();
            }
        }
    }

    private int[] FindHandCardStats(string name)
    {
        string type = name.Substring(0, 3);
        int[] card = new int[4];
        int index = 0;

        switch (type)
        {
            case "ia_":
                card = StatsHandCards.GetAmmoOrHealth(name);
                index = 0;
                break;
            case "sup":
                card = StatsHandCards.GetSupportWeapon(name);
                index = 1;
                break;
            case "hg_":
                card = StatsHandCards.GetHandgun(name);
                index = 2;
                break;
            case "ras":
                card = StatsHandCards.GetShotgunOrRifle(name);
                index = 3;
                break;
            case "mg_":
                card = StatsHandCards.GetMachinegun(name);
                index = 4;
                break;
            case "bg_":
                card = StatsHandCards.GetBigGun(name);
                index = 5;
                break;

        }

        card = CheckCardAbilities(index, name, card);      
        return card;
    }

    public void RemoveCardByName(string name)
    {
        int count = PlayAreaCards.Count;
        for (int i = 0; i < count; i++)
        {
            if (PlayAreaCards[i] == name)
            {
                PlayAreaCards.RemoveAt(i);
                PlayAreaObjects.RemoveAt(i);
                UpdateHandCardsCount(name, -1);
                MainCanvas.GetComponent<HandCardStatsUI>().SetPlayArea(statsList[i][0], statsList[i][1], statsList[i][2], false);
                statsList.RemoveAt(i);
                CheckAbilityHolder();
                return;
            }
        }
        
    }
    private void UpdateHandCardsCount(string name, int value) //value -1 = recrease by 1
    {
        string type = name.Substring(0, 3);
        if (type == "sup")
        {
            if (name == "sup_knife1" || name == "sup_knife2" || name == "sup_knife3")
                knifeCount += value;
            else
                grenadeCount += value;
        }
        else if (type == "hg_")
        {
            handgunCount += value;
        }
        else if (type == "ras")
        {
            if (name == "ras_automaticshotgun" || name == "ras_hydra" || name == "ras_m3" || name == "ras_shotgun1" || name == "ras_shotgun2")
                shotgunCount += value;
            else
                rifleCount += value;
        }
        else if (type == "mg_")
        {
            machinegunCount += value;
        }
        else if (type == "bg_")
        {
            biggunCount += value;
        }
        else if (type == "ac-")
        {
            actionCount += value;
        }

    }
    public void MansionSetPlayerWins(bool wins)
    {
        if (gameMode > 0)
        {
            playerWins = wins;
            if (wins)
            {
                if (PlayAreaCards.Contains("sup_knife2")) // If player defeats infected -> get +10 gold (EDITED)
                {
                    MainCanvas.GetComponent<HandCardStatsUI>().SetPlayArea(0, 0, 10, true); //+10 gold
                }
            }
        }
    }

    private int[] CheckCardAbilities(int index, string name, int[] stats)
    {
        if (index == 0) //Ammo and Health recorvery
        {
            switch (name)
            {
                case "ia_greenherb":
                    GetComponent<HandCardAbilityModifier>().AddOrRemovePlayerHP(20);
                    GetComponent<HandCardAbilityModifier>().TrashACardInstantly(CurrentCardObj);
                    break;
                case "ia_redherb": //Currently functions as same as green herb!
                    GetComponent<HandCardAbilityModifier>().AddOrRemovePlayerHP(20);
                    GetComponent<HandCardAbilityModifier>().TrashACardInstantly(CurrentCardObj);
                    break;
                case "ia_firstaidkit2":
                    GetComponent<HandCardAbilityModifier>().AddOrRemovePlayerHP(200);
                    GetComponent<HandCardAbilityModifier>().TrashACardInstantly(CurrentCardObj);
                    break;
            }
        }
        else if (index == 1) //Knife and throwable weapons
        {
            switch (name)
            {
                case "sup_knife1":
                    knifeCount++;
                    break;

                case "sup_knife2":
                    knifeCount++;
                    stats[2] += 10;
                    break;

                case "sup_knife3":
                    knifeCount++;
                    stats[1] += 5 * (knifeCount - 1); //+5 dmg for every other used knife weapon
                    break;

                case "sup_bow2":
                    if (actionCount == 0)
                    {
                        stats[1] += 10; //+10 dmg if no actions played -> lock the use of actions
                        actionsLocked = true;
                    }
                    break;

                case "sup_stunrod":
                    knifeCount++;
                    break;

                case "sup_flash1": //WIP
                    grenadeCount++;
                    GetComponent<HandCardAbilityModifier>().TrashACardWhenTurnEnds(CurrentCardObj);
                    break;

                case "sup_flash2": //WIP
                    grenadeCount++;
                    GetComponent<HandCardAbilityModifier>().TrashACardWhenTurnEnds(CurrentCardObj);
                    break;

                case "sup_grenade1":
                    grenadeCount++;
                    GetComponent<HandCardAbilityModifier>().TrashACardWhenTurnEnds(CurrentCardObj);
                    break;

                case "sup_grenade2":
                    grenadeCount++;
                    GetComponent<HandCardAbilityModifier>().TrashACardWhenTurnEnds(CurrentCardObj);
                    break;

                case "sup_grenade3":
                    grenadeCount++;
                    GetComponent<HandCardAbilityModifier>().TrashACardWhenTurnEnds(CurrentCardObj);
                    break;

            }
        }
        else if (index == 2) // Handguns
        {
            handgunCount++;
            int playerAmmo = MainCanvas.GetComponent<HandCardStatsUI>().GetPlayerAmmoValue();
            int ammoLeft = playerAmmo + stats[0];
            switch (name)
            {
                case "hg_brokenbutterfly":
                    //This Weapon gets +20 Damage if the Exploring Character's Player has 10 or more cards in their Inventory.
                    if (playerDeckSize >= 10)
                        stats[1] += 20; // If inventory size is bigger than 10 -> +20 dmg
                    break;

                case "hg_lighninghawk":
                    //No effect currently
                    break;

                case "hg_m93r":
                    // While Attacking with more than 1 weapon, this Weapon gets +20 damage.
                    cardAbilityHolder.Add(name);
                    cardAbilityValues.Add(0);
                    break;

                case "hg_magnum2":
                    // Trash 1 ammunation from your Play Area. This card cannot be attached to Characters.
                    if (gameMode == 2 && ammoLeft >= 10)
                    {
                        List<Sprite> ammoCards = new List<Sprite>();
                        List<string> ammoNames = new List<string>();
                        string ammo20and30 = "ia_ammo20";
                        if (ammoLeft >= 30)
                            ammo20and30 = "ia_ammo"; //All the ammos

                        foreach (GameObject child in PlayAreaObjects)
                        {
                            Sprite sprt = child.GetComponent<Image>().sprite;
                            if (sprt.name.Contains("ia_ammo10") || sprt.name.Contains(ammo20and30))
                            {
                                ammoCards.Add(sprt);
                                ammoNames.Add(child.GetComponent<HandCard>().currentCard);
                            }
                        }
                        UIBlackedMain.GetComponent<UIBlackedGridContent>().AddHandCardNameAndSprite("ammo", ammoNames.ToArray(), ammoCards.ToArray()); //Set card(s) name and sprite(s)
                        UIBlackedMain.GetComponent<UIBlackedGridContent>().UISetActive("Choose 'Ammo' card to be removed", 0, true); // Pop up new ui with chosen cards
                    }
                    else if (gameMode == 2)
                        MainCanvas.GetComponent<HandCardStatsUI>().SetPlayerTurnTitle("Not enough Ammo!", 2f); //0 = shows text forever

                    break;

                case "hg_pistol1":
                    //This Weapon gets +5 Damage for each non-Item you Gained this turn.
                    int count = boughtCardsList.Count;
                    int nonItem = 0;
                    for (int i = 0; i < count; i++)
                    {
                        if (!boughtCardsList[i].Contains("ia_") || boughtCardsList[i].Contains("ia_ammo"))
                            nonItem++;
                    }
                    stats[1] += nonItem * 5;
                    break;

                case "hg_pistol2":
                    //You get +1 card and +1 Action during this turn.
                    playerActions++;
                    maxHandCards++;
                    break;

                case "hg_pistol3":
                    //You can give this Weapon+10 Damage during this turn. In that case, Trash this Weapon at the end of this turn.
                    UIBlackedMain.GetComponent<UIBlackedBoolean>().UISetActive("Get +10 damage and Trash this card at the end of this turn?", CurrentCardObj); // Pop up new ui with chosen cards
                    break;

                case "hg_polizei":
                    //You can Discard any number of "Pistol" Weapons from your Hand to give this Weapon +10 Damage during this turn for each "Pistol"
                    //Weapon Discarded due to this effect.
                    List<Sprite> pistolCards = new List<Sprite>();
                    List<string> cardNames = new List<string>();
                    int hg_count = PlayerHandCardsParent.transform.childCount;
                    for (int i = 0; i < hg_count; i++)
                    {
                        string currentCard = PlayerHandCardsParent.transform.GetChild(i).GetComponent<HandCard>().currentCard;
                        if (currentCard.Contains("hg_") && currentCard != "hg_polizei") //Handguns
                        {
                            cardNames.Add(currentCard);
                            Sprite sprt = PlayerHandCardsParent.transform.GetChild(i).GetComponent<Image>().sprite;
                            pistolCards.Add(sprt);
                        }
                    }

                    UIBlackedMain.GetComponent<UIBlackedGridContent>().AddHandCardNameAndSprite("hg_polizei", cardNames.ToArray(), pistolCards.ToArray()); //Set card(s) name and sprite(s)
                    UIBlackedMain.GetComponent<UIBlackedGridContent>().UISetActive("Get +10 damage for each selected 'Pistol' card and move them in to the Discardpile", 1, true); // Pop up new ui with chosen cards
                    break;

                case "hg_punisher":
                    //You get +2 cards and +1 Action during this turn
                    playerActions++;
                    maxHandCards += 2;
                    break;

                case "hg_samuraiedge":
                    //This Weapon gets +20 Damage for each card you Gained this turn.
                    //Gained = bought ?

                    int buysCount = MainCanvas.GetComponent<HandCardStatsUI>().GetBuysCount();
                    stats[1] += buysCount * 20;
                    break;

                case "hg_usp":
                    //You can pay 20 Ammo. In that case, this Weapon gets +10 Damage during this turn.
                    UIBlackedMain.GetComponent<UIBlackedBoolean>().UISetActive("You can pay 20 Ammo. In that case, this Weapon gets +10 Damage during this turn.", CurrentCardObj);
                    break;

            }
        }
        else if (index == 3) // Shotguns or Rifles
        {

        }
        else if (index == 4) // Machineguns
        {

        }
        else if (index == 5) // Big guns
        {

        }
        return stats;

    }

    private void CheckAbilityHolder() //Checks values constantly
    {
        int[] stats = new int[4];

        for (int i = 0; i < cardAbilityHolder.Count; i++)
        {
            if (cardAbilityHolder[i] == "hg_m93r") //While Attacking with more than 1 weapon -> this weapon gets +20 damage
            {
                int playArea = CountListDuplicants("hg_m93r", PlayAreaCards);
                int holder = CountListDuplicants("hg_m93r", cardAbilityHolder);
                if (playArea!=holder)
                {
                    if (cardAbilityValues[i] == 1)
                    {
                        stats[1] += -20;
                    }
                    cardAbilityHolder.RemoveAt(i);
                    cardAbilityValues.RemoveAt(i);
                }
                else
                {
                    int weaponCount = knifeCount + handgunCount + shotgunCount + rifleCount + machinegunCount + biggunCount;
                    if (weaponCount > 1 && cardAbilityValues[i] == 0)
                    {
                        stats[1] += 20;
                        cardAbilityValues[i] = 1;
                    }
                    else if (weaponCount <= 1 && cardAbilityValues[i] == 1)
                    {
                        stats[1] += -20;
                        cardAbilityValues[i] = 0;
                    }
                }               
            }
        }

        MainCanvas.GetComponent<HandCardStatsUI>().SetPlayArea(stats[0], stats[1], stats[2], true);
    }

    private int CountListDuplicants(string name, List<string> list)
    {
        int count = list.Count;
        int cardCount = 0;
        for (int i = 0; i < count; i++)
        {
            if (list[i] == name)
                cardCount++;
        }
        return cardCount;
    }
}
