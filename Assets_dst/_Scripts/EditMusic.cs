using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class EditMusic : MonoBehaviour
{
    [SerializeField]
    InputField titleNameIF, orquestraIF, singerIF, composerIF, lyricsFromIF, yearPublishedIF, commentIF, priceIF, tagIF;

    [SerializeField]
    Toggle enableToggle, licenceFreeToggle;

    [SerializeField]
    Dropdown categoryDD, subCategoryDD, groupAccessDD;

    [SerializeField]
    Text message;

    [SerializeField]
    Text musicNameText;

    JsonData jsonData;
    Music music;
    NetworkConst.music data;

    public string musicPath;

    public void InitializeData(Music _music)
    {
        this.music = _music;
        NetworkConst.music tmpMusic;
        tmpMusic = _music.getMusic();

        titleNameIF.text = tmpMusic.titleName;
        orquestraIF.text = tmpMusic.orquestra;
        singerIF.text = tmpMusic.singer;
        composerIF.text = tmpMusic.composer;
        lyricsFromIF.text = tmpMusic.lyricsFrom;
        yearPublishedIF.text = tmpMusic.yearPublished;
        commentIF.text = tmpMusic.comment;
        priceIF.text = tmpMusic.price;
        tagIF.text = tmpMusic.tag;
        categoryDD.value = categoryDD.options.FindIndex(option => option.text == tmpMusic.category);
        subCategoryDD.value = subCategoryDD.options.FindIndex(option => option.text == tmpMusic.subcategory);
        groupAccessDD.value = groupAccessDD.options.FindIndex(option => option.text == tmpMusic.groupAccess);
        enableToggle.isOn = tmpMusic.enabled;
        licenceFreeToggle.isOn = tmpMusic.licenceFree;

        showPopup();
    }

    public void selectMusicBtnClicked()
    {
        FileBrowserEg1.Instance.GetMusicPath(onFileSelected);
    }

    public void onFileSelected(string[] paths)
    {
        Debug.Log("Paths is " + paths[0]);
        musicPath = paths[0];
        musicNameText.text = FileBrowserEg1.Instance.getName(musicPath);
        byte[] audiobyte = System.IO.File.ReadAllBytes(musicPath);
    }

    public void DoneBtnClicked()
    {
        PostMusicData();
    }
    public void closePopup()
    {
        setDefaultField();
        gameObject.SetActive(false);
    }
    public void showPopup()
    {
        gameObject.SetActive(true);
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


    //NetworkConst.music data;
    public void updateMusic()
    {
        this.music.setMusic(data);
    }
    
    public void PostMusicData()
    {
        data = new NetworkConst.music();
        data.id = this.music.ID;
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
        Debug.Log(musicPath);
        if(!string.IsNullOrEmpty(musicPath)) data.musicFile = System.IO.File.ReadAllBytes(musicPath);

        string json = JsonUtility.ToJson(data);

        //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //formData.Add(new MultipartFormDataSection("titleName", "nothing"));

        WWWForm wwwform = new WWWForm();
        Debug.Log("ID is " + this.music.ID);
        wwwform.AddField("musicID", this.music.ID);
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
        if(!string.IsNullOrEmpty(musicPath)) wwwform.AddBinaryData("musicFile", File.ReadAllBytes(musicPath), "music.mp3", "audio/wav");

        NetworkingManager.Instance.APIEditMusicData(wwwform, (string resData) =>
        {
            Debug.Log("God Login PLayer Response" + resData);
            jsonData = JsonMapper.ToObject(resData);
            if ((bool)jsonData["status"])
            {
                Debug.Log("Status is True");
                //LevelManager.Instance.createNewLevel(data);
                //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
                updateMusic();
                closePopup();
            }
            else
            {
                Debug.Log("Message ");
                Debug.Log(jsonData["message"].ToString());
                //message.text = levelJsonData["message"].ToString();
            }

            ////PlayerData.Instance.PlayerName = name;
            ////PlayerData.Instance.PlayerGUID = playerRowKey;

        });
    }
}
