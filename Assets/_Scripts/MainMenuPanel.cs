using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPanel : MonoBehaviour
{
    private void Start()
    {
        //GameManager.Instance.loadAllGroupAccess(null);

        //GameManager.Instance.loadRolesData(null);

        PermissionManager.Instance.loadPermissions();

        //PermissionManager.Instance.GetPermissionForRole("Series Author", "create");
    }
    
    public void onMainPanelBtnClick()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
    }
    public void onSeriesEditBtnClick()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.SeriesView);
    }

    public void onRecordManagementBtnClick()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.RecordView);
    }

    public void onDatabaseEditBtnClick()
    {

    }

    public void onMusicPublisherBtnClick()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MusicView);
    }
    public void onLanguageBtnClick()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.LanguageView);
    }
    public void onProfileBtnClick()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.ProfileView, JsonUtility.ToJson(CreatorData.Instance.Author));
    }
    public void Logout()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.Login);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
