using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class EditLanguage : MonoBehaviour
{
    [SerializeField]
    InputField titleNameIF, shortNameIF;

    [SerializeField]
    Text message;

    [SerializeField]
    Text languageNameText;

    JsonData jsonData;
    //Language language;
    NetworkConst.language data,_selectedLanguage;

    public string languagePath;

    public void Start()
    {
     
    }

    private void OnEnable()
    {
        //updateCategoryDD();
    }

    public void InitializeData(NetworkConst.language _language)
    {
        this._selectedLanguage = _language;
        titleNameIF.text = this._selectedLanguage.name;
        languageNameText.text = this._selectedLanguage.file.ToString();

        showPopup();
    }
    public void setData(Language _language)
    {
        NetworkConst.language tmpLanguage;
        tmpLanguage = _language.getLanguage();

        titleNameIF.text = tmpLanguage.name;
        languageNameText.text = tmpLanguage.languageFileData.ToString();

        showPopup();
    }

    public void selectLanguageBtnClicked()
    {
        FileBrowserEg1.Instance.GetLanguagePath(onFileSelected);
    }

    public void onFileSelected(string[] paths)
    {
        Debug.Log("Paths is " + paths[0]);
        languagePath = paths[0];
        string tmpName = FileBrowserEg1.Instance.getName(languagePath);
        Debug.Log(tmpName);
        languageNameText.text = tmpName;
        //languageNameText.gameObject.SetActive(false);
    }

    public void DoneBtnClicked()
    {
        PostLanguageData();
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
        languagePath = null;
  
    }


    //NetworkConst.language data;
    public void updateLanguage()
    {
        //this.language.setLanguage(data);
    }

    public void PostLanguageData()
    {
        NetworkConst.language data;
        data = this._selectedLanguage;
        data.name = titleNameIF.text;
        Debug.Log("Name is " + FileBrowserEg1.Instance.getName(languagePath)+" Path is :"+languagePath);
        if (!string.IsNullOrEmpty(languagePath)) data.csv_file = System.IO.File.ReadAllBytes(languagePath);

        //string json = JsonUtility.ToJson(data);

        //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //formData.Add(new MultipartFormDataSection("titleName", "nothing"));

        WWWForm wwwform = new WWWForm();
        Debug.Log("ID is " + this._selectedLanguage.id);
        wwwform.AddField("appLangID", this._selectedLanguage.id);
        wwwform.AddField("name", titleNameIF.text);
        wwwform.AddField("sort_name", this._selectedLanguage.sort_name);
        if (!string.IsNullOrEmpty(languagePath)) wwwform.AddBinaryData("csv_file", File.ReadAllBytes(languagePath), languageNameText.text, "text / csv");

        NetworkingManager.Instance.APIEditLanguageData(wwwform, (string resData) =>
        {
            Debug.Log("God Login PLayer Response" + resData);
            jsonData = JsonMapper.ToObject(resData);
            if ((bool)jsonData["status"])
            {
                Debug.Log("Status is True");
                LanguageManager.Instance.EditNewLanguage(data);
                closePopup();
            }
            else
            {
                Debug.Log("Message ");
                Debug.Log(jsonData["message"].ToString());
                //message.text = levelJsonData["message"].ToString();
            }

        });
    }
}
