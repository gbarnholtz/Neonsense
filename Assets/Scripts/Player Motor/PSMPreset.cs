using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PSMPreset", menuName = "ScriptableObjects/PSMPreset")]
public class PSMPreset : SerializedScriptableObject
{
    [OdinSerialize] public MoveState EntryState;
    [OdinSerialize] public HashSet<MoveState> MoveStates;
    [SerializeReference] public Dictionary<MoveState, Dictionary<Transition, MoveState>> Transitions;
}
