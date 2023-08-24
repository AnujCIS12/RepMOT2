using JetBrains.Annotations;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddRecordingPopup : MonoBehaviour
{

    [SerializeField]
    InputField nameIF, roleIF, entryDateIF, needVersionIF, gameLevelIF, commentIF;

    [SerializeField]
    Toggle privateToggle;

    [SerializeField]
    Dropdown categoryDD, subCategoryDD, fileDatDD, fileInputTraceDD, filesettingDD, selectRecOptionsDD, recordingsDD;

    [SerializeField]
    GameObject selectFromLocalBtn;

    public TMP_Dropdown fileDatDropDown;

    private string[] dirEntries;
    private string dirPath = "Assets/_Recordings/";
    private string fileName;
    private string filePath;
    private string inputtracePath;
    private string settingsPath;

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

    public void OnEnable()
    {
        OpenPopup();
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

    private void selectRecOptionsDDValueChanged(Dropdown _dd)
    {
        Debug.Log("Value Changed " + _dd.value);
        switch (_dd.value)
        {
            case 0:
                print("Select from Local");
                optSelectFromLocal();
                break;
            case 1:
                print("Select From Cloud");
                optLoadFromCloud();
                break;
        }
    }
    public void optSelectFromLocal()
    {
        if (recordingsDD.gameObject.activeSelf) recordingsDD.gameObject.SetActive(false);
        OpenPopup();
        fileDatDropDown.gameObject.SetActive(true);
    }
    public void optLoadFromCloud()
    {
        if (fileDatDropDown.gameObject.activeSelf) fileDatDropDown.gameObject.SetActive(false);
        recordingsDD.gameObject.SetActive(true);
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

    public void OpenPopup()
    {
        OpenDirectory();
        //entryDateIF.text = System.DateTime.UtcNow.ToString("dd-MM-yyyy");
        //entryDateIF.interactable = false;
        //popup.SetActive(true);
    }

    private void ProcessFile([CanBeNull] string _filename)
    {
        fileDatDropDown.options.Add(new TMP_Dropdown.OptionData() { text = Path.GetFileNameWithoutExtension(_filename) });
    }


    private void OpenDirectory()
    {
        // Option: append the '@' verbatim to the directory path string  
        // -> this gives the complete path like D://            dirPath = @"" + dirPath;
        dirEntries = Directory.GetFiles(dirPath);
        //fileDatDropDown.ClearOptions();
        if (dirEntries.Length >= 1)
        {
            foreach (var dirName in dirEntries)
            {
                ProcessFile(dirName);
            }
        }
        else
        {
            // former: ButtonFunctions.MessageToUser("No recording folder like this:   " + dirPath, 3);  
            // "Sorry, can not find this recording folder. Spelling right?"
            Debug.Log("Didn't find Recording");

            //var eventParam = new EventParam();

            //eventParam.message_text = "No recording folder like this:   " + dirPath;
            //eventParam.message_time = 3f;
            //EventManager.TriggerEvent("MessageToUser", eventParam: eventParam);
        }
    }

    void PostRecordData()
    {

        NetworkConst.record1 data = new NetworkConst.record1();
        //data.ID = "1";
        data.name = nameIF.text;
        data.accountsID = CreatorData.Instance.CreatorID.ToString();
        data.creator = roleIF.text;
        data.entryDate = entryDateIF.text;
        data.needVersion = needVersionIF.text;
        data.gameLevel = gameLevelIF.text;
        data.comment = commentIF.text;
        data.category_record = categoryDD.options[categoryDD.value].text;
        data.subCategory_record = subCategoryDD.options[subCategoryDD.value].text;
        data.fileDat = fileDatDD.options[fileDatDD.value].text;
        data.fileInputTrace = fileInputTraceDD.options[fileInputTraceDD.value].text;
        data.fileSettings = filesettingDD.options[filesettingDD.value].text;
        data.fileMusic = System.IO.File.ReadAllBytes(musicPath);

        string json = JsonUtility.ToJson(data);

        //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //formData.Add(new MultipartFormDataSection("titleName", "nothing"));

        WWWForm wwwform = new WWWForm();
        wwwform.AddField("accountsID", CreatorData.Instance.CreatorID.ToString());
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
        wwwform.AddBinaryData("fileMusic", File.ReadAllBytes(musicPath), "music.mp3", "audio/wav");


        NetworkingManager.Instance.APIAddRecordData(wwwform, (string resData) =>
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
