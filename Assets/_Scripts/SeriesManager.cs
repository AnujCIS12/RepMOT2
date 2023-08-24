using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class SeriesManager : SingletonMonoBehaviour<SeriesManager>, IPanel
{
    [SerializeField]
    Series prefabSeries;
    [SerializeField]
    GameObject parentContent;

    [SerializeField] GameObject createSeriesBtn;

    [SerializeField]
    GameObject seriesViewPanel;
    [SerializeField]
    GameObject seriesManager;

    public delegate void onSuccess(string Text);
    public Dictionary<string, NetworkConst.category> categoryDic = new Dictionary<string, NetworkConst.category>();
    public Dictionary<string, NetworkConst.subcategory> subcategoryDic = new Dictionary<string, NetworkConst.subcategory>();
    public Dictionary<string, NetworkConst.groupAccess> groupDic = new Dictionary<string, NetworkConst.groupAccess>();
    public Dictionary<int, NetworkConst.groupAccess> groupDic1 = new Dictionary<int, NetworkConst.groupAccess>();
    private bool ascending_Name, ascending_Level, ascending_AvaialabilityMode, ascending_Creator;

    List<Series> serieslist = new List<Series>();
    Series selectedSeries;


    //public JsonData jsonResponse;

    public void Initialize(string data, string namel="", string authtasknamel="")
    {
        ascending_AvaialabilityMode = ascending_Creator = ascending_Level = ascending_Name = false;
        if (serieslist.Count > 0) deleteAllSeries();
        loadAllSeries();
        setUpForRole();
    }
    public void RemoveChild()
    {
        int childCount = parentContent.transform.childCount;
        for (int x = 0; x < childCount; x++)
        { Transform child = parentContent.transform.GetChild(x);
            if(child.gameObject.activeInHierarchy)
            {
                if (child.gameObject.tag == "series")
                {
                    Destroy(child.gameObject);
                }
            }
            
        }
        
    }
    public void onClickHomebtn()
    {
        RemoveChild();
        if (serieslist.Count > 0)
        {
            serieslist.Clear();
        }
        seriesManager.SetActive(false);
    }
    public void SortSeriesbyname()
    {
        RemoveChild();
       
        List<NetworkConst.series> temp_Series = new List<NetworkConst.series>();
        foreach (var x in serieslist)
        {
           temp_Series.Add(x.getSeries());
        }
        
        if (ascending_Name==false)
        {
            var Convert_List = from a in temp_Series orderby a.name select a;
            ascending_Name = true;
            foreach (var x in Convert_List)
            {
                Series tmpSeries;
                tmpSeries = Instantiate(prefabSeries, parentContent.transform);
                tmpSeries.gameObject.SetActive(true);
                tmpSeries.initializeData(x);
            }
            temp_Series.Clear();
        }
        else if(ascending_Name==true)
        {
           var  Convert_List = temp_Series.OrderByDescending(a => a.name);
            ascending_Name = false;
            foreach (var x in Convert_List)
            {
                Series tmpSeries;
                tmpSeries = Instantiate(prefabSeries, parentContent.transform);
                tmpSeries.gameObject.SetActive(true);
                tmpSeries.initializeData(x);
            }
            temp_Series.Clear();
        }
        
       
    }
    public void SortSeriesbyAvailability()
    {
        RemoveChild();
        List<NetworkConst.series> temp_Series = new List<NetworkConst.series>();
        foreach (var x in serieslist)
        {
            temp_Series.Add(x.getSeries());
        }
        if(ascending_AvaialabilityMode==false)
        {
            var Convert_List = from a in temp_Series orderby a.availabilityMode select a;
            ascending_AvaialabilityMode = true;
            foreach (var x in Convert_List)
            {
                Series tmpSeries;
                tmpSeries = Instantiate(prefabSeries, parentContent.transform);
                tmpSeries.gameObject.SetActive(true);
                tmpSeries.initializeData(x);
            }
            temp_Series.Clear();
        }
        else if(ascending_AvaialabilityMode==true)
        {
            var Convert_List = temp_Series.OrderByDescending(a => a.availabilityMode);
            ascending_AvaialabilityMode = false;
            foreach (var x in Convert_List)
            {
                Series tmpSeries;
                tmpSeries = Instantiate(prefabSeries, parentContent.transform);
                tmpSeries.gameObject.SetActive(true);
                tmpSeries.initializeData(x);
            }
            temp_Series.Clear();
        }
        
    }
    public void SortSeriesbyLevel()
    {
        RemoveChild();
        List<NetworkConst.series> temp_Series = new List<NetworkConst.series>();
        foreach (var x in serieslist)
        {
            temp_Series.Add(x.getSeries());
        }
        if (ascending_Level == false)
        {
            var Convert_List = from a in temp_Series orderby a.level select a;
            ascending_Level = true;
            foreach (var x in Convert_List)
            {
                Series tmpSeries;
                tmpSeries = Instantiate(prefabSeries, parentContent.transform);
                tmpSeries.gameObject.SetActive(true);
                tmpSeries.initializeData(x);
            }
            temp_Series.Clear();
        }
        else if (ascending_Level == true)
        {
            var Convert_List = temp_Series.OrderByDescending(a => a.level);
            ascending_Level = false;
            foreach (var x in Convert_List)
            {
                Series tmpSeries;
                tmpSeries = Instantiate(prefabSeries, parentContent.transform);
                tmpSeries.gameObject.SetActive(true);
                tmpSeries.initializeData(x);
            }
            temp_Series.Clear();
        }
    }
    public void SortSeriesbyCreator()
    {
        RemoveChild();
        List<NetworkConst.series> temp_Series = new List<NetworkConst.series>();
        foreach (var x in serieslist)
        {
            temp_Series.Add(x.getSeries());
        }
        if (ascending_Creator == false)
        {
            var Convert_List = from a in temp_Series orderby a.creatorAccountID select a;
            ascending_Creator = true;
            foreach (var x in Convert_List)
            {
                Series tmpSeries;
                tmpSeries = Instantiate(prefabSeries, parentContent.transform);
                tmpSeries.gameObject.SetActive(true);
                tmpSeries.initializeData(x);
            }
            temp_Series.Clear();
        }
        else if (ascending_Creator == true)
        {
            var Convert_List = temp_Series.OrderByDescending(a => a.creatorAccountID);
            ascending_Creator = false;
            foreach (var x in Convert_List)
            {
                Series tmpSeries;
                tmpSeries = Instantiate(prefabSeries, parentContent.transform);
                tmpSeries.gameObject.SetActive(true);
                tmpSeries.initializeData(x);
            }
            temp_Series.Clear();
        }
    }
    public void setUpForRole()
    {

        bool canCreate = PermissionManager.instance.GetPermissionForRole1("Series", "create");
        createSeriesBtn.SetActive(canCreate);
    }
    public void addSeriesBtnClicked()
    {

        loadAllCategory((string message) => {
            UIPopupManager.Instance.showPopup(UIPopupManager.ePopup.AddSeries);
        });
    }

    public void createNewSeries(NetworkConst.series tempSeries)
    {

        //Deck tempDeck = new Deck();
        //tempDeck.ID = deckID;
        //tempDeck.Name = name;
        //tempDeck.TeacherID = Constants.teacherID;
        //tempDeck.TeacherName = Constants.teacherName;
        //teacher.AddDeck(tempDeck);

        //UIDeck tempUIDeck;
        //tempUIDeck = Instantiate(uIDeck, parentContent.transform) as UIDeck;
        //tempUIDeck.gameObject.SetActive(true);
        //tempUIDeck.InitializeData(tempDeck);
        //AddUIDeck(tempUIDeck);


        Series tmpSeries;
        tmpSeries = Instantiate(prefabSeries, parentContent.transform);
        tmpSeries.gameObject.SetActive(true);
        tmpSeries.initializeData(tempSeries);
        addSeries(tmpSeries);
        UIPopupManager.instance.HideSelectedPopUp();

    }

    public void setFieldForRole()
    {

    }
        public void addSeries(Series series)
    {
        serieslist.Add(series);
    }
    public void deleSeries(Series _series)
    {
        serieslist.Remove(_series);
        Destroy(_series.gameObject);
    }
    public void deleSeries1(Series _series)
    {
        Destroy(_series.gameObject);
    }
    public void deleteAllSeries()
    {
        Debug.Log("Count is " + serieslist.Count);
        for (int i = 0; i < serieslist.Count; i++)
        {
            Debug.Log(i);
            deleSeries1(serieslist[i]);
        }
        serieslist.Clear();
        //gameObject.SetActive(false);
        //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
    }

    public void hideDeleteConfirmPopup()
    {
        UIPopupManager.Instance.HideSelectedPopUp();
    }
    public void setSelectedSeries(Series _series)
    {
        this.selectedSeries = _series;
        Debug.Log("Selected Series ID is " + _series.ID);
    }
    public void calldeleteSeriesAPI(Series series)
    {
        deleteSeries(series.getSeries().id);
    }

    public void loadAllSeries()
    {
        Debug.Log("<color=yellow> Load called </color>");
        NetworkConst.allSeriesRes _allSeriesRes;
        //string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.postSeries data = new NetworkConst.postSeries();
        data.limit = 10;
        data.page_no = 1;

        string json = JsonUtility.ToJson(data);

        NetworkingManager.Instance.APILoadAllSeriesData(json, (string data) =>
        {
            _allSeriesRes = JsonUtility.FromJson<NetworkConst.allSeriesRes>(data);
            JsonData jsonResponse;
            jsonResponse = JsonMapper.ToObject(data);
            Debug.Log("God Login PLayer Response" + jsonResponse["data"]);
            Debug.Log(jsonResponse);
            if ((bool)jsonResponse["status"])
            {
                //CreatorData.Instance.setCreatorData(this._loginRes.data);
                //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
                Debug.Log("Name " + _allSeriesRes.data[0].name);
                Debug.Log("<color=Yellow>=====>Number of rows "+_allSeriesRes.data.Length+"</color>");
                foreach (NetworkConst.series tmpSeries in _allSeriesRes.data)
                {
                    createNewSeries(tmpSeries);
                }
                seriesViewPanel.gameObject.SetActive(true);
            }
            else
            {
                //message.text = jsonResponse["message"].ToString();
                Debug.Log("Message is " + _allSeriesRes.message);
            }

        });
    }
    public void deleteSeries(int id)
    {
        //string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.series data = new NetworkConst.series();
        data.id = id;
        Debug.Log("ID is " + id);


        WWWForm wwwform = new WWWForm();
        wwwform.AddField("id", id);
        string json = JsonUtility.ToJson(data);
        NetworkingManager.Instance.APIDeleteSeriesData(wwwform, (string resData) =>
        {
            Debug.Log("God Login PLayer Response" + resData);
            JsonData jsonResponse;
            jsonResponse = JsonMapper.ToObject(resData);
            if ((bool)jsonResponse["status"])
            {
                Debug.Log("Status is True");
                //SeriesManager.Instance.createNewSeries(data);
                //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
                deleSeries(selectedSeries);
                UIPopupManager.instance.HideSelectedPopUp();

            }
            else
            {
                Debug.Log("Message ");
                Debug.Log(jsonResponse["message"].ToString());
                //message.text = seriesJsonData["message"].ToString();
            }

        });
    }


    public int getCategoryIDByName(string name)
    {
        //Debug.Log(name);
        //if (string.IsNullOrEmpty(name)) return -1; 
        //return categoryDic[name].id;

        Debug.Log(name);
        NetworkConst.category entry;
        if (string.IsNullOrEmpty(name)) return -1;
        Debug.Log("Before Try to get category " + name);
        if (categoryDic.TryGetValue(name, out entry))
        {
            Debug.Log("Entry" + entry);
            return entry.id;
        }
        else return -1;
    }

    public void loadAllCategory(onSuccess callback)
    {
        NetworkConst.allCategoryRes _allCategoryRes;
        //string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.postCategory data = new NetworkConst.postCategory();
        data.moduleID = 1;

        string json = JsonUtility.ToJson(data);

        NetworkingManager.Instance.APILoadAllCategoryData(json, (string data) =>
        {
            _allCategoryRes = JsonUtility.FromJson<NetworkConst.allCategoryRes>(data);
            JsonData jsonResponse;
            jsonResponse = JsonMapper.ToObject(data);
            categoryDic.Clear();
            if ((bool)jsonResponse["status"])
            {
                //CreatorData.Instance.setCreatorData(this._loginRes.data);
                //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
                Debug.Log("---------Data -------------" + _allCategoryRes);
                foreach (NetworkConst.category tmpCategory in _allCategoryRes.data)
                {
                    Debug.Log(tmpCategory.name + "..................Name adding");
                    if (!categoryDic.ContainsKey(tmpCategory.name))
                        categoryDic.Add(tmpCategory.name, tmpCategory);
                }
                callback("Success");
            }
            else
            {
                //message.text = jsonResponse["message"].ToString();
                Debug.Log("Message is " + _allCategoryRes.message);
            }

        });
    }
    public void loadAllSubCategory(string categoryName, onSuccess callback)
    {
        NetworkConst.allSubCategoryRes _allSubCategoryRes;
        //string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.postSubCategory data = new NetworkConst.postSubCategory();
        int _Cid = getCategoryIDByName(categoryName);
        if (_Cid < 0)
        {
            callback("Error");
            return;
        }
        data.categoryID = _Cid;

        string json = JsonUtility.ToJson(data);

        NetworkingManager.Instance.APILoadAllSubCategoryData(json, (string data) =>
        {
            subcategoryDic.Clear();
            _allSubCategoryRes = JsonUtility.FromJson<NetworkConst.allSubCategoryRes>(data);
            Debug.Log("---------data -------------" + _allSubCategoryRes);
            foreach (NetworkConst.subcategory tmpSubCategory in _allSubCategoryRes.data)
            {
                if (!subcategoryDic.ContainsKey(tmpSubCategory.name))
                    subcategoryDic.Add(tmpSubCategory.name, tmpSubCategory);
            }
            callback("Success");

        }, (string data) => { callback("Error"); });
    }

    public void PublishSeries(int id)
    {
        WWWForm wwwform = new WWWForm();
        wwwform.AddField("seriesID", id);
        NetworkingManager.Instance.APIPublishSeriesData(wwwform, (string resData) =>
        {
            Debug.Log("God Login PLayer Response" + resData);


        });
    }   
}
