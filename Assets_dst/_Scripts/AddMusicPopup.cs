using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AddMusicPopup : MonoBehaviour
{
    [SerializeField]
    InputField titleNameIF, orquestraIF, singerIF, composerIF, lyricsFromIF, yearPublishedIF, commentIF, priceIF, tagIF;

    [SerializeField]
    Toggle enableToggle,licenceFreeToggle;

    [SerializeField]
    Dropdown categoryDD, subCategoryDD, groupAccessDD;

    [SerializeField]
    Text message;

    [SerializeField]
    Text musicNameText;

    JsonData jsonData;
    public static string musicVersion;

    public string musicPath;
    void Start()
    {

    }
    public static void setMusicVersion(string data)
    {
        Debug.Log("Series Version is " + data);
        musicVersion = data;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void selectMusicBtnClicked()
    {
        FileBrowserEg1.Instance.GetMusicPath(onFileSelected);
    }
    public void onFileSelected(string[] paths)
    {
        Debug.Log("Paths is "+paths[0]);
        musicPath = paths[0];
        musicNameText.text = FileBrowserEg1.Instance.getName(musicPath);
        byte[] audiobyte = System.IO.File.ReadAllBytes(musicPath);
    }

    public void DoneBtnClicked()
    {
        PostMusicData();
        //StartCoroutine(UploadDataOnServer());
        //StartCoroutine(Upload1());
    }
    public void closePopup()
    {
        setDefaultField();
        gameObject.SetActive(false);
    }
    public void setDefaultField()
    {
        titleNameIF.text = "";
        orquestraIF.text = "";
        singerIF.text = "";
        composerIF.text = "";
        lyricsFromIF.text = "";
        yearPublishedIF.text = "";
        commentIF.text = "";
        priceIF.text = "";
        tagIF.text = "";
        categoryDD.value = categoryDD.options.FindIndex(option => option.text == "Category");
        subCategoryDD.value = subCategoryDD.options.FindIndex(option => option.text == "Sub Category");
        groupAccessDD.value = groupAccessDD.options.FindIndex(option => option.text == "Group Access");
        enableToggle.isOn = false;
        licenceFreeToggle.isOn = false;
    }

    void PostMusicData()
    {

        NetworkConst.music data = new NetworkConst.music();
        //data.ID = "1";
        data.titleName = titleNameIF.text;
        data.orquestra = orquestraIF.text;
        data.lyricsFrom = lyricsFromIF.text;
        data.singer = singerIF.text;
        data.composer = composerIF.text;
        data.yearPublished = yearPublishedIF.text;
        data.licenceFree = false;
        data.comment = commentIF.text;
        data.price = priceIF.text;
        data.tag = tagIF.text;
        data.category = categoryDD.options[categoryDD.value].text;
        data.subcategory = subCategoryDD.options[subCategoryDD.value].text;
        data.groupAccess = groupAccessDD.options[groupAccessDD.value].text;
        data.enabled = enableToggle.isOn;
        data.licenceFree = licenceFreeToggle.isOn;
        data.musicFile = System.IO.File.ReadAllBytes(musicPath);

        string json = JsonUtility.ToJson(data);

        //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //formData.Add(new MultipartFormDataSection("titleName", "nothing"));

        WWWForm wwwform = new WWWForm();        
        wwwform.AddField("titleName", titleNameIF.text);
        wwwform.AddField("orquestra", orquestraIF.text);
        wwwform.AddField("lyricsFrom", lyricsFromIF.text);
        wwwform.AddField("singer", singerIF.text);
        wwwform.AddField("composer", composerIF.text);
        wwwform.AddField("yearPublished", yearPublishedIF.text);
        wwwform.AddField("licenceFree", licenceFreeToggle.isOn.ToString());
        wwwform.AddField("comment", commentIF.text);
        wwwform.AddField("price", priceIF.text);
        wwwform.AddField("tag", tagIF.text);
        wwwform.AddField("category", categoryDD.options[categoryDD.value].text);
        wwwform.AddField("subcategory", subCategoryDD.options[subCategoryDD.value].text);
        wwwform.AddField("groupAccess", groupAccessDD.options[groupAccessDD.value].text);
        wwwform.AddField("public", "1");
        wwwform.AddField("enabled", enableToggle.isOn.ToString());
        wwwform.AddBinaryData("musicFile", File.ReadAllBytes(musicPath), "music.mp3", "audio/wav");


        NetworkingManager.Instance.APIAddMusicData(wwwform, (string resData) =>
        {
            Debug.Log("God Login PLayer Response" + resData);
            jsonData = JsonMapper.ToObject(resData);
            if ((bool)jsonData["status"])
            {
                Debug.Log("Status is True");
                MusicManager.Instance.createNewMusic(data);
                //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
            }
            else
            {
                Debug.Log("Message ");
                Debug.Log(jsonData["message"].ToString());
                //message.text = seriesJsonData["message"].ToString();
            }

        });
    }

    IEnumerator Upload()
    {
        NetworkConst.music data = new NetworkConst.music();
        //data.ID = "1";
        data.titleName = titleNameIF.text;
        data.orquestra = orquestraIF.text;
        data.lyricsFrom = lyricsFromIF.text;
        data.singer = singerIF.text;
        data.composer = composerIF.text;
        data.yearPublished = yearPublishedIF.text;
        data.comment = commentIF.text;
        data.price = priceIF.text;
        data.tag = tagIF.text;
        data.category = categoryDD.options[categoryDD.value].text;
        data.subcategory = subCategoryDD.options[subCategoryDD.value].text;
        data.groupAccess = groupAccessDD.options[groupAccessDD.value].text;
        data.enabled = enableToggle.isOn;
        //data.musicFile = System.IO.File.ReadAllBytes(musicPath);
        string json = JsonUtility.ToJson(data);

        WWWForm wwwform = new WWWForm();
        wwwform.AddBinaryData("musicFile", File.ReadAllBytes(musicPath), "music.mp3", "audio/wav");
        wwwform.AddField("titleName", "nothing");
        wwwform.AddField("orquestra", orquestraIF.text);
        wwwform.AddField("lyricsFrom", lyricsFromIF.text);
        wwwform.AddField("singer", singerIF.text);
        wwwform.AddField("composer", composerIF.text);
        wwwform.AddField("yearPublished", yearPublishedIF.text);

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("titleName=foo&orquestra=bar"));
        formData.Add(new MultipartFormDataSection("titleName=", "sagar"));
        //formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));

        //UnityWebRequest www = UnityWebRequest.Post("https://rt.cisinlive.com/mot-vk/public/api/add-music", wwwform);
        UnityWebRequest www = UnityWebRequest.Post("https://rt.cisinlive.com/mot-vk/public/api/add-music", formData);
        //www.uploadHandler.contentType = "multipart/form-data";
        www.SetRequestHeader("Accept", "application/json");
        www.SetRequestHeader("Content-Type", "multipart/form-data");
        if (!string.IsNullOrEmpty(CreatorData.Instance.Token))
        {
            Debug.Log("Token     " + CreatorData.Instance.Token);
            www.SetRequestHeader("Authorization", CreatorData.Instance.Token);
        }
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        //www.uploadHandler.contentType = "multipart/form-data";
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        //yield return www.SendWebRequest();
        yield return www.Send();


        
        //yield return www.Send();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
            string result = ((DownloadHandler)www.downloadHandler).text;
            Debug.Log(result);
        }
    }
    IEnumerator Upload1()
    {
        
     
        //data.musicFile = ;


        WWWForm form = new WWWForm();
        form.AddField("myField", "myData");
        form.AddField("titleName", "nothing");
        form.AddField("orquestra", orquestraIF.text);
        form.AddField("lyricsFrom", lyricsFromIF.text);
        form.AddField("singer", singerIF.text);
        form.AddField("composer", composerIF.text);
        form.AddField("yearPublished", yearPublishedIF.text);

        form.AddField("comment", commentIF.text);
        form.AddField("price", priceIF.text);
        form.AddField("tag", tagIF.text);
        form.AddField("category", categoryDD.options[categoryDD.value].text);
        form.AddField("subcategory", subCategoryDD.options[subCategoryDD.value].text);
        form.AddField("groupAccess", groupAccessDD.options[groupAccessDD.value].text);
        form.AddField("enabled", enableToggle.isOn.ToString());
        form.AddBinaryData("musicFile", File.ReadAllBytes(musicPath), "music.mp3", "audio/wav");

        UnityWebRequest www = UnityWebRequest.Post("https://rt.cisinlive.com/mot-vk/public/api/add-music", form);
        www.SetRequestHeader("Authorization", CreatorData.Instance.Token);
        www.SetRequestHeader("Accept", "application/json");
        //www.SetRequestHeader("Content-Type", "multipart/form-data");
        {
            yield return www.Send();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
                Debug.Log(www.downloadHandler.text);
            }
        }


    }
    IEnumerator UploadDataOnServer()
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("titleName", "nothing"));
        UnityWebRequest www = UnityWebRequest.Post("https://rt.cisinlive.com/mot-vk/public/api/add-music", formData);

        www.uploadHandler.contentType = "multipart/form-data";
        www.SetRequestHeader("Accept", "application/json");
        www.SetRequestHeader("Content-Type", "multipart/form-data");
        if (!string.IsNullOrEmpty(CreatorData.Instance.Token))
        {
            Debug.Log("Token     " + CreatorData.Instance.Token);
            www.SetRequestHeader("Authorization", CreatorData.Instance.Token);
        }
        
        //UnityWebRequest www = UnityWebRequest.Post(Constants.APIHeader + "add-question-data", wwwform);
        //www.SetRequestHeader("Content-Type", 'multipart/form-data; boundary="ezlWWIApx6hVhlgLaVjiUfj3fOP9oevkwLb4K7QK"');
        //www.SetRequestHeader("Content-Type", "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW");
        print("Request send to API");
        //UnityWebRequest www = UnityWebRequest.Post("https://lokendrarj.elblocal.cisinlive.com/upload-file", wwwform);

        yield return www.SendWebRequest();

        if (!string.IsNullOrEmpty(www.error))
        {
            print("Error");
            print("Error:" + www.error);
            Debug.Log(www.error);
        }

        else
        {
            print("I'm in else of add card data");
            Debug.Log(www.downloadHandler.text);
            
        }
    }
        //public void PostMusicData()
        //{
        //    //string playerRowKey = System.Guid.NewGuid().ToString();
        //    NetworkConst.music data = new NetworkConst.music();
        //    //data.ID = "1";
        //    data.titleName = titleNameIF.text;
        //    data.orquestra = orquestraIF.text;
        //    data.lyricsFrom = lyricsFromIF.text;
        //    data.singer = singerIF.text;
        //    data.composer = composerIF.text;
        //    data.yearPublished = yearPublishedIF.text;
        //    data.comment = commentIF.text;
        //    data.price = priceIF.text;
        //    data.tag = tagIF.text;
        //    data.category = categoryDD.options[categoryDD.value].text;
        //    data.subcategory = subCategoryDD.options[subCategoryDD.value].text;
        //    data.groupAccess= groupAccessDD.options[groupAccessDD.value].text;
        //    data.enabled = enableToggle.isOn;
        //    data.musicFile = System.IO.File.ReadAllBytes(musicPath);


        //    string json = JsonUtility.ToJson(data);
        //    NetworkingManager.Instance.APIAddMusicData(json, (string resData) =>
        //    {
        //        Debug.Log("God Login PLayer Response" + resData);
        //        jsonData = JsonMapper.ToObject(resData);
        //        if ((bool)jsonData["status"])
        //        {
        //            Debug.Log("Status is True");
        //            MusicManager.Instance.createNewMusic(data);
        //            //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
        //        }
        //        else
        //        {
        //            Debug.Log("Message ");
        //            Debug.Log(jsonData["message"].ToString());
        //            //message.text = seriesJsonData["message"].ToString();
        //        }

        //        ////PlayerData.Instance.PlayerName = name;
        //        ////PlayerData.Instance.PlayerGUID = playerRowKey;

        //    });
        //}
    }
