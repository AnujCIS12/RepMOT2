
/*
using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;

namespace _Scripts.GUI
{
    public sealed class TabToNextController : MonoBehaviour, IUpdateSelectedHandler
    {
        public Selectable nextField;
       

        public void OnUpdateSelected(BaseEventData data)
        {
            if (LoginPanel.keyTab )
                nextField.Select();

        
            //Debug.Log("Return/Enter key was pressed." + gameObject.name);
        }

    
        void Start()
        {
            
        }

        void Update()
        {
            //Detect when the Return key is pressed down
        
           


           
         //Detect when the Tab key has been released
         if (!Keyboard.current.tabKey.wasReleasedThisFrame) return;
         nextField.Select();
         Debug.Log("Tab was pressed.");

         //
         //Detect when the Return key is pressed down
         if (Input.GetKeyDown(KeyCode.KeypadEnter))
         {
             //Debug.Log("Return/Enter key was pressed.");
         }

         //Detect when the Return key has been released
         if (Input.GetKeyUp(KeyCode.KeypadEnter))
         {
             //Debug.Log("Return/Enter key was released.");
         }
        
        }

    }
}
 */

