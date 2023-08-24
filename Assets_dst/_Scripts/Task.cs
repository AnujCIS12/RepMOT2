using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Task:MonoBehaviour
{
    string name;
    int recordID;
    int maxScorepointToGet;
    int pointsAccomplished;
    string threshold;
    string instructions;
    string Explanations;
    string Record;
    byte[] Pictures;
    byte[] Sound;
    byte[] video;

    private void Start()
    {
        
    }

    [SerializeField]
    Text taskNameText;

    NetworkConst.task _task;

    [SerializeField]
    EditTask editTask;

    public void initializeData(NetworkConst.task task)
    {
        this._task = task;
        taskNameText.text = task.name;
    }
    public void deleteTask()
    {
        //seri
    }
    public void taskBtnClicked()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.TaskView, this._task.id.ToString());
    }
    public void setTask(NetworkConst.task tmpTask)
    {
        this._task = tmpTask;
    }
    public int ID
    {
        get { return this._task.id; }
    }
    public NetworkConst.task getTask()
    {
        return this._task;
    }
    public void showDeleteConfirmPopup()
    {
        TaskManager.Instance.setSelectedTask(this);
        UIPopupManager.Instance.showPopup(UIPopupManager.ePopup.ConirmPopup);
    }
    public void showEditPopup()
    {
        editTask.InitializeData(this);
    }
}
