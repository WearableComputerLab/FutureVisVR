using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Net;

using Newtonsoft.Json;
// using Microsoft.MixedReality.Toolkit.UI;
// using Microsoft.MixedReality.Toolkit;
// using System.Text.Json;

public class MovableFootball : MonoBehaviour
{
    /*** URL or Load Game***/
    public enum GameResourceType { WebRequest, PregeneratedGame }
    [SerializeField]
    public GameResourceType ResourceType;
    private string selectedResourceType;

    public enum Scenarios
    {
        OriginalDuration2_FutureAmount5_FutureFar20_Interval3,
        OriginalDuration500_FutureAmount5_FutureFar20_Interval5,
        OriginalDuration1000_FutureAmount10_FutureFar20_Interval3,
        OriginalDuration202_FutureAmount20_FutureFar100_Interval1
    }
    [SerializeField]
    public Scenarios ScenariosConfiguration;
    private string selectedScenarioConfiguration;

    ///*** Right Team ***/
    public static string RightPlayer = "mRightPlayer";

    ///*** Left Team ***/
    public static string LeftPlayer = "mLeftPlayer";

    /*** Ball Location ***/
    public List<List<float>> Ball = new List<List<float>>();

    /*** Controlled Team Player Events***/
    public List<List<List<float>>> Player_Event = new List<List<List<float>>>();

    /*** Time Control ***/
    float time;
    public float TimeDelay = 0.1f;
    float updateHeatmapTime = 0;

    /*** TimeStamp Control ***/
    public static bool timeControl = false;

    /*** Duration ***/
    public static int gameDuration;

    /*** Step Number ***/
    // [Range(0, 500)]
    public static int StepNum = 0;

    /*** Each Team Players Number ***/
    public int EachPlayerNumber = 6;
    // public Dictionary<string, List<List<List<float>>>> New_Team = new Dictionary<string, List<List<List<float>>>>();
    public List<Dictionary<string, List<List<List<float>>>>> New_Team = new List<Dictionary<string, List<List<List<float>>>>>();

    /*** Game Pause ***/
    public static bool gamePlay = false; // Pause: false; Play: true

    /*** Game Reset ***/
    public static bool gameReset = false;

    /*** Show Future ***/
    public static bool showFuture = false;
    public static bool onlick = false;

    /*** Far Future ***/
    public int FarFuture = 5;
    public static int multiFutureFar;

    [Range(0.1f, 1)]
    public float FutureDetails = 0.5f;
    public int futureAmount = 2;
    public static int multiFutureAmount;

    public int stepToCalculate = 3;

    /*** Get Real World Positions ***/
    public static bool getRealWorldPos = false;

    /*** Pitch Size ***/
    public static int scaleSize = 200;

    /*** Heatmap ***/
    public static bool showHeatmap = false;
    public Gradient heatmapGradient;
    [Range(-1, 1)]
    public float Saturation = 0;
    [Range(-1, 1)]
    public float Value = 0;
    [Range(1, 0)]
    public float Transparent = 0;
    List<List<Vector2>> playersPositions = new List<List<Vector2>>();
    List<List<Vector2>> mplayersPositions = new List<List<Vector2>>();
    [Range(1, 30)]
    public int Radius = 1;
    [Range(10, 1000)]
    public int Resolution = 100;
    [Range(1, 30)]
    public int Sigma = 3;

    public int fixedGameDuration = 300;

    /*** Copy Event Player ***/
    public static GameObject copyEventPlayer;

    /*** New New_Team 13/04/2023 ***/
    Dictionary<string, List<Dictionary<string, Dictionary<string, List<List<float>>>>>> originalStructuredGame = new Dictionary<string, List<Dictionary<string, Dictionary<string, List<List<float>>>>>>();
    Dictionary<string, List<List<List<float>>>> originalGame = new Dictionary<string, List<List<List<float>>>>();
    Dictionary<string, List<Dictionary<string, List<List<List<float>>>>>> stepMultiFuture = new Dictionary<string, List<Dictionary<string, List<List<List<float>>>>>>();
    /***
    stepMultiFuture Structure:
    {
    "Step0": [new_team1, new_team2],
    "Step1": [new_team1, new_team2],
    }
    new_team: {'RightTeamLocations':[
                [[ x1, y1], [ x2, y2], [ x3, y3]],  //Player1 [step0, step1, step3]
                [[ x1, y1], [ x2, y2], [ x3, y3]],  //Player2 [step0, step1, step3]
                [[ x1, y1], [ x2, y2], [ x3, y3]]   //Player3 [step0, step1, step3]],
            'LeftTeamLocations':[
                [[ x1, y1], [ x2, y2], [ x3, y3]],  //Player1 [step0, step1, step3]
                [[ x1, y1], [ x2, y2], [ x3, y3]],  //Player2 [step0, step1, step3]
                [[ x1, y1], [ x2, y2], [ x3, y3]]   //Player3 [step0, step1, step3]],
            'BallLocations': [ [[x1, y1], [x2, y2], [x3, y3]] ]  // need to use in this form: New_Team["BallLocations"][0]
            'Event': [
			    [[Lid 1, a1], [Rid 1, a1]],
            	[[Lid 2, a2], [Rid 2, A2]]]}
    ***/

