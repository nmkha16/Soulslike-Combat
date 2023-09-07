using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using AI.Maria;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(MariaBoss))]
public class MariaHitDetection : MonoBehaviour
{
    [SerializeField] private MariaBoss maria;
    [SerializeField] private float raycastDistance = 2f;
    [SerializeField] private LayerMask targetLayerMask;
    private Transform hitComponentTransform;
    private RaycastHit[] hit = new RaycastHit[5];

    private void Awake(){
        if (maria == null){
            maria = GetComponent<MariaBoss>();
        }
    }

    private void Start(){
        this.maria.OnActivateHitbox += ActivateHitbox;
        this.maria. OnEndHitbox += EndHitbox;
        this.enabled = false;
    }

    private void OnDestroy() {
        this.maria.OnActivateHitbox -= ActivateHitbox;
        this.maria. OnEndHitbox -= EndHitbox;
    }

    private void Update() {
        Debug.DrawRay(hitComponentTransform.position,hitComponentTransform.forward * raycastDistance,Color.Lerp(Color.red,Color.green,UnityEngine.Random.Range(0.2f,1f)),2f);
        int hits = Physics.RaycastNonAlloc(hitComponentTransform.position,hitComponentTransform.forward,hit, raycastDistance,targetLayerMask);

        if (hits != 0){
            for (int i =0 ; i < hits; i++){
                if (hit[i].transform.TryGetComponent<IDamagable>(out var damagable)){
                    damagable.TakeDamage(10);
                }
            }
        }
    }

    private void ActivateHitbox(Transform hitComponent){
        this.hitComponentTransform = hitComponent;
        this.enabled = true;
    }

    private void EndHitbox(){
        this.enabled = false;
    }
}

