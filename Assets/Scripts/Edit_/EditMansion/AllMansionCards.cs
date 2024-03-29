using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllMansionCards : MonoBehaviour
{
    public string[] GetLowTierMansionCards()
	{
		string[] low = new string[22] {
			"0-ma-001_premier_majini1",
			"0-ma-002_premier_zombie_male1",
			"0-ma-003_premier_zombie_female1",
			"0-ma-004_premier_zombie_butcher1",
			"0-ma-005_premier_bui_kichwa1",
			"0-ma-011_premier_dr_salvador1",
			"0-ma-015_premier_cerberus1",
			"0-ma-022_alliance_lurker1",
			"0-ma-023_alliance_infected_bat1",
			"0-ma-027_alliance_zombie_male1",
			"0-ma-044_outbreak_grave_zombie",
			"0-ma-045_outbreak_wasp",
			"0-ma-051_outbreak_zombie_cop",
			"0-ma-059_nightmare_militia_flail",
			"0-ma-060_nightmare_militia_stun_rod",
			"0-ma-062_nightmare_novistador",
			"0-ma-063_nightmare_zombie_security_guard",
			"0-ma-066_nightmare_ivy",
			"0-ma-073_mercenaries_town_majini",
			"0-ma-077_mercenaries_chicken",
			"0-ma-087_mercenaries_venomous_snake",
			"0-ma-093_mercenaries_los_ganados_handscythe"
		};
		return low;
	}

	public string[] GetMidTierMansionCards()
	{
		string[] mid = new string[22] {
		"1_ma-009_premier_mimicry_marcus1",
		"1_ma-017_premier_executioner1",
		"1_ma-024_alliance_licker_beta1",
		"1_ma-028_alliance_kipepeo_las_plagas1",
		"1_ma-030_alliance_cephalo_las_plagas1",
		"1_ma-033_alliance_zombie_horde1",
		"1_ma-035_alliance_los_illuminados_monk1",
		"1_ma-037_alliance_guardian_of_insanity1",
		"1_ma-048_outbreak_crimson_head",
		"1_ma-049_outbreak_reinforced_licker",
		"1_ma-050_outbreak_grave_digger",
		"1_ma-052_outbreak_eliminator",
		"1_ma-058_nightmare_colmillos",
		"1_ma-064_nightmare_zombie_technician",
		"1_ma-067_nightmare_neptune",
		"1_ma-069_nightmare_cerberus",
		"1_ma-074_mercenaries_town_majini_shovel",
		"1_ma-081_mercenaries_los_illuminados_shield",
		"1_ma-090_mercenaries_tribal_majini_shield",
		"1_ma-095_mercenaries_los_ganados",
		"1_ma-096_mercenaries_los_illuminados_bow",
		"1_ma-097_mercenaries_militia_bow",
		};
		return mid;
	}

	public string[] GetTopTierMansionCards()
	{
		string[] top = new string[18] {
		"2_ma-006_premier_licker1",
		"2_ma-007_premier_nemesis_t-type1",
		"2_ma-010_premier_uroboros_aheri1",
		"2_ma-014_premier_gatling_gun_majini1",
		"2_ma-016_premier_el_gigante1",
		"2_ma-025_alliance_duvalia_las_plagas1",
		"2_ma-026_alliance_garrador1",
		"2_ma-029_alliance_albert_wesker1",
		"2_ma-031_alliance_proto_tyrant1",
		"2_ma-032_alliance_iron_maiden1",
		"2_ma-034_alliance_nosferatu1",
		"2_ma-041_outbreak_tyrant_t-002_infection_mod",
		"2_ma-054_nightmare_osmund_saddler",
		"2_ma-056_nightmare_regenerator",
		"2_ma-057_nightmare_jack_krauser",
		"2_ma-065_nightmare_verdugo",
		"2_ma-068_nightmare_hunter",
		"2_ma-079_mercenaries_big_man_majini",
		};
		return top;
	}

	public string[] GetItemMansionCards()
    {
        string[] items = new string[7] {
		"3_ma-038_alliance_explosive_barrel1",
		"3_ma-054_outbreak_kevlar_jacket",
		"3_ma-055_outbreak_antivirus_infection_mod",
		"3_ma-056_outbreak_laser_trap",
		"3_ma-057_outbreak_rock_trap",
		"3_ma-070_nightmare_treasure_map",
		"3_ma-071_nightmare_hidden_treasure"
		};
		return items;
    }

	public string[] GetAllMansionCards()
	{
		string[] allCards = new string[69] {
			"0-ma-001_premier_majini1",
			"0-ma-002_premier_zombie_male1",
			"0-ma-003_premier_zombie_female1",
			"0-ma-004_premier_zombie_butcher1",
			"0-ma-005_premier_bui_kichwa1",
			"0-ma-011_premier_dr_salvador1",
			"0-ma-015_premier_cerberus1",
			"0-ma-022_alliance_lurker1",
			"0-ma-023_alliance_infected_bat1",
			"0-ma-027_alliance_zombie_male1",
			"0-ma-044_outbreak_grave_zombie",
			"0-ma-045_outbreak_wasp",
			"0-ma-051_outbreak_zombie_cop",
			"0-ma-059_nightmare_militia_flail",
			"0-ma-060_nightmare_militia_stun_rod",
			"0-ma-062_nightmare_novistador",
			"0-ma-063_nightmare_zombie_security_guard",
			"0-ma-066_nightmare_ivy",
			"0-ma-073_mercenaries_town_majini",
			"0-ma-077_mercenaries_chicken",
			"0-ma-087_mercenaries_venomous_snake",
			"0-ma-093_mercenaries_los_ganados_handscythe",
			"1_ma-009_premier_mimicry_marcus1",
			"1_ma-017_premier_executioner1",
			"1_ma-024_alliance_licker_beta1",
			"1_ma-028_alliance_kipepeo_las_plagas1",
			"1_ma-030_alliance_cephalo_las_plagas1",
			"1_ma-033_alliance_zombie_horde1",
			"1_ma-035_alliance_los_illuminados_monk1",
			"1_ma-037_alliance_guardian_of_insanity1",
			"1_ma-048_outbreak_crimson_head",
			"1_ma-049_outbreak_reinforced_licker",
			"1_ma-050_outbreak_grave_digger",
			"1_ma-052_outbreak_eliminator",
			"1_ma-058_nightmare_colmillos",
			"1_ma-064_nightmare_zombie_technician",
			"1_ma-067_nightmare_neptune",
			"1_ma-069_nightmare_cerberus",
			"1_ma-074_mercenaries_town_majini_shovel",
			"1_ma-081_mercenaries_los_illuminados_shield",
			"1_ma-090_mercenaries_tribal_majini_shield",
			"1_ma-095_mercenaries_los_ganados",
			"1_ma-096_mercenaries_los_illuminados_bow",
			"1_ma-097_mercenaries_militia_bow",
			"2_ma-006_premier_licker1",
			"2_ma-007_premier_nemesis_t-type1",
			"2_ma-010_premier_uroboros_aheri1",
			"2_ma-014_premier_gatling_gun_majini1",
			"2_ma-016_premier_el_gigante1",
			"2_ma-025_alliance_duvalia_las_plagas1",
			"2_ma-026_alliance_garrador1",
			"2_ma-029_alliance_albert_wesker1",
			"2_ma-031_alliance_proto_tyrant1",
			"2_ma-032_alliance_iron_maiden1",
			"2_ma-034_alliance_nosferatu1",
			"2_ma-041_outbreak_tyrant_t-002_infection_mod",
			"2_ma-054_nightmare_osmund_saddler",
			"2_ma-056_nightmare_regenerator",
			"2_ma-057_nightmare_jack_krauser",
			"2_ma-065_nightmare_verdugo",
			"2_ma-068_nightmare_hunter",
			"2_ma-079_mercenaries_big_man_majini",
			"3_ma-038_alliance_explosive_barrel1",
			"3_ma-054_outbreak_kevlar_jacket",
			"3_ma-055_outbreak_antivirus_infection_mod",
			"3_ma-056_outbreak_laser_trap",
			"3_ma-057_outbreak_rock_trap",
			"3_ma-070_nightmare_treasure_map",
			"3_ma-071_nightmare_hidden_treasure",
		};
		return allCards;
	}
}

