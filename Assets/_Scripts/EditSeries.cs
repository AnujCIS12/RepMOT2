using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class EditSeries : MonoBehaviour
{
    [SerializeField]
    InputField nameIF, commentIF, explanationIF, tagIF, dateCreateIF, serieVersionIF, settingIF, priceIF, previewTextIF, accessIF,levelIF;

    [SerializeField]
    Dropdown availModeDD, categoryDD, subCategoryDD;

    [SerializeField]
    MultipleSelectionDropDown groupSelectionDD;

    [SerializeField]
    Toggle enableToggle;

    [SerializeField] Button selectPictureBtn,updateBtn,viewLevelBtn;

    [SerializeField]
    Text message, pictureNameText;

    public string picturePath;

    JsonData seriesJsonData;
    Series series;

    int availType;

    private void Awake()
    {
        availModeDD.onValueChanged.AddListener(delegate
        {
            selectAvailOptionsDDValueChanged(availModeDD);
            //Code to handle logic of manage Avail DD Changes
        });
    }
    public void Start()
    {
        availModeDD.onValueChanged.AddListener(delegate
        {
            selectAvailOptionsDDValueChanged(availModeDD);
            //Code to handle logic of manage Avail DD Changes
        });
        categoryDD.onValueChanged.AddListener(delegate {
            //DropdownValueChanged(m_Dropdown);
            loadSubCategory();
        });
    }

    public void InitializeData(Series _series)
    {
        this.series = _series;
        NetworkConst.series tmpSeries;
        tmpSeries = _series.getSeries();
        nameIF.text = tmpSeries.name;
        commentIF.text = tmpSeries.comment;
        explanationIF.text = tmpSeries.explanations;
        tagIF.text = tmpSeries.tag;
        dateCreateIF.text = tmpSeries.dateCreated.ToString();
        serieVersionIF.text = tmpSeries.serieVersion;
        settingIF.text = tmpSeries.settings;
        priceIF.text = tmpSeries.price;
        levelIF.text = tmpSeries.level.ToString();
        previewTextIF.text = tmpSeries.previewText;
        enableToggle.isOn = tmpSeries.enabled;
        int availMode;
        int.TryParse(tmpSeries.availabilityMode,out availMode);
        Debug.Log("*************Avail Model values is " + availMode);
        //availModeDD.value = availModeDD.options[availMode];
        
        //availModeDD.gameObject.SetActive(false);
        showPopup();
        updateCategoryDD();
        availModeDD.value = availMode-1;
        setUpForRole();
    }

    public void loadSubCategory()
    {
        //SeriesManager.Instance.loadAllSubCategory(this.series.getSeries().category_series, (string status) => {
        //    if (status == "Error") return;
        //    updateSubCategoryDD();
        //});

        subCategoryDD.ClearOptions();
        SeriesManager.Instance.loadAllSubCategory(categoryDD.options[categoryDD.value].text, (string status) => {
            if (status == "Error")
            {
                Debug.LogError("NOt find subcategory");
                return;
            }
            updateSubCategoryDD();
        });

        //string selectedCategory = this.series.getSeries().category_series;
        //if (string.IsNullOrEmpty(selectedCategory) || SeriesManager.Instance.getCategoryIDByName(selectedCategory) < 0) selectedCategory = categoryDD.options[categoryDD.value].text;
        //SeriesManager.Instance.loadAllSubCategory(selectedCategory, (string status) => {
        //    if (status == "Error") return;
        //    updateSubCategoryDD();
        //});
    }

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
        categoryDD.value = categoryDD.options.FindIndex(option => option.text == this.series.getSeries().category_series);
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
        subCategoryDD.value = subCategoryDD.options.FindIndex(option => option.text == this.series.getSeries().subCategory_series);
    }

    public void DoneBtnClicked()
    {
        PostSeriesData();
    }
    public void LevelBtnClicked()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.LevelView, this.series.getSeries().id.ToString());
        closePopup();
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
        commentIF.text = "";
        explanationIF.text = "";
        tagIF.text = "";
        dateCreateIF.text = "";
        serieVersionIF.text = "";
        settingIF.text = "";
        priceIF.text = "";
        levelIF.text = "0";
        previewTextIF.text = "";
        picturePath = null;
        enableToggle.isOn = false;
        availModeDD.value = 0;
    }
    public void setUpForRole()
    {

        bool canEdit = PermissionManager.Instance.GetPermissionForRole1("Series", "edit");
        //Debug.LogError("Can Edit " + canEdit);

        if (!canEdit)
        {
            nameIF.interactable = false;
            commentIF.interactable = false;
            explanationIF.interactable = false;
            tagIF.interactable = false;
            dateCreateIF.interactable = false;
            serieVersionIF.interactable = false;
            settingIF.interactable = false;
            priceIF.interactable = false;
            levelIF.interactable = false;
            previewTextIF.interactable = false;

            selectPictureBtn.interactable = false;
            updateBtn.interactable = false;
            viewLevelBtn.interactable = false;

            enableToggle.interactable = false;

            categoryDD.interactable = false;
            subCategoryDD.interactable = false;

            availModeDD.interactable = false;
        }
        else
        {
                nameIF.interactable = true;
                commentIF.interactable = true;
                explanationIF.interactable = true;
                tagIF.interactable = true;
                dateCreateIF.interactable = true;
                serieVersionIF.interactable = true;
                settingIF.interactable = true;
                priceIF.interactable = true;
                levelIF.interactable = true;
                previewTextIF.interactable = true;

                bool canAddFiles = PermissionManager.Instance.GetPermissionForRole1("Series", "add files");
                selectPictureBtn.interactable = canAddFiles;
                updateBtn.interactable = true;
                viewLevelBtn.interactable = true;

                bool canEnable = PermissionManager.Instance.GetPermissionForRole1("Series", "enable");
                enableToggle.interactable = canEnable;

                categoryDD.interactable = true;
                subCategoryDD.interactable = true;

                availModeDD.interactable = PermissionManager.Instance.GetPermissionForRole1("Series", "publish/unpublish/enable");
        }
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
        if (!accessIF.gameObject.activeSelf) accessIF.gameObject.SetActive(true);
        groupSelectionDD.gameObject.SetActive(false);
    }
    void setForPrivateAvail()
    {
        //priceIF.gameObject.SetActive(false);
        accessIF.gameObject.SetActive(false);
        groupSelectionDD.gameObject.SetActive(false);
    }

    public void updateSeries(NetworkConst.series _data)
    {
        this.series.setSeries(_data);
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
        updateSelectedGroup();
    }
    //public void updateSelectedGroup()
    //{
    //    string[] ids = series.getSeries().groupIds.Split(',');
    //    List<string> items = new List<string>();
    //    for(int i=0;i<ids.Length-1;i++)
    //    {
    //        int id;
    //        NetworkConst.groupAccess _groupAccess;
    //        int.TryParse(ids[i],out id);
    //        SeriesManager.Instance.groupDic1.TryGetValue(id, out _groupAccess);
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
    //        //string str = SeriesManager.Instance.groupDic()
    //        Debug.Log(name);
    //        if (!string.IsNullOrEmpty(name)) s.Append(SeriesManager.Instance.groupDic[name].id.ToString() + ",");
    //    }
    //    Debug.Log(s);
    //    return s.ToString();
    //}

    public void updateSelectedGroup()
    {
        string[] ids = series.getSeries().groupIds.Split(',');
        Debug.Log("Group iDS array Lenght : " + ids.Length);
        List<string> items = new List<string>();
        for (int i = 0; i < ids.Length; i++)
        {
            int id;
            NetworkConst.groupAccess _groupAccess;
            int.TryParse(ids[i], out id);
            SeriesManager.Instance.groupDic1.TryGetValue(id, out _groupAccess);
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
            if (!string.IsNullOrEmpty(name)) s.Append(SeriesManager.Instance.groupDic[name].id.ToString() + ",");
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
                SeriesManager.Instance.groupDic.Clear();
                SeriesManager.Instance.groupDic1.Clear();
                foreach (NetworkConst.groupAccess tmpGroup in _allGroupRes.data)
                {
                    //createNewSeries(tmpSeries);
                    Debug.Log("Group Name " + tmpGroup.name);
                    if (!SeriesManager.Instance.groupDic.ContainsKey(tmpGroup.name))
                    {
                        SeriesManager.Instance.groupDic.Add(tmpGroup.name, tmpGroup);
                        SeriesManager.Instance.groupDic1.Add(tmpGroup.id, tmpGroup);
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

    NetworkConst.series data;
    public void PostSeriesData()
    {
        //string playerRowKey = System.Guid.NewGuid().ToString();
        data = new NetworkConst.series();
        data.id = this.series.ID;
        data.name = nameIF.text;
        data.serieVersion = serieVersionIF.text;
        data.creatorAccountID = CreatorData.Instance.CreatorID;
        //data.creatorAccountID = 1;
        data.price = priceIF.text;
        data.enabled = enableToggle.isOn;
        //data.availabilityMode = availModeDD.value;
        data.dateCreated = dateCreateIF.text.ToString();
        data.comment = commentIF.text;
        data.settings = "a";
        data.explanations = explanationIF.text;
        //int.TryParse(serieVersionIF.text,out data.level_serie);
        //data.level_serie = serieVersionIF.text.ToString();
        data.category_series = categoryDD.options[categoryDD.value].text;
        data.subCategory_series = subCategoryDD.options[subCategoryDD.value].text;
        data.tag = tagIF.text;
        
        
        data.previewText = previewTextIF.text;

        WWWForm wwwform = new WWWForm();
        wwwform.AddField("id", this.series.ID);
        wwwform.AddField("name", nameIF.text);
        wwwform.AddField("serieVersion", serieVersionIF.text);
        wwwform.AddField("creatorAccountID", CreatorData.Instance.CreatorID);
        wwwform.AddField("price", priceIF.text);
        wwwform.AddField("enabled", enableToggle.isOn.ToString());
        wwwform.AddField("availabilityMode", availType);
        if (availType == 2) wwwform.AddField("group_id", getIDFromList());
        if (availType == 3) wwwform.AddField("accessKey", priceIF.text);
        if (availType == 4) wwwform.AddField("accessKey", priceIF.text);
        wwwform.AddField("comment", commentIF.text);
        wwwform.AddField("settings", settingIF.text);

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
        NetworkingManager.Instance.APIEditSeriesData(wwwform, (string resData) =>
        {
            Debug.Log("God Login PLayer Response" + resData);
            seriesJsonData = JsonMapper.ToObject(resData);
            if ((bool)seriesJsonData["status"])
            {
                Debug.Log("Status is True");
                //SeriesManager.Instance.createNewSeries(data);
                //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
                _newSeriesRes = JsonUtility.FromJson<NetworkConst.newSeriesRes>(resData);
                seriesJsonData = JsonMapper.ToObject(resData);
                updateSeries(_newSeriesRes.data);
                closePopup();
            }
            else
            {
                Debug.Log("Message ");
                Debug.Log(seriesJsonData["message"].ToString());
                //message.text = seriesJsonData["message"].ToString();
            }

            ////PlayerData.Instance.PlayerName = name;
            ////PlayerData.Instance.PlayerGUID = playerRowKey;

        });
    }
}
