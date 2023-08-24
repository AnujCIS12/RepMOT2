using UnityEngine;
using Michsky.UI.ModernUIPack;

public class EGDropdown : MonoBehaviour
{
    public CustomDropdown myDropdown; // Your dropdown variable
    public DropdownMultiSelect multiDropdown;

    void YourFunction()
    {
        // Creating a new item
        myDropdown.CreateNewItem("Item Title",null);

        //// Creating items within a loop
        //for (int i = 0; i < yourIndexOrVariable; ++i)
        //{
        //    // Use false when using this in a loop
        //    myDropdown.CreateNewItem("Item Title", spriteIcon, false);
        //}

        // Initialize the new items
        myDropdown.SetupDropdown();

        // Add int32 (dynamic) events
        //myDropdown.onValueChanged.AddListener(TestFunction);

        //myDropdown.ChangeDropdownInfo(3); // Changing index & updating UI
        //myDropdown.RemoveItem("Item Title"); // Delete a specific item
        myDropdown.Animate(); // Animate dropdown
    }

    void TestFunction(int value)
    {
        Debug.Log("Changed index to: " + value.ToString());
    }
    public void printSelectedList()
    {
        myDropdown.PrintSelectedDropdownName();
    }
}