    /*** Movable Miniature***/
    public static bool showMiniatureView;
    public static bool showMovableMiniature;


    // Start is called before the first frame update
    void Start()
    {
        // mainCameraObject = GameObject.Find("Main Camera");
        time = 0f;
        gameDuration = 0;
        // TimeDelay = 0.1f; // * Speed Control*

        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        switch (ScenariosConfiguration)
        {
            case Scenarios.OriginalDuration2_FutureAmount5_FutureFar20_Interval3:
                selectedScenarioConfiguration = "OriginalDuration2_FutureAmount5_FutureFar20_Interval3";
                break;
            case Scenarios.OriginalDuration500_FutureAmount5_FutureFar20_Interval5:
                selectedScenarioConfiguration = "OriginalDuration500_FutureAmount5_FutureFar20_Interval5";
                break;
            case Scenarios.OriginalDuration1000_FutureAmount10_FutureFar20_Interval3:
                selectedScenarioConfiguration = "OriginalDuration1000_FutureAmount10_FutureFar20_Interval3";
                break;
            case Scenarios.OriginalDuration202_FutureAmount20_FutureFar100_Interval1:
                selectedScenarioConfiguration = "OriginalDuration202_FutureAmount20_FutureFar100_Interval1";
                break;
        }

        // Find the index of the underscore (_) separating the two substrings
        int underscoreIndex = selectedScenarioConfiguration.IndexOf("_");
        if (underscoreIndex != -1)
        {
            fixedGameDuration = int.Parse(selectedScenarioConfiguration.Substring(0, underscoreIndex).Replace("OriginalDuration", ""));
            string FutureAmount_FutureFar_Interval = selectedScenarioConfiguration.Remove(0, underscoreIndex + 1);
            underscoreIndex = FutureAmount_FutureFar_Interval.IndexOf("_");
            if (underscoreIndex != -1)
            {
                futureAmount = int.Parse(FutureAmount_FutureFar_Interval.Substring(0, underscoreIndex).Replace("FutureAmount", ""));
                string FutureFar_Interval = FutureAmount_FutureFar_Interval.Remove(0, underscoreIndex + 1);
                underscoreIndex = FutureFar_Interval.IndexOf("_");
                if (underscoreIndex != -1)
                    FarFuture = int.Parse(FutureFar_Interval.Substring(0, underscoreIndex).Replace("FutureFar", ""));
            }
        }
        print("Selected Fixed Game Duration: " + fixedGameDuration);
        print("Selected Future Amount: " + futureAmount);
        print("Select Far Future: " + FarFuture);

        switch (ResourceType)
        {
            case GameResourceType.WebRequest:
                selectedResourceType = "WebRequest";
                break;

            case GameResourceType.PregeneratedGame:
                selectedResourceType = "PregeneratedGame";
                break;
        }
        if (selectedResourceType == "WebRequest")
        {
            print("Sending Web Request To Python...");
            string url = generateURL(urlType.getUnityPos.ToString(), true, fixedGameDuration, EachPlayerNumber);
            originalGame = PyFootball(EachPlayerNumber, true, url)["OriginalGame"][0];
            stepMultiFuture = calculateFuture(stepToCalculate, FarFuture, originalStructuredGame);
        }
        else if (selectedResourceType == "PregeneratedGame")
        {
            print("Loading Json Files From Folders...");
            print(selectedScenarioConfiguration);
            originalGame = loadPregenerateJson(selectedScenarioConfiguration, "OriginalGame")["OriginalGame"][0];
            stepMultiFuture = loadPregenerateJson(selectedScenarioConfiguration, "Step");
        }

        sw.Stop();
        print("Cost time: " + sw.ElapsedMilliseconds);

        MultiFuture.startFutureInfo(stepMultiFuture, FarFuture);
        // MiniatureView.startMiniatureView();
        MiniatureView.startMovableMiniature();
    }

