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
        int c = 0;
        foreach(User each in mp.GetUsers())
        {
            ProcedureParameters parameters = new ProcedureParameters();
            parameters.Set("team", (c++ % 2));
            mp.InvokeRemoteProcedure("GetDividedIntoTeam", each.Index, parameters);
        }
    }

    void GetDividedIntoTeam(ushort fromUser, ProcedureParameters parameters, uint callId, ITransportStreamReader processor)
    {
        team = parameters.Get("team", 0);
    }

    void Start()
    {
        mp = FindObjectOfType<Multiplayer>();
        mp.RegisterRemoteProcedure("GetDividedIntoTeam", GetDividedIntoTeam);
    }
}
