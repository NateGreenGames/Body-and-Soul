using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sequence", menuName = "Create New Dialogue Sequence")]
public class DialogueSequenceSO : ScriptableObject
{
    public DialogueStepData[] dialogueSequence;
}

[System.Serializable]
public class DialogueStepData
{
    [Header("Graphics:")]
    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite backgroundImage;

    [Space]
    [Header("Audio:")]
    public AudioClip[] OnChangeSound;

    [Space]
    [Header("Animation:")]
    public string animationToPlay;

    [Space]
    [Header("Speaker Settings:")]
    public bool displaySpeakerName = true;
    public string speakerName;
    public bool mirrorSpeakerLabel = false;

    [Space]
    [Header("Dialogue Settings:")]
    public bool displayDialogueBox = true;
    [TextArea(3, 20)] public string dialogue;
    [Tooltip("This is the time between when each character appears in the dialogue box.")] public float talkingSpeed = 0.05f;

    [Space]
    [Header("Decision Settings:")]
    [Tooltip("If there are any decisionTexts in an object, the game will believe that this panel is a decision.")] public DialogueDecision[] possibleDecisions;

}

[System.Serializable]
public class DialogueDecision
{
    public string decisionText;
    public DialogueSequenceSO newDialoguePath;
    public RelationshipChangeData[] relationshipEffects;
}
