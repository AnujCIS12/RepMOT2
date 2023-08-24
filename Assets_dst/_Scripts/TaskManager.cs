using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : SingletonMonoBehaviour<TaskManager>
{
    [SerializeField]
    Task prefabTask;
    [SerializeField]
    GameObject parentContent;

    [SerializeField]
    GameObject taskViewPanel;

    List<Task> tasklist = new List<Task>();
    Task selectedTask;


    public JsonData jsonResponse;

    public void Initialize(string data)
    {
        loadAllTask();
    }
    public void addTaskBtnClicked()
    {
        UIPopupManager.Instance.showPopup(UIPopupManager.ePopup.AddTask);
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
    public void calldeleteTaskAPI()
    {
        deleteTask(selectedTask.ID);
    }

    public void loadAllTask()
    {
        NetworkConst.allTaskRes _allTaskRes;
        //string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.postTask data = new NetworkConst.postTask();
        data.limit = 10;
        data.page_no = 1;

        string json = JsonUtility.ToJson(data);

        NetworkingManager.Instance.APILoadAllTaskData(json, (string data) =>
        {
            _allTaskRes = JsonUtility.FromJson<NetworkConst.allTaskRes>(data);
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
        data.id = id;
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

    public void editTask(int id)
    {
        //string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.task data = new NetworkConst.task();
        data.id = id;

        string json = JsonUtility.ToJson(data);
        NetworkingManager.Instance.APIEditTaskData(json, (string resData) =>
        {
            Debug.Log("God Login PLayer Response" + resData);
            //taskJsonData = JsonMapper.ToObject(resData);
            //if ((bool)taskJsonData["status"])
            //{
            //    Debug.Log("Status is True");
            //    TaskManager.Instance.createNewTask(data);
            //    //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
            //}
            //else
            //{
            //    Debug.Log("Message ");
            //    Debug.Log(taskJsonData["message"].ToString());
            //    //message.text = taskJsonData["message"].ToString();
            //}

            ////PlayerData.Instance.PlayerName = name;
            ////PlayerData.Instance.PlayerGUID = playerRowKey;

        });
    }
}