    // Update is called once per frame
    void Update()
    {
        multiFutureFar = FarFuture;
        multiFutureAmount = futureAmount;
        MultiFuture.updateFutureFar(StepNum, FarFuture, FutureDetails);
        MultiFuture.updateFutureAmount(showFuture);
        /*** Movable Miniature***/
        // GameObject.Find("MovableMiniature").transform.position = new Vector3(mainCameraObject.transform.position.x, 0, mainCameraObject.transform.position.z + 0.2f);
        // GameUserInterface.updateGUIView();
        // MiniatureView.updateMiniatureView(showMiniatureView);
        MiniatureView.updateMovableMiniature(showMovableMiniature);
        /*** Movable Buttons***/


        // updateHeatmapTime = updateHeatmapTime + 1f * Time.deltaTime;
        // if (updateHeatmapTime > 0.2)
        // {
        //     // updateHeatmapTime = 0;
        //     // HeatmapGenerator.GenerateHeatmap(playersPositions, Radius, Resolution, Sigma);
        //     // HeatmapGenerator.GenerateHeatmap(mplayersPositions, Radius, Resolution, Sigma, "Miniature");
        //     // HeatmapGenerator.updateHeatmap(Saturation, Value, Transparent, heatmapGradient);
        // }


        // ****** Big Ball ******
        // ****** Ball Locations ******
        GameObject BallSphere = GameObject.Find("mBall");
        // float bx = Ball[StepNum][0];
        // float bz = Ball[StepNum][1];
        // float by = Ball[StepNum][2];
        // BallSphere.transform.position = new Vector3(scale_x(bx) / ScaleSize, by - 0.194f, (scale_z(bz) / ScaleSize));
        GameObject bigBall = GameObject.Find("BigBall");
        bigBall.transform.position = new Vector3(BallSphere.transform.position.x * scaleSize, -1.7f, BallSphere.transform.position.z * scaleSize);

        if (gamePlay)
        {
            time = time + 1f * Time.deltaTime;
            if (time >= TimeDelay && StepNum < fixedGameDuration)
            {
                time = 0f;

                player(originalGame, StepNum);
                MultiFuture.updateFutureInfo(StepNum, FarFuture, showFuture);
                realTimeHeatmap(stepMultiFuture, StepNum, showHeatmap);

                StepNum += 1;
            }
        }

        if (!gamePlay)
        {
            if (StepNum < fixedGameDuration)
            {
                // MultiFuture.updateFutureInfo(StepNum, FarFuture, showFuture);
                // player(originalGame, StepNum);
                player(originalGame, StepNum);
                MultiFuture.updateFutureInfo(StepNum, FarFuture, showFuture);
                realTimeHeatmap(stepMultiFuture, StepNum, showHeatmap);
            }
        }
    }

    public void realTimeHeatmap(Dictionary<string, List<Dictionary<string, List<List<List<float>>>>>> stepMultiFuture, int currentStep, bool showHeatmap)
    {
        foreach (var stepFuturesPair in stepMultiFuture)
        {
            if (stepFuturesPair.Key.Replace("Step", String.Empty).Equals(currentStep.ToString()))
            {
                List<List<Vector2>> playersPositions = new List<List<Vector2>>();
                List<List<Vector2>> mplayersPositions = new List<List<Vector2>>();
                for (int playerId = 0; playerId < EachPlayerNumber; playerId++)
                {
                    List<Vector2> rightPlayer = new List<Vector2>();
                    List<Vector2> leftPlayer = new List<Vector2>();
                    for (int i = 0; i < futureAmount; i++)
                    {

                        foreach (List<float> position in stepMultiFuture[stepFuturesPair.Key][i]["RightTeamLocations"][playerId])
                        {
                            // print("Player1 Positions: (" + position[0] + ", " + position[1] + ")");
                            rightPlayer.Add(new Vector2(scale_x(-position[0]), scale_z(-position[1])));
                        }
                        foreach (List<float> position in stepMultiFuture[stepFuturesPair.Key][i]["LeftTeamLocations"][playerId])
                        {
                            // print("Player1 Positions: (" + position[0] + ", " + position[1] + ")");
                            leftPlayer.Add(new Vector2(scale_x(-position[0]), scale_z(-position[1])));
                        }
                    }
                    playersPositions.Add(rightPlayer);
                    playersPositions.Add(leftPlayer);

                    List<Vector2> mRightPlayer = new List<Vector2>();
                    List<Vector2> mLeftPlayer = new List<Vector2>();
                    for (int i = 0; i < futureAmount; i++)
                    {

                        foreach (List<float> position in stepMultiFuture[stepFuturesPair.Key][i]["RightTeamLocations"][playerId])
                        {
                            // print("Player1 Positions: (" + position[0] + ", " + position[1] + ")");
                            mRightPlayer.Add(new Vector2(scale_x(-position[0]) / scaleSize, scale_z(-position[1]) / scaleSize));
                        }
                        foreach (List<float> position in stepMultiFuture[stepFuturesPair.Key][i]["LeftTeamLocations"][playerId])
                        {
                            // print("Player1 Positions: (" + position[0] + ", " + position[1] + ")");
                            mLeftPlayer.Add(new Vector2(scale_x(-position[0]) / scaleSize, scale_z(-position[1]) / scaleSize));
                        }
                    }
                    mplayersPositions.Add(mRightPlayer);
                    mplayersPositions.Add(mLeftPlayer);
                }
                HeatmapGenerator.GenerateHeatmap(playersPositions, Radius, Resolution, Sigma);
                HeatmapGenerator.GenerateHeatmap(mplayersPositions, Radius, Resolution, Sigma, "Miniature");
                HeatmapGenerator.updateHeatmap(Saturation, Value, Transparent, heatmapGradient, showHeatmap);
            }
        }
    }

