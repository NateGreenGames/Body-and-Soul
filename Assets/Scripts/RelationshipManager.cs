using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCharacters { None, Thana, Nikita, Cora, Paris, Terminator }
public class RelationshipManager : MonoBehaviour
{
    public delegate void RelationshipStatChange(eCharacters _character, int _friendshipChange);
    public static event RelationshipStatChange RelationshipChangeEvent;


    public static void InvokeRelationshipChangeEvent(RelationshipChangeData _change)
    {
        RelationshipChangeEvent?.Invoke(_change.effectedCharacter, _change.friendshipChange);
    }

}

[System.Serializable]
public class RelationshipInfromation
{
    public eCharacters myKey;
    public int friendship;

    public RelationshipInfromation(eCharacters _myCharacter)
    {
        this.myKey = _myCharacter;
        this.friendship = 0;
    }
}

[System.Serializable]
public class RelationshipChangeData
{
    public eCharacters effectedCharacter;
    public int friendshipChange;
}
