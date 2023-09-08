using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HitDetection : MonoBehaviour
{
    [SerializeField] private Transform handTransform;
    [SerializeField] private float raycastDistance = 2f;
    [SerializeField] private LayerMask targetLayerMask;
    private RaycastHit[] hit = new RaycastHit[5];
    private void Awake(){
        this.enabled = false;
    }

    private void FixedUpdate() {
        Debug.DrawRay(handTransform.position,handTransform.forward * raycastDistance,Color.Lerp(Color.red,Color.green,UnityEngine.Random.Range(0.2f,1f)),2f);
        int hits = Physics.RaycastNonAlloc(handTransform.position,handTransform.forward,hit, raycastDistance,targetLayerMask);

        if (hits != 0){
            for (int i =0 ; i < hits; i++){
                if (hit[i].transform.TryGetComponent<IDamagable>(out var damagable)){
                    damagable.TakeDamage(this.transform, 10);
                }
            }
        }
    }
}
