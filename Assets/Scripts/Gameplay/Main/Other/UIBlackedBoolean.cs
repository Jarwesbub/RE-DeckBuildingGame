using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBlackedBoolean : MonoBehaviour
{
    public GameObject MainCanvas, UIControl;
    private GameObject cardObject;
    private int ammo, dmg;
    public void UISetActive(string text, GameObject card)
    {
        cardObject = card;
        GetComponent<UIBlackedMain>().SetMainInfoText(text);
        int[] extraStats = CheckCardExtraStats(card.GetComponent<HandCard>().currentCard);
        ammo = extraStats[0];
        dmg = extraStats[1];
        gameObject.SetActive(true);
    }
    private int[] CheckCardExtraStats(string name)
    {
        int[] values = new int[2];

        switch (name)
        {
            case "hg_pistol3":  //You can give this Weapon+10 Damage during this turn. In that case, Trash this Weapon at the end of this turn.
                values[0] = 0; //Extra Ammo requirement
                values[1] = 10; //Extra Damage
                break;
            case "hg_usp":  //You can pay 20 Ammo. In that case, this Weapon gets +10 Damage during this turn.
                values[0] = -20;
                values[1] = 10;
                break;

        }

        return values;

    }

    public void UISetInActive()
    {
        GetComponent<UIBlackedMain>().SetMainInfoText("");
        gameObject.SetActive(false);
    }

    public void OnClickChooseYes()
    {
        UIControl.GetComponent<DeleteCardsControl>().AddToEndTurnTrashList(cardObject);
        MainCanvas.GetComponent<HandCardStatsUI>().AddToWeaponStats(ammo, dmg);
        UISetInActive();
    }
    public void OnClickChooseNo()
    {

        UISetInActive();
    }

    public void OnClickExit()
    {
        cardObject = null;
        UISetInActive();
    }

}
