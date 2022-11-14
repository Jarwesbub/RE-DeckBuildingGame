using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LM_GetStarterHandCards : MonoBehaviour
{
    //This script gets starter hand cards from text file
    //When Start function is done -> ShopControl takes the control when new card is bought

    public GameObject LeftMenuControl, HandCardPrefab, HandCardsGridContent;
    [SerializeField]private List<string> sDeck;

    void Start()
    {
        int count = GetComponent<TextFileToList>().GetTextListCount();

        for (int i = 0; i < count; i++)
        {
            string name = GetComponent<TextFileToList>().GetStringFromTextByNumber(i);
            sDeck.Add(name);
        }

        for (int i = 0; i < count; i++)
        {
            //GetComponent<SpriteFromAtlas>().ChangeCardSprite(sDeck[i]);
            GetComponent<SpriteFromAtlas>().GetLM_StarterHandCards(sDeck[i]);
            GameObject handCard = Instantiate(HandCardPrefab);
            Sprite sprt = GetComponent<Image>().sprite;
            handCard.GetComponent<Image>().sprite = GetComponent<Image>().sprite;
            //handCard.transform.parent = HandCardsGridContent.transform;
            handCard.transform.SetParent(HandCardsGridContent.transform);
            handCard.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        LeftMenuControl.GetComponent<LeftMenuControl>().GetStarterHandCards();
        Destroy(gameObject);
    }

}
