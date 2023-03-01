using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StatsHandCards
{
    public static int[] GetBasicAmmo(int value) // Ammo10, Ammo20, Ammo30;
    {
        if (value == 10 || value == 20 || value == 30)
        {
            int[] stats = { value, value }; // 
            return stats;
        }
        else
        {
            int[] stats = {0,0};
            return stats;
        }
    }

    public static int[] GetAmmoOrHealth(string item)
    {
        int[] stats = new int[4];
        switch (item)
        {
            case "ia_ammo10":
                stats[0] = 10; //Ammo
                stats[1] = 0; //Damage
                stats[2] = 10; //Gold (money)
                stats[3] = 0; //Has ability (true && false)
                return stats;
            case "ia_ammo20":
                stats[0] = 20;
                stats[1] = 0;
                stats[2] = 30;
                stats[3] = 0;
                return stats;
            case "ia_ammo30":
                stats[0] = 30;
                stats[1] = 0;
                stats[2] = 60;
                stats[3] = 0;
                return stats;
        }
        return GetHPitem(item);
    }
    private static int[] GetHPitem(string item) //WIP
    {
        int[] stats = new int[4];
        switch (item)
        {
            case "ia_firstaidkit2":
                stats[0] = 0; //Ammo requirement
                stats[1] = 0; //Damage
                stats[2] = 0; //Gold
                stats[3] = 1; //Has ability (true && false)
                break;
            case "ia_greenherb":
                stats[0] = 0;
                stats[1] = 0;
                stats[2] = 0;
                stats[3] = 1;
                break;
            case "ia_redherb":
                stats[0] = 0;
                stats[1] = 0;
                stats[2] = 0;
                stats[3] = 1;
                break;
        }
        return stats;
    }

    public static int[] GetSupportWeapon(string knifeAndThrowable)
    {
        int[] stats = new int[4];
        switch (knifeAndThrowable)
        {
            case "sup_knife1":
                stats[0] = 0; //Ammo requirement
                stats[1] = 5; //Damage
                stats[2] = 0; //Gold (money)
                stats[3] = 0; //Has ability (true && false)
                break;
            case "sup_knife2": //Special ability
                stats[0] = 0;
                stats[1] = 5;
                stats[2] = 0;
                stats[3] = 1;
                break;
            case "sup_knife3": //Special ability
                stats[0] = 0;
                stats[1] = 10;
                stats[2] = 0;
                stats[3] = 1;
                break;
            case "sup_bow1":
                stats[0] = 0;
                stats[1] = 25;
                stats[2] = 0;
                stats[3] = 0;
                break;
            case "sup_bow2": //Special ability
                stats[0] = 0;
                stats[1] = 20;
                stats[2] = 0;
                stats[3] = 1;
                break;
            case "sup_stunrod": //NOT IN USE
                stats[0] = 0;
                stats[1] = 10;
                stats[2] = 0;
                stats[3] = 1;
                break;
            //THROWABLES
            case "sup_flash1":
                stats[0] = 0; //Ammo requirement
                stats[1] = 0; //Damage
                stats[2] = 0; //Gold (money)
                stats[3] = 1; //Has ability (true && false)
                break;
            case "sup_flash2":
                stats[0] = 0;
                stats[1] = 10;
                stats[2] = 0;
                stats[3] = 1;
                break;
            case "sup_grenade1":
                stats[0] = 0;
                stats[1] = 15;
                stats[2] = 0;
                stats[3] = 1;
                break;
            case "sup_grenade2":
                stats[0] = 0;
                stats[1] = 20;
                stats[2] = 0;
                stats[3] = 1;
                break;
            case "sup_grenade3":
                stats[0] = 0;
                stats[1] = 10;
                stats[2] = 0;
                stats[3] = 1;
                break;
        }
        return stats;
    }

    public static int[] GetHandgun(string handgun)
    {
        int[] stats = new int[4];
        switch (handgun)
        {
            case "hg_brokenbutterfly":
                stats[0] = -40; //Ammo requirement
                stats[1] = 40; //Damage
                stats[2] = 0; //Gold (money)
                stats[3] = 1; //Has ability (true && false)
                break;
            case "hg_lighninghawk": //SA
                stats[0] = -40;
                stats[1] = 40; //+20
                stats[2] = 0;
                stats[3] = 1;
                break;
            case "hg_m9": 
                stats[0] = -20;
                stats[1] = 10;
                stats[2] = 0;
                stats[3] = 0;
                break;
            case "hg_m93r":
                stats[0] = -30;
                stats[1] = 20;
                stats[2] = 0;
                stats[3] = 1;
                break;
            case "hg_magnum1": 
                stats[0] = -50;
                stats[1] = 50;
                stats[2] = 0;
                stats[3] = 0;
                break;
            case "hg_magnum2":
                stats[0] = -80;
                stats[1] = 80;
                stats[2] = 0;
                stats[3] = 1;
                break;
            case "hg_pistol1": 
                stats[0] = -10;
                stats[1] = 10;
                stats[2] = 0;
                stats[3] = 1;
                break;
            case "hg_pistol2":
                stats[0] = -10;
                stats[1] = 10;
                stats[2] = 0;
                stats[3] = 1;
                break;
            case "hg_pistol3":
                stats[0] = -20;
                stats[1] = 10;
                stats[2] = 0;
                stats[3] = 1;
                break;
            case "hg_polizei": 
                stats[0] = -30;
                stats[1] = 20;
                stats[2] = 0;
                stats[3] = 1;
                break;
            case "hg_punisher":
                stats[0] = -30;
                stats[1] = 30;
                stats[2] = 0;
                stats[3] = 1;
                break;
            case "hg_samuraiedge":
                stats[0] = -60;
                stats[1] = 30;
                stats[2] = 0;
                stats[3] = 1;
                break;
            case "hg_usp":
                stats[0] = -10;
                stats[1] = 10;
                stats[2] = 0;
                stats[3] = 1;
                break;
        }
        return stats;
    }

    public static int[] GetShotgunOrRifle(string gun)
    {
        int[] stats = new int[4];
        switch (gun)
        {
            case "ras_automaticshotgun":
                stats[0] = -80; //Ammo requirement
                stats[1] = 50; //Damage
                stats[2] = 0; //Gold (money)
                stats[3] = 1; //Has ability (true && false)
                break;
            case "ras_hydra":
                stats[0] = -80;
                stats[1] = 50;
                stats[2] = 0;
                stats[3] = 1;
                break;
            case "ras_m3":
                stats[0] = -60;
                stats[1] = 45;
                stats[2] = 0;
                stats[3] = 1;
                break;
            case "ras_shotgun1":
                stats[0] = -40;
                stats[1] = 25;
                stats[2] = 0;
                stats[3] = 1;
                break;
            case "ras_shotgun2":
                stats[0] = -40;
                stats[1] = 30;
                stats[2] = 0;
                stats[3] = 1;
                break;
            //RIFLES ->
            case "ras_psg1":
                stats[0] = -50;
                stats[1] = 30;
                stats[2] = 0;
                stats[3] = 1;
                break;
            case "ras_rifle1":
                stats[0] = -50;
                stats[1] = 20;
                stats[2] = 0;
                stats[3] = 1;
                break;
            case "ras_rifle2":
                stats[0] = -40;
                stats[1] = 30;
                stats[2] = 0;
                stats[3] = 1;
                break;
            case "ras_rifle3":
                stats[0] = -50;
                stats[1] = 40;
                stats[2] = 0;
                stats[3] = 1;
                break;
            case "ras_rifle4":
                stats[0] = -50;
                stats[1] = 50;
                stats[2] = 0;
                stats[3] = 1;
                break;
            case "ras_svd":
                stats[0] = -70;
                stats[1] = 30;
                stats[2] = 0;
                stats[3] = 1;
                break;
        }
        return stats;
    }

    public static int[] GetMachinegun(string machinegun)
    {
        int[] stats = new int[4];
        switch (machinegun)
        {
            case "mg_ak":
                stats[0] = 0; //Ammo requirement
                stats[1] = 0; //Damage
                stats[2] = 0; //Gold (money)
                stats[3] = 1; //Has ability (true && false)
                break;
            case "mg_mp5":
                stats[0] = -30;
                stats[1] = 20;
                stats[2] = 0;
                stats[3] = 1;
                break;
            case "mg_mp5plus":
                stats[0] = -70;
                stats[1] = 40;
                stats[2] = 0;
                stats[3] = 1;
                break;
            case "mg_scorpion":
                stats[0] = -40;
                stats[1] = 20;
                stats[2] = 0;
                stats[3] = 0;
                break;
            case "mg_sig":
                stats[0] = 0;
                stats[1] = 0;
                stats[2] = 0;
                stats[3] = 1;
                break;
            case "mg_thompson":
                stats[0] = -50;
                stats[1] = 50;
                stats[2] = 0;
                stats[3] = 1;
                break;
            case "mg_tmp":
                stats[0] = -30;
                stats[1] = 20;
                stats[2] = 20; //Effect: +20 gold
                stats[3] = 0;
                break;
        }
        return stats;
    }

    public static int[] GetBigGun(string biggun)
    {
        int[] stats = new int[4];
        switch (biggun)
        {
            case "bg_flamethrower":
                stats[0] = 0; //Ammo requirement
                stats[1] = 0; //Damage
                stats[2] = 0; //Gold (money)
                stats[3] = 1; //Has ability (true && false)
                break;
            case "bg_gatlinggun":
                stats[0] = 0;
                stats[1] = 0;
                stats[2] = 0;
                stats[3] = 1;
                break;
            case "bg_grenadelauncher":
                stats[0] = 0;
                stats[1] = 20;
                stats[2] = 0;
                stats[3] = 1;
                break;
            case "bg_minethrower":
                stats[0] = 0;
                stats[1] = 0;
                stats[2] = 0;
                stats[3] = 1;
                break;
            case "bg_rp7night":
                stats[0] = 0;
                stats[1] = 60;
                stats[2] = 0;
                stats[3] = 1;
                break;
            case "bg_rpg7":
                stats[0] = 0;
                stats[1] = 90;
                stats[2] = 0;
                stats[3] = 1;
                break;
        }
        return stats;
    }

}
