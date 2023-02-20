using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonFocus : MonoBehaviour
{
    [SerializeField] private GameObject lastSelectedButton;

    private void Start()
    {
        lastSelectedButton = new GameObject();
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastSelectedButton);
        }
        else
        {
            lastSelectedButton = EventSystem.current.currentSelectedGameObject;
        }
    }

    public void UpdateSelectedButton(GameObject newButton)
    {
        lastSelectedButton = newButton;
        EventSystem.current.SetSelectedGameObject(lastSelectedButton);
    }
    
}