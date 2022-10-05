using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class StatsControl : MonoBehaviour
{
    public GameObject PhotonRoomMaster;

    private void OnEnable()
    {
        CurrentMansionCard.SetCard(0,0,0, false);

    }
    public void PlayerGetsPointsFromCurrentMansionCard()
    {
        int addPoints = CurrentMansionCard.GetPoints();
        PhotonRoomMaster.GetComponent<PlayerListControl>().UpdateCurrentPlayerPoints(addPoints);
    }

    public void CheckMansionCardStats(int playerID, string cardName)
    {
        int cardTier = Int32.Parse(cardName.Substring(0, 1));
        int cardNumber = Int32.Parse(cardName.Substring(5, 3)); //Row 5 -> next 3 letters

        GetMansionCardValue(cardTier, cardNumber);

    }
    private int GetMansionCardValue(int tier, int cardNumber)
    {
        int points=0; //aka. decorations

        if(tier==0) //Low tier
            points = MansionLowTierCards(cardNumber);
        else if (tier==1) //Mid tier
            points = MansionMidTierCards(cardNumber);
        else // tier==2 //High tier
            points = MansionTopTierCards(cardNumber);

        return points;
    }
    private int MansionLowTierCards(int cardNumber)
    {
        //Low tier
        // 1 Decoration //1-5,22,23,27,28,44,47,51,59,60,62,63,73,74,83,84,87-89,95,97
        // 2 Decoration //11,33,45,52,64,66,76,93
        // 3 Decoration //35,77

        //003 = If defeated: Choose a PLAYER who doesn't use ACTIONS in their next turn
        //004 = If defeated: Choose a PLAYER that skips their next turn
        //035 = Is revealed: It gets +5 HEALTH and +5 DAMAGE for each INFECTED attached to current PLAYER
        //045 = Is revealed: PLAYER takes 20 damage
        //047 = While infected is attached: PLAYER's decoration requirement for level 2 is lowered to level 1 ???
        //062 = Is revealed: If PLAYER has 3 or more ACTIONS in their PLAY AREA -> no damage to the enemy
        //063 = If defeated: PLAYER gets +20 GOLD and +1 BUY
        //066 = Is revealed: PLAYER heals +20 HP
        //073 = Is defeated: Select another PLAYER who gains 1 "Ammo x10" card
        //WIP

        int hp = 0, dmg = 0, deco = 0;

        switch (cardNumber)
        {
            case 1:
                hp = 15; dmg = 10; deco = 1;
                break;
            case 2:
                hp = 20; dmg = 20; deco = 1;
                break;
            case 3:
                hp = 15; dmg = 10; deco = 1;
                break;
            case 4:
                hp = 15; dmg = 10; deco = 1;
                break;
            case 5:
                hp = 10; dmg = 10; deco = 1;
                break;
            case 11:
                hp = 20; dmg = 15; deco = 2;
                break;
            case 22:
                hp = 10; dmg = 10; deco = 1;
                break;
            case 23:
                hp = 15; dmg = 20; deco = 1;
                break;
            case 27:
                hp = 20; dmg = 10; deco = 1;
                break;
            case 28:
                hp = 20; dmg = 20; deco = 1;
                break;
            case 33:
                hp = 30; dmg = 20; deco = 2;
                break;
            case 35:
                hp = 15; dmg = 20; deco = 3;
                break;
            case 44:
                hp = 10; dmg = 10; deco = 1;
                break;
            case 45:
                hp = 15; dmg = 0; deco = 1; //PLAYER TAKES 20 DMG
                break;
            case 47:
                hp = 20; dmg = 10; deco = 1;
                break;
            case 51:
                hp = 15; dmg = 10; deco = 1;
                break;
            case 52:
                hp = 20; dmg = 20; deco = 2;
                break;
            case 59:
                hp = 15; dmg = 10; deco = 1;
                break;
            case 60:
                hp = 20; dmg = 10; deco = 1;
                break;
            case 62:
                hp = 15; dmg = 10; deco = 1;
                break;
            case 63:
                hp = 15; dmg = 10; deco = 1;
                break;
            case 64:
                hp = 20; dmg = 20; deco = 2;
                break;
            case 66:
                hp = 20; dmg = 20; deco = 1;
                break;
            case 73:
                hp = 10; dmg = 10; deco = 1;
                break;
            case 74:
                hp = 20; dmg = 20; deco = 1;
                break;
            case 76:
                hp = 20; dmg = 20; deco = 2;
                break;
            case 77:
                hp = 5; dmg = 5; deco = 3;
                break;
            case 83:
                hp = 10; dmg = 15; deco = 1;
                break;
            case 84:
                hp = 10; dmg = 10; deco = 1;
                break;
            case 87:
                hp = 10; dmg = 10; deco = 1;
                break;
            case 88:
                hp = 15; dmg = 10; deco = 1;
                break;
            case 89:
                hp = 15; dmg = 10; deco = 1;
                break;
            case 93:
                hp = 20; dmg = 10; deco = 2;
                break;
            case 95:
                hp = 20; dmg = 20; deco = 1;
                break;
            case 97:
                hp = 20; dmg = 20; deco = 1;
                break;
        }

        CurrentMansionCard.SetCard(hp, dmg, deco, false);

        Debug.Log("Mansion card - Tier: LOW " + " hp: " + hp + " dmg: " + dmg + " deco: " + deco);

        return deco;
    }
    private int MansionMidTierCards(int cardNumber)
    {
        //Mid Tier

        int hp = 0, dmg = 0, deco = 0;

        switch (cardNumber)
        {
            case 6:
                hp = 40; dmg = 30; deco = 3;
                break;
            case 8:
                hp = 40; dmg = 30; deco = 4;
                break;
            case 9:
                hp = 30; dmg = 20; deco = 2;
                break;
            case 15:
                hp = 25; dmg = 10; deco = 2;
                break;
            case 17:
                hp = 30; dmg = 25; deco = 3;
                break;
            case 24:
                hp = 35; dmg = 20; deco = 3;
                break;
            case 30:
                hp = 25; dmg = 20; deco = 2;
                break;
            case 36:
                hp = 35; dmg = 40; deco = 4;
                break;
            case 37:
                hp = 30; dmg = 20; deco = 2;
                break;
            case 46:
                hp = 30; dmg = 10; deco = 3;
                break;
            case 48:
                hp = 40; dmg = 20; deco = 4;
                break;
            case 49:
                hp = 30; dmg = 20; deco = 3;
                break;
            case 50:
                hp = 30; dmg = 30; deco = 3;
                break;
            case 58:
                hp = 30; dmg = 20; deco = 2;
                break;
            case 61:
                hp = 30; dmg = 0; deco = 2;
                break;
            case 67:
                hp = 30; dmg = 10; deco = 2;
                break;
            case 69:
                hp = 30; dmg = 10; deco = 1;
                break;
            case 81:
                hp = 30; dmg = 10; deco = 2;
                break;
            case 85:
                hp = 25; dmg = 20; deco = 2;
                break;
            case 86:
                hp = 25; dmg = 20; deco = 2;
                break;
            case 90:
                hp = 25; dmg = 20; deco = 2;
                break;
            case 92:
                hp = 25; dmg = 10; deco = 2;
                break;
            case 94:
                hp = 25; dmg = 20; deco = 2;
                break;
            case 96:
                hp = 30; dmg = 10; deco = 2;
                break;
            case 98:
                hp = 30; dmg = 10; deco = 2;
                break;
        }
        CurrentMansionCard.SetCard(hp, dmg, deco, false);
        Debug.Log("Mansion card - Tier: MID " + " hp: " + hp + " dmg: " + dmg + " deco: " + deco);
        return deco;
    }
    private int MansionTopTierCards(int cardNumber)
    {
        //Top Tier

        int hp = 0, dmg = 0, deco = 0; bool isMainBoss=false;

        switch (cardNumber)
        {
            case 7:
                hp = 60; dmg = 40; deco = 5; //Nemesis T-Type
                break;
            case 10:
                hp = 90; dmg = 70; deco = 8; isMainBoss = true; //Uroboros Aheri //MAIN BOSS
                break;
            case 14:
                hp = 40; dmg = 25; deco = 4; //Gatling Gun Majini
                break;
            case 16:
                hp = 40; dmg = 40; deco = 4; //El Gigante
                break;
            case 25:
                hp = 60; dmg = 40; deco = 5; //Duvalia (Las Plagas)
                break;
            case 26:
                hp = 50; dmg = 40; deco = 4; //Garrador
                break;
            case 29:
                hp = 90; dmg = 70; deco = 9; isMainBoss = true; //Albert Wesker //MAIN BOSS
                break;
            case 31:
                hp = 60; dmg = 40; deco = 5; //Proto Tyrant
                break;
            case 32:
                hp = 50; dmg = 30; deco = 4; //Iron Maiden
                break;
            case 34:
                hp = 55; dmg = 30; deco = 4; //Nosferatu
                break;
            case 41:
                hp = 90; dmg = 70; deco = 8; isMainBoss = true; //Tyrant T-002 //MAIN BOSS
                break;
            case 42:
                hp = 40; dmg = 30; deco = 4; //Yawn
                break;
            case 43:
                hp = 60; dmg = 40; deco = 5; //Nemesis (2nd Form)
                break;
            case 53:
                hp = 40; dmg = 20; deco = 0; //Lisa Trevor
                break;
            case 54:
                hp = 90; dmg = 60; deco = 8; isMainBoss = true; //Osmund Saddler // MAIN BOSS
                break;
            case 55:
                hp = 70; dmg = 50; deco = 6; //Bitores Mendez
                break;
            case 56:
                hp = 40; dmg = 20; deco = 4; //Regenerator
                break;
            case 57:
                hp = 50; dmg = 30; deco = 4; //Jack Krauser
                break;
            case 65:
                hp = 40; dmg = 30; deco = 4; //Verdugo
                break;
            case 68:
                hp = 40; dmg = 30; deco = 4; //Hunter
                break;
            case 75:
                hp = 50; dmg = 30; deco = 4; //Giant Majini
                break;
            case 78:
                hp = 80; dmg = 60; deco = 8; //Red Executioner
                break;
            case 79:
                hp = 40; dmg = 20; deco = 3; //Big Man Majini
                break;
            case 80:
                hp = 50; dmg = 30; deco = 4; //Chainsaw Majini
                break;
            case 82:
                hp = 50; dmg = 40; deco = 4; //J.J
                break;
            case 91:
                hp = 40; dmg = 30; deco = 3; //Crocodile
                break;

        }
        CurrentMansionCard.SetCard(hp, dmg, deco, isMainBoss);
        Debug.Log("Mansion card - Tier: LOW " + " hp: " + hp + " dmg: " + dmg + " deco: " + deco + " isBoss: "+isMainBoss);
        return deco;
    }

}

public static class CurrentMansionCard
{
    private static int hp, dmg, deco;
    private static bool isBoss;

    public static void SetCard(int health, int damage, int decoration, bool isMainBoss)
    {
        hp = health; dmg = damage; deco = decoration; isBoss = isMainBoss;

    }

    public static int GetPoints()
    {
        return deco;
    }
    public static int GetHP()
    {
        return hp;
    }
    public static int GetDMG()
    {
        return dmg;
    }

    public static bool GetIsBossCard()
    {
        return isBoss;
    }
}
