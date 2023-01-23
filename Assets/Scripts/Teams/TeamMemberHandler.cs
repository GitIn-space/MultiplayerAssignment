using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;
using Alteruna.Trinity;
using TMPro;

public class TeamMemberHandler : MonoBehaviour
{
    private Multiplayer mp;
    [SerializeField] private TextMeshProUGUI text;

    private int team0 = 0;
    private int team1 = 0;

    private void RegisterPlayer(ushort fromUser, ProcedureParameters parameters, uint callId, ITransportStreamReader processor)
    {
        switch(parameters.Get("team", -1))
        {
            case 0:
                team0++;
                break;
            case 1:
                team1++;
                break;
        }
    }

    private void DeregisterPlayer(ushort fromUser, ProcedureParameters parameters, uint callId, ITransportStreamReader processor)
    {
        switch (parameters.Get("team", -1))
        {
            case 0:
                team0--;
                break;
            case 1:
                team1--;
                break;
        }

        if (team0 <= 0 || team1 <= 0)
        {
            ProcedureParameters parms = new ProcedureParameters();
            parms.Set("team", team0 <= 0 ? 1 : 0);
            mp.InvokeRemoteProcedure("TeamWin", UserId.AllInclusive, parms);
        }
    }

    private void TeamWin(ushort fromUser, ProcedureParameters parameters, uint callId, ITransportStreamReader processor)
    {
        text.text = "Team " + parameters.Get("team", -1) + " won";
    }

    private void Start()
    {
        mp = FindObjectOfType<Multiplayer>();
        mp.RegisterRemoteProcedure("RegisterPlayer", RegisterPlayer);
        mp.RegisterRemoteProcedure("DeregisterPlayer", DeregisterPlayer);
        mp.RegisterRemoteProcedure("TeamWin", TeamWin);

        text.text = "";
    }

    private void Update()
    {
        
    }
}
