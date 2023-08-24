using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class EditRecord : MonoBehaviour
{
    [SerializeField]
    InputField nameIF, roleIF, entryDateIF, needVersionIF, gameLevelIF, commentIF;

    [SerializeField]
    Toggle privateToggle;

    [SerializeField]
    Dropdown categoryDD, subCategoryDD, fileDatDD, fileInputTraceDD, filesettingDD;

    [SerializeField]
    Text message;

    [SerializeField]
    Text musicNameText;

    JsonData jsonData;
    Record obj;
    NetworkConst.record1 data;

    public string musicPath;

    public void InitializeData(Record _obj)
    {
        this.obj = _obj;
        NetworkConst.record1 tmpObj;
        tmpObj = _obj.getStructObj();

        nameIF.text = tmpObj.name;
        roleIF.text = tmpObj.role;
        entryDateIF.text =tmpObj.entryDate;
        needVersionIF.text = tmpObj.needVersion;
        gameLevelIF.text = tmpObj.gameLevel;
        commentIF.text = tmpObj.comment;
        categoryDD.value = categoryDD.options.FindIndex(option => option.text == tmpObj.category);
        subCategoryDD.value = subCategoryDD.options.FindIndex(option => option.text == tmpObj.subCategorie);
        fileDatDD.value = fileDatDD.options.FindIndex(option => option.text == tmpObj.fileDat);
        fileInputTraceDD.value = fileInputTraceDD.options.FindIndex(option => option.text == tmpObj.fileInputTrace);
        filesettingDD.value = filesettingDD.options.FindIndex(option => option.text == tmpObj.fileSettings);
        privateToggle.isOn = false;

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
        PostObjData();
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
        nameIF.text = "";
        roleIF.text = "";
        entryDateIF.text = "";
        needVersionIF.text = "";
        gameLevelIF.text = "";
        commentIF.text = "";
        categoryDD.value = categoryDD.options.FindIndex(option => option.text == "Category");
        subCategoryDD.value = subCategoryDD.options.FindIndex(option => option.text == "Sub Category");
        fileDatDD.value = fileDatDD.options.FindIndex(option => option.text == "FileDat");
        fileInputTraceDD.value = fileInputTraceDD.options.FindIndex(option => option.text == "File Input Trace");
        filesettingDD.value = filesettingDD.options.FindIndex(option => option.text == "File Setting");
        privateToggle.isOn = false;
    }


    //NetworkConst.music data;
    public void updateObj()
    {
        this.obj.setRecord(data);
    }

    public void PostObjData()
    {
        NetworkConst.record1 data = new NetworkConst.record1();
        //data.ID = "1";
        data.recordID = this.obj.ID;
        data.name = nameIF.text;
        data.role = roleIF.text;
        data.entryDate = entryDateIF.text;
        data.needVersion = needVersionIF.text;
        data.gameLevel = gameLevelIF.text;
        data.comment = commentIF.text;
        data.category = categoryDD.options[categoryDD.value].text;
        data.subCategorie = subCategoryDD.options[subCategoryDD.value].text;
        data.fileDat = fileDatDD.options[fileDatDD.value].text;
        data.fileInputTrace = fileInputTraceDD.options[fileInputTraceDD.value].text;
        data.fileSettings = filesettingDD.options[filesettingDD.value].text;
        data.fileMusic = System.IO.File.ReadAllBytes(musicPath);

        string json = JsonUtility.ToJson(data);

        //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //formData.Add(new MultipartFormDataSection("titleName", "nothing"));

        WWWForm wwwform = new WWWForm();
        wwwform.AddField("recordID", this.obj.ID);
        wwwform.AddField("name", nameIF.text);
        wwwform.AddField("role", roleIF.text);
        wwwform.AddField("entryDate", entryDateIF.text);
        wwwform.AddField("needVersion", needVersionIF.text);
        wwwform.AddField("gameLevel", gameLevelIF.text);
        wwwform.AddField("comment", commentIF.text);
        wwwform.AddField("category", categoryDD.options[categoryDD.value].text);
        wwwform.AddField("subcategory", subCategoryDD.options[subCategoryDD.value].text);
        wwwform.AddField("fileDat", fileDatDD.options[fileDatDD.value].text);
        wwwform.AddField("fileInputTrace", fileInputTraceDD.options[fileInputTraceDD.value].text);
        wwwform.AddField("fileSettings", filesettingDD.options[filesettingDD.value].text);
        wwwform.AddBinaryData("musicFile", File.ReadAllBytes(musicPath), "music.mp3", "audio/wav");

        NetworkingManager.Instance.APIEditRecordData(wwwform, (string resData) =>
        {
            Debug.Log("God Login PLayer Response" + resData);
            jsonData = JsonMapper.ToObject(resData);
            if ((bool)jsonData["status"])
            {
                Debug.Log("Status is True");
                //LevelManager.Instance.createNewLevel(data);
                //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
                updateObj();
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