    public Dictionary<string, List<List<List<float>>>> passDatatoStrcuture(int EachPlayerNumber, Dictionary<string, Dictionary<string, List<List<float>>>> steps)
    {
        Dictionary<string, List<List<List<float>>>> New_Team = Create_Team(EachPlayerNumber);

        foreach (var eachstep in steps)
        {
            for (int i = 0; i < EachPlayerNumber; i++)
            {
                List<List<float>> right_team_locations = eachstep.Value["right_team"];
                right_team_locations[i][1] = -right_team_locations[i][1];
                New_Team["RightTeamLocations"][i].Add(right_team_locations[i]);

                List<List<float>> right_team_directions = eachstep.Value["right_team_direction"];
                New_Team["RightTeamDirections"][i].Add(right_team_directions[i]);

                List<List<float>> left_team_locations = eachstep.Value["left_team"];
                left_team_locations[i][1] = -left_team_locations[i][1];
                New_Team["LeftTeamLocations"][i].Add(left_team_locations[i]);

                List<List<float>> left_team_directions = eachstep.Value["left_team_direction"];
                New_Team["LeftTeamDirections"][i].Add(left_team_directions[i]);
            }

            //print("+++++++ Ball Locations ++++++");
            List<List<float>> ball_locations = eachstep.Value["ball"];
            ball_locations[0][1] = -ball_locations[0][1];
            // Ball.Add(ball_locations[0]);
            New_Team["BallLocations"][0].Add(ball_locations[0]);

            /*
            "event" for controlled player
            */
            List<List<float>> playerEvent = eachstep.Value["event"];
            // Player_Event.Add(playerEvent);
            New_Team["Event"].Add(playerEvent);
        }
        return New_Team;
    }

    public Dictionary<string, List<Dictionary<string, List<List<List<float>>>>>> calculateFuture(int stepToCalculate, int farFuture, Dictionary<string, List<Dictionary<string, Dictionary<string, List<List<float>>>>>> originalStructuredGame)
    {
        /***
        Structure:
        {
            "Step0": {
                [future 1],
                [future 2]
            },
            "Step5": {
                [future 1],
                [future 2]
            }
        }
        ***/
        Dictionary<string, List<Dictionary<string, List<List<List<float>>>>>> stepMultiFuture = new Dictionary<string, List<Dictionary<string, List<List<List<float>>>>>>();

        foreach (var infoPair in originalStructuredGame["OriginalGame"][0])
        {
            if (Int16.Parse(infoPair.Key) % stepToCalculate == 0)
            {
                Dictionary<string, List<List<float>>> teamLocations = new Dictionary<string, List<List<float>>>();
                foreach (var stepSituationPair in infoPair.Value)
                {
                    if (stepSituationPair.Key == "right_team")
                    {
                        teamLocations[stepSituationPair.Key] = stepSituationPair.Value;
                        teamLocations[stepSituationPair.Key].RemoveAt(0);
                    }

                    else if (stepSituationPair.Key == "left_team")
                    {
                        teamLocations[stepSituationPair.Key] = stepSituationPair.Value;
                        teamLocations[stepSituationPair.Key].RemoveAt(0);
                    }
                    else if (stepSituationPair.Key == "ball")
                        teamLocations[stepSituationPair.Key] = stepSituationPair.Value;
                }

                string url = generateURL(urlType.getOriginalGamePos.ToString(), false, farFuture, infoPair.Key, teamLocations, futureAmount);
                // print("Calculate Future URL: " + url);
                var pyfootball = PyFootball(EachPlayerNumber, false, url);

                foreach (var stepGamePair in pyfootball)
                    stepMultiFuture.Add(stepGamePair.Key, stepGamePair.Value);
            }
        }
        return stepMultiFuture;
    }

