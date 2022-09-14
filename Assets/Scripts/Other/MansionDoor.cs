using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MansionDoor : MonoBehaviour
{
    public GameObject MainCanvas;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        MainCanvas.GetComponent<MansionControl>().MansionDoorReset();
        anim.Play("closedDoor_anim");

    }

    public void OpenMansionDoor()
    {
        anim.Play("openDoor_anim");
    }


    public void MansionDoorIsOpen() //Controlled in animation (Can be used later)
    {
        //MainCanvas.GetComponent<MansionCards>().MansionDoorIsOpen();

    }
    public void SetDoorInActive() //Controlled in animation
    {
        MainCanvas.GetComponent<MansionControl>().DoorAnimationEnds();
        gameObject.SetActive(false);
        
    }

}
