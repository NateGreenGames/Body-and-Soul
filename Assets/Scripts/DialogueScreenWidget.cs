using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueScreenWidget : MonoBehaviour
{
    [SerializeField] GameObject decisionWidgetPrefab;
    [Space]
    [SerializeField] Image sprite1;
    [SerializeField] Image sprite2;
    [SerializeField] Image soloSprite;
    [SerializeField] Image background;
    [SerializeField] Image speakerLabel;
    [SerializeField] Image dialogueIndicator;
    [SerializeField] Image decisionPrivacyPanel;
    [Space]
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI speakerTextLeft;
    [SerializeField] TextMeshProUGUI speakerTextRight;
    [Space]
    [SerializeField] GameObject toggleObject;
    [SerializeField] GameObject decisionLayOutGroup;
    [SerializeField] GameObject dialogueBox;
    [SerializeField] GameObject speakerBoxLeft;
    [SerializeField] GameObject speakerBoxRight;

    private Queue<DialogueStepData> activeDialogueSequence;
    private DialogueSequenceSO copyOfSequence;
    private bool currentlyInADialogueSequence = false;
    private bool currentlyMakingChoice = false;
    private bool speakerLabelCurrentlyOnRight = false;
    private bool finishedTalking = true;
    private string currentDialogueString;
    private AudioSource m_AS;
    private void OnEnable()
    {
        DialogueManager.dialogueEvent += StartDialogue;
        DialogueManager.decisionEvent += DecisionMade;
    }

    private void OnDisable()
    {
        DialogueManager.dialogueEvent -= StartDialogue;
        DialogueManager.decisionEvent -= DecisionMade;
    }


    private void Start()
    {
        m_AS = GetComponent<AudioSource>();
        toggleObject.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (currentlyMakingChoice == false && currentlyInADialogueSequence == true && finishedTalking == true)
            {
                ProgressDialogue();
            }
            else if(finishedTalking == false )
            {
                SkipDialogue();
            }
        }
    }

    public void StartDialogue(DialogueSequenceSO _dialogueInfo, bool _isOpening)
    {
        if(_dialogueInfo != null && _isOpening)
        {
            activeDialogueSequence = new Queue<DialogueStepData>(_dialogueInfo.dialogueSequence);
            copyOfSequence = _dialogueInfo;
            currentlyInADialogueSequence = true;
            toggleObject.SetActive(true);
            ProgressDialogue();
        }
    }
    private void ProgressDialogue()
    {
        //First we check to see if we've reached the end of the dialogue sequence.
        if (activeDialogueSequence.Count <= 0)
        {
            EndDialogue();
            return;
        }

        //If we get here, we have at least one step left, pull down the next step from the queue.
        DialogueStepData currentStep = activeDialogueSequence.Dequeue();
        //IF there are no sprites, turn off all sprites. If there is only one sprite, set it to the current star sprite position, and if there are two sprites, assign them to their corresponding sprite slot.
        if (currentStep.sprite1 == null && currentStep.sprite2 == null)
        {
            sprite1.enabled = false;
            sprite2.enabled = false;
            soloSprite.enabled = false;
        }
        else if (currentStep.sprite1 == null || currentStep.sprite2 == null)
        {
            sprite1.enabled = false;
            sprite2.enabled = false;
            soloSprite.enabled = true;
            if (currentStep.sprite2 == null)
            {
                soloSprite.sprite = currentStep.sprite1;
            }
            else
            {
                soloSprite.sprite = currentStep.sprite2;
            }
            soloSprite.SetNativeSize();
        }
        else if (currentStep.sprite1 != null && currentStep.sprite2 != null)
        {
            sprite1.enabled = true;
            sprite2.enabled = true;
            soloSprite.enabled = false;
            sprite1.sprite = currentStep.sprite1;
            sprite2.sprite = currentStep.sprite2;
            sprite1.SetNativeSize();
            sprite2.SetNativeSize();
        }

        //Update our background image
        background.sprite = currentStep.backgroundImage;



        //If the dialogue box is displayed on this step, enable the box and update the text. Otherwise, disable the dialogue box and it's children.

        if (currentStep.displayDialogueBox)
        {
            dialogueBox.SetActive(true);
            StartCoroutine(DialogueRoutine(currentStep.dialogue, currentStep.talkingSpeed));
            currentDialogueString = currentStep.dialogue;
        }
        else
        {
            dialogueBox.SetActive(false);
        }

        //First, check if the speaker label is hidden. Then check which side the panel should appear on and do so.

        if(currentStep.displaySpeakerName == false)
        {
            speakerBoxLeft.SetActive(false);
            speakerBoxRight.SetActive(false);
        }
        else
        {
            if (!currentStep.mirrorSpeakerLabel)
            {
                speakerBoxLeft.SetActive(true);
                speakerBoxRight.SetActive(false);
                speakerTextLeft.text = currentStep.speakerName;
            }
            else
            {
                speakerBoxLeft.SetActive(false);
                speakerBoxRight.SetActive(true);
                speakerTextRight.text = currentStep.speakerName;
            }
        }

        //Check to see if audio files exist on this step, if they do, cycle through the array and play all of them that are not null.
        for (int i = 0; i < currentStep.OnChangeSound.Length; i++)
        {
            if(currentStep.OnChangeSound[i] != null)
            {
                m_AS.PlayOneShot(currentStep.OnChangeSound[i]);
            }
        }


        //Check to see if any decision options exist for the current dialogue step. If they do, spawn the buttons and set them up. Prevent dialogue progression 
        if (currentStep.possibleDecisions.Length > 0)
        {
            currentlyMakingChoice = true;
            decisionPrivacyPanel.enabled = true;
            if (activeDialogueSequence.Count > 0)
            {
                Debug.Log("Unreachable dialogue detected. You have dialouge steps after a dialogue node that contains a decision.");
            }
            for (int i = 0; i < currentStep.possibleDecisions.Length; i++)
            {
                //spawn the decision option boxes.
                Widget_DialogueDecision newDecision = Instantiate(decisionWidgetPrefab, decisionLayOutGroup.transform).GetComponent<Widget_DialogueDecision>();
                newDecision.Init(currentStep.possibleDecisions[i].newDialoguePath, currentStep.possibleDecisions[i].decisionText, currentStep.possibleDecisions[i].relationshipEffects, this);
            }
        }

    }
    public void EndDialogue()
    {
        DialogueManager.instance.TriggerDialogueEvent(copyOfSequence, false);
        Debug.Log("Closing Dialogue.");
        activeDialogueSequence = null;
        currentlyInADialogueSequence = false;
        toggleObject.SetActive(false);
    }
    private void DecisionMade()
    {
        currentlyMakingChoice = false;
        decisionPrivacyPanel.enabled = false;
    }
    private IEnumerator DialogueRoutine(string _StringToTransitionTo, float _talkingSpeed)
    {
        finishedTalking = false;
        dialogueIndicator.enabled = false;
        int currentLetterBeingWorkedOn = 0;
        while (!finishedTalking)
        {
            currentLetterBeingWorkedOn++;
            dialogueText.text = _StringToTransitionTo.Substring(0, currentLetterBeingWorkedOn);
            if (currentLetterBeingWorkedOn == _StringToTransitionTo.Length)
            {
                finishedTalking = true;
                dialogueIndicator.enabled = true;
            }
            yield return new WaitForSeconds(_talkingSpeed);
        }
    }

    private void SkipDialogue()
    {
        StopCoroutine("DialogueRoutine");
        dialogueText.text = currentDialogueString;
        finishedTalking = true;
        dialogueIndicator.enabled = true;
    }

}
