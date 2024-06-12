using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Widget_NavigationButton : MonoBehaviour
{
    private Viewpoint myLinkedViewpoint;
    public void Init(Viewpoint _linkedViewPoint)
    {
        myLinkedViewpoint = _linkedViewPoint;
    }


    public void OnClick()
    {
        ViewpointManager.instance.ChangeViewpoint(myLinkedViewpoint);
    }
}
