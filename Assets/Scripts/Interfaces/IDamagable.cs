using UnityEngine;

public interface IDamagable 
{
    int Health{get;set;}
    public void TakeDamage(Transform from, int amount);
}
