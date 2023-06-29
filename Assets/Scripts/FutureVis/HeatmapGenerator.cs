using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class HeatmapGenerator : MonoBehaviour
{
    public Gradient heatMapGradient;

    // public static Vector2 playingAreaSize = new Vector2(105f, 68f);
    public static int heatmapResolution; //100
    public static int radius; //1
    public static float sigma; //3f

    public static Gradient colorGradient = new Gradient();

    public static Color lowIntensityColor = Color.gray;  // Color.blue

    // public static Color lowIntensityColor = Color.blue;  // Color.blue
    // public static Color lowIntensityColor = new Color(255, 165, 0, 1); // Color Orange
    // public static Color lowIntensityColor = new Color(128, 0, 128, 1); // Color Purple
    public static Color highIntensityColor = Color.red;  // Color.red
    // public static Color highIntensityColor = new Color(255, 165, 0, 1);
    // public static Color concentratedColor = Color.red;
    // public static Color scatteredColor = Color.blue;
    public static float threshold = 0.5f;

    // public static float[,] heatmap = new float[heatmapResolution, heatmapResolution];
    public static float[,] heatmap;



    // public static void GenerateHeatmap(List<Vector2> positions)
    public static void GenerateHeatmap(List<List<Vector2>> playersPositions, int _radius, int _heatmapResolution, int _sigma, string heatmapType = "RealLife")
    {
        radius = _radius;
        heatmapResolution = _heatmapResolution;
        sigma = _sigma;
        heatmap = new float[heatmapResolution, heatmapResolution];


        Vector2 playingAreaSize;
        if (heatmapType.Equals("Miniature"))
            playingAreaSize = new Vector2(0.525f, 0.34f);
        else
            playingAreaSize = new Vector2(105f, 68f);
        // Compute the cell size and create an empty heatmap
        float cellSizeX = playingAreaSize.x / heatmapResolution;
        float cellSizeY = playingAreaSize.y / heatmapResolution;
        // float[,] heatmap = new float[heatmapResolution, heatmapResolution];

        // Compute the Gaussian kernel
        float[,] kernel = new float[2 * radius + 1, 2 * radius + 1];
        for (int i = 0; i < 2 * radius + 1; i++)
        {
            for (int j = 0; j < 2 * radius + 1; j++)
            {
                kernel[i, j] = Mathf.Exp(-((i - radius) * (i - radius) + (j - radius) * (j - radius)) / (2 * sigma * sigma));
            }
        }

        foreach (List<Vector2> positions in playersPositions)
        {
            foreach (Vector2 position in positions)
            {
                int i = Mathf.FloorToInt((position.x + playingAreaSize.x / 2f) / cellSizeX);
                int j = Mathf.FloorToInt((position.y + playingAreaSize.y / 2f) / cellSizeY);
                // print("Heapmap i: " + i + " ;  j: " + j);
                if (i >= 0 && i < heatmapResolution && j >= 0 && j < heatmapResolution)
                {
                    for (int ii = 0; ii < 2 * radius + 1; ii++)
                    {
                        for (int jj = 0; jj < 2 * radius + 1; jj++)
                        {
                            if (i + ii - radius >= 0 && i + ii - radius < heatmapResolution && j + jj - radius >= 0 && j + jj - radius < heatmapResolution)
                            {
                                // heatmap[j + jj - radius, i + ii - radius] += kernel[ii, jj];
                                heatmap[i + ii - radius, j + jj - radius] += kernel[ii, jj];
                            }
                        }
                    }
                }
            }
        }
    }

    public static void updateHeatmap(float saturationAdjustment, float valueAdjustment, float transparentAdjustment, Gradient gradient, bool showHeatmap)
    {
        // if (showHeatmap)
        // {        // Find the minimum and maximum intensity values in the heatmap
        float minIntensity = Mathf.Infinity;
        float maxIntensity = -Mathf.Infinity;
        for (int i = 0; i < heatmapResolution; i++)
        {
            for (int j = 0; j < heatmapResolution; j++)
            {
                float intensity = heatmap[i, j];
                if (intensity < minIntensity)
                {
                    minIntensity = intensity;
                }
                if (intensity > maxIntensity)
                {
                    maxIntensity = intensity;
                }
            }
        }

        // Display the heatmap
        Texture2D texture = new Texture2D(heatmapResolution, heatmapResolution);
        for (int i = 0; i < heatmapResolution; i++)
        {
            for (int j = 0; j < heatmapResolution; j++)
            {
                float intensity = heatmap[i, j];
                float normalizedIntensity = (intensity - minIntensity) / (maxIntensity - minIntensity); // Normalize intensity to [0, 1]

                //if (intensity > 0)
                {
                    Color color;
                    if (showHeatmap)

                        color = gradient.Evaluate(normalizedIntensity);// Color.Lerp(lowIntensityColor, highIntensityColor, normalizedIntensity);
                    else
                        color = gradient.Evaluate(0);
                    // Color.Lerp(lowIntensityColor, highIntensityColor, normalizedIntensity);
                    // print("Color: " + color);
                    //Color.RGBToHSV(color, out float h, out float s, out float v);
                    //s += saturationAdjustment;
                    //v += valueAdjustment;

                    //s = Mathf.Clamp01(s);
                    //v = Mathf.Clamp01(v);
                    //Color newColor = Color.HSVToRGB(h, s, v);
                    //newColor.a = transparentAdjustment;
                    var newColor = color;
                    texture.SetPixel(i, j, newColor);
                }
                //else
                {
                    //  Color color = new Color(normalizedIntensity, normalizedIntensity, normalizedIntensity, 1f);
                    //texture.SetPixel(i, j, color);
                }
            }
        }

        texture.Apply();

        GameObject heatmapObject = GameObject.Find("HeatmapPlane");

        Material originalSoccerFieldMaterial = Resources.Load<Material>("Material/soccerFieldHeatmap");

        Material heatmapMaterial = new Material(originalSoccerFieldMaterial);
        heatmapMaterial.SetTexture("_MainTex", texture);

        heatmapObject.GetComponent<Renderer>().material = heatmapMaterial;


        GameObject miniatureHeatmapObject = GameObject.Find("MiniatureHeatmapPlane");
        Material miniatureHeatmapMaterial = new Material(originalSoccerFieldMaterial);
        miniatureHeatmapMaterial.SetTexture("_MainTex", texture);

        miniatureHeatmapObject.GetComponent<Renderer>().material = heatmapMaterial;
        // }
        // else if (!showHeatmap)
        // {
        //     Texture2D texture = new Texture2D(heatmapResolution, heatmapResolution);
        //     for (int i = 0; i < heatmapResolution; i++)
        //     {
        //         for (int j = 0; j < heatmapResolution; j++)
        //         {
        //             float intensity = heatmap[i, j];
        //             // float normalizedIntensity = (intensity - minIntensity) / (maxIntensity - minIntensity); // Normalize intensity to [0, 1]

        //             //if (intensity > 0)
        //             {
        //                 Color color = gradient.Evaluate(0);// Color.Lerp(lowIntensityColor, highIntensityColor, normalizedIntensity);
        //                                                    // print("Color: " + color);
        //                                                    //Color.RGBToHSV(color, out float h, out float s, out float v);
        //                                                    //s += saturationAdjustment;
        //                                                    //v += valueAdjustment;

        //                 //s = Mathf.Clamp01(s);
        //                 //v = Mathf.Clamp01(v);
        //                 //Color newColor = Color.HSVToRGB(h, s, v);
        //                 //newColor.a = transparentAdjustment;
        //                 var newColor = color;
        //                 texture.SetPixel(i, j, newColor);
        //             }
        //             //else
        //             {
        //                 //  Color color = new Color(normalizedIntensity, normalizedIntensity, normalizedIntensity, 1f);
        //                 //texture.SetPixel(i, j, color);
        //             }
        //         }
        //     }
        //     texture.Apply();

        //     GameObject heatmapObject = GameObject.Find("HeatmapPlane");

        //     Material originalSoccerFieldMaterial = Resources.Load<Material>("Material/soccerFieldHeatmap");

        //     Material heatmapMaterial = new Material(originalSoccerFieldMaterial);
        //     heatmapMaterial.SetTexture("_MainTex", texture);

        //     heatmapObject.GetComponent<Renderer>().material = heatmapMaterial;


        //     GameObject miniatureHeatmapObject = GameObject.Find("MiniatureHeatmapPlane");
        //     Material miniatureHeatmapMaterial = new Material(originalSoccerFieldMaterial);
        //     miniatureHeatmapMaterial.SetTexture("_MainTex", texture);

        //     miniatureHeatmapObject.GetComponent<Renderer>().material = heatmapMaterial;

        //     // GameObject heatmapObject = GameObject.Find("HeatmapPlane");
        //     // GameObject miniatureHeatmapObject = GameObject.Find("MiniatureHeatmapPlane");

        //     // Material originalSoccerFieldMaterial = Resources.Load<Material>("Material/soccerFieldHeatmap");
        //     // Material heatmapMaterial = new Material(originalSoccerFieldMaterial);

        //     // heatmapObject.GetComponent<Renderer>().material = heatmapMaterial;
        //     // miniatureHeatmapObject.GetComponent<Renderer>().material = heatmapMaterial;
    }
}


