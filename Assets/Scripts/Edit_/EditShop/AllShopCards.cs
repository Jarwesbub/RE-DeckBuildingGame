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
		//"sup_stunrod"
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
		"mg_ak",
		"mg_mp5",
		"mg_mp5plus",
		"mg_scorpion",
		"mg_sig",
		"mg_thompson",
		"mg_tmp",
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
		//HOX! If you edit this string size -> remember to edit text file: ShopData.txt to match the row count!
		//21 length = 010101010101010101010101010101010101010101
		string[] explosives = new string[21]
		{
			"ac-001_premier_mansion_foyer1",
			"ac-002_premier_deadly_aim1",
			//"ac-003_premier_shattered_memories1",
			"ac-004_premier_escape_from_dead_city",
			//"ac-005_premier_reload1",
			"ac-006_premier_the_merchant1",
			//"ac-007_premier_umbrella_corporation1",
			"ac-008_premier_back_to_back1",
			"ac-009_premier_item_management1",
			"ac-010_premier_ominous_battle1",
			//"ac-011_premier_master_of_unlocking1",
			"ac-012_premier_struggle_for_survival1",
			"ac-013_alliance_partners_partner_mod",
			"ac-014_alliance_star-crossed_duo_partner_mod",
			"ac-015_alliance_great_ambition_partner_mod",
			//"ac-016_alliance_archrival_partner_mod",
			"ac-017_alliance_fierce_battle1",
			//"ac-018_alliance_uroboros_injection1",
			"ac-019_alliance_quirk_of_fate1",
			//"ac-020_alliance_cornered_partner_mod",
			//"ac-021_alliance_gathering_forces1",
			"ac-022_alliance_desperate_escape1",
			//"ac-023_outbreak_power_of_the_t-virus_infection_mod",
			"ac-024_outbreak_i_have_this",
			//"ac-025_outbreak_weskers_secret_infection_mod",
			//"ac-026_outbreak_injection_infection_mod",
			//"ac-027_outbreak_by_any_means_necessary_infection_mod",
			"ac-028_outbreak_higher_priorities",
			"ac-029_outbreak_parting_ways",
			//"ac-030_outbreak_returned_favor",
			//"ac-031_outbreak_the_gathering_darkness",
			"ac-032_nightmare_lonewolf",
			"ac-033_nightmare_high_value_targets",
			//"ac-034_nightmare_raccoon_city_police_department",
			//"ac-035_nightmare_pda",
			"ac-036_nightmare_toe_to_toe",
			//"ac-037_nightmare_a_gift",
			//"ac-038_nightmare_mind_control",
			//"ac-039_nightmare_long_awaited_dawn",
			"ac-040_nightmare_vengeful_intention",
			//"ac-041_nightmare_symbol_of_evil"
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
		"mg_ak",
		"mg_mp5",
		"mg_mp5plus",
		"mg_scorpion",
		"mg_sig",
		"mg_thompson",
		"mg_tmp",
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
