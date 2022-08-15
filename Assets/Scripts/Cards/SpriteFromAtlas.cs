using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SpriteFromAtlas : MonoBehaviour
{
    public GameObject MainCanvas;
    [SerializeField] List<SpriteAtlas> atlas;
    public int currentAtlas;
    public string spriteName, characterName;
    private Image image;
    [SerializeField] private bool isVisible, isCharacterCard, isHandCard, isMansionCard;
    public int characterCardNumber;

    void Awake()
    {
        image = GetComponent<Image>();
        SetHandCardSpriteVisibility(isVisible);
        //SetHandCardSpriteVisibility(isVisible);
    }




    void Start()
    {
        if(!isCharacterCard && isHandCard) //Player hand card
        {
            //string getSprite = SpawnCardsPrefab.GetComponent<SpawnCards>().currentCard; //Get card sprite name!
            string getSprite = GetComponent<HandCard>().currentCard;
            spriteName = getSprite;
            int count = atlas.Count;

            for (int i = 0; i < count; i++)
            {
                image.sprite = atlas[i].GetSprite(spriteName);
                currentAtlas = i;
                if (image.sprite != null)
                    break;
            }
        }
    }
    public void CharacterCardStart()
    {
        if (isCharacterCard)
        {
            if (image.sprite == null)
            {
                currentAtlas = 1;
                image.sprite = atlas[currentAtlas].GetSprite(spriteName);

                if (image.sprite == null) //Bug hotfix: If +4 players -> someones spritename get messed up (multiple names in a row))
                {
                    spriteName = "ch-002_premier_leon_s_kennedy1";
                    Debug.Log("Sprite image name was null!");
                    SetHandCardSpriteVisibility(true);
                }
            }
        }
    }


    public void GetLM_StarterHandCards(string getSprite) // LM_GetStarterHandCards.cs
    {
        spriteName = getSprite;
        int count = atlas.Count;

        for (int i = 0; i < count; i++)
        {
            image.sprite = atlas[i].GetSprite(spriteName);
            currentAtlas = i;
            if (image.sprite != null)
                break;
        }
        ChangeCardSprite(getSprite);
    }
    
    public void ChangeCardSprite(string name) //CharacterControl.cs
    {
        spriteName = name;
        SetHandCardSpriteVisibility(true);

    }

    public void SetHandCardSpriteVisibility(bool show)
    {
        if (show)
        {
            image.sprite = atlas[currentAtlas].GetSprite(spriteName);
            image.color = new Color(255, 255, 255, 255);
        }
        else
            image.color = new Color(0, 0, 0, 255);
    }

    public void SetMansionCardSprite(string name) //MANSION DECK
    {
        if (isMansionCard)
        {
            image.color = new Color(255, 255, 255, 255);
            spriteName = name;

            int count = atlas.Count;

            for (int i = 0; i < count; i++)
            {
                image.sprite = atlas[i].GetSprite(spriteName);
                currentAtlas = i;
                if (image.sprite != null)
                    break;
            }

            image.sprite = atlas[currentAtlas].GetSprite(name);
        }
    }

}
