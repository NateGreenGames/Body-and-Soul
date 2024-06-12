using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTest : MonoBehaviour, IInteractable
{
    [SerializeField] DialogueSequenceSO dialogue;
    public bool isInteractable { get; set; }

    private void Start()
    {
        isInteractable = true;
    }
    public void OnInteractStart()
    {
        DialogueManager.instance.TriggerDialogueEvent(dialogue, true);
    }
}
