using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public static class GameStats
{
    public static int MansionDeckValue { get; set; }
    public static int MansionDeckCount { get; set; }

    public static int ShopDeckDataValue { get; set; }
    public static int ShopDeckDataCount { get; set; }

    public static int CharacterDeckValue { get; set; }
    public static int CharacterDeckCount { get; set; }
    public static int CharacterDeckType { get; set; } //0 = Normal, 1 = Custom


    public static int CurrentPlayerID { get; set; }

    public static Hashtable PlayerInfos { get; set; }

}


