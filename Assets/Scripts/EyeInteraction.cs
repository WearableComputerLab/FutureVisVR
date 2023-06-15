using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeInteraction : MonoBehaviour
{
    [SerializeField]
    private float rayDistance = 5f;
    [SerializeField]
    private GameObject hitObject;
    RaycastHit hit;

    private void Start()
    {
        hitObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        hitObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Material/Highlight");
        hitObject.GetComponent<Collider>().enabled = false;
    }
    private void FixedUpdate()
    {
        Vector3 rayCastDirection = transform.TransformDirection(Vector3.forward) * rayDistance;

        if (Physics.Raycast(transform.position, rayCastDirection, out hit, Mathf.Infinity))
        {
            hitObject.transform.position = hit.point;
            print("Hit game object: " + hit.collider.gameObject + " ;   Hit collider tag: " + hit.collider.tag + " ;   Hit Position: " + hit.point);
        }
    }
}
