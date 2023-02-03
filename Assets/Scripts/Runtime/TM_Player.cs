using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TM_Player : MonoBehaviour
{
    // ReSharper disable InconsistentNaming
    protected float mouseX = 0, mouseY = 0, rotateSpeed = 3;

    protected Quaternion initRotation = Quaternion.identity;

    protected Transform goTransform = null;

    void Start()
    {
        goTransform = gameObject.transform;
        initRotation = goTransform.rotation;
    }

    void Update()
    {
        UpdateRotation();
    }

    void UpdateRotation()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = -Input.GetAxis("Mouse Y");

        Vector3 _rot = new Vector3(mouseY * rotateSpeed, mouseX * rotateSpeed, 0);

        goTransform.Rotate(_rot);
    }
}