using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour {

    public enum Teams
    {
        Team1,
        Team2,
        Team3,
        Team4,
        Neutral
    };

    public Teams currentTeam;

    public Teams GetTeam()
    {
        return currentTeam;
    }

    public void SetTeam(Teams t)
    {
        currentTeam = t;
    }

    public bool CanAttack(Teams targetTeam)
    {
        return (!currentTeam.Equals(targetTeam));
    }
}
