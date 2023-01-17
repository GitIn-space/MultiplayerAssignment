using UnityEngine;

public class HealthTest : MonoBehaviour
{
    public int health = 3;

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}