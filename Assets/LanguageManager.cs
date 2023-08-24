using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LanguageManager : SingletonMonoBehaviour<LanguageManager>, IPanel
{

    [SerializeField]
    GameObject viewPanel;

    [SerializeField]
    EditLanguage editLanguage;

    [SerializeField]
    Text langFileNameText;

    [SerializeField]
    Dropdown languageDD;

    JsonData jsonData;
    public string langFilePath;

    [SerializeField] Button addBtn, deleteBtn, downloadBtn, uploadBtn;

    public delegate void onSuccess(string Text);    

    public Dictionary<string, NetworkConst.language> languageDic = new Dictionary<string, NetworkConst.language>();
    
    NetworkConst.language _selectedLanguage;
    
    string base_url;


    public JsonData jsonResponse;

    private void Start()
    {
        languageDD.onValueChanged.AddListener(delegate {
            LanguageChanged();
        });

    }
    public void Initialize(string data, string namel, string authtasknamel)
    {
        loadAllLanguage();
        setUpForRole();
    }
    public void setUpForRole()
    {
        bool canAdd = PermissionManager.Instance.GetPermissionForRole1("Language", "add");
        addBtn.interactable = canAdd;

        bool canView = PermissionManager.Instance.GetPermissionForRole1("Language", "view");
        languageDD.interactable = canView;
        

        bool canEdit = PermissionManager.Instance.GetPermissionForRole1("Language", "edit");

        if (!canEdit)
        {
            uploadBtn.interactable = canEdit;
            downloadBtn.interactable = canEdit;
        }
        else
        {
            uploadBtn.interactable = canEdit;
            downloadBtn.interactable = canEdit;
        }


        bool canDelete = PermissionManager.Instance.GetPermissionForRole1("Language", "delete");
        deleteBtn.interactable = canDelete;
    }
        public void LanguageChanged()
    {
        Debug.Log("Language Changed");
        this._selectedLanguage = getLanguageByName(languageDD.options[languageDD.value].text);
    }
    public void addLanguageBtnClicked()
    {
        

        UIPopupManager.Instance.showPopup(UIPopupManager.ePopup.AddLanguage);
    }
    public void editLanguageBtnClicked()
    {
        editLanguage.InitializeData(this._selectedLanguage);
    }

    public void deleteLanguageBtnClicked()
    {
        showDeleteConfirmPopup();
    }
    public void showDeleteConfirmPopup()
    {
        UIPopupManager.Instance.showPopup(UIPopupManager.ePopup.ConirmPopup);

        DeleteConfirmPopup.Instance.initialize(()=> {
            deleteLanguage(this._selectedLanguage.id);
        }, "language");
    }

    
    public void AddNewLanguage(NetworkConst.language tempLanguage)
    {
        if (!languageDic.ContainsKey(tempLanguage.name))
            languageDic.Add(tempLanguage.name, tempLanguage);
        Debug.Log("Load All Language");
        languageTitle.Add(tempLanguage.name);
        Debug.Log(tempLanguage.name);
        languageDD.ClearOptions();
        languageDD.AddOptions(languageTitle);
    }
    public void EditNewLanguage(NetworkConst.language tempLanguage)
    {
        if (languageDic.ContainsKey(this._selectedLanguage.name))
            languageDic.Remove(this._selectedLanguage.name);
        if (!languageDic.ContainsKey(tempLanguage.name))
            languageDic.Add(tempLanguage.name, tempLanguage);

        languageTitle.Remove(this._selectedLanguage.name);
        languageTitle.Add(tempLanguage.name);
        Debug.Log(tempLanguage.name);
        languageDD.ClearOptions();
        languageDD.AddOptions(languageTitle);

        selectNewLanguage(tempLanguage);
    }
    public void deleteNewLanguage(NetworkConst.language tempLanguage)
    {
        if (languageDic.ContainsKey(tempLanguage.name))
            languageDic.Remove(tempLanguage.name);

        languageTitle.Remove(tempLanguage.name);

        languageDD.ClearOptions();
        languageDD.AddOptions(languageTitle);

        selectNewLanguage(languageDic.First().Value);
    }
    public void selectNewLanguage(NetworkConst.language tempLanguage)
    {
        this._selectedLanguage = tempLanguage;
        languageDD.value = languageDD.options.FindIndex(option => option.text == this._selectedLanguage.name);
    }
    
    public void deleLanguage(Language _language)
    {
        Destroy(_language.gameObject);
    }
    public void hideDeleteConfirmPopup()
    {
        UIPopupManager.Instance.HideSelectedPopUp();
    }
    public void downloadFileBtnClicked()
    {
        string fileName = this._selectedLanguage.file;
        string tName = fileName.Substring(0, fileName.LastIndexOf("."));
        if(tName!=this._selectedLanguage.sort_name) Application.OpenURL(base_url + this._selectedLanguage.file);
        else Application.OpenURL(base_url + this._selectedLanguage.sort_name + ".csv");
    }
    //public void selectMusicBtnClicked()
    //{
    //    FileBrowserEg1.Instance.GetLanguagePath(onFileSelected);
    //}
    //public void onFileSelected(string[] paths)
    //{
    //    Debug.Log("Paths is " + paths[0]);
    //    langFilePath = paths[0];
    //    langFileNameText.text = FileBrowserEg1.Instance.getName(langFilePath);
    //}
    public void calldeleteLanguageAPI(Language _music)
    {
        deleteLanguage(_music.ID);
    }
    public void updateBtnClicked()
    {
        PostLanguageData();
    }
    public void gotoMainMenu()
    {
        langFilePath = null;
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
    }
    public NetworkConst.language getLanguageByName(string name)
    {
        Debug.Log(name);
        if (string.IsNullOrEmpty(name)) Debug.Log("Language Name is null--------------");
        return languageDic[name];
    }
    List<string> languageTitle = new List<string>();
    public void updateLanguageDD(NetworkConst.allLanguageRes resLanguage)
    {
        languageDD.ClearOptions();
        languageTitle.Clear();
        foreach (NetworkConst.language _language in resLanguage.data)
        {
            languageTitle.Add(_language.name);
            Debug.Log(_language.name);
        }
        languageDD.AddOptions(languageTitle);
        this._selectedLanguage = resLanguage.data[0];
    }

    

    public void loadAllLanguage()
    {
        NetworkConst.allLanguageRes _allLanguageRes;
        NetworkConst.postLanguage data = new NetworkConst.postLanguage();
        data.limit = 10;
        data.page_no = 1;

        string json = JsonUtility.ToJson(data);

        NetworkingManager.Instance.APILoadAllLanguageData(json, (string data) =>
        {
            _allLanguageRes = JsonUtility.FromJson<NetworkConst.allLanguageRes>(data);
            jsonResponse = JsonMapper.ToObject(data);
            Debug.Log("God Login PLayer Response" + jsonResponse["data"]);
            Debug.Log(jsonResponse);
            if ((bool)jsonResponse["status"])
            {
                
                foreach (NetworkConst.language tmpLanguage in _allLanguageRes.data)
                {
                    if (!languageDic.ContainsKey(tmpLanguage.name))
                        languageDic.Add(tmpLanguage.name, tmpLanguage);
                }
                Debug.Log("Load All Language");
                updateLanguageDD(_allLanguageRes);
                base_url = _allLanguageRes.base_url;
                viewPanel.gameObject.SetActive(true);
            }
            else
            {
                //message.text = jsonResponse["message"].ToString();
                Debug.Log("Message is " + _allLanguageRes.message);
            }

        });
    }
    public void deleteLanguage(int id)
    {
        Debug.Log("ID is " + id);

        NetworkConst.language data = new NetworkConst.language();
        data.appLangID = id;
        string json = JsonUtility.ToJson(data);

        WWWForm wwwform = new WWWForm();
        Debug.Log("ID is " + this._selectedLanguage.id);
        wwwform.AddField("appLangID", this._selectedLanguage.id);

        NetworkingManager.Instance.APIDeleteLanguageData(wwwform, (string resData) =>
        {
            Debug.Log("God Login PLayer Response" + resData);
            jsonResponse = JsonMapper.ToObject(resData);
            if ((bool)jsonResponse["status"])
            {
                Debug.Log("Status is True");
                deleteNewLanguage(this._selectedLanguage);
                UIPopupManager.Instance.HideSelectedPopUp();

            }
            else
            {
                Debug.Log("Message ");
                Debug.Log(jsonResponse["message"].ToString());
            }

        });
    }
    void PostLanguageData()
    {

        NetworkConst.language data = new NetworkConst.language();
        data.name = this._selectedLanguage.name;
        data.sort_name = this._selectedLanguage.sort_name;
        data.csv_file = File.ReadAllBytes(langFilePath);

        string json = JsonUtility.ToJson(data);

        //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //formData.Add(new MultipartFormDataSection("titleName", "nothing"));

        WWWForm wwwform = new WWWForm();
        wwwform.AddField("name", this._selectedLanguage.name);
        wwwform.AddField("sort_name", this._selectedLanguage.sort_name);
        wwwform.AddBinaryData("csv_file", File.ReadAllBytes(langFilePath), this._selectedLanguage.sort_name + ".csv", "text/csv");


        NetworkingManager.Instance.APIEditLanguageData(wwwform, (string resData) =>
        {
            Debug.Log("God Login PLayer Response" + resData);
            jsonData = JsonMapper.ToObject(resData);
            if ((bool)jsonData["status"])
            {
                Debug.Log("Status is True");
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
