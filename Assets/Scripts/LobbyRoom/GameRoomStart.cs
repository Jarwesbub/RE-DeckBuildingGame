using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoomStart : MonoBehaviour
{
    public GameObject localCharCntrl, otherCharCntrl;


    // Start is called before the first frame update
    private void Start()
    {
        Destroy(gameObject);
    }



    void OnDestroy()
    {
        localCharCntrl.SetActive(true);
            //localCharCntrl.GetComponent<CharacterControl>().PlayerInfoObjectsReady();
            //otherCharCntrl.GetComponent<CharacterControl>().PlayerInfoObjectsReady();

    }

}
