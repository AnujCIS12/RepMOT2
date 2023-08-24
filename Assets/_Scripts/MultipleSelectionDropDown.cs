using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MultipleSelectionDropDown : MonoBehaviour
{
    public GameObject template;
    public GameObject content;
    public GameObject prefab;
    Text SelectedItemName;

    public bool limitation;
    public int limitationLimit;

    public List<GameObject> items = new List<GameObject>();
    public List<string> selectedItemList;
    public Button btn;

    public delegate void toggleValueChanged();

    // Start is called before the first frame update
    //private void Awake()
    //{
    //    template = transform.Find("Template").gameObject;
    //    content = template.transform.Find("Viewport").transform.Find("Content").gameObject;
    //    prefab = content.transform.Find("Item").gameObject;
    //    SelectedItemName = transform.Find("Label").transform.GetComponent<Text>();
    //    SelectedItemName.text = "";
    //    //InstantiateItem(10, );
    //    template.SetActive(false);
    //}
    private void Awake()
    {
        btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(ToggleList);
    }
    void Start()
    {
        //template = transform.Find("Template").gameObject;
        //content = template.transform.Find("Viewport").transform.Find("Content").gameObject;
        //prefab = content.transform.Find("Item").gameObject;

        //SelectedItemName = transform.Find("Label").transform.GetComponent<Text>();
        //SelectedItemName.text = "";

        //InstantiateItem(10, );
        template.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ToggleList(){
        if (template.activeSelf)
        {
            template.SetActive(false);
        }
        else template.SetActive(true);
    }

    public void InstantiateItem(List<string> names, bool tempLimitation = false, int tempLimitationLimit = 1000)
    {
        limitation = tempLimitation;
        limitationLimit = tempLimitationLimit;
        GetParentRefrence();
        print("I'm in Instantiate" + name);
        print("Name Count" + names.Count);
        content.GetComponent<RectTransform>().sizeDelta = prefab.GetComponent<RectTransform>().sizeDelta * names.Count;
        GameObject item;
        for (int i = 0; i < names.Count; i++){
            //int name = i+1;
            print("I'm in instantiate object"+ names[i]);
            item = Instantiate(prefab, content.transform);
            item.transform.Find("Item Label").GetComponent<Text>().text = names[i];
            //item.GetComponent<Toggle>().onValueChanged.AddListener(delegate {
            //    toggleChanged(item.GetComponent<Toggle>());
            //});
            //item.GetComponent<Toggle>().onValueChanged.AddListener((value) => {
            //    Debug.Log(value);
            //});
            item.GetComponent<Toggle>().onValueChanged.AddListener(delegate { });
            item.SetActive(true);
            items.Add(item);
        }
        //Destroy(prefab);

    }
    public void toggleChanged(Toggle toggle)
    {
        string itemName = toggle.transform.Find("Item Label").GetComponent<Text>().text;
        Debug.Log(itemName);
        if (toggle.isOn)
        {
            Debug.Log("Adding Item");
            
            selectedItemList.Add(itemName);
        }
        if (!toggle.isOn)
        {
            Debug.Log("Removing Item");
            if(selectedItemList.Contains(itemName)) selectedItemList.Remove(itemName);
        }
        UpdateLabel();

    }
    public void InstatiateSelectedList()
    {
        selectedItemList = new List<string>();
    }
    public void AddOptions(List<string> _list)
    {
        InstantiateItem(_list);
    }
    public void ClearOptions()
    {
        foreach(GameObject item in items)
        {
            Destroy(item);
        }
    }

    //new to show check mark on selected items
    
    public void OnSelectedItem(List<string> tempSelectedItemList)
    {
        //int i = 0;
        selectedItemList = tempSelectedItemList;
        print("I'm on Selectd");
        print("Selected item List Count " + selectedItemList.Count);
        print("Item Count" + items.Count);
        for(int i= 0;i<selectedItemList.Count;i++)
        {
            for(int j = 0; j < items.Count; j++)
            {
                if (selectedItemList[i] == items[j].transform.Find("Item Label").GetComponent<Text>().text)
                {
                    print("I'm on Selectd and item Matched"+j);
                    print(selectedItemList[i]);
                    print(items[j].transform.Find("Item Label").GetComponent<Text>().text);
                    items[j].transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                    //selectedItemList.Add(selectedItemList[i]);
                }
            }
            //print("I'm on Selectd and on loop");
            //print(items[i].transform.Find("Item Label").GetComponent<Text>().text);
            //print(selectedItemList[i]);


        }
    }
    public void EnableSelectedItemList(List<string> _selectedItemList)
    {
        selectedItemList = _selectedItemList;
        Debug.Log("*&&*&*&*&*&*&*&*&&*&*&*&*&*&*&*&*&*&*&* selected List item count"+_selectedItemList.Count);
        foreach(Transform item in content.transform)
        {
            Text itemLabel = item.Find("Item Label").GetComponent<Text>();
            string itemName = itemLabel.text;
            Debug.Log("Item Name is " + itemName);
            if(selectedItemList.Contains(itemName))
            {
                Debug.Log("Current Item is selected");
                item.GetComponent<Toggle>().isOn = true;
            }
        }
        UpdateLabel();
    }

    public void SelectItem(){
        GameObject currentlySelectedItem;
        GameObject checkBox;
        string selectedName;
        currentlySelectedItem = EventSystem.current.currentSelectedGameObject;
        checkBox = currentlySelectedItem.transform.Find("Item Checkmark").gameObject;
        selectedName = currentlySelectedItem.transform.GetComponent<Text>().text;
        if(checkBox.activeSelf){
            print("I'm on Remove item from selected list");
            checkBox.SetActive(false);
            selectedItemList.Remove(selectedName);
            UpdateLabel();
        }
        else{
            if (limitation && selectedItemList.Count >= limitationLimit)
            {
                print("you are rich on limitation");
            }
            else
            {
                print("I'm on Add item from selected list and  Item Name is " + selectedName);
                checkBox.SetActive(true);
                selectedItemList.Add(selectedName);
                UpdateLabel();
            }
            //print("I'm on Add item from selected list and  Item Name is "+selectedName);
            //checkBox.SetActive(true);
            //selectedItemList.Add(selectedName);
            //UpdateLabel();
        }
    }
    public void UpdateLabel(){
        for (int i = 0; i <= selectedItemList.Count; i++){
            SelectedItemName.text = string.Join(",", selectedItemList.ToArray());
        }

    }
    public void GetParentRefrence(){
        template = transform.Find("Template").gameObject;
        template.SetActive(true);
        content = template.transform.Find("Viewport").transform.Find("Content").gameObject;
        prefab = content.transform.Find("Item").gameObject;
        SelectedItemName = transform.Find("Label").transform.GetComponent<Text>();
        SelectedItemName.text = "";
        //InstantiateItem(10, );
        template.SetActive(false);
    }
    

}
