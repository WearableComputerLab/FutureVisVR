using UnityEngine;

public class ControllerRayLocation : MonoBehaviour
{
    public OVRInput.Controller controllerType;
    // public XRRayInteractor rayInteractor;
    // public GameObject cursor;

    public static Transform miniMapObject;
    public static OVRInput.Controller controller;
    public static LayerMask miniMapLayer;

    private void Start()
    {
        miniMapObject = GameObject.Find("MovableMiniature").transform;
    }
    private void Update()
    {
        // Check if the specified Oculus controller is connected
        // if (OVRInput.IsControllerConnected(controllerType))
        {

            // // Raycast from the controller to the mini-map object
            // Ray ray = new Ray(transform.position, transform.forward);
            // RaycastHit hit1;

            // if (Physics.Raycast(ray, out hit1, Mathf.Infinity, miniMapLayer))
            // {
            //     // Get the cursor location relative to the mini-map object
            //     Vector3 cursorPosition = miniMapObject.InverseTransformPoint(hit1.point);

            //     // Do something with the cursor position
            //     Debug.Log("Cursor Position: " + cursorPosition);
            // }


            // // Get the local position and rotation of the controller
            // Vector3 controllerPosition = OVRInput.GetLocalControllerPosition(controllerType);
            // Quaternion controllerRotation = OVRInput.GetLocalControllerRotation(controllerType);

            // // Transform the local position and rotation to world space
            // Vector3 rayLocation = transform.TransformPoint(controllerPosition);
            // Quaternion rayRotation = transform.rotation * controllerRotation;

            // // Use the rayLocation and rayRotation as needed
            // Debug.Log("Controller Ray Location: " + rayLocation);
            // Debug.Log("Controller Ray Rotation: " + rayRotation.eulerAngles);

            // print("Controller Cursor Location: " + transform.TransformPoint(GameObject.Find("RaycasterCursorVisual").transform.localPosition));
            // Get the local position and rotation of the controller

            // print("Cursor Object Location: " + GameObject.Find("OVRCameraRig").transform.TransformPoint(GameObject.Find("RaycasterCursorVisual").transform.position));


            Vector3 controllerPosition = OVRInput.GetLocalControllerPosition(controllerType);
            Quaternion controllerRotation = OVRInput.GetLocalControllerRotation(controllerType);

            // Transform the local position and rotation to world space
            //             Vector3 rayOrigin = transform.TransformPoint(controllerPosition);
            // Quaternion rayRotation = transform.rotation * controllerRotation;
            Vector3 rayOrigin = GameObject.Find("OVRCameraRig").transform.TransformPoint(controllerPosition);
            Quaternion rayRotation = GameObject.Find("OVRCameraRig").transform.rotation * controllerRotation;

            RaycastHit hit;
            // if (Physics.Raycast(controllerPosition, controllerRotation * Vector3.forward, out hit))
            // if (Physics.Raycast(rayOrigin, rayRotation * Vector3.forward, out hit))
            if (OVRInput.IsControllerConnected(controllerType))
            {
                // Check if the raycast hit the cube object

                // Get the cursor location on the cube surface
                // Vector3 cursorLocation = hit.transform.InverseTransformPoint(hit.point);
                // Vector3 cursorLocation = GameObject.Find("MovableMiniature").transform.InverseTransformPoint(hit.point);
                // Vector3 cursorLocation = hit.point;

                // Use the cursor location as needed
                // print("Game Object Hit: " + hit.collider.gameObject);
                // Debug.Log("Cursor Location: " + cursorLocation);
                Debug.Log("Cursor Location: " + GameObject.Find("MovableMiniature").transform.InverseTransformPoint(GameObject.Find("FootballEngine").transform.TransformPoint(GameObject.Find("RaycasterCursorVisual").transform.position)));


            }
        }
        // RaycastResult res;
        // if (rayInteractor.TryGetCurrentUIRaycastResult(out res))
        // {
        //     cursor.transform.position = res.worldPosition;
        // }
    }
}
