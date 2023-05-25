using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Range(0, 10)]
    public float CameraHeight = 2.5f;
    [Range(0, 10)]

    public float CameraDistance = 1f;
    [Range(0, 100)]
    public float FollowingSpeed = 1f;
    [Range(0, 20)]
    public float RotationSpeed = 1f;


    public static string AttachedGamePlayer = null;

    Vector3 Target_Position;

    Transform Camera_Position;
    // Use this for initialization
    void Start()
    {

        Camera_Position = GameObject.Find("OVRCameraRig").transform;
    }

    // Update is called once per frame

    void LateUpdate()
    {
        if (AttachedGamePlayer != null)
        {

            GameObject player = GameObject.Find(AttachedGamePlayer);

            Target_Position = player.transform.position + Vector3.up * CameraHeight - player.transform.forward * CameraDistance;

            Camera_Position.position = Vector3.Lerp(transform.position, Target_Position, FollowingSpeed);

            // Camera_Position.LookAt(Target_Position);
            Camera_Position.rotation = Quaternion.Lerp(transform.rotation, player.transform.rotation, RotationSpeed);
            // Camera_Position.rotation = player.transform.rotation;
        }
    }

    public static void resetPosition()
    {
        AttachedGamePlayer = null;
        GameObject.Find("OVRCameraRig").transform.position = new Vector3(0, 0, 0);
    }
}
