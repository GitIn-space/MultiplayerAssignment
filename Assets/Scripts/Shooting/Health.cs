using UnityEngine;
using Alteruna;
using UnityEngine.Events;

public class Health : Synchronizable
{
    [SerializeField] int health = 3;
    int oldHealth;

    public UnityEvent<int> OnHealthChanged;

    public override void AssembleData(Writer writer, byte LOD = 100)
    {
        writer.Write(health);
    }

    public override void DisassembleData(Reader reader, byte LOD = 100)
    {
        health = reader.ReadInt();
        oldHealth = health;
        OnHealthChanged?.Invoke(health);
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
        OnHealthChanged?.Invoke(health);
    }
}