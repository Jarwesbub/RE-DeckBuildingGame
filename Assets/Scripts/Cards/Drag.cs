using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using UnityEngine.UI;

public class Drag : MonoBehaviour
{
    [SerializeField]
    private GameObject MainCanvas;
    public GameObject DeleteCardPlatform;
    private Canvas canvas;
    private PhotonView view;
    private PhotonTransformViewClassic transformView;
    [SerializeField]
    private Vector2 cardPosition;
    [SerializeField]
    private Vector2 lastPos;
    public bool showCardScale; //Owner sees card scale all the time -> others when card gets visible
    private bool isCardVisibleToOthers, isOnDeletePlatform;
    private Vector2 deleteCardPosMin = new Vector2 (6.6f,0.3f), deleteCardPosMax = new Vector2(8.6f, 1.0f);
    //[SerializeField] //DEBUG
    private float minX = -8.5f, maxX = 8.5f, minY = -4.5f, maxY = 4.5f, pnLimitY = -0.5f;

    void Awake()
    {
        DeleteCardPlatform = GameObject.FindWithTag("DeleteCardPlatform");
        MainCanvas = GameObject.FindWithTag("MainCanvas");
        canvas = MainCanvas.GetComponent<Canvas>();
        //Debug.Log("MainCanvas =" + MainCanvas + "inDrag.cs");       
        isCardVisibleToOthers = false;
        isOnDeletePlatform = false;
        showCardScale = false;
    }
    void Start()
    {
        view = GetComponent<PhotonView>();
        transformView = GetComponent<PhotonTransformViewClassic>();

        if (view.IsMine)
            transformView.m_ScaleModel.SynchronizeEnabled = true;

        else
            transformView.m_ScaleModel.SynchronizeEnabled = false;


        lastPos = transform.position;
        cardPosition = transform.position;
    }

    [PunRPC]
    public void DragHandler(BaseEventData data)
    {
        PointerEventData pointerData = (PointerEventData)data;

        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            pointerData.position,
            canvas.worldCamera,
            out position);

        transform.position = canvas.transform.TransformPoint(position);
        transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        
    }
    public void PointerEnter()
    {
        transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        transform.SetAsLastSibling();
    }
    public void PointerExit()
    {
        transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
    }
    public void DragEnd()
    {
        cardPosition = transform.position;

        if (cardPosition.x <= minX || cardPosition.x >= maxX
     || cardPosition.y <= minY || cardPosition.y >= maxY)
        {
            transform.position = lastPos;
        }

        isOnDeletePlatform = CheckIfOnDeletePlatform(transform.position);
        if(!isOnDeletePlatform)
            lastPos = transform.position;

        view.RPC("RPC_PointerExit", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void RPC_PointerExit()
    {
        if (!isOnDeletePlatform)
            JumpBackToLastPosition();

        if (!isCardVisibleToOthers)
            if (lastPos.y >= pnLimitY) //When card crosses "visibility line" pnLimitY
                view.RPC("SetHandCardVisible", RpcTarget.AllBuffered);

    }

    [PunRPC]
    public void SetHandCardVisible()
    {
        GetComponent<SpriteFromAtlas>().SetHandCardSpriteVisibility(true);
        isCardVisibleToOthers = true;
        transformView.m_ScaleModel.SynchronizeEnabled = true;
        //Debug.Log("Card is now visible to others!");
    }


    private bool CheckIfOnDeletePlatform(Vector2 pos)
    {
        if (pos.x > deleteCardPosMin.x && pos.y > deleteCardPosMin.y //position is bigger than deleteCardPosMin
            && pos.x < deleteCardPosMax.x && pos.y < deleteCardPosMax.y) //and position is smaller than deleteCardPosMax
        {
            
            DeleteCardPlatform.GetComponent<DeleteCardsControl>().AskDeleteAction(gameObject);
            return true;
        }
        else
            return false;
    }
    public void JumpBackToLastPosition()
    {
        transform.position = lastPos;
    }
    /*
    public void LockCardMovement(bool lockMovement)
    {
        if (lockMovement)
        {
            transformView.m_PositionModel.SynchronizeEnabled = false;
            transformView.m_ScaleModel.SynchronizeEnabled = false;
        }
        else
        {
            transformView.m_PositionModel.SynchronizeEnabled = true;
            transformView.m_ScaleModel.SynchronizeEnabled = true;
        }
    }*/
}

