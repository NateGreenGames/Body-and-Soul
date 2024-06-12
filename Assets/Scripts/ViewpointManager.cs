using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum eNavDirection { up, right, down, left, terminator }
[System.Serializable]
public class Viewpoint
{
    public CinemachineVirtualCamera viewpointCamera;
    public NavigationOption[] navigationOptions;
}

[System.Serializable]
public class NavigationOption
{
    public eNavDirection direction;
    public int linkedViewpointIndex;
}
public class ViewpointManager : MonoBehaviour
{
    public delegate void ViewpointChange(Viewpoint newViewpoint);
    public static event ViewpointChange viewpointChangeEvent;



    public static ViewpointManager instance;
    [SerializeField] int startingViewpointIndex;
    [SerializeField] Viewpoint[] viewpoints;
    private void Awake()
    {
        //We don't need a singleton in this case because this manager is going to be scene specific and must be overridden when you enter a new scene.
        instance = this;
    }

    private void Start()
    {
        ChangeViewpoint(viewpoints[startingViewpointIndex]);
    }

    public void ChangeViewpoint(Viewpoint _newViewpoint)
    {
        viewpointChangeEvent?.Invoke(_newViewpoint);
        for (int i = 0; i < viewpoints.Length; i++)
        {
            if(viewpoints[i] == _newViewpoint)
            {
                viewpoints[i].viewpointCamera.Priority = 10;
            }
            else
            {
                viewpoints[i].viewpointCamera.Priority = 0;
            }
        }
    }

    public Viewpoint GetViewpointByIndex(int _index)
    {
        return viewpoints[_index];
    }
}
