using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteConfirmPopup : SingletonMonoBehaviour<DeleteConfirmPopup>
{
    public Text headText;
    public delegate void onConfirm();
    onConfirm onconfirm;
    // Start is called before the first frame update
    
    public void initialize(onConfirm onconfirmCallback, string name)
    {
        onconfirm = onconfirmCallback;
        headText.text = "Are you sure you want to delete this " + name + "?";
    }

    public void yesBtnClicked()
    {
        onconfirm();
        UIPopupManager.Instance.HideSelectedPopUp();
    }
    public void cancelBtnClicked()
    {
        UIPopupManager.Instance.HideSelectedPopUp();
    }
}
