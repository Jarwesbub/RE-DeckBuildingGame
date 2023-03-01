using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIBlackedMain : MonoBehaviour
{
    public TMP_Text title;

    public void SetMainInfoText(string titleText)
    {
        title.text = titleText;
    }



}
