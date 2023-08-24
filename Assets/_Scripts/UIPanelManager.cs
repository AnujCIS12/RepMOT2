using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelManager : SingletonMonoBehaviour<UIPanelManager>
{
    [SerializeField]
    GameObject[] panels;
    GameObject selectedPanel;
    public ePanel currentPanel;
    public ePanel prevPanel;
    [SerializeField]
    GameObject background;
    string tempData;

    public enum ePanel
    {
        Login,
        Registration,
        Recover,
        MainMenu,
        SeriesView,
        LevelView,
        TaskView,
        MusicView,
        RecordView,
        LanguageView,
        ProfileView

    }

    public void Start()
    {
        //Debug.Log("Time is ");
        //Debug.Log(UnixTime.ConvYYYYMMDD_HHMMSS(UnixTime.FromDateTime(DateTime.Now.Date)));
    }
    public void changeMode(ePanel next_mode, string data = "",string levelauthour="",string authourtask="" )
    {
        tempData = data;
      
        prevPanel = currentPanel;
        currentPanel = next_mode;
        StartCoroutine(ChangeModeCoroutine(next_mode, levelauthour,authourtask));
    }

    IEnumerator ChangeModeCoroutine(ePanel next_mode, string levelauthorname,string authourtaskl)
    {
        Debug.Log("New Mode is " + next_mode);
        switch (next_mode)
        {
            case ePanel.Login:
                showLoginPanel();
                break;
            case ePanel.Registration:
                showRegistrationPanel();
                break;
            case ePanel.Recover:
                showRecoveryPanel();
                break;
            case ePanel.MainMenu:
                showMainMenuPanel();
                break;
            case ePanel.SeriesView:
                Debug.Log("Series view called ");
                showSeriesViewPanel();
                break;
            case ePanel.LevelView:
                showLevelViewPanel(levelauthorname);
                break;
            case ePanel.TaskView:
                showTaskViewPanel(levelauthorname,authourtaskl);
                break;
            case ePanel.MusicView:
                showMusicViewPanel();
                break;
            case ePanel.RecordView:
                showRecordViewPanel();
                break;
            case ePanel.LanguageView:
                showLanguageViewPanel();
                break;
            case ePanel.ProfileView:
                showProfileViewPanel();
                break;
        }

        yield return null;
    }

    void showLoginPanel()
    {
        if (selectedPanel != null) selectedPanel.gameObject.SetActive(false);
        //background.SetActive(true);
        panels[0].SetActive(true);
        selectedPanel = panels[0];

    }
    void showRegistrationPanel()
    {
        if (selectedPanel != null) selectedPanel.gameObject.SetActive(false);
        //background.SetActive(true);
        panels[1].SetActive(true);
        selectedPanel = panels[1];
    }

    void showRecoveryPanel()
    {
        if (selectedPanel != null) selectedPanel.gameObject.SetActive(false);
        //background.SetActive(true);
        panels[2].SetActive(true);
        selectedPanel = panels[2];
    }

    void showMainMenuPanel()
    {
        if (selectedPanel != null) selectedPanel.gameObject.SetActive(false);
        selectedPanel = panels[3];
        selectedPanel.SetActive(true);
    }

    void showSeriesViewPanel()
    {
        if (selectedPanel != null) selectedPanel.gameObject.SetActive(false);
        selectedPanel = panels[4];
        selectedPanel.GetComponent<IPanel>().Initialize(tempData);
        selectedPanel.SetActive(true);
    }
    void showLevelViewPanel( string authourname )
    {
        if (selectedPanel != null) { }// selectedPanel.gameObject.SetActive(false);
        selectedPanel = panels[5];
        Debug.Log("<color=yellow> Temp Data "+tempData+"</color>");
        selectedPanel.GetComponent<IPanel>().Initialize(tempData,authourname);
        selectedPanel.SetActive(true);
    }
    void showTaskViewPanel(string authourname, string authtaskl)
    {
        if (selectedPanel != null) { }// selectedPanel.gameObject.SetActive(false);
        selectedPanel = panels[6];
//        Debug.Log("<color=yellow> Temp Data "+tempData+"Authour "+authourname+"auth task => "+authtaskl+" </color> ");
        selectedPanel.GetComponent<IPanel>().Initialize(tempData, authourname, authtaskl);
        selectedPanel.SetActive(true);
    }
    void showMusicViewPanel()
    {
        if (selectedPanel != null)
        {
            selectedPanel.gameObject.SetActive(false);
        }
        selectedPanel = panels[7];
        selectedPanel.GetComponent<IPanel>().Initialize(tempData);
        selectedPanel.SetActive(true);
    }
    void showRecordViewPanel()
    {
        if (selectedPanel != null)
        {
            selectedPanel.gameObject.SetActive(false);
        }
        selectedPanel = panels[8];
        selectedPanel.GetComponent<IPanel>().Initialize(tempData);
        selectedPanel.SetActive(true);
    }

    void showLanguageViewPanel()
    {
        if (selectedPanel != null)
        {
            selectedPanel.gameObject.SetActive(false);
        }
        selectedPanel = panels[9];
        selectedPanel.GetComponent<IPanel>().Initialize(tempData);
        selectedPanel.SetActive(true);
    }

    void showProfileViewPanel()
    {
        if (selectedPanel != null)
        {
            selectedPanel.gameObject.SetActive(false);
        }
        selectedPanel = panels[10];
        selectedPanel.GetComponent<IPanel>().Initialize(tempData);
        selectedPanel.SetActive(true);
    }
}
