using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

///<summary>
///Example use of Modal Panel. This example Shows how to spawn a panel with an image and one button. image is optional, one button is mandatory. 2 max buttons Grab reference at Awake. Create EventButtonDetails type buttons, 2 maximum, discuss if needed more. pass buttons to Spawnpanel as in Example 2 or use directly as in Example2
///</summary>

//You need an event EventButtonDetails button and string title to get working panel. image and second button are not mandatory.
public class TestModalPanel : MonoBehaviour {

    public Sprite icon;
    
    private ModalPanel modalPanel;


    //grab instance of panel
    void Awake () {
        modalPanel = ModalPanel.Instance();
    }
    
    //example use case 1. Assing directly
    public void TestExample1 () {
        ModalPanelDetails modalPanelDetails = new ModalPanelDetails ();
        modalPanelDetails.question = "This is an announcement!\nIf you don't like it, shove off!";
        modalPanelDetails.button1Details = new EventButtonDetails ();
        modalPanelDetails.button1Details.buttonTitle = "Gotcha!";
        modalPanelDetails.button1Details.action = TestCancelFunction;

        modalPanel.SpawnWithDetails (modalPanelDetails);
    }

    //example use case 2. use from method.
    // two buttons and image
    // SpawnPanel("Test","This is an test of modal panel functionality", button1Detail,button2Detail,icon);
    // one button and image
    // SpawnPanel("Test","This is an test of modal panel functionality", button1Detail,null,icon);
    // one button and no image
    // SpawnPanel("Test","This is an test of modal panel functionality", button1Detail);

    public void TestExample2 () {
        EventButtonDetails button1Detail = new EventButtonDetails {buttonTitle = "Gotcha!", action = TestCancelFunction};
        EventButtonDetails button2Detail = new EventButtonDetails {buttonTitle = "Gotcha!", action = TestCancelFunctionforExample2ofButton1};
        //two buttons and image
        SpawnPanel("Test","This is an test of modal panel functionality", button1Detail,button2Detail,icon);
    }

/// <summary>
/// Spawn a Modal Panel with max button count of 2 and an optinal image
/// </summary>
/// <param name="button2">can be null parameter</param>
/// <param name="icon">can be null parameter</param>
    void SpawnPanel(string mainTitle, string titleDescription, EventButtonDetails button1, EventButtonDetails button2 = null, Sprite icon = null ) {
         ModalPanelDetails modalPanelDetails = new ModalPanelDetails {question = mainTitle,description = titleDescription, iconImage = icon};
          modalPanelDetails.button1Details = button1;
          modalPanelDetails.button2Details = button2;
          modalPanel.SpawnWithDetails (modalPanelDetails);
    }


    //callback for buttons
    void TestCancelFunction () {
        Debug.Log("I am button call 1!");
    }

    void TestCancelFunctionforExample2ofButton1 () {
        Debug.Log("I am button call 2!");
    }

}
