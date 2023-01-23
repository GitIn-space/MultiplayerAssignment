using UnityEngine;
using Alteruna;
using UnityEngine.Events;

public class Health : Synchronizable
{
    [SerializeField] int health = 3;
    [SerializeField] SpriteRenderer render;

    public UnityEvent<int> OnHealthChanged;

    private void Start()
    {
        OnHealthChanged.AddListener(HealthChanged);
    }

    private void OnDisable()
    {
        OnHealthChanged.RemoveListener(HealthChanged);
    }

    public override void AssembleData(Writer writer, byte LOD = 100)
    {
        writer.Write(health);
    }

    public override void DisassembleData(Reader reader, byte LOD = 100)
    {
        health = reader.ReadInt();
        OnHealthChanged?.Invoke(health);
    }

    private void Update()
    {    
        base.SyncUpdate();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        OnHealthChanged?.Invoke(health);
        Commit();
    }

    void HealthChanged(int health)
    {
        if (health <= 0)
            GetComponentInParent<Player>().EnablePlayer(false);
    }
}