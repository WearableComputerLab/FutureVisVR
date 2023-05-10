using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

public class realSoccerPos : MonoBehaviour
{
    static float ftime;
    // string result;

    public static Dictionary<string, List<int>> initialState;
    public void reqPos()
    {
        MovableFootball.getRealWorldPos = true;

    }
    public static void getRealSoccerPos()
    {
        string url = "http://192.168.0.124:5000/detectedPos";

        print(url);
        WebRequest wReq = WebRequest.Create(url);
        WebResponse wResp = wReq.GetResponse();
        System.IO.Stream respStream = wResp.GetResponseStream();

        using (System.IO.StreamReader Observation = new System.IO.StreamReader(respStream))
        {
            string Steps = "";
            while ((Steps = Observation.ReadLine()) != null)
            {
                print(Steps);
                initialState = JsonConvert.DeserializeObject<Dictionary<string, List<int>>>(Steps);
            }
            Observation.Close();
        }
    }

}
