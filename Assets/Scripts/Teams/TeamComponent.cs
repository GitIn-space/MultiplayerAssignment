using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;
using Alteruna.Trinity;

public class TeamComponent : MonoBehaviour
{
    public int Team
    {
        get
        {
            return team;
        }
    }

    private int team = -1;
    private Multiplayer mp;

    public void StartDividingIntoTeams()
    {

    }

    void GetDividedIntoTeam(ushort fromUser, ProcedureParameters parameters, uint callId, ITransportStreamReader processor)
    {
        parameters.Get("team", team);
    }

    void Start()
    {
        mp = FindObjectOfType<Multiplayer>();
        mp.RegisterRemoteProcedure("GetDividedIntoTeam", GetDividedIntoTeam);
    }
}
