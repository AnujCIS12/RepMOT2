using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
public class TaskManager : SingletonMonoBehaviour<TaskManager>, IPanel
{
    [SerializeField]
    Task prefabTask;
    [SerializeField]
    GameObject parentContent;
    [SerializeField]
    TextMeshProUGUI taskText;
    [SerializeField]
    GameObject taskViewPanel;
    public GameObject LevelPanel, SeriesPanel;
    public Dictionary<string, NetworkConst.record1> recordDic = new Dictionary<string, NetworkConst.record1>();

    public delegate void onSuccess(string Text);
    List<Task> tasklist = new List<Task>();
    Task selectedTask;

    public NetworkConst.level _level;
    public NetworkConst.selectedTaskDetail _listavTask;

    public JsonData jsonResponse;

    private void OnDisable()
    {
        RemoveChild();
        tasklist.Clear();
    }

    public void RemoveChild()
    {
        int childCount = parentContent.transform.childCount;
        for (int x = 0; x < childCount; x++)
        {
            Transform child = parentContent.transform.GetChild(x);
            if (child.gameObject.activeInHierarchy)
            {
                if (child.gameObject.tag == "task")
                {
                    Destroy(child.gameObject);
                }
            }

        }

    }

    public void Initialize(string data, string namel="",string authtasknamel="")
    {
        //        Debug.Log("<color=Red>Task Data " + data +"name "+namel+"authtaskname "+authtasknamel+ "</color>");
//        Debug.Log("Task Data => "+data);
        this._level = JsonUtility.FromJson<NetworkConst.level>(data);
        Debug.Log("Parent Level is : ");
        Debug.Log(this._level.id);
        loadAllTask();
        taskText.text = namel + " -> Level " + authtasknamel + " -> Tasks";
    }
    public void OnClickHomeBtn()
    {
        if(LevelPanel.activeInHierarchy)
        {
            LevelPanel.SetActive(false);
        }
        if(SeriesPanel.activeInHierarchy)
        {
            SeriesPanel.SetActive(false);
        }
    }
    public void addTaskBtnClicked()
    {
        UIPopupManager.Instance.showPopup(UIPopupManager.ePopup.AddTask);
    }

