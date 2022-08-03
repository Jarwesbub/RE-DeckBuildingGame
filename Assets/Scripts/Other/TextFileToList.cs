using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using Photon.Pun;

public class TextFileToList : MonoBehaviourPun
{
    [SerializeField] private bool shuffleTextList; //Not in actual use rn
    [SerializeField] private string textFileName;
    [SerializeField] private string[] TextList;
    public List<string> RandomList;
    public int textListCount;

    PhotonView view;

    void Awake()
    {
        view = GetComponent<PhotonView>();

        {
            //string readFromFilePath = Application.streamingAssetsPath + "/Recall_Chat/" + textFileName + ".txt";
            //string[] fileLines = File.ReadAllLines(readFromFilePath).ToArray();
            string readFromFilePath = Application.persistentDataPath + "/Game_data/" + textFileName + ".txt";
            string[] fileLines = File.ReadAllLines(readFromFilePath).ToArray();
            TextList = fileLines;
            textListCount = TextList.Length;
            if (shuffleTextList)
                ShuffleTextList();
        }


    }

    public string GetStringFromTextByNumber(int numb)
    {
        return TextList[numb];
    }

    public string GetRandomizedCharacterName()
    {
        int count = TextList.Length;
        int value = Random.Range(0, count);
        return TextList[value];

    }

    private void ShuffleTextList()
    {
        int count = TextList.Length;
        for (int i = 0; i < count; i++)
        {
            string temp = TextList[i];
            int randomIndex = Random.Range(i, TextList.Length);
            TextList[i] = TextList[randomIndex];
            TextList[randomIndex] = temp;
        }
    }
}
