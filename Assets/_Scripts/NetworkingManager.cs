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
using LitJson;
using TMPro;

public class NetworkingManager : SingletonMonoBehaviour<NetworkingManager>
{


    public static string BASE_URL = "https://rt.cisinlive.com/mot-vk/public/api/";
    [SerializeField]
    GameObject blackoutImage;
    [SerializeField]
    MessagePopup _messagePopup;
    public Image image;
    public AudioSource audioSource;
    public GameObject errorGO;
    public GameObject TaskErrormessage;
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
        Debug.Log("<color=blue>URL : " + url + "</color>");
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
            ShowError(www.error);
            HideBlackout();
        }
        else
        {
            Debug.Log("All OK");
            Debug.Log("Status Code: " + www.responseCode);
            Debug.Log("Response is : " + www.downloadHandler.text);
            Debug.Log("<color=yellow>Success: " + www.downloadHandler.text + "</color>");
            checkMessageStatus(www.downloadHandler.text, successCallBack);
            //successCallBack(www.downloadHandler.text);
        }
        HideBlackout();
    }

    public IEnumerator POSTData(string url, string data, onSuccess successCallBack, onError errorCallBack = null)
    {
        Debug.Log("<color=blue>URL : " + url + "</color>");
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
            ShowError(www.error);
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
            checkMessageStatus(www.downloadHandler.text, successCallBack);
            //successCallBack(www.downloadHandler.text);
        }
        HideBlackout();

    }

    public IEnumerator FormData(string url, WWWForm data, onSuccess successCallBack, onError errorCallBack = null)
    {
        Debug.Log("<color=blue>URL : " + url + "</color>");
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
            ShowError(www.error);
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
            checkMessageStatus(www.downloadHandler.text, successCallBack);
        }
        HideBlackout();

    }

    public IEnumerator PATCHData(string url, string data, onSuccess successCallBack, onError errorCallBack = null)
    {
        Debug.Log("<color=blue>URL : " + url + "</color>");
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
            ShowError(request.error);
            HideBlackout();
            Debug.Log("Erro: " + request.error);
        }
        else
        {
            HideBlackout();
            Debug.Log("All OK");
            Debug.Log("Status Code: " + request.responseCode);
            //successCallBack(request.downloadHandler.text);
            checkMessageStatus(www.downloadHandler.text, successCallBack);
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

    public void checkMessageStatus(string data, onSuccess successCallBack)
    {
        JsonData loginJsonResponse;
        loginJsonResponse = JsonMapper.ToObject(data); 
        if ((bool)loginJsonResponse["status"])
        {
            successCallBack(data);
        }
        else
        {
            ShowError(loginJsonResponse["message"].ToString());
        }
     }

    public void ShowError(string message)
    {
        //UIPopupManager.Instance.showPopup(UIPopupManager.ePopup.Message);
        _messagePopup.gameObject.SetActive(true);
        TaskErrormessage.gameObject.GetComponent<TextMeshProUGUI>().text = message+" Add some Tasks";
       MessagePopup.Instance.initialize(() => {
            
        }, message);
    }
    public void ActivateTaskError()
    {
        TaskErrormessage.SetActive(true);
        errorGO.SetActive(true);
        Invoke(nameof(DeactivateTaskError),3);
       
    }
    public void DeactivateTaskError()
    {
        errorGO.SetActive(false);
        TaskErrormessage.SetActive(false);
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
    ////////////////////////////////////Roles/////////////////////////////////////////
    public void APILoadAllRolesData(string json, onSuccess successCallback)
    {
        string url = BASE_URL + "role-list";

        StartCoroutine(GETData(url, json, successCallback));
    }
    public void APILoadAssignedRolesData(string json, onSuccess successCallback)
    {
        string url = BASE_URL + "assign-role";

        StartCoroutine(GETData(url, json, successCallback));
    }
    public void APIEditAuthorData(WWWForm form, onSuccess successCallback)
    {
        string url = BASE_URL + "update-profile";

        StartCoroutine(FormData(url, form, successCallback));
    }

    public void APIPostRecoverData(string json, onSuccess successCallback)
    {
        string url = BASE_URL + "forgot-password";

        StartCoroutine(POSTData(url, json, successCallback));
    }

    public void APIAddSeriesData(WWWForm data, onSuccess successCallback)
    {
        string url = BASE_URL + "add-series";

        StartCoroutine(FormData(url, data, successCallback));
    }
    public void APILoadAllSeriesData(string json, onSuccess successCallback)
    {
        string url = BASE_URL + "series-list";

        StartCoroutine(GETData(url, json, successCallback));
    }
    public void APIDeleteSeriesData(WWWForm data, onSuccess successCallback)
    {
        string url = BASE_URL + "delete-series";

        StartCoroutine(FormData(url, data, successCallback));
    }
    public void APIEditSeriesData(WWWForm data, onSuccess successCallback)
    {
        string url = BASE_URL + "edit-series";

        StartCoroutine(FormData(url, data, successCallback));
    }
    public void APIPublishSeriesData(WWWForm data, onSuccess successCallback)
    {
        string url = BASE_URL + "publish-series";

        StartCoroutine(FormData(url, data, successCallback));
    }
    public void APIUnPublishSeriesData(WWWForm data, onSuccess successCallback)
    {
        string url = BASE_URL + "unpublish-series";

        StartCoroutine(FormData(url, data, successCallback));
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
    public void APIAddTaskData(WWWForm data, onSuccess successCallback)
    {
        string url = BASE_URL + "add-task";

        StartCoroutine(FormData(url, data, successCallback));
    }
    public void APILoadAllTaskData(string json, onSuccess successCallback)
    {
        string url = BASE_URL + "task-list";
        Debug.Log("Call Load All Task API's ");
        StartCoroutine(GETData(url, json, successCallback));
    }
    public void APIDeleteTaskData(string json, onSuccess successCallback)
    {
        string url = BASE_URL + "delete-task";

        StartCoroutine(POSTData(url, json, successCallback));
    }
    public void APIEditTaskData(WWWForm data, onSuccess successCallback)
    {
        string url = BASE_URL + "edit-task";

        StartCoroutine(FormData(url, data, successCallback));
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


    ////////////////////////////////////Category/////////////////////////////////////////
    public void APILoadAllCategoryData(string json, onSuccess successCallback)
    {
        string url = BASE_URL + "categories";

        StartCoroutine(POSTData(url, json, successCallback));
    }
    public void APILoadAllSubCategoryData(string json, onSuccess successCallback, onError errorCallback)
    {
        string url = BASE_URL + "sub-categories";

        StartCoroutine(POSTData(url, json, successCallback,errorCallback));
    }

    ////////////////////////////////////Group Access/////////////////////////////////////////
    public void APILoadAllGroupAccessData(string json, onSuccess successCallback)
    {
        string url = BASE_URL + "group-access";

        StartCoroutine(GETData(url, json, successCallback));
    }
    public void APILoadAllGroupListData(WWWForm data, onSuccess successCallback)
    {
        string url = BASE_URL + "group-list";

        StartCoroutine(FormData(url, data, successCallback));
    }


    ////////////////////////////////////Language/////////////////////////////////////////
    public void APIAddNewLanguageData(WWWForm data, onSuccess successCallback)
    {
        string url = BASE_URL + "add-app-language";

        StartCoroutine(FormData(url, data, successCallback));
    }
    public void APIAddLanguageData(WWWForm data, onSuccess successCallback)
    {
        string url = BASE_URL + "add-language";

        StartCoroutine(FormData(url, data, successCallback));
    }
    public void APIEditLanguageData(WWWForm data, onSuccess successCallback)
    {
        string url = BASE_URL + "add-app-language";

        StartCoroutine(FormData(url, data, successCallback));
    }
    public void APILoadAllLanguageData(string json, onSuccess successCallback)
    {
        string url = BASE_URL + "app-language";

        StartCoroutine(GETData(url, json, successCallback));
    }
    public void APIDeleteLanguageData(WWWForm data, onSuccess successCallback)
    {
        string url = BASE_URL + "delete-app-language";

        StartCoroutine(FormData(url, data, successCallback));
    }

}