/*

Currently unsupported Mansion cards:

LOW TIER
0-ma-047_outbreak_reinforced_zombie
0-ma-076_mercenaries_base_majini_gun_skill_mod
0-ma-083_mercenaries_los_ganados_male
0-ma-084_mercenaries_los_ganados_female
0-ma-088_mercenaries_tribal_majini_bow
0-ma-089_mercenaries_tribal_majini_oil_pot

MID TIER
1_ma-008_premier_hunter1
1_ma-036_alliance_reaper_partner_mod
1_ma-046_outbreak_web_spinner_infection_mod
1_ma-061_nightmare_militia_rocket_launcher
1_ma-085_mercenaries_adjule_skill_mod
1_ma-086_mercenaries_los_illuminados_scythe
1_ma-092_mercenaries_town_majini_motorcycle
1_ma-094_mercenaries_los_ganados_pitchfork
1_ma-098_mercenaries_base_majini_shield

TOP TIER
2_ma-042_outbreak_yawn_infection_mod
2_ma-043_outbreak_nemesis_2nd_from_infection_mod
2_ma-053_outbreak_lisa_trevor_infection_mod
2_ma-055_nightmare_bitores_mendez
2_ma-075_mercenaries_giant_majini
2_ma-078_mercenaries_red_executioner
2_ma-080_mercenaries_chainsaw_majini
2_ma-082_mercenaries_jj
2_ma-091_mercenaries_crocodile

ITEMS
3_it-002_premier_yellow_herb1
3_ma-012_premier_rocket_launcher_case1
3_ma-013_premier_gatling_gun_case1
3_ma-018_premier_time_bonus_01
3_ma-019_premier_time_bonus_02
3_ma-020_premier_time_bonus_03
3_ma-021_premier_combo_bonus1
3_ma-039_alliance_collapsing_floor_traps1
3_ma-040_alliance_laser_targeting_device1
3_ma-072_nightmare_prl_412

*/