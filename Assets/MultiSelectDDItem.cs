using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiSelectDDItem : MonoBehaviour
{
    MultipleSelectionDropDown mDD;
    private void Awake()
    {
        mDD = gameObject.GetComponentInParent(typeof(MultipleSelectionDropDown)) as MultipleSelectionDropDown;
    }
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Toggle>().onValueChanged.AddListener(delegate {
            onValueChanged(GetComponent<Toggle>());
        });
    }

    public void onValueChanged(Toggle toggle)
    {
        mDD.toggleChanged(toggle);
    }
}