    protected enum urlType { getRealWorldPos, getUnityPos, getOriginalGamePos }
    public string generateURL(string urlType, bool ifOriginalGame, int gameDuration, int EachPlayerNumber) /***urlType: getRealWorldPos; getUnityPos; getOriginalGamePos***/
    {
        // string url = string.Format("http://10.233.53.149:5000/get?gameDuration=" + gameDuration.ToString() + "&futureAmount=0" + "&originalGame=" + (ifOriginalGame ? ("0") : ("1")));
        string url = string.Format("http://192.168.0.124:5000/get?gameDuration=" + gameDuration.ToString() + "&futureAmount=0" + "&originalGame=" + (ifOriginalGame ? ("0") : ("1")));

        if (urlType.Equals("getRealWorldPos"))
        {
            realSoccerPos.getRealSoccerPos();

            float realBallX = ((float)realSoccerPos.initialState["mBall"][0] - 52) / scaleSize;
            float realBallZ = (32 - (float)realSoccerPos.initialState["mBall"][1]) / scaleSize;
            print("mBall X: " + realBallX);
            print("mBall Z: " + realBallZ);
            url = url + "&bp=" + (scaleDown_x(realBallX * scaleSize)).ToString();
            url = url + "&bp=" + (-scaleDown_z(realBallZ * scaleSize)).ToString();

            // 2022-12-02 AM
            for (int i = 1; i < EachPlayerNumber; i++)
            {
                if (realSoccerPos.initialState.ContainsKey(RightPlayer + i.ToString()))
                {
                    float realRX = ((float)realSoccerPos.initialState[RightPlayer + i.ToString()][0] - 52) / scaleSize;
                    float realRZ = (32 - (float)realSoccerPos.initialState[RightPlayer + i.ToString()][1]) / scaleSize;
                    url = url + "&rp=" + (-scaleDown_x(realRX * scaleSize)).ToString();
                    url = url + "&rp=" + (scaleDown_z((realRZ) * scaleSize)).ToString();
                    GameObject mR = GameObject.Find(RightPlayer + i.ToString()); //For Testing
                    mR.transform.position = new Vector3(realRX, -0.2f, realRZ);
                }
                else
                {
                    Vector3 RightP = GameObject.Find(RightPlayer + i.ToString()).transform.position;
                    url = url + "&lp=" + (-scaleDown_x(RightP.x * scaleSize)).ToString();
                    url = url + "&lp=" + (scaleDown_z((RightP.z) * scaleSize)).ToString();
                }
            }
            for (int i = 1; i < EachPlayerNumber; i++)
            {
                if (realSoccerPos.initialState.ContainsKey(LeftPlayer + i.ToString()))
                {
                    float realLX = ((float)realSoccerPos.initialState[LeftPlayer + i.ToString()][0] - 52) / scaleSize;
                    float realLZ = (32 - (float)realSoccerPos.initialState[LeftPlayer + i.ToString()][1]) / scaleSize;
                    url = url + "&rp=" + (scaleDown_x(realLX * scaleSize)).ToString();
                    url = url + "&rp=" + (-scaleDown_z((realLZ) * scaleSize)).ToString();
                    GameObject mL = GameObject.Find(LeftPlayer + i.ToString()); //For Testing
                    mL.transform.position = new Vector3(realLX, -0.2f, realLZ);
                }
                else
                {
                    Vector3 LeftP = GameObject.Find(LeftPlayer + i.ToString()).transform.position;
                    url = url + "&lp=" + (scaleDown_x(LeftP.x * scaleSize)).ToString();
                    url = url + "&lp=" + (-scaleDown_z((LeftP.z) * scaleSize)).ToString();
                }
            }
            getRealWorldPos = false;
        }
        else if (urlType.Equals("getUnityPos"))
        {
            Vector3 ballPos = GameObject.Find("mBall").transform.position;
            url = url + "&bp=" + (scaleDown_x(ballPos.x * scaleSize)).ToString();
            url = url + "&bp=" + (-scaleDown_z(ballPos.z * scaleSize)).ToString();

            for (int i = 1; i < EachPlayerNumber; i++)
            {
                Vector3 RightP = GameObject.Find(RightPlayer + i.ToString()).transform.position;

                url = url + "&rp=" + (-scaleDown_x(RightP.x * scaleSize)).ToString();
                url = url + "&rp=" + (scaleDown_z((RightP.z) * scaleSize)).ToString();
            }
            for (int i = 1; i < EachPlayerNumber; i++)
            {
                Vector3 LeftP = GameObject.Find(LeftPlayer + i.ToString()).transform.position;

                url = url + "&lp=" + scaleDown_x(LeftP.x * scaleSize).ToString();
                url = url + "&lp=" + (-scaleDown_z((LeftP.z) * scaleSize)).ToString();
            }
        }
        return url;
    }

