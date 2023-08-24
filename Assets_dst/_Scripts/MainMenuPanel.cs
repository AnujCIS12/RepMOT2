using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPanel : MonoBehaviour
{
    public void onSeriesEditBtnClick()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.SeriesView);
    }

    public void onRecordManagementBtnClick()
    {
        
    }

    public void onDatabaseEditBtnClick()
    {

    }

    public void onMusicPublisherBtnClick()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MusicView);
    }
}
