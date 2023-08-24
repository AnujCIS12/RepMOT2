using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePopup : SingletonMonoBehaviour<MessagePopup>
{
    public Text headText;
    public delegate void onConfirm();
    onConfirm onconfirm;

    public void initialize(onConfirm onconfirmCallback, string message)
    {
        gameObject.SetActive(true);
        onconfirm = onconfirmCallback;
        headText.text = "Message: " + message;
    }


    public void OkBtnClicked()
    {
        //UIPopupManager.Instance.HideSelectedPopUp();
        headText.text = "";
        gameObject.SetActive(false);
    }
}
