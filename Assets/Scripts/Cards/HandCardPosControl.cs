using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCardPosControl : MonoBehaviour
{
    public GameObject parentObject;

    void Awake()
    {
        parentObject = GameObject.FindWithTag("HandCardsParent");
        gameObject.transform.SetParent(parentObject.transform);
        transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
    }

    void OnMouseDrag()
    {
        transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

    }
    void OnMouseUp()
    {
        transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
    }


    public void ParentCardPosition(Vector2 pos)
    {
        transform.position = pos;


    }

    public void ChangePosition(Vector2 pos)
    {
        gameObject.transform.position = pos;
    }
}
