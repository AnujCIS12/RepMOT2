using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class AddSeriesPopup : MonoBehaviour
{
    [SerializeField]
    InputField nameIF, commentIF, explanationIF, tagIF, dateCreateIF, serieVersionIF, settingsIF, previewTextIF, priceIF, accessIF,levelIF;

    [SerializeField]
    Dropdown availModeDD, categoryDD, subCategoryDD;

    [SerializeField]
    MultipleSelectionDropDown groupSelectionDD;

    [SerializeField]
    Toggle enableToggle;

    [SerializeField]
    Button selectPictureBtn, addButton;

    [SerializeField]
    Text pictureNameText;

   [SerializeField]
    Text message;

    int availType;

    JsonData seriesJsonData;

    public string picturePath;
    void Start()
    {
        availModeDD.onValueChanged.AddListener(delegate {
            selectRecOptionsDDValueChanged(availModeDD);
        //Code to handle logic of manage Avail DD Changes
        });
        categoryDD.onValueChanged.AddListener(delegate {
            //DropdownValueChanged(m_Dropdown);
            loadSubCategory();
        });
        //loadAllGroup();
        //Debug.Log("Active Self : " + priceIF.gameObject.activeSelf);
    }

    
    private void OnEnable()
    {
        updateCategoryDD();
        setFieldForRole();
    }
    public void DoneBtnClicked()
    {
        PostSeriesData();
    }
    public void closePopup()
    {
        setDefaultField();
        gameObject.SetActive(false);
    }
    public void setDefaultField()
    {
        nameIF.text = "";
        commentIF.text = "";
        explanationIF.text = "";
        tagIF.text = "";
        dateCreateIF.text = "";
        serieVersionIF.text = "";
        settingsIF.text = "";
        priceIF.text = "";
        levelIF.text = "0";
        picturePath = null;
        enableToggle.isOn = false;
        //availModeDD.value = availModeDD.options.FindIndex(option => option.text == "Public");
    }

    public void setFieldForRole()
    {

        
        //nameIF.interactable = false;
        //commentIF.interactable = false;
        //explanationIF.interactable = false;
        //tagIF.interactable = false;
        //dateCreateIF.interactable = false;
        //serieVersionIF.interactable = false;
        //settingsIF.interactable = false;
        //priceIF.interactable = false;
        //levelIF.interactable = false;
        //previewTextIF.interactable = false;

        bool canAddFiles = PermissionManager.Instance.GetPermissionForRole1("Series", "add files");
        selectPictureBtn.interactable = canAddFiles;
        //updateBtn.interactable = false;
        //viewLevelBtn.interactable = false;

        bool canEnable = PermissionManager.Instance.GetPermissionForRole1("Series", "enable");
        enableToggle.interactable = canEnable;

        //categoryDD.interactable = false;
        //subCategoryDD.interactable = false;
        bool canMakeAvail = PermissionManager.Instance.GetPermissionForRole1("series", "availabilityMode");
        availModeDD.interactable = canMakeAvail;
    }

    private void selectRecOptionsDDValueChanged(Dropdown _dd)
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
        priceIF.gameObject.SetActive(false);
        accessIF.gameObject.SetActive(false);
        groupSelectionDD.gameObject.SetActive(false);
    }
    void setForCloseGroupAvail()
    {
        groupSelectionDD.gameObject.SetActive(true);
        priceIF.gameObject.SetActive(false);
        accessIF.gameObject.SetActive(false);
        loadAllGroup();
    }
    void setForKeyAccessAvail()
    {
        groupSelectionDD.gameObject.SetActive(false);
        priceIF.gameObject.SetActive(false);
        accessIF.gameObject.SetActive(true);
    }
    void setForInAppPurchaseAvail()
    {
        //priceIF.gameObject.SetActive(true);
        if(!accessIF.gameObject.activeSelf) accessIF.gameObject.SetActive(true);
        groupSelectionDD.gameObject.SetActive(false);
    }
    void setForPrivateAvail()
    {
        //priceIF.gameObject.SetActive(false);
        accessIF.gameObject.SetActive(false);
        groupSelectionDD.gameObject.SetActive(false);
    }
    public void loadSubCategory()
    {
        SeriesManager.Instance.loadAllSubCategory(categoryDD.options[categoryDD.value].text, (string status) => {
            if (status == "Error") return;
            updateSubCategoryDD();
        });
    }

    //public void loadGroups()
    //{

    //}
    
    public void updateCategoryDD()
    {
        //updateGroupAccessDD();
        categoryDD.ClearOptions();
        List<string> category = new List<string>();
        foreach (KeyValuePair<string, NetworkConst.category> _category in SeriesManager.Instance.categoryDic)
        {
            category.Add(_category.Key);
            Debug.Log(_category.Key);
        }
        categoryDD.AddOptions(category);
        loadSubCategory();
    }
    public void updateSubCategoryDD()
    {
        subCategoryDD.ClearOptions();
        List<string> subCategory = new List<string>();
        foreach (KeyValuePair<string, NetworkConst.subcategory> _subcategory in SeriesManager.Instance.subcategoryDic)
        {
            subCategory.Add(_subcategory.Value.name);
        }
        subCategoryDD.AddOptions(subCategory);
        
    }
    public void updateGroupDD()
    {
        //updateGroupAccessDD();
        groupSelectionDD.ClearOptions();
        List<string> groups = new List<string>();
        foreach (KeyValuePair<string, NetworkConst.groupAccess> _group in SeriesManager.Instance.groupDic)
        {
            groups.Add(_group.Key);
            Debug.Log(_group.Key);
        }
        groupSelectionDD.AddOptions(groups);
        //loadSubCategory();
        //groupSelectionDD.EnableSelectedItemList();
    }
    public string getIDFromList()
    {
        Debug.Log("Selected Item List");
        StringBuilder s = new StringBuilder();
        foreach (string name in groupSelectionDD.selectedItemList)
        {
            Debug.Log(name);
            if (!string.IsNullOrEmpty(name)) s.Append(SeriesManager.Instance.groupDic[name].id.ToString() + ",");
        }
        Debug.Log(s);
        string selectedGroupIds = s.ToString();
        if (s.Length > 1) selectedGroupIds = selectedGroupIds.Remove(selectedGroupIds.Length - 1, 1);
        Debug.Log(selectedGroupIds);
        return selectedGroupIds;
    }
    //public string getIDFromList()
    //{
    //    StringBuilder s = new StringBuilder();
    //    foreach (string name in groupSelectionDD.selectedItemList)
    //    {
    //        //stringbuilder
    //        //string str = SeriesManager.Instance.groupDic()
    //        Debug.Log(name);
    //        s.Append( SeriesManager.Instance.groupDic[name].id.ToString()+",");
    //    }
    //    Debug.Log(s);
    //    return s.ToString();
    //}
    public void selectPictureBtnClicked()
    {
        FileBrowserEg1.Instance.GetImagePath((string[] paths) => {
            Debug.Log("Paths is " + paths[0]);
            picturePath = paths[0];
            pictureNameText.text = FileBrowserEg1.Instance.getName(picturePath);
            //byte[] picturebyte =
            //System.IO.File.ReadAllBytes(picturePath);
        });
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
                SeriesManager.Instance.groupDic.Clear();
                foreach (NetworkConst.groupAccess tmpGroup in _allGroupRes.data)
                {
                    //createNewSeries(tmpSeries);
                    Debug.Log("Group Name " + tmpGroup.name);
                    if (!SeriesManager.Instance.groupDic.ContainsKey(tmpGroup.name))
                        SeriesManager.Instance.groupDic.Add(tmpGroup.name, tmpGroup);
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

    public void PostSeriesData()
    {
        getIDFromList();
        //string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.series data = new NetworkConst.series();
        //data.ID = "1";
        data.name = nameIF.text;
        data.serieVersion = serieVersionIF.text;
        data.creatorAccountID = CreatorData.Instance.CreatorID;
        //data.creatorAccountID = 1;
        data.price = priceIF.text;
        data.enabled = enableToggle.isOn;
        data.availabilityMode = availModeDD.options[availModeDD.value].text;
        //data.dateCreated = DateTime.Now.Date;
        data.comment = commentIF.text;
        data.settings = settingsIF.text;
        data.explanations = explanationIF.text;
        data.level_serie = 0;
        data.level = 20;
        data.category_series = categoryDD.options[categoryDD.value].text;
        data.subCategory_series = subCategoryDD.options[subCategoryDD.value].text;
        data.tag = tagIF.text;
        
        data.previewText = previewTextIF.text;

        WWWForm wwwform = new WWWForm();
        wwwform.AddField("name", nameIF.text);
        wwwform.AddField("serieVersion", serieVersionIF.text);
        wwwform.AddField("creatorAccountID", CreatorData.Instance.CreatorID);
        wwwform.AddField("availabilityMode", availType);
        if (availType == 2) wwwform.AddField("group_id", getIDFromList());
        if (availType == 3) wwwform.AddField("accessKey", priceIF.text);
        if (availType==4) wwwform.AddField("accessKey", priceIF.text);
        wwwform.AddField("enabled", enableToggle.isOn.ToString());
        wwwform.AddField("comment", commentIF.text);
        wwwform.AddField("settings", settingsIF.text);
        //wwwform.AddField("price", 0);
        wwwform.AddField("explanations", explanationIF.text);
        wwwform.AddField("level_serie", levelIF.text);
        wwwform.AddField("level", levelIF.text);
        wwwform.AddField("category_series", categoryDD.options[categoryDD.value].text);
        wwwform.AddField("subCategory_series", subCategoryDD.options[subCategoryDD.value].text);
        wwwform.AddField("tag", tagIF.text);
        if (!string.IsNullOrEmpty(picturePath))
        {
            data.previewPicture = System.IO.File.ReadAllBytes(picturePath);
            wwwform.AddBinaryData("pictures[]", File.ReadAllBytes(picturePath), pictureNameText.text, "jpg/gif");
        }
        
        wwwform.AddField("previewText", previewTextIF.text);

        NetworkConst.newSeriesRes _newSeriesRes;

        string json = JsonUtility.ToJson(data);
        NetworkingManager.Instance.APIAddSeriesData(wwwform, (string resData) =>
        {
            Debug.Log("God Login PLayer Response" + resData);
                _newSeriesRes = JsonUtility.FromJson<NetworkConst.newSeriesRes>(resData);
                seriesJsonData = JsonMapper.ToObject(resData);
                Debug.Log("Status is True");
                SeriesManager.Instance.createNewSeries(_newSeriesRes.data);
                //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);

            ////PlayerData.Instance.PlayerName = name;
            ////PlayerData.Instance.PlayerGUID = playerRowKey;

        });
    }
}
