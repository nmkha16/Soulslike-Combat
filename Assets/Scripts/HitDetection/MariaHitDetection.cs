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
    [SerializeField] private Transform hitComponentTransform;
    private HitWeapon hitWeapon;
    //private RaycastHit[] hit = new RaycastHit[5];
    private Collider[] hitColliders = new Collider[5];

    private HashSet<Transform> hashSet = new HashSet<Transform>();
    private float elapsed = 0f;

    private void Awake(){
        if (maria == null){
            maria = GetComponent<MariaBoss>();
        }
    }

    private void Start(){
        this.maria.OnActivateHitbox += ActivateHitbox;
        this.maria.OnEndHitbox += EndHitbox;
        this.enabled = false;
    }

    private void OnEnable(){
        gizmoTransform = hitComponentTransform;
        elapsed = 0f;
    }

    private void OnDestroy() {
        this.maria.OnActivateHitbox -= ActivateHitbox;
        this.maria. OnEndHitbox -= EndHitbox;
    }

    private void Update() {
        elapsed += Time.deltaTime;
        if (elapsed > 1f){
            hashSet.Clear();
            elapsed = 0;
    }

        int hits = Physics.OverlapBoxNonAlloc(hitComponentTransform.position,size, hitColliders, Quaternion.identity, targetLayerMask);
        if (hits != 0){
            for (int i =0 ; i < hits; i++){
                if (!hashSet.Contains(hitColliders[i].transform)){
                    hashSet.Add(hitColliders[i].transform);
                    if (hitColliders[i].TryGetComponent<IDamagable>(out var damagable)){
                        damagable.TakeDamage(this.transform, hitWeapon, 10);
                    }
                }
            }
        }
    }

    private void ActivateHitbox(Transform hitComponent, HitWeapon hitWeapon){
        this.hitComponentTransform = hitComponent;
        this.hitWeapon = hitWeapon;
        this.enabled = true;
    }

    private void EndHitbox(){
        this.enabled = false;
        hashSet.Clear();
    }

    void OnDrawGizmos(){
        if (gizmoTransform == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawCube(gizmoTransform.position,size);
    }
}

