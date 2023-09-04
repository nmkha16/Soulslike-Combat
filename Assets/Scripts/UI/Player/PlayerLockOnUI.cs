using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLockOnUI : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    private Transform mainCameraTransform;
    private Vector3 dir;
    private void Awake(){
        if (canvas == null){
            canvas = this.transform.parent.gameObject;
        }

        mainCameraTransform = Camera.main.transform;
    }

    private void Start(){
        canvas.SetActive(false);
    }

    private void Update(){
        dir = mainCameraTransform.position - this.transform.position;
        this.transform.localRotation = Quaternion.LookRotation(dir);
    }

    public void ToggleLockOnIndicator(bool toggle){
        canvas.SetActive(toggle);
    }

    public void UpdateLockOnIndicator(Vector3 position){
        canvas.transform.position = position;
    }
}
