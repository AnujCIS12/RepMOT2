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
        MusicView

    }

    public void Start()
    {
        //Debug.Log("Time is ");
        //Debug.Log(UnixTime.ConvYYYYMMDD_HHMMSS(UnixTime.FromDateTime(DateTime.Now.Date)));
    }
    public void changeMode(ePanel next_mode,string data="")
    {
        tempData = data;
        prevPanel = currentPanel;
        currentPanel = next_mode;
        StartCoroutine(ChangeModeCoroutine(next_mode));
    }

    IEnumerator ChangeModeCoroutine(ePanel next_mode)
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
                showSeriesViewPanel();
                break;
            case ePanel.LevelView:
                showLevelViewPanel();
                break;
            case ePanel.TaskView:
                showTaskViewPanel();
                break;
            case ePanel.MusicView:
                showMusicViewPanel();
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
    void showLevelViewPanel()
    {
        if (selectedPanel != null) selectedPanel.gameObject.SetActive(false);
        selectedPanel = panels[5];
        selectedPanel.GetComponent<IPanel>().Initialize(tempData);
        selectedPanel.SetActive(true);
    }
    void showTaskViewPanel()
    {
        if (selectedPanel != null) selectedPanel.gameObject.SetActive(false);
        selectedPanel = panels[6];
        selectedPanel.GetComponent<IPanel>().Initialize(tempData);
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
}
