using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiDDController : MonoBehaviour
{

    public MultipleSelectionDropDown multiSelectDD;
    public List<string> dropDownList;
    // Start is called before the first frame update
    void Start()
    {
      multiSelectDD.InstantiateItem(dropDownList);
     // multiSelectDD.InstatiateSelectedDeckList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
