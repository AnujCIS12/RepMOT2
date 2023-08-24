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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void initialize(onConfirm onconfirmCallback, string name)
    {
        onconfirm = onconfirmCallback;
        headText.text = headText.text + name + "?";
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
