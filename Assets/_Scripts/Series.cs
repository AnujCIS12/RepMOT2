using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LitJson;

public class Series:MonoBehaviour
{
    enum availabilityMode
    {
        Public, ClosedGroup, Keyaccess, InAppPurchase 
    }
    //List<Level> levels;

    [SerializeField]
    Text seriesNameText=null;

   internal NetworkConst.series _series;

    [SerializeField]
    EditSeries editSeries=null;

    [SerializeField] Button editBtn=null, deleteBtn=null,levelViewBtn=null;

    #region AnujCode
    #region UIGameoBjects
   
    [SerializeField]
    TextMeshProUGUI idText,nameText, seriesVersionText, creatorText, priceText, availabilityModeText, levelText, seriesCategoryText, seriesSubCategoryText, tagText,createdDateText;
    #endregion UIGameoBjects
   
    public void SetSeriesData(NetworkConst.series myseries, Dictionary<int,string> groupIdsList)
    {
        string[] str = myseries.groupIds.Split(',');
        if (!string.IsNullOrEmpty(str[0]))
        {
            if (str.Length <= 1)
            {
                int i = Convert.ToInt32(str[0]);
                string xmlfile;
                if (groupIdsList.TryGetValue(i, out xmlfile))
                {
                    tagText.text = xmlfile;                    
                }
            }
            else
            {
                bool b = false;
                for (int i = 0; i < str.Length; i++)
                {
                    int j = Convert.ToInt32(str[i]);
                    string xmlfile;
                    if (groupIdsList.TryGetValue(j, out xmlfile))
                    {
                        
                        tagText.text = b ? tagText.text +", " + xmlfile : xmlfile;
                        b = true;
                    }
                }
            }
        }
        idText.text = myseries.id.ToString();
        nameText.text = myseries.name;
        seriesVersionText.text = myseries.serieVersion.ToString();
        creatorText.text = myseries.creatorAccountID.ToString();
        priceText.text = myseries.price;
        availabilityModeText.text = myseries.availabilityMode;
        levelText.text = myseries.level.ToString();
        seriesCategoryText.text = myseries.category_series;
        seriesSubCategoryText.text = myseries.subCategory_series;
        ///tagText.text = myseries.groupIds;
        createdDateText.text = myseries.comment;
    }    


    public void initializeData(NetworkConst.series series)
    {

        var list = GameManager.Instance.groupIdsList;
        this._series = series;
        Debug.Log("<color=yellow> Name Count "+series.name+"</color>");
       //seriesNameText.text = series.name;

       setUpForRole();
        SetSeriesData(series, list);
    }
    #endregion AnujCode
    public void setUpForRole()
    {
        bool canView = PermissionManager.Instance.GetPermissionForRole1("Series", "view");
        levelViewBtn.gameObject.SetActive(canView);

        //bool canEdit = PermissionManager.Instance.GetPermissionForRole1("Series", "edit");
        editBtn.gameObject.SetActive(canView);

        bool canDelete = PermissionManager.Instance.GetPermissionForRole1("Series", "delete");
        deleteBtn.gameObject.SetActive(canDelete);

        
    }
    public void deleteSeries()
    {
        //seri
    }
    public void seriesBtnClicked()
    {
        //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.LevelView, this._series.serieVersion);
    }
    public void levelBtnClicked()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.LevelView,this._series.id.ToString(), this._series.name);
    }
    public void setSeries(NetworkConst.series tmpSeries)
    {
        this._series = tmpSeries;
        setName(this._series.name);
    }
    public void setName(string name)
    {
        seriesNameText.text = name;
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

        DeleteConfirmPopup.Instance.initialize(()=> {
            SeriesManager.Instance.calldeleteSeriesAPI(this);
        }, "series");
    }
    public void showEditPopup()
    {
        
        SeriesManager.Instance.loadAllCategory((string message) => {
            editSeries.InitializeData(this);
        });
    }
}
