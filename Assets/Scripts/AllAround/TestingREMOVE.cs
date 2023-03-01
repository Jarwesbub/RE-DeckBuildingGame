using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingREMOVE : MonoBehaviour
{
    public GameObject CurrentCardObj;
    /*
    private int[] CheckCardAbilities2(int index, string name, int[] stats)
    {
        switch (name)
        {
            case "":

                break;
            case "":

                break;
            case "":

                break;
        }


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
            else if (index == 2) // Handguns
            {
                handgunCount++;
                int playerAmmo = HandCardStatsUI.GetComponent<HandCardStatsUI>().GetPlayerAmmoValue();
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
                            List<string> cardNames = new List<string>();
                            string ammo20and30 = "ia_ammo20";
                            if (ammoLeft >= 30)
                                ammo20and30 = "ia_ammo"; //All the ammos

                            foreach (GameObject child in PlayAreaObjects)
                            {
                                Sprite sprt = child.GetComponent<Image>().sprite;
                                if (sprt.name.Contains("ia_ammo10") || sprt.name.Contains(ammo20and30))
                                {
                                    ammoCards.Add(sprt);
                                    cardNames.Add(child.GetComponent<HandCard>().currentCard);
                                }
                            }
                            UIBlackedMain.GetComponent<UIBlackedGridContent>().AddHandCardNameAndSprite("ammo", cardNames.ToArray(), ammoCards.ToArray()); //Set card(s) name and sprite(s)
                            UIBlackedMain.GetComponent<UIBlackedGridContent>().UISetActive("Choose 'Ammo' card to be removed", 0, true); // Pop up new ui with chosen cards
                        }
                        else if (gameMode == 2)
                            HandCardStatsUI.GetComponent<HandCardStatsUI>().SetPlayerTurnTitle("Not enough Ammo!", 2f); //0 = shows text forever

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
                        int count = PlayerHandCardsParent.transform.childCount;
                        for (int i = 0; i < count; i++)
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

                        int buysCount = HandCardStatsUI.GetComponent<HandCardStatsUI>().GetBuysCount();
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

        }

        return stats;

    }




    //OLD
    private int[] CheckCardAbilities(int index, string name, int[] stats)
    {
        if (index == 0) //Ammo and Health recorvery
        {
            if (name == "ia_greenherb")
            {
                GetComponent<HandCardAbilityModifier>().AddOrRemovePlayerHP(20);
                GetComponent<HandCardAbilityModifier>().TrashACardInstantly(CurrentCardObj);
            }
            else if (name == "ia_redherb") //Currently functions as same as green herb!
            {
                GetComponent<HandCardAbilityModifier>().AddOrRemovePlayerHP(20);
                GetComponent<HandCardAbilityModifier>().TrashACardInstantly(CurrentCardObj);
            }
            else if (name == "ia_firstaidkit2")
            {
                GetComponent<HandCardAbilityModifier>().AddOrRemovePlayerHP(200);
                GetComponent<HandCardAbilityModifier>().TrashACardInstantly(CurrentCardObj);
            }
        }
        if (index == 1) //Knife and throwable weapons
        {
            if (name == "sup_knife1")
            {
                knifeCount++;
            }
            if (name == "sup_knife2")
            {
                knifeCount++;
                stats[2] += 10;
            }
            if (name == "sup_knife3")
            {
                knifeCount++;
                stats[1] += 5 * (knifeCount - 1); //+5 dmg for every other used knife weapon
            }
            else if (name == "sup_bow2")
            {
                if (actionCount == 0)
                {
                    stats[1] += 10; //+10 dmg if no actions played -> lock the use of actions
                    actionsLocked = true;
                }
            }
            else if (name == "sup_stunrod") //Not in use rn
            {
                knifeCount++;
            }
            //THROWABLES
            else if (name == "sup_flash1")
            {
                grenadeCount++;
                GetComponent<HandCardAbilityModifier>().TrashACardWhenTurnEnds(CurrentCardObj);
            }
            else if (name == "sup_flash2")
            {
                grenadeCount++;
                GetComponent<HandCardAbilityModifier>().TrashACardWhenTurnEnds(CurrentCardObj);
            }
            else if (name == "sup_grenade1")
            {
                grenadeCount++;
                GetComponent<HandCardAbilityModifier>().TrashACardWhenTurnEnds(CurrentCardObj);
            }
            else if (name == "sup_grenade2")
            {
                grenadeCount++;
                GetComponent<HandCardAbilityModifier>().TrashACardWhenTurnEnds(CurrentCardObj);
            }
            else if (name == "sup_grenade3")
            {
                grenadeCount++;
                GetComponent<HandCardAbilityModifier>().TrashACardWhenTurnEnds(CurrentCardObj);
            }
        }
        if (index == 2) // Handguns
        {
            handgunCount++;
            int playerAmmo = HandCardStatsUI.GetComponent<HandCardStatsUI>().GetPlayerAmmoValue();
            int ammoLeft = playerAmmo + stats[0];
            if (name == "hg_brokenbutterfly")
            {
                //This Weapon gets +20 Damage if the Exploring Character's Player has 10 or more cards in their Inventory.
                if (playerDeckSize >= 10)
                    stats[1] += 20; // If inventory size is bigger than 10 -> +20 dmg
            }
            else if (name == "hg_lighninghawk")
            {
                //No effect currently
            }
            else if (name == "hg_m93r")
            {
                // While Attacking with more than 1 weapon, this Weapon gets +20 damage.
                cardAbilityHolder.Add(name);
                cardAbilityValues.Add(0);
            }
            else if (name == "hg_magnum2")
            {
                // Trash 1 ammunation from your Play Area. This card cannot be attached to Characters.
                if (gameMode == 2 && ammoLeft >= 10)
                {
                    List<Sprite> ammoCards = new List<Sprite>();
                    List<string> cardNames = new List<string>();
                    string ammo20and30 = "ia_ammo20";
                    if (ammoLeft >= 30)
                        ammo20and30 = "ia_ammo"; //All the ammos

                    foreach (GameObject child in PlayAreaObjects)
                    {
                        Sprite sprt = child.GetComponent<Image>().sprite;
                        if (sprt.name.Contains("ia_ammo10") || sprt.name.Contains(ammo20and30))
                        {
                            ammoCards.Add(sprt);
                            cardNames.Add(child.GetComponent<HandCard>().currentCard);
                        }
                    }
                    UIBlackedMain.GetComponent<UIBlackedGridContent>().AddHandCardNameAndSprite("ammo", cardNames.ToArray(), ammoCards.ToArray()); //Set card(s) name and sprite(s)
                    UIBlackedMain.GetComponent<UIBlackedGridContent>().UISetActive("Choose 'Ammo' card to be removed", 0, true); // Pop up new ui with chosen cards
                }
                else if (gameMode == 2)
                    HandCardStatsUI.GetComponent<HandCardStatsUI>().SetPlayerTurnTitle("Not enough Ammo!", 2f); //0 = shows text forever
            }
            else if (name == "hg_pistol1")
            {
                //This Weapon gets +5 Damage for each non-Item you Gained this turn.
                int count = boughtCardsList.Count;
                int nonItem = 0;
                for (int i = 0; i < count; i++)
                {
                    if (!boughtCardsList[i].Contains("ia_") || boughtCardsList[i].Contains("ia_ammo"))
                        nonItem++;
                }
                stats[1] += nonItem * 5;
            }
            else if (name == "hg_pistol2")
            {
                //You get +1 card and +1 Action during this turn.
                playerActions++;
                maxHandCards++;
            }
            else if (name == "hg_pistol3")
            {
                //You can give this Weapon+10 Damage during this turn. In that case, Trash this Weapon at the end of this turn.
                UIBlackedMain.GetComponent<UIBlackedBoolean>().UISetActive("Get +10 damage and Trash this card at the end of this turn?", CurrentCardObj); // Pop up new ui with chosen cards
            }
            else if (name == "hg_polizei")
            {
                //You can Discard any number of "Pistol" Weapons from your Hand to give this Weapon +10 Damage during this turn for each "Pistol"
                //Weapon Discarded due to this effect.
                List<Sprite> pistolCards = new List<Sprite>();
                List<string> cardNames = new List<string>();
                int count = PlayerHandCardsParent.transform.childCount;
                for (int i = 0; i < count; i++)
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
            }
            else if (name == "hg_punisher")
            {
                //You get +2 cards and +1 Action during this turn
                playerActions++;
                maxHandCards += 2;
            }
            else if (name == "hg_samuraiedge")
            {
                //This Weapon gets +20 Damage for each card you Gained this turn.
                //Gained = bought ?

                int buysCount = HandCardStatsUI.GetComponent<HandCardStatsUI>().GetBuysCount();
                stats[1] += buysCount * 20;
            }
            else if (name == "hg_usp")
            {
                //You can pay 20 Ammo. In that case, this Weapon gets +10 Damage during this turn.
                UIBlackedMain.GetComponent<UIBlackedBoolean>().UISetActive("You can pay 20 Ammo. In that case, this Weapon gets +10 Damage during this turn.", CurrentCardObj);
            }
        }
        if (index == 3) // Shotguns or Rifles
        {

        }
        if (index == 4) // Machineguns
        {

        }
        if (index == 5) // Big guns
        {

        }
        return stats;
    }
    */
}
