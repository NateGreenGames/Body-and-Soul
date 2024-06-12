using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationButtonController : MonoBehaviour
{
    [SerializeField] GameObject navigationButtonPrefab;
    private List<GameObject> currentButtons = new List<GameObject>();
    private void OnEnable()
    {
        ViewpointManager.viewpointChangeEvent += OnViewPointChange;
    }
    private void OnDisable()
    {
        ViewpointManager.viewpointChangeEvent -= OnViewPointChange;
    }



    public void OnViewPointChange(Viewpoint _viewpointToChangeTo)
    {
        ClearButtons();
        for (int i = 0; i < _viewpointToChangeTo.navigationOptions.Length; i++)
        {
            GameObject newlySpawnedButton = Instantiate(navigationButtonPrefab, this.transform);
            currentButtons.Add(newlySpawnedButton);
            RectTransform newRectTran = newlySpawnedButton.GetComponent<RectTransform>();
            newlySpawnedButton.GetComponent<Widget_NavigationButton>().Init(ViewpointManager.instance.GetViewpointByIndex(_viewpointToChangeTo.navigationOptions[i].linkedViewpointIndex));
            switch (_viewpointToChangeTo.navigationOptions[i].direction)
            {
                case eNavDirection.up:
                    newRectTran.anchoredPosition = new Vector2(0, 220);
                    newRectTran.Rotate(newRectTran.forward, 90);
                    break;
                case eNavDirection.right:
                    newRectTran.anchoredPosition = new Vector2(395, 0);
                    break;
                case eNavDirection.down:
                    newRectTran.anchoredPosition = new Vector2(0, -220);
                    newRectTran.Rotate(newRectTran.forward, 90);
                    break;
                case eNavDirection.left:
                    newRectTran.anchoredPosition = new Vector2(-395, 0);
                    break;
                case eNavDirection.terminator:
                    break;
            }
        }
    }

    private void ClearButtons()
    {
        for (int i = currentButtons.Count - 1; i > -1; i--)
        {
            GameObject referenceToObject = currentButtons[i];
            currentButtons.Remove(referenceToObject);
            Destroy(referenceToObject);
        }
    }


}
