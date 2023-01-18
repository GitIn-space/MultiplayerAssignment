using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;
using Alteruna.Trinity;
using TMPro;

public class TeamComponent : MonoBehaviour
{
    private Alteruna.Avatar avatar;
    [SerializeField] private SpriteRenderer render;
    private Multiplayer mp;
    private TeamSynch ts;

    private int team = -1;
    private static TeamSynch myTeam;
    public int Team
    {
        get
        {
            return team;
        }
    }

    private void StartDividingIntoTeams()
    {
        foreach (User each in mp.GetUsers())
        {
            ProcedureParameters parameters = new ProcedureParameters();
            parameters.Set("team", (each.Index % 2));
            mp.InvokeRemoteProcedure("GetDividedIntoTeam", each.Index, parameters);
        }
        /*foreach(Alteruna.Avatar each in GameObject.FindObjectsOfType<Alteruna.Avatar>())
        {
            print(each.Possessor.Name);
            each.GetComponent<TeamSynch>().team = each.Possessor.Index % 2;
        }*/
    }

    private void GetDividedIntoTeam(ushort fromUser, ProcedureParameters parameters, uint callId, ITransportStreamReader processor)
    {
        team = parameters.Get("team", -1);
        GameObject.Find("WinText").GetComponent<TextMeshProUGUI>().text = "team " + team;
        ts.team = team;
    }

    private void Start()
    {
        avatar = GetComponent<Alteruna.Avatar>();

        mp = FindObjectOfType<Multiplayer>();
        mp.RegisterRemoteProcedure("GetDividedIntoTeam", GetDividedIntoTeam);

        ts = GetComponentInChildren<TeamSynch>();
    }

    private void Update()
    {
        if (!avatar.IsMe)
        {
            //print(ts.team);
            if (ts.team == -1)
                render.color = new Color(255, 0, 255);
            else if (ts.team == myTeam.team)
                render.color = Color.blue;
            else
                render.color = Color.red;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.T))
                StartDividingIntoTeams();

            if(myTeam == null)
                myTeam = GetComponentInChildren<TeamSynch>();
            //print(ts.team);
        }
        //team = ts.team;
    }
}
