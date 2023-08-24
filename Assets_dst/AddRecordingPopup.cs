using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AddRecordingPopup : MonoBehaviour
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
    public static string musicVersion;

    public string musicPath;
    // Start is called before the first frame update
    void Start()
    {
        
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
        Debug.Log("Paths is " + paths[0]);
        musicPath = paths[0];
        musicNameText.text = FileBrowserEg1.Instance.getName(musicPath);
        byte[] audiobyte = System.IO.File.ReadAllBytes(musicPath);
    }

    public void DoneBtnClicked()
    {
        PostRecordData();
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

    void PostRecordData()
    {

        NetworkConst.record1 data = new NetworkConst.record1();
        //data.ID = "1";
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


        NetworkingManager.Instance.APIAddMusicData(wwwform, (string resData) =>
        {
            Debug.Log("God Login PLayer Response" + resData);
            jsonData = JsonMapper.ToObject(resData);
            if ((bool)jsonData["status"])
            {
                Debug.Log("Status is True");
                RecordManager.Instance.createNewObj(data);
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

}
