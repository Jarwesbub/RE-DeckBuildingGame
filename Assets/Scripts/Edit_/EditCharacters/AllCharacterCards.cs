using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Newest version
public class CharacterCardsList
{
	public string[] GetSupportedCharacterCardsByType(int type) // 0 = ORIGINAL , 1 = CUSTOM
    {
		if (type == 1)
			return GetAllCustomCharacterCards();
		else
			return GetSupportedCharacterCards();

	}

	public string[] GetAllNormalCharacterCards()
	{
		return GetAllCharacterCards();
	}


    private string[] GetSupportedCharacterCards()
    {
		string[] normalCharacters = new string[26]
		{
		"ch-001_premier_albert_wesker1",
		"ch-002_premier_leon_s_kennedy1",
		"ch-003_premier_claire_redfield1",
		"ch-005_premier_barry_burton1",
		"ch-006_premier_ada_wong1",
		"ch-007_premier_jack_krauser1",
		"ch-008_premier_chris_redfield1",
		"ch-009_premier_jill_valentine1",
		"ch-019_alliance_chris_redfield1",
		"ch-020_alliance_claire_redfield1",
		"ch-024_outbreak_mark_wilkins",
		"ch-027_outbreak_ada_wong",
		"ch-028_outbreak_chris_redfield",
		"ch-029_outbreak_rebecca_chambers_infection_mod",
		"ch-030_outbreak_hunk",
		"ch-034_nightmare_chris_redfield",
		"ch-035_nightmare_sergei_vladimir",
		"ch-036_nightmare_luis_sera",
		"ch-038_nightmare_mikhail_victor",
		"ch-039_nightmare_carlos_oliveira",
		"ch-041_nightmare_albert_wesker",
		"ch-045_mercenaries_hunk",
		"ch-046_mercenaries_claire_redfield",
		"ch-047_mercenaries_chris_redfield",
		"ch-100_rare_promo_cloak_hunk",
		"ch-101_rare_promo_nurse_rebecca"
		};
		string[] customCharacters = GetAllCustomCharacterCards();

        return normalCharacters.Concat(customCharacters).ToArray(); // Combines normal and custom character arrays

    }

	private string[] GetAllCharacterCards()
	{
        string[] characters = new string[47]
		{
        "ch-001_premier_albert_wesker1",
        "ch-002_premier_leon_s_kennedy1",
        "ch-003_premier_claire_redfield1",
        "ch-004_premier_sheva_alomar1",
        "ch-005_premier_barry_burton1",
        "ch-006_premier_ada_wong1",
        "ch-007_premier_jack_krauser1",
        "ch-008_premier_chris_redfield1",
        "ch-009_premier_jill_valentine1",
        "ch-011_alliance_carlos_oliveira1",
        "ch-012_alliance_josh_stone_partner_mod",
        "ch-013_alliance_steve_burnside1",
        "ch-014_alliance_jack_krauser1",
        "ch-015_alliance_hunk_partner_mod",
        "ch-016_alliance_jill_valentine_partner_mod",
        "ch-017_alliance_billy_coen1",
        "ch-018_alliance_leon_s_kennedy1",
        "ch-019_alliance_chris_redfield1",
        "ch-020_alliance_claire_redfield1",
        "ch-021_outbreak_excella_gionne_infection_mod",
        "ch-022_outbreak_kevin_ryman",
        "ch-023_outbreak_david_king",
        "ch-024_outbreak_mark_wilkins",
        "ch-025_outbreak_jill_valentine_infection_mod",
        "ch-026_outbreak_leon_s_kennedy",
        "ch-027_outbreak_ada_wong",
        "ch-028_outbreak_chris_redfield",
        "ch-029_outbreak_rebecca_chambers_infection_mod",
        "ch-030_outbreak_hunk",
        "ch-034_nightmare_chris_redfield",
        "ch-035_nightmare_sergei_vladimir",
        "ch-036_nightmare_luis_sera",
        "ch-037_nightmare_josh_stone",
        "ch-038_nightmare_mikhail_victor",
        "ch-039_nightmare_carlos_oliveira",
        "ch-040_nightmare_mysterious_mask",
        "ch-041_nightmare_albert_wesker",
        "ch-042_nightmare_leon_s_kennedy",
        "ch-043_nightmare_ada_wong",
        "ch-044_mercenaries_jack_krauser",
        "ch-045_mercenaries_hunk",
        "ch-046_mercenaries_claire_redfield",
        "ch-047_mercenaries_chris_redfield",
        "ch-048_mercenaries_jill_valentine",
        "ch-049_mercenaries_albert_wesker",
        "ch-100_rare_promo_cloak_hunk",
        "ch-101_rare_promo_nurse_rebecca"
		};
        return characters;
    }

