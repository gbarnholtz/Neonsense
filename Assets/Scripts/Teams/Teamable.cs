using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class Teamable : SerializedMonoBehaviour
{
    public readonly Team Team;

    private void Awake()
    {
        ITeamable[] teamables = GetComponents<ITeamable>();
        for (int i = 0; i < teamables.Length; i++)
        {
            teamables[i].Team = Team;
        }
    }
}


