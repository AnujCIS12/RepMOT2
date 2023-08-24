// NetworkingManager.cs
using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class NetworkingManager : SingletonMonoBehaviour<NetworkingManager>
{


    public static string BASE_URL = "https://rt.cisinlive.com/mot-vk/public/api/";
    [SerializeField]
    GameObject blackoutImage;
    public Image image;
    public AudioSource audioSource;

    public delegate void onSuccess(string Text);
    public delegate void onError(string Text);

    UnityWebRequest www;

    public const string MatchEmailPattern =
      @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
      + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
      + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
      + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

    public bool validateEmail(string email)
    {
        Debug.Log("Email is " + email);
        if (email != null)
            return Regex.IsMatch(email, MatchEmailPattern);
        else
            return false;
    }

    public bool IsValidEmail(string email)
    {
        var trimmedEmail = email.Trim();

        if (trimmedEmail.EndsWith("."))
        {
            return false; // suggested by @TK-421
        }
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == trimmedEmail;
        }
        catch
        {
            return false;
        }
    }

    public string Encorde64(string src)
    {
        byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(src);
        string encodedText = Convert.ToBase64String(bytesToEncode);
        return encodedText;
    }
    public string Decode64(string src64)
    {
        src64 = src64.Replace("\"", "");
        byte[] decodedBytes = Convert.FromBase64String(src64);
        string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
        return decodedText;
    }
    IEnumerator GETData(string url, string data, onSuccess successCallBack, onError errorCallBack = null)
    {
        ShowBlackout();
        www = UnityWebRequest.Get(url);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        www.uploadHandler.contentType = "application/x-www-form-urlencoded";
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Accept", "application/json");
        www.SetRequestHeader("Content-Type", "application/json");
        if (!string.IsNullOrEmpty(CreatorData.instance.Token))
        {
            Debug.Log("Token     " + CreatorData.instance.Token);
            www.SetRequestHeader("Authorization", CreatorData.instance.Token);
        }
        yield return www.SendWebRequest();

        if (www.error != null)
        {
            Debug.Log("Erro: " + www.error);
            HideBlackout();
        }
        else
        {
            Debug.Log("All OK");
            Debug.Log("Status Code: " + www.responseCode);
            Debug.Log("Response is : " + www.downloadHandler.text);
            Debug.Log("<color=yellow>Success: " + www.downloadHandler.text + "</color>");
            successCallBack(www.downloadHandler.text);
        }
        HideBlackout();
    }

    public IEnumerator POSTData(string url, string data, onSuccess successCallBack, onError errorCallBack = null)
    {
        ShowBlackout();
        Debug.Log("Data is ");
        Debug.Log(data);

        www = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        www.uploadHandler.contentType = "application/x-www-form-urlencoded";
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        if(!string.IsNullOrEmpty(CreatorData.instance.Token))
        {
            Debug.Log("Token     "+CreatorData.instance.Token);
            www.SetRequestHeader("Authorization", CreatorData.instance.Token);
        }
        
        //request.SetRequestHeader("password", "Admin123#");
        yield return www.SendWebRequest();

        if (www.error != null)
        {
            HideBlackout();
            Debug.Log("Erro: " + www.error);
            Debug.Log("<color=red>Error: " + www.error + "</color>");
        }
        else
        {
            //HideBlackout();
            Debug.Log("All OK");
            Debug.Log("Status Code: " + www.responseCode);
            Debug.Log("Response is : " + www.downloadHandler.text);
            Debug.Log("<color=yellow>Success: "+ www.downloadHandler.text+ "</color>");
            successCallBack(www.downloadHandler.text);
        }
        HideBlackout();

    }

    public IEnumerator FormData(string url, WWWForm data, onSuccess successCallBack, onError errorCallBack = null)
    {
        ShowBlackout();
        Debug.Log("Data is ");
        Debug.Log(data);

        UnityWebRequest www = UnityWebRequest.Post(url, data);
        //byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
        //www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        //www.uploadHandler.contentType = "application/x-www-form-urlencoded";
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        //www.SetRequestHeader("Content-Type", "multipart/form-data");
        if (!string.IsNullOrEmpty(CreatorData.Instance.Token))
        {
            Debug.Log("Token     " + CreatorData.instance.Token);
            www.SetRequestHeader("Authorization", CreatorData.instance.Token);
        }

        //request.SetRequestHeader("password", "Admin123#");
        yield return www.SendWebRequest();

        if (www.error != null)
        {
            HideBlackout();
            Debug.Log("Erro: " + www.error);
            Debug.Log("<color=red>Error: " + www.error + "</color>");
        }
        else
        {
            //HideBlackout();
            Debug.Log("All OK");
            Debug.Log("Status Code: " + www.responseCode);
            Debug.Log("Response is : " + www.downloadHandler.text);
            Debug.Log(www.downloadHandler.text);
            Debug.Log("<color=yellow>Success: " + www.downloadHandler.text + "</color>");
            successCallBack(www.downloadHandler.text);
        }
        HideBlackout();

    }

    public IEnumerator PATCHData(string url, string data, onSuccess successCallBack, onError errorCallBack = null)
    {
        ShowBlackout();
        Debug.Log("Data is " + data);

        var request = new UnityWebRequest(url, "PATCH");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            HideBlackout();
            Debug.Log("Erro: " + request.error);
        }
        else
        {
            HideBlackout();
            Debug.Log("All OK");
            Debug.Log("Status Code: " + request.responseCode);
            successCallBack(request.downloadHandler.text);
        }
        HideBlackout();

    }

    

    public void ShowBlackout()
    {
        blackoutImage.SetActive(true);
    }
    public void HideBlackout()
    {
        blackoutImage.SetActive(false);
    }




    public void APIPostLoginData(string json, onSuccess successCallback)
    {
        string url = BASE_URL+"login";

        StartCoroutine(POSTData(url, json, successCallback));
    }

    public void APIPostRegisterData(string json, onSuccess successCallback)
    {
        string url = BASE_URL + "registration";

        StartCoroutine(POSTData(url, json, successCallback));
    }

    public void APIPostRecoverData(string json, onSuccess successCallback)
    {
        string url = BASE_URL + "forgot-password";

        StartCoroutine(POSTData(url, json, successCallback));
    }

    public void APIAddSeriesData(string json, onSuccess successCallback)
    {
        string url = BASE_URL + "add-series";

        StartCoroutine(POSTData(url, json, successCallback));
    }
    public void APILoadAllSeriesData(string json, onSuccess successCallback)
    {
        string url = BASE_URL + "series-list";

        StartCoroutine(GETData(url, json, successCallback));
    }
    public void APIDeleteSeriesData(string json, onSuccess successCallback)
    {
        string url = BASE_URL + "delete-series";

        StartCoroutine(POSTData(url, json, successCallback));
    }
    public void APIEditSeriesData(string json, onSuccess successCallback)
    {
        string url = BASE_URL + "edit-series";

        StartCoroutine(POSTData(url, json, successCallback));
    }


    /////////////////////////////////////Level//////////////////////////////////
    public void APIAddLevelData(string json, onSuccess successCallback)
    {
        string url = BASE_URL + "add-level";

        StartCoroutine(POSTData(url, json, successCallback));
    }
    public void APILoadAllLevelData(string json, onSuccess successCallback)
    {
        string url = BASE_URL + "level-list";

        StartCoroutine(GETData(url, json, successCallback));
    }
    public void APIDeleteLevelData(string json, onSuccess successCallback)
    {
        string url = BASE_URL + "delete-level";

        StartCoroutine(POSTData(url, json, successCallback));
    }
    public void APIEditLevelData(string json, onSuccess successCallback)
    {
        string url = BASE_URL + "edit-level";

        StartCoroutine(POSTData(url, json, successCallback));
    }


    ////////////////////////////////////Task/////////////////////////////////////////
    ///
    public void APIAddTaskData(string json, onSuccess successCallback)
    {
        string url = BASE_URL + "add-task";

        StartCoroutine(POSTData(url, json, successCallback));
    }
    public void APILoadAllTaskData(string json, onSuccess successCallback)
    {
        string url = BASE_URL + "task-list";

        StartCoroutine(GETData(url, json, successCallback));
    }
    public void APIDeleteTaskData(string json, onSuccess successCallback)
    {
        string url = BASE_URL + "delete-task";

        StartCoroutine(POSTData(url, json, successCallback));
    }
    public void APIEditTaskData(string json, onSuccess successCallback)
    {
        string url = BASE_URL + "edit-task";

        StartCoroutine(POSTData(url, json, successCallback));
    }



    ////////////////////////////////////Music/////////////////////////////////////////
    public void APIAddMusicData(WWWForm data, onSuccess successCallback)
    {
        string url = BASE_URL + "add-music";

        StartCoroutine(FormData(url, data, successCallback));
    }
    public void APIEditMusicData(WWWForm data, onSuccess successCallback)
    {
        string url = BASE_URL + "edit-music";

        StartCoroutine(FormData(url, data, successCallback));
    }
    public void APILoadAllMusicData(string json, onSuccess successCallback)
    {
        string url = BASE_URL + "music-list";

        StartCoroutine(GETData(url, json, successCallback));
    }
    public void APIDeleteMusicData(string json, onSuccess successCallback)
    {
        string url = BASE_URL + "delete-music";

        StartCoroutine(POSTData(url, json, successCallback));
    }


    ////////////////////////////////////Record/////////////////////////////////////////
    public void APIAddRecordData(WWWForm data, onSuccess successCallback)
    {
        string url = BASE_URL + "add-record";

        StartCoroutine(FormData(url, data, successCallback));
    }
    public void APIEditRecordData(WWWForm data, onSuccess successCallback)
    {
        string url = BASE_URL + "edit-record";

        StartCoroutine(FormData(url, data, successCallback));
    }
    public void APILoadAllRecordData(string json, onSuccess successCallback)
    {
        string url = BASE_URL + "record-list";

        StartCoroutine(GETData(url, json, successCallback));
    }
    public void APIDeleteRecordData(string json, onSuccess successCallback)
    {
        string url = BASE_URL + "delete-record";

        StartCoroutine(POSTData(url, json, successCallback));
    }


}