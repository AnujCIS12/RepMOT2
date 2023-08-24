using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopupManager : SingletonMonoBehaviour<UIPopupManager>
{
    [SerializeField]
    GameObject[] popups;
    GameObject selectedPopup;
    public ePopup currentPopup;
    public ePopup prevPopup;
    [SerializeField]
    GameObject background;

    public enum ePopup
    {
        AddSeries,
        EditSeries,
        ConirmPopup,
        AddLevel,
        EditLevel,
        AddTask,
        EditTask,
        AddMusic,
        EditMusic,
        AddRecord,
        EditRecord,
        MainMenu,
    }

    public void showPopup(ePopup next_mode)
    {
        prevPopup = currentPopup;
        currentPopup = next_mode;
        StartCoroutine(ChangePopupCoroutine(next_mode));
    }

    IEnumerator ChangePopupCoroutine(ePopup next_mode)
    {
        Debug.Log("New Mode is " + next_mode);
        switch (next_mode)
        {
            case ePopup.AddSeries:
                showAddSeriesPopup();
                break;
            case ePopup.EditSeries:
                showEditSeriesPanel();
                break;
            case ePopup.ConirmPopup:
                showConfirmPopup();
                break;
            case ePopup.AddLevel:
                showAddLevelPopup();
                break;
            case ePopup.EditLevel:
                showEditLevelPopup();
                break;
            case ePopup.AddTask:
                showAddTaskPopup();
                break;
            case ePopup.EditTask:
                showEditTaskPopup();
                break;
            case ePopup.AddMusic:
                showAddMusicPopup();
                break;
            case ePopup.EditMusic:
                showEditMusicPopup();
                break;
            
        }

        yield return null;
    }
    public void HideSelectedPopUp()
    {
        if (selectedPopup != null) selectedPopup.gameObject.SetActive(false);
    }

    void showAddSeriesPopup()
    {
        if (selectedPopup != null) selectedPopup.gameObject.SetActive(false);
        //background.SetActive(true);
        popups[0].SetActive(true);
        selectedPopup = popups[0];

    }

    void showEditSeriesPanel()
    {
        if (selectedPopup != null) selectedPopup.gameObject.SetActive(false);
        //background.SetActive(true);
        popups[1].SetActive(true);
        selectedPopup = popups[1];
    }
    void showConfirmPopup()
    {
        if (selectedPopup != null) selectedPopup.gameObject.SetActive(false);
        //background.SetActive(true);
        popups[2].SetActive(true);
        selectedPopup = popups[2];
    }

    void showAddLevelPopup()
    {
        if (selectedPopup != null) selectedPopup.gameObject.SetActive(false);
        //background.SetActive(true);
        popups[3].SetActive(true);
        selectedPopup = popups[3];

    }

    void showEditLevelPopup()
    {
        if (selectedPopup != null) selectedPopup.gameObject.SetActive(false);
        //background.SetActive(true);
        popups[4].SetActive(true);
        selectedPopup = popups[4];
    }

    void showAddTaskPopup()
    {
        if (selectedPopup != null) selectedPopup.gameObject.SetActive(false);
        //background.SetActive(true);
        popups[5].SetActive(true);
        selectedPopup = popups[5];

    }

    void showEditTaskPopup()
    {
        if (selectedPopup != null) selectedPopup.gameObject.SetActive(false);
        //background.SetActive(true);
        popups[6].SetActive(true);
        selectedPopup = popups[6];
    }

    void showAddMusicPopup()
    {
        if (selectedPopup != null) selectedPopup.gameObject.SetActive(false);
        //background.SetActive(true);
        popups[7].SetActive(true);
        selectedPopup = popups[7];

    }

    void showEditMusicPopup()
    {
        if (selectedPopup != null) selectedPopup.gameObject.SetActive(false);
        //background.SetActive(true);
        popups[8].SetActive(true);
        selectedPopup = popups[8];
    }
}