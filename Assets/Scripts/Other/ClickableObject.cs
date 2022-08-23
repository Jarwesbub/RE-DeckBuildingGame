using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickableObject : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject MainCanvas;
    [SerializeField] bool isMansionButton;
    public int OnClickButton;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            OnClickButton = 1;
        else if (eventData.button == PointerEventData.InputButton.Right)
            OnClickButton = 2;
        else if (eventData.button == PointerEventData.InputButton.Middle)
            OnClickButton = 3;
        else
            OnClickButton = 0;

        if(isMansionButton)
            MainCanvas.GetComponent<MansionCards>().ClickEnterMansion(OnClickButton);
    }
}