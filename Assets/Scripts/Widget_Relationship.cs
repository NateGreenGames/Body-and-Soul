using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Widget_Relationship : MonoBehaviour
{
    private bool currentlyBeingDisplayed = false;
    private Animation m_anim;

    private void Start()
    {
        m_anim = GetComponent<Animation>();
    }
    private void OnEnable()
    {
        RelationshipManager.RelationshipChangeEvent += OnRelationshipEvent;
    }
    private void OnDisable()
    {
        RelationshipManager.RelationshipChangeEvent -= OnRelationshipEvent;
    }

    private void OnRelationshipEvent(eCharacters _character, int _change)
    {
        if(!currentlyBeingDisplayed)
        {
            StartCoroutine(ShowRoutine());
        }
    }

    private IEnumerator ShowRoutine()
    {
        m_anim.Play();
        currentlyBeingDisplayed = true;
        while (m_anim.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }
        currentlyBeingDisplayed = false;
    }
}
