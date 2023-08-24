using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditLevel : MonoBehaviour
{
    [SerializeField]
    InputField nameIF, commentIF, explanationIF, thresholdIF, dateCreatedIF, levelVersionIF, creatorIDIF, settingIF;

    [SerializeField]
    Toggle enableToggle;

    [SerializeField]
    Dropdown categoryDD, subCategoryDD;

    [SerializeField]
    Text message;

    JsonData levelJsonData;
    Level level;

    private void Start()
    {
        categoryDD.onValueChanged.AddListener(delegate {
            //DropdownValueChanged(m_Dropdown);
            loadSubCategory();
        });
    }

    public void InitializeData(Level _level)
    {
        Debug.Log("ID is " + _level.ID + " id " + _level.getLevel().levelID);
        this.level = _level;
        NetworkConst.level tmpLevel;
        //tmpLevel = _level.getLevel();
        tmpLevel = _level.getLevel();
        nameIF.text = tmpLevel.name;
        commentIF.text = tmpLevel.comment;
        explanationIF.text = tmpLevel.explanations;
        thresholdIF.text = tmpLevel.threshold;
        dateCreatedIF.text = tmpLevel.dateCreated;
        levelVersionIF.text = tmpLevel.version;
        creatorIDIF.text = tmpLevel.creatorAccountID.ToString();
        settingIF.text = tmpLevel.settings;
        enableToggle.isOn = tmpLevel.enabled;
        showPopup();
        updateCategoryDD();
    }

    public void DoneBtnClicked()
    {
        PostLevelData();
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
        thresholdIF.text = "";
        levelVersionIF.text = "";
        dateCreatedIF.text = "";
        commentIF.text = "";
        settingIF.text = "";
        explanationIF.text = "";
        enableToggle.isOn = false;
    }
    public void updateLevel(NetworkConst.level _level)
    {
        this.level.setLevel(_level);
    }

    public void loadSubCategory()
    {
        //SeriesManager.Instance.loadAllSubCategory(this.series.getSeries().category_series, (string status) => {
        //    if (status == "Error") return;
        //    updateSubCategoryDD();
        //});
        subCategoryDD.ClearOptions();
        LevelManager.Instance.loadAllSubCategory(categoryDD.options[categoryDD.value].text, (string status) => {
            if (status == "Error") {
                Debug.LogError("NOt find subcategory");
                return;
            }
            updateSubCategoryDD();
        });

        //string selectedCategory = this.level.getLevel().level_category;
        //if (string.IsNullOrEmpty(selectedCategory) || LevelManager.Instance.getCategoryIDByName(selectedCategory) < 0) selectedCategory = categoryDD.options[categoryDD.value].text;
        //LevelManager.Instance.loadAllSubCategory(selectedCategory, (string status) => {
        //    if (status == "Error") return;
        //    updateSubCategoryDD();
        //});
    }
    public void updateCategoryDD()
    {
        //updateGroupAccessDD();
        categoryDD.ClearOptions();
        List<string> category = new List<string>();
        foreach (KeyValuePair<string, NetworkConst.category> _category in LevelManager.Instance.categoryDic)
        {
            category.Add(_category.Key);
            Debug.Log(_category.Key);
        }
        categoryDD.AddOptions(category);
        categoryDD.value = categoryDD.options.FindIndex(option => option.text == this.level.getLevel().level_category);
        loadSubCategory();
    }
    public void updateSubCategoryDD()
    {
        subCategoryDD.ClearOptions();
        List<string> subCategory = new List<string>();
        foreach (KeyValuePair<string, NetworkConst.subcategory> _subcategory in LevelManager.Instance.subcategoryDic)
        {
            subCategory.Add(_subcategory.Value.name);
        }
        subCategoryDD.AddOptions(subCategory);
        subCategoryDD.value = subCategoryDD.options.FindIndex(option => option.text == this.level.getLevel().level_subcategory);
    }
    NetworkConst.level data;
    public void PostLevelData()
    {
        //string playerRowKey = System.Guid.NewGuid().ToString();
        data = new NetworkConst.level();
        data.id = this.level.getLevel().id;
        data.seriesID = this.level.getLevel().seriesID;
        data.levelID = this.level.getLevel().id;
        Debug.Log("ID at submitting time " + data.levelID);
        data.name = nameIF.text;
        data.version = levelVersionIF.text;
        data.creatorAccountID = CreatorData.Instance.CreatorID;
        //data.creatorAccountID = 1;
        data.enabled = enableToggle.isOn;
        data.threshold = thresholdIF.text;
        data.dateCreated = dateCreatedIF.text;
        data.comment = commentIF.text;
        data.settings = settingIF.text;
        data.explanations = explanationIF.text;
        if (categoryDD.options.Count > 0) data.level_category = categoryDD.options[categoryDD.value].text;
        if(subCategoryDD.options.Count>0) data.level_subcategory = subCategoryDD.options[subCategoryDD.value].text;

        string json = JsonUtility.ToJson(data);
        NetworkingManager.Instance.APIEditLevelData(json, (string resData) =>
        {
            Debug.Log("God Login PLayer Response" + resData);
            levelJsonData = JsonMapper.ToObject(resData);
            if ((bool)levelJsonData["status"])
            {
                Debug.Log("Status is True");
                //LevelManager.Instance.createNewLevel(data);
                //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
                NetworkConst.newLevelRes _newLevelRes;
                _newLevelRes = JsonUtility.FromJson<NetworkConst.newLevelRes>(resData);
                updateLevel(_newLevelRes.data);
                closePopup();
            }
            else
            {
                Debug.Log("Message ");
                Debug.Log(levelJsonData["message"].ToString());
                //message.text = levelJsonData["message"].ToString();
            }

            ////PlayerData.Instance.PlayerName = name;
            ////PlayerData.Instance.PlayerGUID = playerRowKey;

        });
    }
}
