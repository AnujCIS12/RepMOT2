using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddLevelPopup : MonoBehaviour
{
    [SerializeField]
    InputField nameIF, commentIF, explanationIF, thresholdIF, dateCreatedIF, levelVersionIF, creatorIDIF, settingIF;

    [SerializeField]
    Dropdown categoryDD, subCategoryDD;

    [SerializeField]
    Toggle enableToggle;

    [SerializeField]
    Text message;

    JsonData seriesJsonData;
    public static string seriesVersion;
    void Start()
    {
        categoryDD.onValueChanged.AddListener(delegate {
            loadSubCategory();
        });
    }
    private void OnEnable()
    {
        updateCategoryDD();
        //setFieldForRole();
    }
    public static void setSeriesVersion(string data)
    {
        Debug.Log("Series Version is " + data);
        seriesVersion = data;
    }

    // Update is called once per frame
    void Update()
    {

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

    public void loadSubCategory()
    {
        LevelManager.Instance.loadAllSubCategory(categoryDD.options[categoryDD.value].text, (string status) => {
            if (status == "Error") return;
            updateSubCategoryDD();
        });
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

    }

    public void PostLevelData()
    {
        //string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.level data = new NetworkConst.level();
        //data.ID = "1";
        data.name = nameIF.text;
        data.levelVersion = levelVersionIF.text;
        data.seriesID = seriesVersion;
        data.threshold = thresholdIF.text;
        data.version = levelVersionIF.text;
        data.creatorAccountID = CreatorData.Instance.CreatorID;
        data.sinceDate = DateTime.Now.Date.ToString();
        data.comment = commentIF.text;
        data.settings = settingIF.text;
        data.explanations = explanationIF.text;
        if (categoryDD.options.Count > 0) data.level_category = categoryDD.options[categoryDD.value].text;
        if (subCategoryDD.options.Count > 0) data.level_subcategory = subCategoryDD.options[subCategoryDD.value].text;


        string json = JsonUtility.ToJson(data);
        NetworkingManager.Instance.APIAddLevelData(json, (string resData) =>
        {
            Debug.Log("God Login PLayer Response" + resData);
            seriesJsonData = JsonMapper.ToObject(resData);
            if ((bool)seriesJsonData["status"])
            {
                Debug.Log("Status is True");
                NetworkConst.newLevelRes _newLevelRes;
                _newLevelRes = JsonUtility.FromJson<NetworkConst.newLevelRes>(resData);
                LevelManager.Instance.createNewLevel(_newLevelRes.data);
                //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
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
