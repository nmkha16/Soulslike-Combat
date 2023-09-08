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
    //[SerializeField] private float sphereRadius = 2f;
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private Transform gizmoTransform;
    [SerializeField] private Vector3 size;
    private Transform hitComponentTransform;
    //private RaycastHit[] hit = new RaycastHit[5];
    private Collider[] hitColliders = new Collider[5];

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
        int hits = Physics.OverlapBoxNonAlloc(hitComponentTransform.position,size, hitColliders, Quaternion.identity,targetLayerMask);

        if (hits != 0){
            for (int i =0 ; i < hits; i++){
                if (hitColliders[i].TryGetComponent<IDamagable>(out var damagable)){
                    Debug.Log("hit " + hitColliders[i].name);
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

    void OnDrawGizmos(){
        Gizmos.color = Color.green;
        Gizmos.DrawCube(gizmoTransform.position,size);
    }
}

