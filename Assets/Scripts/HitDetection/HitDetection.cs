using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    [SerializeField] private Transform handTransform;
    [SerializeField] private float raycastDistance = 2f;
    [SerializeField] private LayerMask targetLayerMask;
    private RaycastHit[] hit = new RaycastHit[5];
    private HitWeapon hitWeapon = HitWeapon.Sword;
    private HashSet<Transform> hashSet = new HashSet<Transform>();
    private float elapsed = 0f;

    private void Awake(){
        this.enabled = false;
    }

    private void Update() {
        elapsed += Time.deltaTime;
        if (elapsed > 0.75f){
            hashSet.Clear();
            elapsed = 0;
        }
    }

    private void OnDisable() {
        hashSet.Clear();
    }

    private void FixedUpdate() {
        Debug.DrawRay(handTransform.position,handTransform.forward * raycastDistance,Color.Lerp(Color.red,Color.green,UnityEngine.Random.Range(0.2f,1f)),2f);
        int hits = Physics.RaycastNonAlloc(handTransform.position,handTransform.forward,hit, raycastDistance,targetLayerMask);

        if (hits != 0){
            for (int i =0 ; i < hits; i++){
                if (!hashSet.Contains(hit[i].transform)){
                    hashSet.Add(hit[i].transform);
                    if (hit[i].transform.TryGetComponent<IDamagable>(out var damagable)){
                        damagable.TakeDamage(this.transform, hitWeapon, 10);
                    }
                }
            }
        }
    }
}
