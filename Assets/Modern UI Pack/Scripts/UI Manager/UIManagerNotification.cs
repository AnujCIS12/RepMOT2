﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Michsky.UI.ModernUIPack
{
    [ExecuteInEditMode]
    public class UIManagerNotification : MonoBehaviour
    {
        [Header("Settings")]
        public UIManager UIManagerAsset;
        public bool webglMode = false;

        [Header("Resources")]
        public Image background;
        public Image icon;
        public TextMeshProUGUI title;
        public TextMeshProUGUI description;

        void Awake()
        {
            if (Application.isPlaying && webglMode == true)
                return;

            try
            {
                if (UIManagerAsset == null)
                    UIManagerAsset = Resources.Load<UIManager>("MUIP Manager");

                this.enabled = true;

                if (UIManagerAsset.enableDynamicUpdate == false)
                {
                    UpdateNotification();
                    this.enabled = false;
                }
            }

            catch { Debug.Log("<b>[Modern UI Pack]</b> No UI Manager found, assign it manually.", this); }
        }

        void LateUpdate()
        {
            if (UIManagerAsset == null)
                return;

            if (UIManagerAsset.enableDynamicUpdate == true)
                UpdateNotification();
        }

        void UpdateNotification()
        {
            if (Application.isPlaying && webglMode == true)
                return;

            try
            {
                background.color = UIManagerAsset.notificationBackgroundColor;
                icon.color = UIManagerAsset.notificationIconColor;
                title.color = UIManagerAsset.notificationTitleColor;
                description.color = UIManagerAsset.notificationDescriptionColor;
                title.font = UIManagerAsset.notificationTitleFont;
                title.fontSize = UIManagerAsset.notificationTitleFontSize;
                description.font = UIManagerAsset.notificationDescriptionFont;
                description.fontSize = UIManagerAsset.notificationDescriptionFontSize;
            }

            catch { }
        }
    }
}