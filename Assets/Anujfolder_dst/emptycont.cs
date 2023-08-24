using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class emptycont : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    int count;
    private void OnEnable()
    {
        count = gameObject.transform.childCount;
        for(int i=0;i<count;i++)
        {
            if(gameObject.transform.GetChild(i).gameObject.tag=="Task")
            {
                GameObject temp = gameObject.transform.GetChild(i).gameObject;
                Destroy(temp.gameObject);
            }
            
        }
    }
}
