using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderChangeOnEnable : MonoBehaviour
{
    private void OnEnable()
    {
        transform.SetAsLastSibling();
    }
    private void OnDisable()
    {
        //transform.SetAsFirstSibling();
        transform.SetSiblingIndex(1);
    }
}
