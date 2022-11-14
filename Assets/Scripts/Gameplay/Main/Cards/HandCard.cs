using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCard : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    [SerializeField] private GameObject SpawnCards;
    public string currentCard;
    PhotonView view;

    void Awake()
    {
        SpawnCards = GameObject.FindWithTag("Respawn");
        view = GetComponent<PhotonView>();
        
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] instantiationData = info.photonView.InstantiationData;
        currentCard = (string)instantiationData[0];

        if (view.IsMine)
        {
            string currentCard1 = currentCard;
            view.RPC("SendMyHandCardToSpriteFromAtlas", RpcTarget.OthersBuffered, currentCard1);
        }

    }
    [PunRPC]
    void SendMyHandCardToSpriteFromAtlas(string cardName)
    {
        currentCard = cardName;
        
    }

    public void DeleteThisCardCompletely()
    {
        SpawnCards.GetComponent<SpawnCards>().DeleteHandCardsCompletely(currentCard);
        PhotonNetwork.Destroy(gameObject);
    }
}