	public bool CheckIfCardIsSupported(string cardName)
	{
		string[] supportedCards = GetSupportedCharacterCards();
		if (supportedCards.Contains(cardName))
		{
			return true;
		}
		return false;
	}

	public string[] GetAllCustomCharacterCards()
    {
		string[] customCharacters = new string[37]
		{
		"ch-201_ada_wong_2_by_huangzhenyanghz_d4mapjs",
		"ch-202_ada_wong_3_by_huangzhenyanghz_d4marpd",
		"ch-203_albert_wesker_by_huangzhenyanghz_d4me4s0",
		"ch-204_allyson_by_huangzhenyanghz_d4mapwh",
		"ch-205_ashley_graham_2_by_huangzhenyanghz_d4mark4",
		"ch-206_ashley_graham_by_huangzhenyanghz_d4marc5",
		"ch-207_chris_redfield_2_by_huangzhenyanghz_d4masoz",
		"ch-208_chris_redfield_by_huangzhenyanghz_d4masio",
		"ch-209_claire_redfield_2_by_huangzhenyanghz_d4mathd",
		"ch-210_fiona_belli_by_huangzhenyanghz_d4mauew",
		"ch-211_fong_ling_by_huangzhenyanghz_d4mauly",
		"ch-212_heather_mason_by_huangzhenyanghz_d4mb7bv",
		"ch-213_hunk_by_huangzhenyanghz_d4mb7m8",
		"ch-214_injured_ada_by_huangzhenyanghz_d4m3lsz",
		"ch-215_jack_krauser_by_huangzhenyanghz_d4mbd96",
		"ch-216_jill_valentine_2_by_huangzhenyanghz_d4mb8dl",
		"ch-217_jill_valentine_3_by_huangzhenyanghz_d4mb8nz",
		"ch-218_jill_valentine_4_by_huangzhenyanghz_d4mb98c",
		"ch-219_jill_valentine_7_by_huangzhenyanghz_d4mba2r",
		"ch-220_jill_valentine_bsaa_by_huangzhenyanghz_d4masam",
		"ch-221_jill_valentine_revelations_2_by_huangzhenyanghz_d4me90u",
		"ch-222_injured_ada_by_huangzhenyanghz_d4m3lsz",
		"ch-223_jill_valentine_6_by_huangzhenyanghz_d4mb9u5",
		"ch-224_kevin_ryman_by_huangzhenyanghz_d4mbciy",
		"ch-225_lady_by_huangzhenyanghz_d4mbdjv",
		"ch-226_leon_scott_kennedy_2_by_huangzhenyanghz_d4mbksg",
		"ch-227_leon_scott_kennedy_4_by_huangzhenyanghz_d4mbl7g",
		"ch-228_leon_scott_kennedy_by_huangzhenyanghz_d4mbki3",
		"ch-229_luis_sera_by_huangzhenyanghz_d4mbnby",
		"ch-230_nicholai_ginovaef_by_huangzhenyanghz_d4me8bd",
		"ch-231_rebecca_chambers_2_by_huangzhenyanghz_d4mbpjr",
		"ch-232_rebecca_chambers_3_by_huangzhenyanghz_d4mbpsr",
		"ch-233_regina_by_huangzhenyanghz_d4mbqlq",
		"ch-234_richard_aiken_by_huangzhenyanghz_d4mbqzg",
		"ch-235_sheva_alomar_2_by_huangzhenyanghz_d4mbuy3",
		"ch-236_sheva_alomar_by_huangzhenyanghz_d4matz8",
		"ch-237_steve_burnside_by_huangzhenyanghz_d4me46u"
		};
		return customCharacters;
    }

}
