using UnityEngine;
using Alteruna;

public class HealthTest : Synchronizable
{
    public int health = 3;

    private int oldHealth;

    public override void AssembleData(Writer writer, byte LOD = 100)
    {
        writer.Write(health);
    }

    public override void DisassembleData(Reader reader, byte LOD = 100)
    {
        health = reader.ReadInt();
        oldHealth = health;
    }

    private void Update()
    {
        if (health != oldHealth)
        {
            oldHealth = health;
            Commit();
        }
        base.SyncUpdate();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}