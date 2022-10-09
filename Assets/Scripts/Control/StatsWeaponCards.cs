using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

public class StatsWeaponCards// : MonoBehaviour
{
    public void GetMyWeaponCardsStats(string cardName)
    {
        string cardType = cardName[..1];
        string[] stats = new string[2];

        if (cardType == "a")
            stats = MachinegunStats(cardName);
        else if (cardType == "b")
            stats = BigGunStats(cardName); //Minethrowers, Bazookas etc.
        else if (cardType == "h")
            stats = HandgunStats(cardName);
        else if (cardType == "r") 
            stats = RifleShotgunStats(cardName); //Rifles and Shotguns
        else
            stats = SupportStats(cardName);


        Debug.Log(stats);
    }

    private string[] MachinegunStats(string cardName)
    {
        int ammo=0; int dmg=0; string info="";

        switch(cardName)
        {
            case "arsg_ak": //"arsg_ak"
                ammo = 0; dmg = 0; info = "You cannot use more than 20 Ammo for this weapon.";
                break;
            case "arsg_mp5": //"arsg_mp5"
                ammo = 30; dmg = 20; info = "";
                break;
            case "arsg_mp5plus": //"arsg_mp5plus"
                ammo = 70; dmg = 40; info = "If you have more than 100 Ammo, this Weapon gets +30 Damage.";
                break;
            case "arsg_scorpion": //"arsg_scorpion"
                ammo = 40; dmg = 20; info = "";
                break;
            case "arsg_sig": //"arsg_sig"
                ammo = 0; dmg = 0; info = "You cannot use more than 60 Ammo for this Weapon.";
                break;
            case "arsg_thompson":
                ammo = 50; dmg = 50; info = "This Weapon gets +10 Damage during this turn for each 'Machine gun' Weapon in your Discard Pile";
                break;
            case "arsg_tmp":
                ammo = 30; dmg = 20; info = "You get +20 Gold during this turn.";
                break;

        }

        return new string[] { ammo.ToString() , dmg.ToString() ,info };
    }
    private string[] BigGunStats(string cardName)
    {
        int ammo = 0; int dmg = 0; string info = "";

        switch (cardName)
        {
            case "bg_flamethrower":
                ammo = 0; dmg = 0; info = "Damage X = 5 times the number of cards in your Discard Pile.";
                break;
            case "bg_gatlinggun":
                ammo = 0; dmg = 0; info = "X Ammo and X Damage";
                break;
            case "bg_grenadelauncher":
                ammo = 0; dmg = 20; info = "All 'Explosive' Weapons your Character uses during this turn go to your Discard Pile instead of being Trashed.";
                break;
            case "bg_minethrower":
                ammo = 0; dmg = 0; info = "This Weapon gets +10 Damage during this turn for each Ammunition in your Play Area";
                break;
            case "bg_rp7night":
                ammo = 0; dmg = 60; info = "(TRASH ONCE USED) While it is nighttime outside of the game, this Weapon gets +20 Damage.";
                break;
            case "bg_rpg7": //NOT IN USE
                ammo = 0; dmg = 90; info = "(TRASH ONCE USED) When Trashed, shuffle the 'Rocket Launcher Case' Token into the Mansion.";
                break;

        }
        return new string[] { ammo.ToString(), dmg.ToString(), info };
    }

    private string[] HandgunStats(string cardName)
    {
        int ammo = 0; int dmg = 0; string info = "";

        switch (cardName)
        {
            case "hg_brokenbutterfly":
                ammo = 40; dmg = 40; info = "This Weapon gets +20 Damage if the Exploring Character's Player has 10 or more cards in their Inventory.";
                break;
            case "hg_lighninghawk":
                ammo = 40; dmg = 40; info = "";
                break;
            case "hg_m9":
                ammo = 20; dmg = 10; info = "";
                break;
            case "hg_m93r":
                ammo = 30; dmg = 20; info = "While Attacking with more than 1 Weapon, this Weapon gets +20 Damage.";
                break;
            case "hg_magnum1":
                ammo = 50; dmg = 50; info = "";
                break;
            case "hg_magnum2":
                ammo = 80; dmg = 80; info = "Trash 1 Ammunition from your Play Area. This card cannot be attached to Characters.";
                break;
            case "hg_pistol1":
                ammo = 10; dmg = 10; info = "This Weapon gets +5 Damage for each non-Item you Gained this turn.";
                break;
            case "hg_pistol2":
                ammo = 10; dmg = 10; info = "You get +1 card and +1 Action durin this turn.";
                break;
            case "hq_pistol3":
                ammo = 20; dmg = 10; info = "You can give this Weapon +10 Damage during this turn. In that case, Trash this Weapon at the end of this turn.";
                break;
            case "hg_polizei":
                ammo = 30; dmg = 20; info = "You can Discard any number of 'Pistol' Weapons from your Hand to give this weapon +10 " +
                    "Damage during this turn for each 'Pistol' Weapon Discarded due to this effect.";
                break;
            case "hg_punisher":
                ammo = 30; dmg = 30; info = "You get +2 cards and +1 Actrion during this turn.";
                break;
            case "hg_samuraiedge":
                ammo = 60; dmg = 30; info = "This Weapon gets +20 Damage for each card you Gained this turn.";
                break;
            case "hg_usp":
                ammo = 60; dmg = 30; info = "You can pay 20 Ammo. In that case, this Weapon gets +10 Damage during this turn.";
                break;
        }
        return new string[] { ammo.ToString(), dmg.ToString(), info };
    }

    private string[] RifleShotgunStats(string cardName)
    {
        int ammo = 0; int dmg = 0; string info = "";


        return new string[] { ammo.ToString(), dmg.ToString(), info };
    }

    private string[] SupportStats(string cardName)
    {
        int ammo = 0; int dmg = 0; string info = "";


        return new string[] { ammo.ToString(), dmg.ToString(), info };
    }
}
