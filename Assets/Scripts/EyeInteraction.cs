
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeInteraction : MonoBehaviour
{
    [SerializeField]
    private float rayDistance = 5f;
    [SerializeField]
    // private GameObject hitObject;
    public static RaycastHit hit;

    public static List<Vector3> EyeTrackingPitchPositions = new List<Vector3>();

    // public static int EyeTrackingMiniatureTimes = 0;
    public static float EyeTrackingMiniatureTimer = 0;

    // private void Start()
    // {
    //     hitObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
    //     hitObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Material/Highlight");
    //     hitObject.GetComponent<Collider>().enabled = false;
    // }
    private void Update()
    {
        Vector3 rayCastDirection = transform.TransformDirection(Vector3.forward) * rayDistance;
        // Debug.DrawRay(transform.position, rayCastDirection * rayDistance, Color.red);

        if (Physics.Raycast(transform.position, rayCastDirection, out hit, Mathf.Infinity) && UserStudyInterface.startEyeTracking)
        {
            // hitObject.transform.position = hit.point;
            EyeTrackingPitchPositions.Add(hit.point);

            print("Hit game object: " + hit.collider.gameObject + " ;   Hit collider tag: " + hit.collider.tag + " ;   Hit Position: " + hit.point);

            // print("Check if the hit object is the line renderer: " + hit.collider.gameObject);

            if (hit.collider.tag == "PitchHeatmap")
            {
                // print("Hit Collider Tag: " + hit.collider.tag + " *** Hit Point: " + hit.point);
                EyeTrackingPitchPositions.Add(hit.point);
            }
            else if (hit.collider.tag == "Miniature")
            {
                // print("Hit Collider Tag: Miniature");
                EyeTrackingMiniatureTimer += Time.deltaTime;
                // EyeTrackingMiniatureTimes++;

            }
        }
        else if (!UserStudyInterface.startEyeTracking)
        {
            EyeTrackingPitchPositions = new List<Vector3>();

            EyeTrackingMiniatureTimer = 0;
            // EyeTrackingMiniatureTimes = 0;

        }
    }


}
