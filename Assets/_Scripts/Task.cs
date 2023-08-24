using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Task:MonoBehaviour
{
    public TextMeshProUGUI idText, nameText, maxScorepointsText, pointsAccomplishedText, thresholdText, instructionText, explanationText,createDateText;
    // string name;
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
    Text taskNameText=null;

    NetworkConst.task _task;

    [SerializeField]
    EditTask editTask=null;

    public void initializeData(NetworkConst.task task)
    {
        idText.text = task.id.ToString();
        nameText.text = task.name;
        maxScorepointsText.text = task.maxScorepointToGet.ToString();
        pointsAccomplishedText.text = task.pointsAccomplished.ToString();
        thresholdText.text = task.threshold;
        instructionText.text = task.instructions;
        explanationText.text = task.explanations;
        createDateText.text = task.recordID.ToString();
        this._task = task;
        //taskNameText.text = task.name;
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
        setTaskName(tmpTask.name);
    }
    public void setTaskName(string name)
    {
       // taskNameText.text = name;
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
        //UIPopupManager.Instance.showPopup(UIPopupManager.ePopup.ConirmPopup);

        UIPopupManager.Instance.showPopup(UIPopupManager.ePopup.ConirmPopup);

        DeleteConfirmPopup.Instance.initialize(confirmedDelete, "task");
    }
    public void confirmedDelete()
    {
        TaskManager.Instance.calldeleteTaskAPI(this);
    }
    public void showEditPopup()
    {
        Debug.Log("Task id "+this._task.id);
        editTask.InitializeData(this);
    }
}
