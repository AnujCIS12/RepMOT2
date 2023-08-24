using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Series:MonoBehaviour
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
    int Level;
    string Category; 
    string SubCategory;
    string Tag;
    byte[] previewPicture1;
    string previewText;
    //List<Level> levels;

    [SerializeField]
    Text seriesNameText;

    NetworkConst.series _series;

    [SerializeField]
    EditSeries editSeries;

    public void initializeData(NetworkConst.series series)
    {
        this._series = series;
        seriesNameText.text = series.name;
    }
    public void deleteSeries()
    {
        //seri
    }
    public void seriesBtnClicked()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.LevelView, this._series.serieVersion);
    }
    public void setSeries(NetworkConst.series tmpSeries)
    {
        this._series = tmpSeries;
    }
    public int ID
    {
        get { return this._series.id; }
    }
    public NetworkConst.series getSeries()
    {
        return this._series;
    }
    public void showDeleteConfirmPopup()
    {
        SeriesManager.Instance.setSelectedSeries(this);
        UIPopupManager.Instance.showPopup(UIPopupManager.ePopup.ConirmPopup);
    }
    public void showEditPopup()
    {
        editSeries.InitializeData(this);
    }
}
