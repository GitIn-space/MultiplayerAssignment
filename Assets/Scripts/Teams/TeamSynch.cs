using Alteruna;
using UnityEngine;
using Avatar = Alteruna.Avatar;

public class TeamSynch : Synchronizable
{
    [SerializeField] SpriteRenderer render;

    // Data to be synchronized with other players in our playroom.
    public int team = -1;

    // Used to store the previous version of our data so that we know when it has changed.
    private int oldTeam;

    public override void DisassembleData(Reader reader, byte LOD)
    {
        // Set our data to the updated value we have recieved from another player.
        team = reader.ReadInt();

        // Save the new data as our old data, otherwise we will immediatly think it changed again.
        oldTeam = team;
    }

    public override void AssembleData(Writer writer, byte LOD)
    {
        // Write our data so that it can be sent to the other players in our playroom.
        writer.Write(team);
    }

    private void Update()
    {
        // If the value of our float has changed, sync it with the other players in our playroom.
        if (team != oldTeam)
        {
            // Store the updated value
            oldTeam = team;

            // Tell Alteruna Multiplayer that we want to commit our data.
            Commit();
        }

        // Update the Synchronizable
        base.SyncUpdate();
    }

    private void Start()
    {
        team = GetComponentInParent<Avatar>().Possessor.Index % 2;
        if (team == -1)
            render.color = new Color(255, 0, 255);
        else if (team == 0)
            render.color = Color.blue;
        else
            render.color = Color.red;
    }
}