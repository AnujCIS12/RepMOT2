

using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;

namespace _Scripts.GUI
{
    public sealed class TabToNextController : MonoBehaviour, IUpdateSelectedHandler
    {
        public Selectable nextField;
        private LoginPanel loginPanel;


        public void OnUpdateSelected(BaseEventData data)
        {
            // if (LoginPanel.keyTab )
            //     nextField.Select();

        
            //Debug.Log("Return/Enter key was pressed." + gameObject.name);
        }

    
        void Start()
        {
            loginPanel = GetComponent<LoginPanel>();
        }

        void Update()
        {
            
        
         if (Input.GetKeyUp(KeyCode.Tab) )
         {
             nextField.Select();
             Debug.Log("Tab was pressed.");
         }

      
         if (Input.GetKeyDown(KeyCode.Return))
         {
             Debug.Log("Return/Enter was pressed.");
             loginPanel.onLoginButtonClick();
         }

         //Detect when the Return key has been released
         if (Input.GetKeyUp(KeyCode.KeypadEnter))
         {
             //Debug.Log("Return/Enter key was released.");
             
         }
        
        }

    }
}
 

