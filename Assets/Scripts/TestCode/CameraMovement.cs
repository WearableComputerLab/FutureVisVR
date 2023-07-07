using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float movementSpeed = 3f;

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0f, vertical) * movementSpeed * Time.deltaTime;
        GameObject.Find("OVRCameraRig").transform.Translate(movement);
    }
}
