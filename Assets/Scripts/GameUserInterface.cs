using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUserInterface : MonoBehaviour
{

    public static void updateGUIView()
    {
        /*** Main Camera View  ***/
        Camera mainCamera = Camera.main;

        GameObject mainCameraObject = GameObject.Find("Main Camera");
        // GameObject mainCameraObject = GameObject.Find("MRTK-Quest_OVRCameraRig(Clone)");

        /*** Play/Pause Button ***/
        // // Get the camera's viewport position for the bottom left corner (0, 0)
        // Vector3 viewportPosition = new Vector3(0.5f, 0.75f, mainCamera.nearClipPlane + 0.2f);
        // Vector3 worldPosition = mainCamera.ViewportToWorldPoint(viewportPosition);
        // GameObject.Find("MovableButtons").transform.position = worldPosition;
        // // Button face to camera
        // Vector3 directionToCamera = mainCamera.transform.position - GameObject.Find("MovableButtons").transform.position;
        // GameObject.Find("MovableButtons").transform.rotation = Quaternion.LookRotation(-directionToCamera);
        // fixedGUIComponents("MovableButtons", mainCamera, 0.5f, 0.75f, 0.2f);
        // fixedGUIComponents("GlobalCameraProjectTo", mainCamera, 0.2f, 0.75f, 0.2f);

        Vector3 mainCameraPosition = new Vector3(mainCameraObject.transform.position.x, mainCameraObject.transform.position.x, mainCameraObject.transform.position.z + 0.2f);

        /*** Miniature ***/
        GameObject.Find("MovableMiniature").transform.position = new Vector3(mainCameraPosition.x, 0, mainCameraPosition.z + 0.2f);
        GameObject.Find("GlobalCameraProjectTo").transform.position = new Vector3(mainCameraPosition.x + 0.6f, 0, mainCameraPosition.z + 0.3f);

    }

    private static void fixedGUIComponents(string gameObject, Camera mainCamera, float x, float y, float z)
    {
        // Get the camera's viewport position for the bottom left corner (0, 0)
        Vector3 viewportPosition = new Vector3(x, y, mainCamera.nearClipPlane + z);
        Vector3 worldPosition = mainCamera.ViewportToWorldPoint(viewportPosition);
        GameObject.Find(gameObject).transform.position = worldPosition;
        // Button face to camera
        Vector3 directionToCamera = mainCamera.transform.position - GameObject.Find(gameObject).transform.position;
        GameObject.Find(gameObject).transform.rotation = Quaternion.LookRotation(-directionToCamera);
    }

    private int playPauseFlag = 0;  // %2=0: pause,  %2=1: play
    public void playPause()
    {
        playPauseFlag++;
        if (playPauseFlag % 2 == 0)
            MovableFootball.gamePlay = false;
        if (playPauseFlag % 2 == 1)
            MovableFootball.gamePlay = true;
    }

    private int arrowShowHideFlage = 0;  // %2=0: hide,  %2=1: show
    public void arrowShowHide()
    {
        arrowShowHideFlage++;
        if (arrowShowHideFlage % 2 == 0)
            MovableFootball.showFuture = false;
        if (arrowShowHideFlage % 2 == 1)
            MovableFootball.showFuture = true;
    }

    private int heatmapShowHideFlage = 0;  // %2=0: hide,  %2=1: show
    public void heatmapShowHide()
    {
        heatmapShowHideFlage++;
        if (heatmapShowHideFlage % 2 == 0)
            MovableFootball.showHeatmap = false;
        if (heatmapShowHideFlage % 2 == 1)
            MovableFootball.showHeatmap = true;
    }
}