    public void seriesBtnClicked()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.SeriesView);
    }

    public void levelBtnClicked()
    {
        //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.LevelView, this._level.id.ToString());
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.LevelView, this._level.seriesID.ToString());
    }

    public void createNewTask(NetworkConst.task tempTask)
    {


        Task tmpTask;
        tmpTask = Instantiate(prefabTask, parentContent.transform) as Task;
        tmpTask.gameObject.SetActive(true);
        tmpTask.initializeData(tempTask);
        addTask(tmpTask);
        UIPopupManager.Instance.HideSelectedPopUp();

    }
    public void addTask(Task task)
    {
        tasklist.Add(task);
    }
    public void deleTask(Task _task)
    {
        tasklist.Remove(_task);
        Destroy(_task.gameObject);
    }

    public void hideDeleteConfirmPopup()
    {
        UIPopupManager.Instance.HideSelectedPopUp();
    }
    public void setSelectedTask(Task _task)
    {
        this.selectedTask = _task;
        Debug.Log("Selected Task ID is " + _task.ID);
    }
    public void calldeleteTaskAPI(Task _task)
    {
        deleteTask(_task.getTask().id);
    }

    public void loadAllTask()
    {
        NetworkConst.allTaskRes _allTaskRes;
        //string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.postTask data = new NetworkConst.postTask();
        data.limit = 100;
        data.page_no = 1;
        data.seriesID = this._level.seriesID;
        data.levelID = this._level.id.ToString();

        string json = JsonUtility.ToJson(data);

        NetworkingManager.Instance.APILoadAllTaskData(json, (string data) =>
        {
            Debug.Log("Task All Data =>"+data);
            
            _allTaskRes = JsonUtility.FromJson<NetworkConst.allTaskRes>(data);
            _listavTask = JsonUtility.FromJson<NetworkConst.selectedTaskDetail>(data);
            
            jsonResponse = JsonMapper.ToObject(data);
            Debug.Log("God Login PLayer Response" + jsonResponse["data"]);
            Debug.Log(jsonResponse);
            if ((bool)jsonResponse["status"])
            {
                //CreatorData.Instance.setCreatorData(this._loginRes.data);
                //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
                Debug.Log("Name " + _allTaskRes.data[0].name);
                foreach (NetworkConst.task tmpTask in _allTaskRes.data)
                {
                    createNewTask(tmpTask);
                }
                taskViewPanel.gameObject.SetActive(true);
            }
            else
            {
                //message.text = jsonResponse["message"].ToString();
                Debug.Log("Message is " + _allTaskRes.message);
            }

        });
    }
    public void deleteTask(int id)
    {
        //string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.task data = new NetworkConst.task();
        data.taskID = id.ToString();
        Debug.Log("ID is " + id);

        string json = JsonUtility.ToJson(data);
        NetworkingManager.Instance.APIDeleteTaskData(json, (string resData) =>
        {
            Debug.Log("God Login PLayer Response" + resData);
            jsonResponse = JsonMapper.ToObject(resData);
            if ((bool)jsonResponse["status"])
            {
                Debug.Log("Status is True");
                //TaskManager.Instance.createNewTask(data);
                //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
                deleTask(selectedTask);
                UIPopupManager.Instance.HideSelectedPopUp();

            }
            else
            {
                Debug.Log("Message ");
                Debug.Log(jsonResponse["message"].ToString());
                //message.text = taskJsonData["message"].ToString();
            }

        });
    }

    //public void editTask(int id)
    //{
    //    //string playerRowKey = System.Guid.NewGuid().ToString();
    //    NetworkConst.task data = new NetworkConst.task();
    //    data.id = id;

    //    string json = JsonUtility.ToJson(data);
    //    NetworkingManager.Instance.APIEditTaskData(json, (string resData) =>
    //    {
    //        Debug.Log("God Login PLayer Response" + resData);
    //        //taskJsonData = JsonMapper.ToObject(resData);
    //        //if ((bool)taskJsonData["status"])
    //        //{
    //        //    Debug.Log("Status is True");
    //        //    TaskManager.Instance.createNewTask(data);
    //        //    //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
    //        //}
    //        //else
    //        //{
    //        //    Debug.Log("Message ");
    //        //    Debug.Log(taskJsonData["message"].ToString());
    //        //    //message.text = taskJsonData["message"].ToString();
    //        //}

    //        ////PlayerData.Instance.PlayerName = name;
    //        ////PlayerData.Instance.PlayerGUID = playerRowKey;

    //    });
    //}


    public void loadAllRecords(onSuccess successCallback)
    {
        NetworkConst.allRecordRes _allRecordRes;
        //string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.postRecord data = new NetworkConst.postRecord();
        data.limit = 10;
        data.page_no = 1;

        string json = JsonUtility.ToJson(data);
        
        NetworkingManager.Instance.APILoadAllRecordData(json, (string data) =>
        {
            _allRecordRes = JsonUtility.FromJson<NetworkConst.allRecordRes>(data);
            jsonResponse = JsonMapper.ToObject(data);
            Debug.Log("God Login PLayer Response" + jsonResponse["data"]);
            Debug.Log(jsonResponse);
            if ((bool)jsonResponse["status"])
            {
                //CreatorData.Instance.setCreatorData(this._loginRes.data);
                //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
                Debug.Log("Name " + _allRecordRes.data[0].name);
                successCallback(data);
                //foreach (NetworkConst.record1 tmpObj in _allRecordRes.data)
                //{
                //    //createNewLanguage(tmpLanguage);
                //    if (!recordDic.ContainsKey(tmpObj.name))
                //        recordDic.Add(tmpObj.name, tmpObj);
                //}
                
                //updateLanguageDD(_allRecordRes);
            }
            else
            {
                //message.text = jsonResponse["message"].ToString();
                Debug.Log("Message is " + _allRecordRes.message);
            }

        });
    }

    
}
