using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public static class GameStats
{
    public static int MansionDeckValue { get; set; }
    public static int MansionDeckCount { get; set; }
    public static int CharacterDeckValue { get; set; } //0 = Normal, 1 = Custom

    public static int myPlayerID { get; set; }
    public static string myCharacterCard { get; set; }

    public static Hashtable playerInfos { get; set; }
}


