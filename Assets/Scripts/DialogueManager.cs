using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public delegate void DialogueDelegate(DialogueSequenceSO _dialogueInfo, bool _startingOrEnding);
    public static event DialogueDelegate dialogueEvent;

    public delegate void DecisionDelegate();
    public static event DecisionDelegate decisionEvent;

    [SerializeField] DialogueSequenceSO testingSO;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void TriggerDialogueEvent(DialogueSequenceSO _dialogueInfo, bool _isOpening)
    {
        dialogueEvent?.Invoke(_dialogueInfo, _isOpening);
    }

    public void TriggerDecisionEvent()
    {
        decisionEvent?.Invoke();
    }

}
