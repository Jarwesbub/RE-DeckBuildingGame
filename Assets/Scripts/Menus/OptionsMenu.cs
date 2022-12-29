using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] Button screenModeBtn;
    private TMP_Text fullScreenText;

    void Start()
    {
        fullScreenText = screenModeBtn.transform.GetChild(0).GetComponent<TMP_Text>();
        DrawScreenMode(Screen.fullScreen);
    }

    public void OnClickToggleFullScreen()
    {
        DrawScreenMode(!Screen.fullScreen);
        Screen.fullScreen = !Screen.fullScreen;
    }
    private void DrawScreenMode(bool isFullScreen)
    {
        if (isFullScreen)
        {
            fullScreenText.text = "Screen mode: Fullscreen";
        }
        else
        {
            fullScreenText.text = "Screen mode: Windowed";
        }
    }

}
