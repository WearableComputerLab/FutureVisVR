using UnityEngine;

public class GetFramerate : MonoBehaviour
{
    private float deltaTime = 0.0f;

    private void Update()
    {
        // Calculate the delta time between frames
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    private void OnGUI()
    {
        // Calculate the current framerate
        float fps = 1.0f / deltaTime;
        string fpsText = string.Format("{0:0.} FPS", fps);

        // Display the current framerate
        GUI.Label(new Rect(10, 10, 100, 20), fpsText);
    }
}