    public string generateURL(string urlType, bool ifOriginalGame, int gameDuration, string step, Dictionary<string, List<List<float>>> teamLocations, int futureAmount)
    {
        // string url = string.Format("http://10.233.53.149:5000/get?gameDuration=" + gameDuration.ToString() + "&futureAmount=" + futureAmount + "&originalGame=" + (ifOriginalGame ? ("0") : ("1")) + "&step=" + step);

        string url = string.Format("http://192.168.0.124:5000/get?gameDuration=" + gameDuration.ToString() + "&futureAmount=" + futureAmount + "&originalGame=" + (ifOriginalGame ? ("0") : ("1")) + "&step=" + step);
        foreach (var teamLocationPair in teamLocations)
        {
            if (teamLocationPair.Key.Equals("right_team"))
            {
                foreach (var location in teamLocationPair.Value)
                {
                    url = url + "&rp=" + -location[0];
                    url = url + "&rp=" + location[1];
                }
            }
            else if (teamLocationPair.Key.Equals("left_team"))
            {
                foreach (var location in teamLocationPair.Value)
                {
                    url = url + "&lp=" + location[0];
                    url = url + "&lp=" + -location[1];
                }
            }
            if (teamLocationPair.Key.Equals("ball"))
            {
                url = url + "&bp=" + teamLocationPair.Value[0][0];
                url = url + "&bp=" + teamLocationPair.Value[0][1];
            }
        }
        return url;
    }

    // public static List<string> parseConfigFromFolderName()
    // {
    //     string folderPath = "D:/tmp/Data/";
    //     string[] scenariosJsonFiles = Directory.GetDirectories(folderPath, "OriginalDuration*");

    //     List<string> config = new List<string>();
    //     foreach (var configPath in scenariosJsonFiles)
    //     {
    //         config.Add(Path.GetFileNameWithoutExtension(configPath));
    //     }

    //     return config;
    // }

    public Dictionary<string, List<Dictionary<string, List<List<List<float>>>>>> loadPregenerateJson(string config, string gameType)
    {
        string folderPath = "D:/tmp/Data/";
        string[] scenariosFolder = Directory.GetDirectories(folderPath, "OriginalDuration*");

        Dictionary<string, List<Dictionary<string, List<List<List<float>>>>>> stepMultiFuture = new Dictionary<string, List<Dictionary<string, List<List<List<float>>>>>>();

        string[] structuredGameJsonPaths = Directory.GetFiles(folderPath + config, "*.json");

        if (gameType.Contains("OriginalGame"))
        {
            foreach (var eachJsonFilePath in structuredGameJsonPaths)
            {
                if (eachJsonFilePath.Contains("OriginalGame"))
                {
                    string stepsJson = File.ReadAllText(eachJsonFilePath);
                    return parseGFJson(stepsJson);
                }
            }
        }
        else if (gameType.Contains("Step"))
        {
            foreach (var eachJsonFilePath in structuredGameJsonPaths)
            {
                if (eachJsonFilePath.Contains("Step"))
                {
                    string stepsJson = File.ReadAllText(eachJsonFilePath);
                    string eachJsonFileName = Path.GetFileNameWithoutExtension(eachJsonFilePath);
                    stepMultiFuture.Add(eachJsonFileName, parseGFJson(stepsJson)[eachJsonFileName]);
                }
            }
        }
        return stepMultiFuture;
    }


    public Dictionary<string, List<Dictionary<string, List<List<List<float>>>>>> parseGFJson(string Json)
    {
        Dictionary<string, List<Dictionary<string, List<List<List<float>>>>>> structuredGame = new Dictionary<string, List<Dictionary<string, List<List<List<float>>>>>>();
        var originalGameMultiFuture = JsonConvert.DeserializeObject<Dictionary<string, List<Dictionary<string, Dictionary<string, List<List<float>>>>>>>(Json);

        //TO-DO
        foreach (var infoPair in originalGameMultiFuture)
        {
            if (infoPair.Key.Contains("OriginalGame"))
            {
                Dictionary<string, List<List<List<float>>>> gameSituation = new Dictionary<string, List<List<List<float>>>>();
                gameSituation = passDatatoStrcuture(EachPlayerNumber, infoPair.Value[0]);

                List<Dictionary<string, List<List<List<float>>>>> temp_gameSituation = new List<Dictionary<string, List<List<List<float>>>>>();
                temp_gameSituation.Add(gameSituation);
                structuredGame["OriginalGame"] = new List<Dictionary<string, List<List<List<float>>>>>(temp_gameSituation);

            }
            else if (infoPair.Key.Contains("Step"))
            {
                List<Dictionary<string, List<List<List<float>>>>> stepFuturePair = new List<Dictionary<string, List<List<List<float>>>>>();


                for (int futureNo = 0; futureNo < infoPair.Value.Count; futureNo++)
                {
                    stepFuturePair.Add(passDatatoStrcuture(EachPlayerNumber, infoPair.Value[futureNo]));
                }

                structuredGame[infoPair.Key] = stepFuturePair;
            }
        }
        return structuredGame;
    }

