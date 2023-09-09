using UnityEngine;

public interface IDamagable 
{
    int Health{get;set;}
    public void TakeDamage(Transform from, HitWeapon weap, int amount);
}
