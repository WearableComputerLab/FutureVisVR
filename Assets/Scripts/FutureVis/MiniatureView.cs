using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniatureView : MonoBehaviour
{
    public static Camera cameraToProject;
    public static GameObject objectToProjectTo;
    private static RenderTexture renderTexture;

    public static void startMiniatureView()
    {
        objectToProjectTo = GameObject.Find("GlobalCameraProjectTo");
        cameraToProject = GameObject.Find("GlobalCamera").GetComponent<Camera>();

        Material material = objectToProjectTo.GetComponent<Renderer>().material;
        material.shader = Shader.Find("Unlit/Texture");
        // material.SetFloat("_Mode", 3);
        // // material.DisableKeyword("_LIGHTING_ON");
        // // material.DisableKeyword("_METALLICGLOSSMAP");
        // // material.DisableKeyword("_EMISSION");
        // material.globalIlluminationFlags &= ~MaterialGlobalIlluminationFlags.EmissiveIsBlack;


        renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        // RenderTexture renderTexture = new RenderTexture(512, 512, 24);
        // renderTexture.Create();

        cameraToProject.targetTexture = renderTexture;
        objectToProjectTo.GetComponent<Renderer>().material.mainTexture = renderTexture;

    }



}
