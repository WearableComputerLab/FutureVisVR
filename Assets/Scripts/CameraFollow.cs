using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    float m_Height = 1.9f;

    float m_Distance = 0;

    float m_Speed = 100f;

    Vector3 m_TargetPosition;

    Transform follow;
    // Use this for initialization
    void Start()
    {

        follow = GameObject.Find("RightPlayer2").transform;
    }

    // Update is called once per frame

    void LateUpdate()
    {

        m_TargetPosition = follow.position + Vector3.up * m_Height - follow.forward * m_Distance;

        transform.position = Vector3.Lerp(transform.position, m_TargetPosition, m_Speed);

        transform.rotation = follow.rotation;



        //transform.LookAt(follow);


    }
}