    public Dictionary<string, List<Dictionary<string, List<List<List<float>>>>>> PyFootball(int EachPlayerNumber, bool ifOriginalGame, string url) /*** originalGame=0: original game, originalGame=1: future game***/
    {
        print(url);

        WebRequest wReq = WebRequest.Create(url);
        WebResponse wResp = wReq.GetResponse();
        System.IO.Stream respStream = wResp.GetResponseStream();

        /***
        structuredGame:
        {
            "OriginalGame": [{}]
        }
        or
        {
            "Step0": [{future1}, {future2}]
        }
        ***/
        Dictionary<string, List<Dictionary<string, List<List<List<float>>>>>> structuredGame = new Dictionary<string, List<Dictionary<string, List<List<List<float>>>>>>();
        using (System.IO.StreamReader Observation = new System.IO.StreamReader(respStream))
        {
            string Steps = "";
            while ((Steps = Observation.ReadLine()) != null)
            {
                print(Steps);
                structuredGame = parseGFJson(Steps);
                var originalGameMultiFuture = JsonConvert.DeserializeObject<Dictionary<string, List<Dictionary<string, Dictionary<string, List<List<float>>>>>>>(Steps);
                if (ifOriginalGame)
                {
                    originalStructuredGame = originalGameMultiFuture;
                }
            }
            Observation.Close();
        }
        wResp.Close();
        return structuredGame;
    }

    public void player(Dictionary<string, List<List<List<float>>>> New_Team, int step_num) /*** New_Team is single future of Multiple future: New_Team ***/
    {
        foreach (var infoPair in New_Team)
        {
            if (infoPair.Key.Contains("TeamLocations"))
            {
                String rPlayer = infoPair.Key.Replace("TeamLocations", "Player"); /*** "Right" or "Left" ***/
                String mPlayer = "m" + rPlayer;

                for (int playerId = 0; playerId < infoPair.Value.Count; playerId++)
                {
                    GameObject rPlayerObject = GameObject.Find(rPlayer + playerId.ToString());
                    GameObject mPlayerObject = GameObject.Find(mPlayer + playerId.ToString());

                    float Location_x = infoPair.Value[playerId][step_num][0];
                    float Location_y = infoPair.Value[playerId][step_num][1];

                    rPlayerObject.transform.position = new Vector3(scale_x(Location_x) / 1, -1.8f, scale_z(Location_y) / 1);
                    mPlayerObject.transform.localPosition = new Vector3(scale_x(Location_x) / scaleSize, -0.2f, scale_z(Location_y) / scaleSize);
                }

            }
            if (infoPair.Key.Contains("TeamDirections"))
            {
                String rPlayer = infoPair.Key.Replace("TeamDirections", "Player"); /*** "Right" or "Left" ***/
                String mPlayer = "m" + rPlayer;

                for (int playerId = 0; playerId < infoPair.Value.Count; playerId++)
                {
                    GameObject rPlayerObject = GameObject.Find(rPlayer + playerId.ToString());
                    GameObject mPlayerObject = GameObject.Find(mPlayer + playerId.ToString());

                    /*** Players Directions ***/
                    float Direction_x = infoPair.Value[playerId][step_num][0];
                    float Direction_z = infoPair.Value[playerId][step_num][1];
                    float Direction_c = Mathf.Sqrt(Mathf.Pow(Direction_x, 2) + Mathf.Pow(Direction_z, 2)) + 0.00001f;  // * Pythagorean theorem *
                    float Direction_math_sin = Direction_z / Direction_c;
                    float Direction_math_cos = Direction_x / Direction_c;
                    float Direction_rotation = Mathf.Asin(Direction_math_sin) / Mathf.PI * 180;

                    if (Direction_math_cos > 0 && Direction_math_sin > 0)
                    {
                        rPlayerObject.transform.rotation = Quaternion.Euler(0, 90 - Mathf.Abs(Direction_rotation), 0);
                        mPlayerObject.transform.rotation = Quaternion.Euler(0, 90 - Mathf.Abs(Direction_rotation), 0);
                    }
                    else if (Direction_math_cos < 0 && Direction_math_sin > 0)
                    {
                        rPlayerObject.transform.rotation = Quaternion.Euler(0, -90 + Mathf.Abs(Direction_rotation), 0);
                        mPlayerObject.transform.rotation = Quaternion.Euler(0, -90 + Mathf.Abs(Direction_rotation), 0);
                    }
                    else if (Direction_math_cos < 0 && Direction_math_sin < 0)
                    {
                        rPlayerObject.transform.rotation = Quaternion.Euler(0, -Mathf.Abs(Direction_rotation) - 90, 0);
                        mPlayerObject.transform.rotation = Quaternion.Euler(0, -90 + Mathf.Abs(Direction_rotation), 0);
                    }
                    else if (Direction_math_cos > 0 && Direction_math_sin < 0)
                    {
                        rPlayerObject.transform.rotation = Quaternion.Euler(0, 90 + Mathf.Abs(Direction_rotation), 0);
                        mPlayerObject.transform.rotation = Quaternion.Euler(0, -90 + Mathf.Abs(Direction_rotation), 0);
                    }

                }
            }
        }
    }


