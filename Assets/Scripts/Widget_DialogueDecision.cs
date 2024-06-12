using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Widget_DialogueDecision : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI myButtonText;
    private string myText;
    private DialogueSequenceSO linkedDialogueSequence;
    private DialogueScreenWidget maker;
    private RelationshipChangeData[] consequences;

    private void OnEnable()
    {
        DialogueManager.decisionEvent += SelfDestruct;
    }
    private void OnDisable()
    {
        DialogueManager.decisionEvent -= SelfDestruct;
    }
    public void Init(DialogueSequenceSO _dialogueToLinkTo, string _decisionText, RelationshipChangeData[] _consequences, DialogueScreenWidget _maker)
    {
        maker = _maker;
        consequences = _consequences;
        myText = _decisionText;
        linkedDialogueSequence = _dialogueToLinkTo;
        myButtonText.text = myText;
    }


    public void OnClick()
    {
        DialogueManager.instance.TriggerDecisionEvent();
        for (int i = 0; i < consequences.Length; i++)
        {
            RelationshipManager.InvokeRelationshipChangeEvent(consequences[i]);
        }
        if (linkedDialogueSequence == null || linkedDialogueSequence.dialogueSequence.Length == 0)
        {
            maker.EndDialogue();
        }
        else
        {
            DialogueManager.instance.TriggerDialogueEvent(linkedDialogueSequence, true);
        }
    }

    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
