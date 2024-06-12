using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public bool movementDisabled;

    private void OnEnable()
    {
        DialogueManager.dialogueEvent += OnDialogueEvent;
    }

    private void OnDisable()
    {
        DialogueManager.dialogueEvent -= OnDialogueEvent;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !movementDisabled)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider.gameObject.TryGetComponent<IInteractable>(out IInteractable _interactable))
                {
                    _interactable.OnInteractStart();
                }
            }
        }
    }


    public void OnDialogueEvent(DialogueSequenceSO dialogueSequence, bool _opening)
    {
        movementDisabled = _opening;
    }
}
