using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllShopCards : MonoBehaviour
{
	public string[] GetShopCardByCount(int[] shopDataCounts, int shopDataIndex)
    {
		string[] arrayCardNames = new string[0];
		List<string> arrayFinal = new();

		Debug.Log("Index: " + shopDataIndex + " length: " + shopDataCounts.Length);

		switch (shopDataIndex)
		{
			case 1:
				arrayCardNames = GetHealthItems();
				break;
			case 2:
				arrayCardNames = GetKnifeCards();
				break;
			case 3:
				arrayCardNames = GetHandguns();
				break;
			case 4:
				arrayCardNames = GetShotguns();
				break;
			case 5:
				arrayCardNames = GetMachineguns();
				break;
			case 6:
				arrayCardNames = GetRifles();
				break;
			case 7:
				arrayCardNames = GetExplosives();
				break;
			case 8: case 9: case 10: case 11:case 12: case 13:case 14:
				arrayCardNames = GetActionCards();
				break;
			case 15:
				arrayCardNames = GetAllNonActionCards(); //Extra1
				break;
			case 16:
				arrayCardNames = GetAllNonActionCards(); //Extra2
				break;

		}


		if (shopDataIndex == 0) //Is ammo cards
		{
			int count = shopDataCounts.Length;

			for (int i = 0; i < count; i++)
			{
				//arrayFinal[i] = shopDataCounts[i].ToString();
				arrayFinal.Add(shopDataCounts[i].ToString());
			}

		}
		else
		{
			int index = 0;
			foreach(int value in shopDataCounts)
            {
				string[] namesArray = CountCardValues(arrayCardNames[index], shopDataCounts[index]);
				arrayFinal.AddRange(namesArray);
				index++;
            }
			//arrayFinal = AddCardNamesByCountArray(arrayCardNames, shopDataCounts);
		}

		return arrayFinal.ToArray();

	}
	private string[] CountCardValues(string cardName, int count)
    {
		string[] namesList = new string[count];
        for (int i = 0; i < count; i++)
        {
			namesList[i] = cardName;
        }
		return namesList;
    }
/*
	private string[] AddCardNamesByCountArray(string[] arrayNames, int[] shopDataCounts)
    {
		List<string> cards = new();
		Debug.Log("arrayNames.length: " + arrayNames.Length + " shopDataCounts.length: " + shopDataCounts.Length);

		int index = 0;
		foreach (int value in shopDataCounts)
		{
			for (int i = 0; i < value; i++)
			{
				cards.Add(arrayNames[index]);
				Debug.Log(arrayNames[index] + " added");
			}

			index++;
		}
		return cards.ToArray();
	}*/
	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	public string[] GetAmmoCards()//3 (6)
	{
		string[] ammo = new string[3]
		{
		"ia_ammo10",
		"ia_ammo20",
		"ia_ammo30"
		/*"ia_ammo10_mercenaries",
		"ia_ammo20_mercenaries",
		"ia_ammo30_mercenaries"*/
		};
		return ammo;
	}

	public string[] GetHealthItems()//3
	{
		string[] items = new string[3]
		{
		"ia_firstaidkit2",
		"ia_greenherb",
		"ia_redherb"
		};
		return items;
	}
	public string[] GetKnifeCards()//5
	{
		string[] knives = new string[5]
		{
		"sup_knife1",
		"sup_knife2",
		"sup_knife3",
		"sup_bow1",
		"sup_bow2"
		};
		return knives;
	}

	public string[] GetHandguns()//13
	{
		string[] handguns = new string[13]
		{
		"hg_brokenbutterfly",
		"hg_lighninghawk",
		"hg_m9",
		"hg_m93r",
		"hg_magnum1",
		"hg_magnum2",
		"hg_pistol1",
		"hg_pistol2",
		"hg_pistol3",
		"hg_polizei",
		"hg_punisher",
		"hg_samuraiedge",
		"hg_usp"
		};
		return handguns;
	}

	public string[] GetShotguns()//5
	{
		string[] shotguns = new string[5]
		{
		"ras_automaticshotgun",
		"ras_hydra",
		"ras_m3",
		"ras_shotgun1",
		"ras_shotgun2"
		};
		return shotguns;
	}

	public string[] GetMachineguns()//9
	{
		string[] machineguns = new string[9]
		{
		"arsg_ak",
		"arsg_mp5",
		"arsg_mp5plus",
		"arsg_scorpion",
		"arsg_sig",
		"arsg_thompson",
		"arsg_tmp",
		"bg_flamethrower",
		"bg_gatlinggun"
		};
		return machineguns;
	}

	public string[] GetRifles()//6
	{
		string[] rifles = new string[6]
		{
		"ras_psg1",
		"ras_rifle1",
		"ras_rifle2",
		"ras_rifle3",
		"ras_rifle4",
		"ras_svd"
		};
		return rifles;
	}
	public string[] GetExplosives() //6
	{
		string[] explosives = new string[6]
		{
		"sup_flash2",
		"sup_grenade2",
		"sup_grenade3",
		"bg_grenadelauncher",
		"bg_minethrower",
		"bg_rp7night"
		};
		return explosives;
	}
	public string[] GetActionCards() //24   NOT ALL ACTION CARDS ARE LISTED -> FIX THIS LATER! -> Update "Action" SpriteAtlas too
	{
		string[] explosives = new string[24]
		{
		"_ac01mansionfoyer",
		"_ac02deadlyaim",
		"_ac03shatteredmemories",
		"_ac04escapefromdeadcity",
		"_ac05reload",
		"_ac06themerchant",
		"_ac07umbrellacorporation",
		"_ac08backtoback",
		"_ac10omniousbattle",
		"_ac11masterofunlocking",
		"_ac12struggletosurvival",
		"_ac13partners",
		"_ac14starcrossedduo",
		"_ac15greatambition",
		"_ac16archrival",
		"_ac17fiercebattle",
		"_ac18uroborosinjection",
		"_ac19quirkoffate",
		"_ac22desperateescape",
		"_ac28higherpriorities",
		"_ac33highvaluetargets",
		"_ac34raccooncitypolicedepartment",
		"_ac36toetotoe",
		"_ac40vengefulintentions"
		};
		return explosives;
	}
	public string[] GetAllNonActionCards() //45
    {
		string[] allCards = new string[45]
		{
		"sup_knife1",
		"sup_knife2",
		"sup_knife3",
		"sup_bow1",
		"sup_bow2",
		"ia_custom_jill_sandwich",
		"hg_brokenbutterfly",
		"hg_lighninghawk",
		"hg_m9",
		"hg_m93r",
		"hg_magnum1",
		"hg_magnum2",
		"hg_pistol1",
		"hg_pistol2",
		"hg_pistol3",
		"hg_polizei",
		"hg_punisher",
		"hg_samuraiedge",
		"hg_usp",
		"ras_automaticshotgun",
		"ras_hydra",
		"ras_m3",
		"ras_shotgun1",
		"ras_shotgun2",
		"arsg_ak",
		"arsg_mp5",
		"arsg_mp5plus",
		"arsg_scorpion",
		"arsg_sig",
		"arsg_thompson",
		"arsg_tmp",
		"bg_flamethrower",
		"bg_gatlinggun",
		"ras_psg1",
		"ras_rifle1",
		"ras_rifle2",
		"ras_rifle3",
		"ras_rifle4",
		"ras_svd",
		"sup_flash2",
		"sup_grenade2",
		"sup_grenade3",
		"bg_grenadelauncher",
		"bg_minethrower",
		"bg_rp7night"
		};
		return allCards;


	}
}
