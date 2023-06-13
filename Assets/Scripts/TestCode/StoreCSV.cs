using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

public class StoreCSV : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<Vector3> SelectedPositions = new List<Vector3>();
        SelectedPositions.Add(new Vector3(1, 2, 3));
        SelectedPositions.Add(new Vector3(4, 5, 6));
        SelectedPositions.Add(new Vector3(7, 8, 9));

        storeAsCSV(999, "No_Time_Limit_2Arrows", 3.1415f, SelectedPositions, CalculateAverage(SelectedPositions));

        storeAsCSV(888, "Yes_Time_Limit_2Arrows", 19.95f, SelectedPositions, CalculateAverage(SelectedPositions));


    }

    private void storeAsCSV(int ParticiantID, string Situation, float CompletionTime, List<Vector3> SelectedPositions, Vector3 AveragePosition)
    {
        // Get the current date
        DateTime currentDate = DateTime.Now;

        // Access individual components of the date
        int year = currentDate.Year;
        int month = currentDate.Month;
        int day = currentDate.Day;

        string SelectedPositionsString = ConvertListToCSVString(SelectedPositions);

        string AveragePositionString = "[" + AveragePosition.x + ";" + AveragePosition.y + ";" + AveragePosition.z + "]";

        string filePath = "C:/Users/chenk/Desktop/First_Project/User_Study/Collected_Data_CSV/User_Study_Data.csv";
        using (StreamWriter data = new StreamWriter(filePath, true))
        {
            data.WriteLine($"{currentDate}, {ParticiantID},{Situation},{CompletionTime},{SelectedPositionsString}, {AveragePositionString}");
        }
        // Debug.Log("Participant ID: " + ParticiantID + " Situation: " + Situation + " CompletionTime: " + CompletionTime + " AveragePosition: " + AveragePosition);
    }
    private Vector3 CalculateAverage(List<Vector3> values)
    {
        Vector3 sum = Vector3.zero;
        int index = 0;
        int length = values.ToArray().Length;
        for (int i = length - 1; i > length / 3; --i)
        {
            index++;
            sum += values[i];
        }

        return sum / index;
    }

    // string ConvertListToCSVString(List<Vector3> list)
    // {
    //     string sb = "[";

    //     // Append each Vector3 as a line in the CSV string
    //     for (int i = 0; i < list.Count; i++)
    //     {
    //         Vector3 vector = list[i];
    //         sb = sb + "[" + vector.x + "," + vector.y + "," + vector.z + "]";
    //     }
    //     sb = sb + "]";

    //     return sb;
    // }
    string ConvertListToCSVString(List<Vector3> list)
    {
        StringBuilder sb = new StringBuilder();

        // Append the list as a single column in the CSV string
        sb.Append("\"[");
        for (int i = 0; i < list.Count; i++)
        {
            Vector3 vector = list[i];
            sb.Append("[" + vector.x + "," + vector.y + "," + vector.z + "]");
            if (i < list.Count - 1)
                sb.Append(", ");
        }
        sb.Append("]\"");

        return sb.ToString();
    }
}
