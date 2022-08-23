using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
//using Photon.Realtime;

public class SpawnCards : MonoBehaviourPun
{
    public int handCards, cardCount;
    public GameObject PNCardPrefab, CharacterCardPrefab;
    private GameObject HandCardsParent, MainCanvas;
    [SerializeField] private TMP_Text deckCountTxt, discardpileCountTxt;
    public List<string> sDeck;
    public List<string> sHandCards;
    public List<string> sDiscardPileCards;
    public string currentCard, characterCard;
    float startPosX = -3f, startPosY = -1.8f;
    float posX, posY;

    void Awake()
    {
        int count = GetComponent<TextFileToList>().textListCount;

        for (int i = 0; i < count; i++)
        {
            string name = GetComponent<TextFileToList>().GetStringFromTextByNumber(i);
            sDeck.Add(name);
            cardCount++;
        }

        HandCardsParent = GameObject.FindWithTag("HandCardsParent");
        MainCanvas = GameObject.FindWithTag("MainCanvas");

        GameObject parentDeckCounter;
        parentDeckCounter = GameObject.FindWithTag("DeckCardsCounter");
        deckCountTxt = parentDeckCounter.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        discardpileCountTxt = parentDeckCounter.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        SetDeckCountText();

        posX = startPosX; posY = startPosY;
        ShuffleDeck(false);
    }
    public void DrawCard() //Button
    {
        if (sDeck.Count >= 1)
        {
            handCards++;
            sHandCards.Add(sDeck[0]);

            Vector2 pos;
            if(posX>6f&& posY== startPosY) //Change row
            {
                posX = -3.2f;
                posY = -3.5f;
            }
            else if (posX>6&& posY==-4) //Change row
            {
                posX = startPosX;
                posY = startPosY;
            }
            else if (posX>6)
            {
                posX = startPosX;
                posY = startPosY;
            }

            pos = new Vector2(posX += 1.5f, posY);
            
            GetHandCardSpriteData();

            object[] myCustomInitData = new object[]
            {
                currentCard
            };
            GameObject card = PhotonNetwork.Instantiate(PNCardPrefab.name, pos, Quaternion.identity, 0, myCustomInitData);
            card.transform.SetParent(HandCardsParent.transform, false);
            card.GetComponent<SpriteFromAtlas>().SetHandCardSpriteVisibility(true);

            sDeck.Remove(sDeck[0]);
            
        }
        if(sDeck.Count==0)
        {
            //foreach (GameObject c in DiscardPileCards)
            foreach (string c in sDiscardPileCards)
            {
                sDeck.Add(c);

            }
            sDiscardPileCards.Clear();
            ShuffleDeck(true);
            Debug.Log("Shuffling Deck !");
        }
        SetDeckCountText();
    }
    private void ShuffleDeck(bool sendShuffleInfo)
    {
        int count = sDeck.Count;
        for (int i = 0; i < count; i++)
        {
            string temp = sDeck[i];
            int randomIndex = Random.Range(i, sDeck.Count);
            sDeck[i] = sDeck[randomIndex];
            sDeck[randomIndex] = temp;
        }
        if(sendShuffleInfo)
            MainCanvas.GetComponent<GameControl>().DeckIsShuffled();
    }
    private void SetDeckCountText()
    {
         deckCountTxt.text = "Deck: " + sDeck.Count;
         discardpileCountTxt.text = "Discardpile: " + sDiscardPileCards.Count;
    }


    private void GetHandCardSpriteData()
    {
        if(sDeck.Count>0)
            currentCard = sDeck[0]; 
    }

    public void PutHandCardsToDiscardPile()
    {
        foreach (string c in sHandCards)
        {
            sDiscardPileCards.Add(c);
            
        }
        sHandCards.Clear();

        foreach (Transform child in HandCardsParent.transform)
        {
            PhotonNetwork.Destroy(child.gameObject);
        }
        handCards = 0;
        posX = startPosX; posY = startPosY;
        SetDeckCountText();
    }
    public void DeleteHandCardsCompletely(string name) //When using deleteplatform!
    {
        int count = sHandCards.Count;
        for (int i = 0; i < count; i++)
        {
            if (sHandCards[i] == name)
            {
                sHandCards.Remove(sHandCards[i]);
                cardCount--;
                break;
            }
        }

    }


    public void SHOP_AddCardToDiscardPile(string newCard)
    {
        sDiscardPileCards.Add(newCard);
        cardCount++;
    }

}