    public Dictionary<string, List<List<List<float>>>> Create_Team(int EachPlayerNumber)
    {
        Dictionary<string, List<List<List<float>>>> new_team = new Dictionary<string, List<List<List<float>>>>();

        /*** Structure
        new_team: {'RightTeamLocations':[
            [[ x1, y1], [ x2, y2], [ x3, y3]],  //Player1 [step0, step1, step3]
            [[ x1, y1], [ x2, y2], [ x3, y3]],  //Player2 [step0, step1, step3]
            [[ x1, y1], [ x2, y2], [ x3, y3]]   //Player3 [step0, step1, step3]],
            'LeftTeamLocations':[
            [[ x1, y1], [ x2, y2], [ x3, y3]],  //Player1 [step0, step1, step3]
            [[ x1, y1], [ x2, y2], [ x3, y3]],  //Player2 [step0, step1, step3]
            [[ x1, y1], [ x2, y2], [ x3, y3]]   //Player3 [step0, step1, step3]],
            'BallLocations': [ [[x1, y1], [x2, y2], [x3, y3]] ]  // need to use in this form: New_Team["BallLocations"][0]
            'Event': [[[Lid 1, a1], [Rid 1, a1]],
            [[Lid 2, a2], [Rid 2, A2]]]
        }

        ***/

        List<List<List<float>>> new_players_RightTeamLocations = new List<List<List<float>>>();
        new_team.Add("RightTeamLocations", new_players_RightTeamLocations);

        List<List<List<float>>> new_players_LeftTeamLocations = new List<List<List<float>>>();
        new_team.Add("LeftTeamLocations", new_players_LeftTeamLocations);

        List<List<List<float>>> new_players_RightTeamDirections = new List<List<List<float>>>();
        new_team.Add("RightTeamDirections", new_players_RightTeamDirections);

        List<List<List<float>>> new_players_LeftTeamDirections = new List<List<List<float>>>();
        new_team.Add("LeftTeamDirections", new_players_LeftTeamDirections);

        List<List<float>> ballLocation = new List<List<float>>();
        List<List<List<float>>> ball_locations = new List<List<List<float>>>();
        ball_locations.Add(ballLocation);
        new_team.Add("BallLocations", ball_locations);
        List<List<List<float>>> events = new List<List<List<float>>>();
        new_team.Add("Event", events);

        for (int i = 0; i < EachPlayerNumber; i++)
        {
            List<List<float>> players_RightTeamLocations = new List<List<float>>();
            new_team["RightTeamLocations"].Add(players_RightTeamLocations);

            List<List<float>> players_LeftTeamLocations = new List<List<float>>();
            new_team["LeftTeamLocations"].Add(players_LeftTeamLocations);

            List<List<float>> players_RightTeamDirections = new List<List<float>>();
            new_team["RightTeamDirections"].Add(players_RightTeamDirections);

            List<List<float>> players_LeftTeamDirections = new List<List<float>>();
            new_team["LeftTeamDirections"].Add(players_LeftTeamDirections);
        }
        return new_team;
    }

    /***
    Scale functions
    ***/
    public static float scale_x(float x)
    {
        return x * (float)52.5;
    }
    public static float scale_z(float z)
    {
        return (z / (float)0.42) * 32;
    }
    public static float scaleDown_x(float x)
    {
        return (x) / (float)52.5;
    }
    public static float scaleDown_z(float z)
    {
        return ((z) / 32) * (float)0.42;
    }

}
