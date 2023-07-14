using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SituationFutureInfo : MonoBehaviour
{
    public static void CalculateFutureVariance(string highlightedPlayer, string conditionName, string situation)
    {
        List<List<Vector2>> targetLineRenderersPositions = new List<List<Vector2>>();

        int countVisedFutureNumber = 0;

        int count = 0;

        for (int eachFutureNumber = 1; eachFutureNumber <= 10; eachFutureNumber++)  // start from RightPlayer0LineRenderer_1
        {
            LineRenderer lineRenderer = GameObject.Find(highlightedPlayer + "LineRenderer" + eachFutureNumber).GetComponent<LineRenderer>();
            if (lineRenderer.enabled == true)
            {
                countVisedFutureNumber++;
            }
            print("HAHACT1: " + count);
            count++;

            targetLineRenderersPositions.Add(new List<Vector2>());
            int pointCount = lineRenderer.positionCount;

            for (int j = 0; j < pointCount; j++)
            {
                targetLineRenderersPositions[eachFutureNumber - 1].Add(new Vector2(lineRenderer.GetPosition(j).x, lineRenderer.GetPosition(j).z));
            }

        }
        print("HAHACT2: " + countVisedFutureNumber);

        List<float> distance = new List<float>();
        for (int i = 0; i < countVisedFutureNumber; i++)
        {
            for (int j = i + 1; j < countVisedFutureNumber; j++)
            {
                for (int eachPositionIndex = 0; eachPositionIndex < targetLineRenderersPositions[0].Count; eachPositionIndex++)
                    distance.Add(Vector2.Distance(targetLineRenderersPositions[i][eachPositionIndex], targetLineRenderersPositions[j][eachPositionIndex]));
            }
        }

        float sum = 0f;
        foreach (float value in distance)
        {
            sum += value;
        }
        float mean = sum / distance.Count;

        float sumOfSquaredDifferences = 0f;
        foreach (float value in distance)
        {
            float difference = value - mean;
            sumOfSquaredDifferences += difference * difference;
        }
        float standardDeviation = Mathf.Sqrt(sumOfSquaredDifferences / distance.Count);

        string filePath = "C:/Users/chenk/Desktop/First_Project/User_Study/FutureDiversity/DatasetFutureDiversity.csv";
        bool fileExists = File.Exists(filePath);

        using (StreamWriter data = new StreamWriter(filePath, true))
        {
            if (!fileExists)
            {
                data.WriteLine($"{"Condition"}, {"Situation"}, {"StandardDeviation"}, {"PositionCells"}, {"HeatmapCells"}");
            }
            // data.WriteLine($"{currentDate}, {ParticiantID},{Condition}, {situation[3]},{CompletionTime}, {ObserverTargetDistance}, {SelectedpositionTargetDistance}, {EyeTrackingMiniatureTimer}, {ConvertListVector3ToCSVString(SelectedPositions)}, {ConvertListVector3ToCSVString(EyeTrackingPitchPositions)},  {ConvertListVector3ToCSVString(new List<Vector3> { observerObject.transform.position })}, {ConvertListVector3ToCSVString(new List<Vector3> { highlightedPlayerObject.transform.position })}");
            data.WriteLine($"{conditionName}, {situation}, {standardDeviation}, {countCellsFromGenerateHeatmap(countVisedFutureNumber, targetLineRenderersPositions)[0].ToString()}, {countCellsFromGenerateHeatmap(countVisedFutureNumber, targetLineRenderersPositions)[1].ToString()}");
        }
    }


    /*** SituationFutureInfo.cs Future Diversity***/

    public static List<int> countCellsFromGenerateHeatmap(int countVisedFutureNumber, List<List<Vector2>> playersPositions)
    {
        HashSet<Vector2Int> usedHeatmapCells = new HashSet<Vector2Int>();
        HashSet<Vector2Int> usedPositionCells = new HashSet<Vector2Int>();

        int radius = 6;
        int sigma = 3;
        // heatmap = new float[heatmapResolution, heatmapResolution];
        float[,] heatmap = new float[210, 136];

        Vector2 playingAreaSize = new Vector2(105f, 68f);
        float cellSizeX = playingAreaSize.x / 210;
        float cellSizeY = playingAreaSize.y / 136;

        // Compute the Gaussian kernel
        float[,] kernel = new float[2 * radius + 1, 2 * radius + 1];
        for (int i = 0; i < 2 * radius + 1; i++)
        {
            for (int j = 0; j < 2 * radius + 1; j++)
            {
                kernel[i, j] = Mathf.Exp(-((i - radius) * (i - radius) + (j - radius) * (j - radius)) / (2 * sigma * sigma));
            }
        }

        int count = 0;
        foreach (List<Vector2> positions in playersPositions)
        {
            foreach (Vector2 position in positions)
            {
                // Convert the position to heatmap indices
                int i = Mathf.FloorToInt((position.x + playingAreaSize.x / 2f) / cellSizeX);
                int j = Mathf.FloorToInt((position.y + playingAreaSize.y / 2f) / cellSizeY);
                if (count < countVisedFutureNumber)
                    usedPositionCells.Add(new Vector2Int(i, j));

                // Check if the indices are within the valid range of the heatmap
                if (i >= 0 && i < 210 && j >= 0 && j < 136)
                {
                    // Iterate over the kernel to add the influence to the heatmap
                    for (int ii = 0; ii < 2 * radius + 1; ii++)
                    {
                        for (int jj = 0; jj < 2 * radius + 1; jj++)
                        {
                            // Check if the heatmap indices are within the valid range
                            if (i + ii - radius >= 0 && i + ii - radius < 210 && j + jj - radius >= 0 && j + jj - radius < 136)
                            {
                                heatmap[i + ii - radius, j + jj - radius] += kernel[ii, jj];
                                usedHeatmapCells.Add(new Vector2Int(i + ii - radius, j + jj - radius));
                            }
                        }
                    }
                }
            }
            count++;
        }
        List<int> result = new List<int>();
        result.Add(usedPositionCells.Count);
        result.Add(usedHeatmapCells.Count);
        return result;
    }



    public static void SaveCameraView(string situation)
    {
        // Check if the capture camera is assigned
        Camera captureCamera = GameObject.Find("GlobalCamera").GetComponent<Camera>();
        string folderPath = "C:/Users/chenk/Desktop/First_Project/User_Study/Situation_Info/MiniatureView";
        if (captureCamera != null)
        {
            // Create a new RenderTexture with the same dimensions as the camera's viewport
            RenderTexture renderTexture = new RenderTexture(4096, 2160, 24);

            // Set the camera's target texture to the newly created RenderTexture
            captureCamera.targetTexture = renderTexture;

            // Render the camera view to the target texture
            captureCamera.Render();

            // Activate the target texture
            RenderTexture.active = renderTexture;

            // Create a new Texture2D and read the pixels from the RenderTexture
            Texture2D texture = new Texture2D(4096, 2160, TextureFormat.RGB24, false);
            texture.ReadPixels(new Rect(0, 0, 4096, 2160), 0, 0);
            texture.Apply();

            // Encode the texture as PNG data
            byte[] pngData = texture.EncodeToPNG();

            string savePath = folderPath + "/" + situation + ".png";
            // Save the PNG data to the specified file path
            File.WriteAllBytes(savePath, pngData);

            // Clean up resources
            Destroy(texture);
            RenderTexture.active = null;
            captureCamera.targetTexture = null;
            Destroy(renderTexture);

            Debug.Log("Camera view saved to: " + savePath);
        }
        else
        {
            Debug.LogError("Capture camera not assigned!");
        }
    }
}
