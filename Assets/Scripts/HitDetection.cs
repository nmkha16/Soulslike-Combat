using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    [SerializeField] private Collider hitbox;

    private void Awake(){
        hitbox.enabled = false;
    }

    public void EnableHitbox(){
        hitbox.enabled = true;
    }

    public void DisableHitbox(){
        hitbox.enabled = false;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Enemy"){
            other.TryGetComponent<IDamagable>(out var iDamagable);
            if (iDamagable != null){
                iDamagable.TakeDamage(10);
            }
        }
    }
}
