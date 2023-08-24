using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Record : MonoBehaviour
{
    [SerializeField]
    Text nameText;

    NetworkConst.record1 _record;

    [SerializeField]
    EditRecord editRecord;

    public void initializeData(NetworkConst.record1 record)
    {
        this._record = record;
        setName(record.name);
    }

    public void recordBtnClicked()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MusicView, this._record.id.ToString());
    }
    public void setRecord(NetworkConst.record1 tmpRecord)
    {
        this._record = tmpRecord;
        setName(tmpRecord.name);
    }
    public void setName(string name)
    {
        nameText.text = name;
    }
    public int ID
    {
        get { return this._record.id; }
    }
    public NetworkConst.record1 getStructObj()
    {
        return this._record;
    }
    public void showDeleteConfirmPopup()
    {
        //MusicManager.Instance.setSelectedMusic(this);
        UIPopupManager.Instance.showPopup(UIPopupManager.ePopup.ConirmPopup);

        DeleteConfirmPopup.Instance.initialize(confirmedDelete, "record");
    }
    public void showEditPopup()
    {
        editRecord.InitializeData(this);
    }

    public void confirmedDelete()
    {
        RecordManager.Instance.callDeleteAPI(this);
    }
}
