using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FileManager : MonoBehaviour
{
    string path;

    [SerializeField]
    Text musicNameTxt;

    public void openFileExplorer()
    {
        path = EditorUtility.OpenFilePanel("Show all Music File ", "", "mp3");
        StartCoroutine(loadAudio());
    }

    IEnumerator loadAudio()
    {
        Debug.Log("Path : " + path);
        Debug.Log(getSongName(path));
        musicNameTxt.text = getSongName(path);

        UnityWebRequest www = UnityWebRequestTexture.GetTexture("file:///" + path);
        yield return www.SendWebRequest();

        if(www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture; 
        }
    }
    public string getSongName(string str)
    {
        var lastOperatorIndex = str.LastIndexOfAny(new char[] { '/' });
        var lastOperatorIndex1 = str.LastIndexOfAny(new char[] { '.' });
        return Between(str, lastOperatorIndex, lastOperatorIndex1);
    }

    public string Between(string STR, int startIndex, int endIndex)
    {
        string FinalString;
        FinalString = STR.Substring(startIndex+1, endIndex - startIndex);
        return FinalString;
    }

}
