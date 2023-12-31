using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleFileBrowser;
using static SimpleFileBrowser.FileBrowser;
using System;

public class FileBrowserEg1 : SingletonMonoBehaviour<FileBrowserEg1>
{

    public delegate void onSuccess(string Text);
    public delegate void onError(string Text);
    OnSuccess successCallback;
    onError errorCallback;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectBtnClicked()
    {
        //SetDefaultFilter(".mp3");
        //ShowLoadDialog(onLoaded, onLoadFailed, PickMode.Files, false,null, null, "Load Music", "Select");
        GetMusicPath();
    }
    public void onLoaded(string[] paths)
    {
        
        foreach(string path in paths)
        {
            Debug.Log("Path" + path);
        }
        Debug.Log(getName(paths[0]));
        if(successCallback!=null) successCallback(paths);
        
    }
    public void onLoadFailed()
    {
        Debug.Log("Load Failed");
        errorCallback("Failed");
    }
    public void closePopup()
    {
        HideDialog(false);
        //FileBrowser.Hide();
        //FileBrowser.setActive(false);
    }

    public void GetMusicPath(OnSuccess success=null, onError error=null)
    {
        //FileBrowser.SetFilters(false, new FileBrowser.Filter("Images", ".jpg", ".png"));
        FileBrowser.SetFilters(false, new FileBrowser.Filter("Audio", ".mp3", ".wav", ".aif",".aiff", ".ogg"));
        FileBrowser.SetDefaultFilter("Audio");
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe", "Images", ".jpg", ".png", ".mp4");
        FileBrowser.AddQuickLink("Users", "C:\\Users", null);

        successCallback = success;
        errorCallback = error;
        FileBrowser.ShowLoadDialog(onLoaded,
                                   () => { Debug.Log("Canceled"); },
                                   FileBrowser.PickMode.Files, false, null, null, "Select Music File", "Select");

        // Coroutine example
        //StartCoroutine(ShowLoadDialogCoroutine());
    }
    public void GetImagePath(OnSuccess success = null, onError error = null)
    {
        //FileBrowser.SetFilters(false, new FileBrowser.Filter("Images", ".jpg", ".png"));
        FileBrowser.SetFilters(false, new FileBrowser.Filter("Images", ".jpg", ".png",".jpeg"));
        FileBrowser.SetDefaultFilter(".mp3");
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe", ".mp3", ".aac", ".wav", ".mp4");
        FileBrowser.AddQuickLink("Users", "C:\\Users", null);

        successCallback = success;
        errorCallback = error;
        FileBrowser.ShowLoadDialog(onLoaded,
                                   () => { Debug.Log("Canceled"); },
                                   FileBrowser.PickMode.Files, false, null, null, "Select Music File", "Select");

        // Coroutine example
        //StartCoroutine(ShowLoadDialogCoroutine());
    }

    public void GetVideoPath(OnSuccess success = null, onError error = null)
    {
        //FileBrowser.SetFilters(false, new FileBrowser.Filter("Images", ".jpg", ".png"));
        FileBrowser.SetFilters(false, new FileBrowser.Filter("Video", ".mov", ".m4a", ".m4v", ".mp4"));
        FileBrowser.SetDefaultFilter("Video");
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe", "Images", ".jpg", ".png", ".mp3", ".wav", ".aif", ".aiff", ".ogg");
        FileBrowser.AddQuickLink("Users", "C:\\Users", null);

        successCallback = success;
        errorCallback = error;
        FileBrowser.ShowLoadDialog(onLoaded,
                                   () => { Debug.Log("Canceled"); },
                                   FileBrowser.PickMode.Files, false, null, null, "Select Video File", "Select");

        // Coroutine example
        //StartCoroutine(ShowLoadDialogCoroutine());
    }

    public void GetLanguagePath(OnSuccess success = null, onError error = null)
    {
        //FileBrowser.SetFilters(false, new FileBrowser.Filter("Images", ".jpg", ".png"));
        FileBrowser.SetFilters(false, new FileBrowser.Filter("Language","csv"));
        FileBrowser.SetDefaultFilter("Language");
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe", "Images", ".jpg", ".png", ".mp4");
        FileBrowser.AddQuickLink("Users", "C:\\Users", null);

        successCallback = success;
        errorCallback = error;
        FileBrowser.ShowLoadDialog(onLoaded,
                                   () => { Debug.Log("Canceled"); },
                                   FileBrowser.PickMode.Files, false, null, null, "Select Language File", "Select");

        // Coroutine example
        //StartCoroutine(ShowLoadDialogCoroutine());
    }
    public string getName(string str)
    {
        String[] strlist = str.Split("\\");
        return strlist[strlist.Length - 1];
    }
}
