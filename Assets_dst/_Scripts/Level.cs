using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
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
        levelNameText.text = level.name;
    }
    public void deleteLevel()
    {
        //seri
    }
    public void setLevel(NetworkConst.level tmpLevel)
    {
        this._level = tmpLevel;
    }
    public int ID
    {
        get { return this._level.levelID; }
    }
    public NetworkConst.level getLevel()
    {
        return this._level;
    }
    public void showDeleteConfirmPopup()
    {
        LevelManager.Instance.setSelectedLevel(this);
        UIPopupManager.Instance.showPopup(UIPopupManager.ePopup.ConirmPopup);
    }
    public void showEditPopup()
    {
        editLevel.InitializeData(this);
    }
}
