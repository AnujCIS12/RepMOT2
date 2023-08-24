using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Language : MonoBehaviour
{
    [SerializeField]
    Text nameText;

    NetworkConst.language _language;

    [SerializeField]
    EditLanguage editLanguage;

    public void initializeData(NetworkConst.language language)
    {
        this._language = language;
        setName(language.name);
        Debug.Log("Initialize Name " + language.name);
    }

    public void languageBtnClicked()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.LanguageView, this._language.id.ToString());
    }
    public void setLanguage(NetworkConst.language tmpRecord)
    {
        this._language = tmpRecord;
        setName(tmpRecord.name);
    }
    public void setName(string name)
    {
        nameText.text = name;
    }
    public int ID
    {
        get { return this._language.id; }
    }
    public NetworkConst.language getLanguage()
    {
        return this._language;
    }
    public void showDeleteConfirmPopup()
    {
        //MusicManager.Instance.setSelectedMusic(this);
        UIPopupManager.Instance.showPopup(UIPopupManager.ePopup.ConirmPopup);

        DeleteConfirmPopup.Instance.initialize(confirmedDelete, "language");
    }
    public void showEditPopup()
    {
        //editLanguage.InitializeData(this);
    }

    public void confirmedDelete()
    {
        Debug.Log("ID is " + this.ID);
        //RecordManager.Instance.callDeleteAPI(this);
    }
}
