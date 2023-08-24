using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level : MonoBehaviour
{
    public TextMeshProUGUI idText, nameText, commentText, versionText, explanationText, levelCategoryText, levelSubText;
    //int ID;
    string name;
    string serieVersion;
    int creatorAccountID;
    float price;
    bool enabled;
    enum availabilityMode
    {
        Public, ClosedGroup, Keyaccess, InAppPurchase
    }

    DateTime dateCreated;
    string Comment;
    string Settings;
    string Explanations;
    string Category;
    string SubCategory;
    string Tag;
    byte[] previewPicture1;
    string previewText;
    //List<Level> levels;

    [SerializeField]
    Text levelNameText;

    NetworkConst.level _level;

    [SerializeField]
    EditLevel editLevel;

    public void initializeData(NetworkConst.level level)
    {
        this._level = level;
        idText.text = level.id.ToString();
        nameText.text = level.name;
        commentText.text = level.comment;
        versionText.text = level.version;
        explanationText.text = level.explanations;
        levelCategoryText.text = level.level_category;
        levelSubText.text = level.level_subcategory;
        Debug.Log("ID is " + level.id);
        
       // levelNameText.text = level.name;
    }
    public void deleteLevel()
    {
        //seri
    }
    public void setLevel(NetworkConst.level tmpLevel)
    {
        this._level = tmpLevel;
        setlevelname(tmpLevel.name);
    }
    public void setlevelname(string name)
    {
       // levelNameText.text = name;
    }
    public int ID
    {
        get { return this._level.id; }
    }
    public NetworkConst.level getLevel()
    {
        return this._level;
    }
    public void showDeleteConfirmPopup()
    {
        //LevelManager.Instance.setSelectedLevel(this);
        //UIPopupManager.Instance.showPopup(UIPopupManager.ePopup.ConirmPopup);

        UIPopupManager.Instance.showPopup(UIPopupManager.ePopup.ConirmPopup);
        DeleteConfirmPopup.Instance.initialize(confirmedDelete, "level");
    }
    public void showEditPopup()
    {
        LevelManager.Instance.loadAllCategory((string message) => {
            editLevel.InitializeData(this);
        });
        
    }
    public void taskBtnClicked()
    {
        Debug.Log("ID is " + _level.id+_level.name);
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.TaskView, JsonUtility.ToJson(this._level), LevelManager.Instance.autherpassname,this._level.name);
    }

    public void confirmedDelete()
    {
        LevelManager.Instance.calldeleteLevelAPI(this);
    }
}
