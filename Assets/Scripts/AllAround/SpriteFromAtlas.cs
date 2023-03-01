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
    [SerializeField] private string spriteName;
    private Image image;
    [SerializeField] private bool isVisible, isCharacterCard, isHandCard, isMansionCard;
    public int characterCardNumber;

    void Awake()
    {
        image = GetComponent<Image>();
        SetHandCardSpriteVisibility(isVisible);
    }

    void Start()
    {
        if (!isCharacterCard && isHandCard) //Player hand card
        {
            spriteName = GetComponent<HandCard>().currentCard;
            GetCurrentSpriteFromAtlas(spriteName);
        }
    }
    public string GetSpriteName()
    {
        return spriteName;
    }
    private void GetCurrentSpriteFromAtlas(string name)
    {
            int count = atlas.Count;

            for (int i = 0; i < count; i++)
            {
                image.sprite = atlas[i].GetSprite(name);
                currentAtlas = i;
                if (image.sprite != null)
                    break;
            }
        //spriteName = name;
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

        if (image.sprite == null)
            GetCurrentSpriteFromAtlas(spriteName);
    }

    public void SetMansionCardSprite(string name) //MANSION DECK and EditMansion.cs
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
