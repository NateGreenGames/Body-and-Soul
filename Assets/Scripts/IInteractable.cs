using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public bool isInteractable { get; set; }

    public void OnInteractStart();
}
