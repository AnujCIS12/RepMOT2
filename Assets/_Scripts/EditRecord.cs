using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class EditRecord : MonoBehaviour
{
    [SerializeField]
    InputField nameIF, creatorIF, entryDateIF, needVersionIF, gameLevelIF, commentIF, accessIF;

    [SerializeField]
    Toggle privateToggle;

    [SerializeField]
    MultipleSelectionDropDown groupSelectionDD;

    [SerializeField]
    Dropdown categoryDD, subCategoryDD, fileInputTraceDD, filesettingDD, availModeDD;

    //[SerializeField]
    //Dropdown fileDatDD;

    [SerializeField]
    Text FileDatBtn;

    [SerializeField]
    Text message;

    [SerializeField]
    Text musicNameText;

    JsonData jsonData;
    public Record obj;
    NetworkConst.record1 data;

    public string musicPath;

    int availType;

    private void Awake()
    {
        availModeDD.onValueChanged.AddListener(delegate
        {
            selectAvailOptionsDDValueChanged(availModeDD);
            //Code to handle logic of manage Avail DD Changes
        });
    }
    private void Start()
    {
        categoryDD.onValueChanged.AddListener(delegate {
            //DropdownValueChanged(m_Dropdown);
            loadSubCategory();
        });
    }

    public void InitializeData(Record _obj)
    {
        this.obj = _obj;
        NetworkConst.record1 tmpObj;
        tmpObj = _obj.getStructObj();

        nameIF.text = tmpObj.name;
        creatorIF.text = tmpObj.creator;
        entryDateIF.text = tmpObj.entryDate;
        needVersionIF.text = tmpObj.needVersion;
        gameLevelIF.text = tmpObj.gameLevel;
        commentIF.text = tmpObj.comment;
        accessIF.text = tmpObj.accessKey;
        categoryDD.value = categoryDD.options.FindIndex(option => option.text == tmpObj.category_record);
        subCategoryDD.value = subCategoryDD.options.FindIndex(option => option.text == tmpObj.subCategory_record);
        //fileDatDD.value = fileDatDD.options.FindIndex(option => option.text == tmpObj.fileDat);
        FileDatBtn.text = tmpObj.fileDat;
        fileInputTraceDD.value = fileInputTraceDD.options.FindIndex(option => option.text == tmpObj.fileInputTrace);
        filesettingDD.value = filesettingDD.options.FindIndex(option => option.text == tmpObj.fileSettings);
        privateToggle.isOn = false;
        int availMode;
        int.TryParse(tmpObj.availabilityMode, out availMode);
        showPopup();

        updateCategoryDD();
        availModeDD.value = availMode-1;
        setUpForRole();
    }

    public void setUpForRole()
    {
        bool canEdit = PermissionManager.Instance.GetPermissionForRole1("Record", "edit");

        if (!canEdit)
        {
            nameIF.interactable = false;
            creatorIF.interactable = false;
            entryDateIF.interactable = false;
            needVersionIF.interactable = false;
            gameLevelIF.interactable = false;
            commentIF.interactable = false;
            accessIF.interactable = false;

            privateToggle.interactable = false;

            categoryDD.interactable = false;
            subCategoryDD.interactable = false;
            fileInputTraceDD.interactable = false;
            filesettingDD.interactable = false;
            availModeDD.interactable = false;
            groupSelectionDD.GetComponent<Button>().interactable = false;
        }
        else
        {
            nameIF.interactable = true;
            creatorIF.interactable = true;
            entryDateIF.interactable = true;
            needVersionIF.interactable = true;
            gameLevelIF.interactable = true;
            commentIF.interactable = true;
            accessIF.interactable = true;

            bool canEnable = PermissionManager.Instance.GetPermissionForRole1("Record", "publish/unpublish/enable");
            privateToggle.interactable = canEnable;

            categoryDD.interactable = true;
            subCategoryDD.interactable = true;
            fileInputTraceDD.interactable = true;
            filesettingDD.interactable = true;
            availModeDD.interactable = PermissionManager.Instance.GetPermissionForRole1("Record", "publish/unpublish/enable");
            groupSelectionDD.GetComponent<Button>().interactable = canEnable;
        }
            
    }
    public void loadSubCategory()
    {
        Debug.Log("Load subcategory");

        subCategoryDD.ClearOptions();
        RecordManager.Instance.loadAllSubCategory(categoryDD.options[categoryDD.value].text, (string status) => {
            if (status == "Error")
            {
                Debug.LogError("NOt find subcategory");
                return;
            }
            updateSubCategoryDD();
        });


        //string selectedCategory = this.obj.getStructObj().category_record;
        //if (string.IsNullOrEmpty(selectedCategory) || RecordManager.Instance.getCategoryIDByName(selectedCategory) < 0) selectedCategory = categoryDD.options[categoryDD.value].text;
        //RecordManager.Instance.loadAllSubCategory(selectedCategory, (string status) => {
        //    if (status == "Error") return;
        //    updateSubCategoryDD();
        //});
    }
    public void updateCategoryDD()
    {
        //updateGroupAccessDD();
        categoryDD.ClearOptions();
        List<string> category = new List<string>();
        foreach (KeyValuePair<string, NetworkConst.category> _category in RecordManager.Instance.categoryDic)
        {
            category.Add(_category.Key);
            Debug.Log(_category.Key);
        }
        categoryDD.AddOptions(category);
        categoryDD.value = categoryDD.options.FindIndex(option => option.text == this.obj.getStructObj().category_record);
        loadSubCategory();
    }

    public void updateSubCategoryDD()
    {
        subCategoryDD.ClearOptions();
        List<string> subCategory = new List<string>();
        foreach (KeyValuePair<string, NetworkConst.subcategory> _subcategory in RecordManager.Instance.subcategoryDic)
        {
            subCategory.Add(_subcategory.Value.name);
        }
        subCategoryDD.AddOptions(subCategory);
        subCategoryDD.value = subCategoryDD.options.FindIndex(option => option.text == this.obj.getStructObj().subCategory_record);
    }
    public void updateGroupAccessDD()
    {
        //groupAccessDD.ClearOptions();
        //List<string> groupAccess = new List<string>();
        //foreach (KeyValuePair<string, NetworkConst.groupAccess> _groupAccess in GameManager.Instance.groupAccessDic)
        //{
        //    groupAccess.Add(_groupAccess.Value.name);
        //}
        //groupAccessDD.AddOptions(groupAccess);
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

    public void UpdateBtnClicked()
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
        Debug.Log("Set Default Field");
        nameIF.text = "";
        creatorIF.text = "";
        entryDateIF.text = "";
        needVersionIF.text = "";
        gameLevelIF.text = "";
        commentIF.text = "";
        accessIF.text = "";
        categoryDD.value = categoryDD.options.FindIndex(option => option.text == "Category");
        subCategoryDD.value = subCategoryDD.options.FindIndex(option => option.text == "Sub Category");
        //fileDatDD.value = fileDatDD.options.FindIndex(option => option.text == "FileDat");
        FileDatBtn.text = "";
        fileInputTraceDD.value = fileInputTraceDD.options.FindIndex(option => option.text == "File Input Trace");
        filesettingDD.value = filesettingDD.options.FindIndex(option => option.text == "File Setting");
        privateToggle.isOn = false;
    }
    private void selectAvailOptionsDDValueChanged(Dropdown _dd)
    {
        availType = _dd.value+1;
        Debug.Log("Value Changed " + _dd.value);
        switch (availType)
        {
            case 1:
                setForPublicAvail();
                break;
            case 2:
                setForCloseGroupAvail();
                break;
            case 3:
                setForKeyAccessAvail();
                break;
            case 4:
                setForInAppPurchaseAvail();
                break;
            case 5:
                setForPrivateAvail();
                break;
        }
    }
    void setForPublicAvail()
    {
        accessIF.gameObject.SetActive(false);
        groupSelectionDD.gameObject.SetActive(false);
    }
    void setForCloseGroupAvail()
    {
        groupSelectionDD.gameObject.SetActive(true);
        accessIF.gameObject.SetActive(false);
        loadAllGroup();
    }
    void setForKeyAccessAvail()
    {
        groupSelectionDD.gameObject.SetActive(false);
        accessIF.gameObject.SetActive(true);
    }
    void setForInAppPurchaseAvail()
    {
        //priceIF.gameObject.SetActive(true);
        if (!accessIF.gameObject.activeSelf) accessIF.gameObject.SetActive(true);
        groupSelectionDD.gameObject.SetActive(false);
    }
    void setForPrivateAvail()
    {
        //priceIF.gameObject.SetActive(false);
        accessIF.gameObject.SetActive(false);
        groupSelectionDD.gameObject.SetActive(false);
    }

    //NetworkConst.music data;
    public void updateObj(NetworkConst.record1 _record1)
    {
        Debug.Log("Update Obj" + _record1.name);
        this.obj.setRecord(_record1);
    }

    public void updateGroupDD()
    {
        //updateGroupAccessDD();
        groupSelectionDD.ClearOptions();
        List<string> groups = new List<string>();
        foreach (KeyValuePair<string, NetworkConst.groupAccess> _group in RecordManager.Instance.groupDic)
        {
            groups.Add(_group.Key);
            Debug.Log(_group.Key);
        }
        groupSelectionDD.AddOptions(groups);
        updateSelectedGroup();
    }
    //public void updateSelectedGroup()
    //{
    //    string[] ids = obj.getStructObj().groupIds.Split(',');
    //    List<string> items = new List<string>();
    //    for (int i = 0; i < ids.Length - 1; i++)
    //    {
    //        int id;
    //        NetworkConst.groupAccess _groupAccess;
    //        int.TryParse(ids[i], out id);
    //        RecordManager.Instance.groupDic1.TryGetValue(id, out _groupAccess);
    //        items.Add(_groupAccess.name);
    //    }
    //    groupSelectionDD.EnableSelectedItemList(items);
    //}
    //public string getIDFromList()
    //{
    //    StringBuilder s = new StringBuilder();
    //    foreach (string name in groupSelectionDD.selectedItemList)
    //    {
    //        //stringbuilder
    //        //string str = RecordManager.Instance.groupDic()
    //        Debug.Log(name);
    //        s.Append(RecordManager.Instance.groupDic[name].id.ToString() + ",");
    //    }
    //    Debug.Log(s);
    //    return s.ToString();
    //}

    public void updateSelectedGroup()
    {
        string[] ids = obj.getStructObj().groupIds.Split(',');
        Debug.Log("Group iDS array Lenght : " + ids.Length);
        List<string> items = new List<string>();
        for (int i = 0; i < ids.Length; i++)
        {
            int id;
            NetworkConst.groupAccess _groupAccess;
            int.TryParse(ids[i], out id);
            RecordManager.Instance.groupDic1.TryGetValue(id, out _groupAccess);
            items.Add(_groupAccess.name);
        }
        groupSelectionDD.EnableSelectedItemList(items);
    }
    public string getIDFromList()
    {
        Debug.Log("Selected Item List");
        StringBuilder s = new StringBuilder();
        foreach (string name in groupSelectionDD.selectedItemList)
        {
            Debug.Log(name);
            if (!string.IsNullOrEmpty(name)) s.Append(RecordManager.Instance.groupDic[name].id.ToString() + ",");
        }
        Debug.Log(s);
        string selectedGroupIds = s.ToString();
        if (s.Length > 1) selectedGroupIds = selectedGroupIds.Remove(selectedGroupIds.Length - 1, 1);
        Debug.Log(selectedGroupIds);
        return selectedGroupIds;
    }
    public void loadAllGroup()
    {
        NetworkConst.allGroupAccessRes _allGroupRes;
        //string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.postSeries data = new NetworkConst.postSeries();
        data.limit = 10;
        data.page_no = 1;

        WWWForm wwwform = new WWWForm();
        wwwform.AddField("limit", 100);
        wwwform.AddField("page_no", 1);
        string json = JsonUtility.ToJson(data);


        NetworkingManager.Instance.APILoadAllGroupListData(wwwform, (string data) =>
        {
            _allGroupRes = JsonUtility.FromJson<NetworkConst.allGroupAccessRes>(data);
            JsonData jsonResponse;
            jsonResponse = JsonMapper.ToObject(data);
            Debug.Log("God Login PLayer Response" + jsonResponse["data"]);
            Debug.Log(jsonResponse);
            if ((bool)jsonResponse["status"])
            {
                //CreatorData.Instance.setCreatorData(this._loginRes.data);
                //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
                //Debug.Log("Name " + _allGroupRes.data[0].name);
                RecordManager.Instance.groupDic.Clear();
                RecordManager.Instance.groupDic1.Clear();
                foreach (NetworkConst.groupAccess tmpGroup in _allGroupRes.data)
                {
                    //createNewSeries(tmpSeries);
                    Debug.Log("Group Name " + tmpGroup.name);
                    if (!RecordManager.Instance.groupDic.ContainsKey(tmpGroup.name))
                    {
                        RecordManager.Instance.groupDic.Add(tmpGroup.name, tmpGroup);
                        RecordManager.Instance.groupDic1.Add(tmpGroup.id, tmpGroup);
                    }

                }
                updateGroupDD();
            }
            else
            {
                //message.text = jsonResponse["message"].ToString();
                Debug.Log("Message is " + _allGroupRes.message);
            }

        });
    }
    public void PostObjData()
    {
        //NetworkConst.record1 data = new NetworkConst.record1();
        //data.ID = "1";
        data.recordID = this.obj.ID;
        data.accountsID = CreatorData.Instance.CreatorID.ToString();
        data.name = nameIF.text;
        data.creator = creatorIF.text;
        data.entryDate = entryDateIF.text;
        data.needVersion = needVersionIF.text;
        data.gameLevel = gameLevelIF.text;
        data.comment = commentIF.text;
        data.accessKey = accessIF.text;
        data.category_record = categoryDD.options[categoryDD.value].text;
        data.subCategory_record = subCategoryDD.options[subCategoryDD.value].text;
        //data.fileDat = fileDatDD.options[fileDatDD.value].text;
        //data.fileInputTrace = fileInputTraceDD.options[fileInputTraceDD.value].text;
        //data.fileSettings = filesettingDD.options[filesettingDD.value].text;
        if (!string.IsNullOrEmpty(musicPath)) data.fileMusic = System.IO.File.ReadAllBytes(musicPath);

        string json = JsonUtility.ToJson(data);

        //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //formData.Add(new MultipartFormDataSection("titleName", "nothing"));

        WWWForm wwwform = new WWWForm();
        wwwform.AddField("recordID", this.obj.ID);
        wwwform.AddField("accountsID", CreatorData.Instance.CreatorID.ToString());
        wwwform.AddField("role", "author");
        wwwform.AddField("name", nameIF.text);
        wwwform.AddField("creator", this.obj.getStructObj().accountsID);
        wwwform.AddField("entryDate", entryDateIF.text);
        wwwform.AddField("needVersion", needVersionIF.text);
        wwwform.AddField("gameLevel", gameLevelIF.text);
        wwwform.AddField("comment", commentIF.text);
        wwwform.AddField("availabilityMode", availType);
        Debug.Log("Selecte ID is "+ getIDFromList());
        if (availType == 2) wwwform.AddField("group_id", getIDFromList());
        if (availType == 3) wwwform.AddField("accessKey", accessIF.text);
        if (availType == 4) wwwform.AddField("accessKey", accessIF.text);
        wwwform.AddField("public", 1);
        wwwform.AddField("category_record", categoryDD.options[categoryDD.value].text);
        wwwform.AddField("subCategory_record", subCategoryDD.options[subCategoryDD.value].text);
        wwwform.AddField("fileDat", this.obj.getStructObj().fileDat);
        //wwwform.AddField("fileInputTrace", fileInputTraceDD.options[fileInputTraceDD.value].text);
        //wwwform.AddField("fileSettings", filesettingDD.options[filesettingDD.value].text);
        //if (!string.IsNullOrEmpty(musicPath)) wwwform.AddBinaryData("musicFile", File.ReadAllBytes(musicPath), "music.mp3", "audio/wav");

        NetworkingManager.Instance.APIEditRecordData(wwwform, (string resData) =>
        {
            Debug.Log("God Login PLayer Response" + resData);
            jsonData = JsonMapper.ToObject(resData);
            if ((bool)jsonData["status"])
            {
                Debug.Log("Status is True");
                NetworkConst.newRecordRes _newRecordRes;
                _newRecordRes = JsonUtility.FromJson<NetworkConst.newRecordRes>(resData);
                updateObj(_newRecordRes.data);
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
