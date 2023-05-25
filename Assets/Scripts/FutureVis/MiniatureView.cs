using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniatureView : MonoBehaviour
{
    public static Camera cameraToProject;
    public static GameObject objectToProjectTo;
    private static RenderTexture renderTexture;
    private static List<GameObject> movableMiniatureObjects = new List<GameObject>();
    private static GameObject movableMiniature;
    private static Vector3 mainCameraPosition;
    private static GameObject mainCameraObject;
    private static bool temp_showMovableMiniature;

    // public static void startMiniatureView()
    // {
    //     objectToProjectTo = GameObject.Find("GlobalCameraProjectTo");
    //     cameraToProject = GameObject.Find("GlobalCamera").GetComponent<Camera>();

    //     Material material = objectToProjectTo.GetComponent<Renderer>().material;
    //     material.shader = Shader.Find("Unlit/Texture");

    //     renderTexture = new RenderTexture(Screen.width, Screen.height, 24);

    //     cameraToProject.targetTexture = renderTexture;
    //     objectToProjectTo.GetComponent<Renderer>().material.mainTexture = renderTexture;
    // }

    public static void startMovableMiniature()
    {
        movableMiniature = GameObject.Find("MovableMiniature");
        mainCameraObject = GameObject.Find("OVRCameraRig");
        mainCameraPosition = new Vector3(mainCameraObject.transform.position.x, mainCameraObject.transform.position.x, mainCameraObject.transform.position.z + 0.2f);
    }

    // public static void updateMiniatureView(bool show)
    // {
    //     if (show)
    //     {
    //         Vector3 mainCameraPosition = new Vector3(mainCameraObject.transform.position.x, mainCameraObject.transform.position.x, mainCameraObject.transform.position.z + 0.2f);
    //         GameObject.Find("GlobalCameraProjectTo").GetComponent<Renderer>().enabled = true;
    //     }
    //     else
    //     {
    //         GameObject.Find("GlobalCameraProjectTo").GetComponent<Renderer>().enabled = false;
    //     }
    // }

    public static void updateMovableMiniature(bool show)
    {
        if (show)
        {
            fixedMovableMiniature(movableMiniature, GameObject.Find("CenterEyeAnchor"), 0.5f, 0.5f, 0.5f);
            // movableMiniature.transform.position = new Vector3(0, 0, 0);
            // movableMiniature.transform.position = new Vector3(mainCameraPosition.x + 0.6f, 0, mainCameraPosition.z + 0.3f);
        }
        else
        {
            movableMiniature.transform.position = new Vector3(mainCameraPosition.x, -3, mainCameraPosition.z + 0.3f);
        }
    }

    private static void fixedMovableMiniature(GameObject gameObject, GameObject cameraObject, float x, float y, float z)
    {
        Camera mainCamera = Camera.main;
        Vector3 viewportPosition = new Vector3(x, y, mainCamera.nearClipPlane + z);
        Vector3 worldPosition = mainCamera.ViewportToWorldPoint(viewportPosition);
        gameObject.transform.position = new Vector3(worldPosition.x, cameraObject.transform.position.y - 0.2f, worldPosition.z);
        Vector3 directionToCamera = mainCamera.transform.position - gameObject.transform.position;
        // gameObject.transform.rotation = Quaternion.LookRotation(directionToCamera);
        gameObject.transform.rotation = Quaternion.Euler(0, Quaternion.LookRotation(directionToCamera).y, Quaternion.LookRotation(directionToCamera).z);
    }

    // public Camera miniMapCamera;
    // public float miniMapSize = 0.3f;
    // public float miniMapOffsetX = 0f;
    // public float miniMapOffsetY = 0f;

    // public void OnGUI()
    // // void Start()
    // {
    //     Camera cameraToProject = GameObject.Find("GlobalCamera").GetComponent<Camera>();
    //     GameObject objectToProjectTo = GameObject.Find("GlobalCameraProjectTo");

    //     RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);

    //     cameraToProject.targetTexture = renderTexture;
    //     objectToProjectTo.GetComponent<Renderer>().material.mainTexture = renderTexture;
    //     // Calculate the position and size of the mini-map GUI element
    //     float miniMapPosX = Screen.width * miniMapOffsetX;
    //     float miniMapPosY = Screen.height - (Screen.height * miniMapOffsetY) - (Screen.height * miniMapSize);
    //     float miniMapWidth = Screen.width * miniMapSize;
    //     float miniMapHeight = Screen.height * miniMapSize;

    //     // Set the GUI matrix for scaling and positioning the mini-map
    //     Matrix4x4 originalMatrix = GUI.matrix;
    //     GUI.matrix = Matrix4x4.TRS(new Vector3(miniMapPosX, miniMapPosY, 0f), Quaternion.identity, new Vector3(miniMapWidth / Screen.width, miniMapHeight / Screen.height, 1f));

    //     // Draw the mini-map texture based on the miniMapCamera's output
    //     GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), renderTexture, ScaleMode.ScaleToFit, false);

    //     // Reset the GUI matrix to restore the original GUI transformation
    //     GUI.matrix = originalMatrix;
    // }
}
