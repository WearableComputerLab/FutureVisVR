using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserStudyInterface : MonoBehaviour
{
    [SerializeField]
    int ParticiantID;
    [SerializeField]
    private bool GamePlay = false;
    [SerializeField]
    private bool ShowArrow = false;
    [SerializeField]
    private bool showHeatmap = false;
    [SerializeField]
    private bool ShowMiniature = false;

    public enum GamePlayers
    {
        None, Original,
        BluePlayer0, BluePlayer1, BluePlayer2, BluePlayer3, BluePlayer4, BluePlayer5, BluePlayer6, BluePlayer7, BluePlayer8, BluePlayer9, BluePlayer10,
        RedPlayer0, RedPlayer1, RedPlayer2, RedPlayer3, RedPlayer4, RedPlayer5, RedPlayer6, RedPlayer7, RedPlayer8, RedPlayer9, RedPlayer10

    }

    public GamePlayers HighlightedGamePlayer = GamePlayers.None;
    public GamePlayers AttachedGamePlayer = GamePlayers.None;
    private GamePlayers temp_HighlightedGamePlayer = GamePlayers.None;
    private GamePlayers temp_AttachedGamePlayer = GamePlayers.None;

    private Transform miniMapObject;
    private OVRInput.Controller controllerType;
    private GameObject RaycasterCursorVisual;
    private List<Vector3> CursorWorldPositions;
    private bool triggerHasBeenPressed;

    public enum Conditions
    {
        None,
        No_Time_Limit_2_Arrows, No_Time_Limit_5_Arrows, No_Time_Limit_Heatmap, Yes_Time_Limit_2_Arrows, Yes_Time_Limit_5_Arrows, Yes_Time_Limit_Heatmap
    }

    public Conditions condition;
    private string conditionName;
    // private int[][] currentSituations = new int[][];
    private List<List<int>> currentSituations = new List<List<int>>();
    private List<List<int>> No_Time_Limit_2_Arrows_Trials = new List<List<int>>();
    private List<List<int>> No_Time_Limit_5_Arrows_Trials = new List<List<int>>();
    private List<List<int>> No_Time_Limit_Heatmap_Trials = new List<List<int>>();
    private List<List<int>> Yes_Time_Limit_2_Arrows_Trials = new List<List<int>>();
    private List<List<int>> Yes_Time_Limit_5_Arrows_Trials = new List<List<int>>();
    private List<List<int>> Yes_Time_Limit_Heatmap_Trials = new List<List<int>>();


    private int situationNumber;
    string temp_scenarioName;
    private string current_observer;

    private bool VisButtonAHasBeenPressed;
    private GUIStyle PressAGUIStyle;

    /*** Count Completion Time***/
    private float timer;
    private bool startTimer;

    /*** Countdown Timer ***/
    private bool TimeLimit;
    private float totalTime = 5f;

    /*** Start Experiment***/
    public static bool startCondition;

    /*** Eye Tracking***/
    public static bool startEyeTracking;
    private float eyeTimer;

    /*** Show Hide Players ***/
    private bool showPlayers;
    private int PressButtonATimes = 0;

    private void Start()
    {
        /*** Step Num of Situations in Original Game ***/  // [Observer, Highlighted, Step_number], blue(right) player:1-10, red(left) player:11-20
        Dataset.startDataset();
        No_Time_Limit_2_Arrows_Trials = Dataset.No_Time_Limit_2_Arrows_Trials;
        No_Time_Limit_5_Arrows_Trials = Dataset.No_Time_Limit_5_Arrows_Trials;
        No_Time_Limit_Heatmap_Trials = Dataset.No_Time_Limit_Heatmap_Trials;
        Yes_Time_Limit_2_Arrows_Trials = Dataset.Yes_Time_Limit_2_Arrows_Trials;
        Yes_Time_Limit_5_Arrows_Trials = Dataset.Yes_Time_Limit_5_Arrows_Trials;
        Yes_Time_Limit_Heatmap_Trials = Dataset.Yes_Time_Limit_Heatmap_Trials;

        /*** Select Scenario from Unity Inspector ***/
        switch (condition)
        {
            case Conditions.None:
                // TO-DO Hide Players
                startCondition = false;
                break;
            case Conditions.No_Time_Limit_2_Arrows:
                // parseScenarioName("No_Time_Limit_2_Arrows");
                conditionName = "No_Time_Limit_2Arrows";
                Dataset.shuffleDataset(No_Time_Limit_2_Arrows_Trials);
                currentSituations = No_Time_Limit_2_Arrows_Trials;  // Trials = Situations
                startCondition = true;
                break;
            case Conditions.No_Time_Limit_5_Arrows:
                // parseScenarioName("No_Time_Limit_5_Arrows");
                conditionName = "No_Time_Limit_5Arrows";
                Dataset.shuffleDataset(No_Time_Limit_5_Arrows_Trials);
                currentSituations = No_Time_Limit_5_Arrows_Trials;
                startCondition = true;
                break;
            case Conditions.No_Time_Limit_Heatmap:
                // parseScenarioName("No_Time_Limit_Heatmap");
                conditionName = "No_Time_Limit_Heatmap";
                Dataset.shuffleDataset(No_Time_Limit_Heatmap_Trials);
                currentSituations = No_Time_Limit_Heatmap_Trials;
                startCondition = true;
                break;
            case Conditions.Yes_Time_Limit_2_Arrows:
                // parseScenarioName("Yes_Time_Limit_2_Arrows");
                conditionName = "Yes_Time_Limit_2Arrows";
                Dataset.shuffleDataset(Yes_Time_Limit_2_Arrows_Trials);
                currentSituations = Yes_Time_Limit_2_Arrows_Trials;
                startCondition = true;
                break;
            case Conditions.Yes_Time_Limit_5_Arrows:
                // parseScenarioName("Yes_Time_Limit_5_Arrows");
                conditionName = "Yes_Time_Limit_5Arrows";
                Dataset.shuffleDataset(Yes_Time_Limit_5_Arrows_Trials);
                currentSituations = Yes_Time_Limit_5_Arrows_Trials;
                startCondition = true;
                break;
            case Conditions.Yes_Time_Limit_Heatmap:
                // parseScenarioName("Yes_Time_Limit_Heatmap");
                conditionName = "Yes_Time_Limit_Heatmap";
                Dataset.shuffleDataset(Yes_Time_Limit_Heatmap_Trials);
                currentSituations = Yes_Time_Limit_Heatmap_Trials;
                startCondition = true;
                break;
        }


        if (startCondition)
        {
            /*** Parse Scenario Name ***/
            int underscoreIndex = conditionName.IndexOf("_");
            if (underscoreIndex != -1)
            {
                if (conditionName.Substring(0, underscoreIndex) == "No")
                {
                    TimeLimit = false;
                    timer = 0;
                }
                else if (conditionName.Substring(0, underscoreIndex) == "Yes")
                {
                    TimeLimit = true;
                    timer = totalTime;
                }
            }
            temp_scenarioName = conditionName;
            int temp_underscoreIndex = temp_scenarioName.IndexOf("_");
            while (temp_underscoreIndex != -1)
            {
                temp_scenarioName = temp_scenarioName.Remove(0, temp_underscoreIndex + 1);
                temp_underscoreIndex = temp_scenarioName.IndexOf("_");
            }

            /*** Ray & miniMap***/
            miniMapObject = GameObject.Find("MovableMiniature").transform;
            controllerType = OVRInput.Controller.RTouch;
            RaycasterCursorVisual = GameObject.Find("RaycasterCursorVisual");
            CursorWorldPositions = new List<Vector3>();

            /*** Setup experiment parameters before running ***/
            // previous_observer = currentSituations[0][0];
            eyeTimer = 0;
            triggerHasBeenPressed = false;
            startTimer = false;
            situationNumber = 0;
            current_observer = setSituation(currentSituations[situationNumber]);
            /*** Hide players at the beginning ***/
            showPlayers = false;
            ShowHidePlayers(showPlayers);

            showHeatmap = false;
            ShowArrow = false;
        }
    }

    private void LateUpdate()
    {
        MovableFootball.gamePlay = GamePlay;
        MovableFootball.showFuture = ShowArrow;
        MovableFootball.showHeatmap = showHeatmap;
        MovableFootball.showMiniatureView = ShowMiniature;
        MovableFootball.showMovableMiniature = ShowMiniature;

        if (temp_HighlightedGamePlayer != HighlightedGamePlayer)
        {
            temp_HighlightedGamePlayer = HighlightedGamePlayer;
            switch (HighlightedGamePlayer)
            {
                case GamePlayers.None:
                    GoldHaloEffect.createHighlightedPlayer(null);
                    break;
                case GamePlayers.BluePlayer0:
                    GoldHaloEffect.createHighlightedPlayer("RightPlayer0");
                    break;
                case GamePlayers.BluePlayer1:
                    GoldHaloEffect.createHighlightedPlayer("RightPlayer1");
                    break;
                case GamePlayers.BluePlayer2:
                    GoldHaloEffect.createHighlightedPlayer("RightPlayer2");
                    break;
                case GamePlayers.BluePlayer3:
                    GoldHaloEffect.createHighlightedPlayer("RightPlayer3");
                    break;
                case GamePlayers.BluePlayer4:
                    GoldHaloEffect.createHighlightedPlayer("RightPlayer4");
                    break;
                case GamePlayers.BluePlayer5:
                    GoldHaloEffect.createHighlightedPlayer("RightPlayer5");
                    break;
                case GamePlayers.BluePlayer6:
                    GoldHaloEffect.createHighlightedPlayer("RightPlayer6");
                    break;
                case GamePlayers.BluePlayer7:
                    GoldHaloEffect.createHighlightedPlayer("RightPlayer7");
                    break;
                case GamePlayers.BluePlayer8:
                    GoldHaloEffect.createHighlightedPlayer("RightPlayer8");
                    break;
                case GamePlayers.BluePlayer9:
                    GoldHaloEffect.createHighlightedPlayer("RightPlayer9");
                    break;
                case GamePlayers.BluePlayer10:
                    GoldHaloEffect.createHighlightedPlayer("RightPlayer10");
                    break;
                case GamePlayers.RedPlayer0:
                    GoldHaloEffect.createHighlightedPlayer("LeftPlayer0");
                    break;
                case GamePlayers.RedPlayer1:
                    GoldHaloEffect.createHighlightedPlayer("LeftPlayer1");
                    break;
                case GamePlayers.RedPlayer2:
                    GoldHaloEffect.createHighlightedPlayer("LeftPlayer2");
                    break;
                case GamePlayers.RedPlayer3:
                    GoldHaloEffect.createHighlightedPlayer("LeftPlayer3");
                    break;
                case GamePlayers.RedPlayer4:
                    GoldHaloEffect.createHighlightedPlayer("LeftPlayer4");
                    break;
                case GamePlayers.RedPlayer5:
                    GoldHaloEffect.createHighlightedPlayer("LeftPlayer5");
                    break;
                case GamePlayers.RedPlayer6:
                    GoldHaloEffect.createHighlightedPlayer("LeftPlayer6");
                    break;
                case GamePlayers.RedPlayer7:
                    GoldHaloEffect.createHighlightedPlayer("LeftPlayer7");
                    break;
                case GamePlayers.RedPlayer8:
                    GoldHaloEffect.createHighlightedPlayer("LeftPlayer8");
                    break;
                case GamePlayers.RedPlayer9:
                    GoldHaloEffect.createHighlightedPlayer("LeftPlayer9");
                    break;
                case GamePlayers.RedPlayer10:
                    GoldHaloEffect.createHighlightedPlayer("LeftPlayer10");
                    break;
            }
        }

        if (temp_AttachedGamePlayer != AttachedGamePlayer)
        {
            temp_AttachedGamePlayer = AttachedGamePlayer;
            switch (AttachedGamePlayer)
            {
                case GamePlayers.Original:
                    CameraFollow.resetPosition();
                    break;
                case GamePlayers.BluePlayer0:
                    CameraFollow.AttachedGamePlayer = "RightPlayer0";
                    break;
                case GamePlayers.BluePlayer1:
                    CameraFollow.AttachedGamePlayer = "RightPlayer1";
                    break;
                case GamePlayers.BluePlayer2:
                    CameraFollow.AttachedGamePlayer = "RightPlayer2";
                    break;
                case GamePlayers.BluePlayer3:
                    CameraFollow.AttachedGamePlayer = "RightPlayer3";
                    break;
                case GamePlayers.BluePlayer4:
                    CameraFollow.AttachedGamePlayer = "RightPlayer4";
                    break;
                case GamePlayers.BluePlayer5:
                    CameraFollow.AttachedGamePlayer = "RightPlayer5";
                    break;
                case GamePlayers.BluePlayer6:
                    CameraFollow.AttachedGamePlayer = "RightPlayer6";
                    break;
                case GamePlayers.BluePlayer7:
                    CameraFollow.AttachedGamePlayer = "RightPlayer7";
                    break;
                case GamePlayers.BluePlayer8:
                    CameraFollow.AttachedGamePlayer = "RightPlayer8";
                    break;
                case GamePlayers.BluePlayer9:
                    CameraFollow.AttachedGamePlayer = "RightPlayer9";
                    break;
                case GamePlayers.BluePlayer10:
                    CameraFollow.AttachedGamePlayer = "RightPlayer10";
                    break;
                case GamePlayers.RedPlayer0:
                    CameraFollow.AttachedGamePlayer = "LeftPlayer0";
                    break;
                case GamePlayers.RedPlayer1:
                    CameraFollow.AttachedGamePlayer = "LeftPlayer1";
                    break;
                case GamePlayers.RedPlayer2:
                    CameraFollow.AttachedGamePlayer = "LeftPlayer2";
                    break;
                case GamePlayers.RedPlayer3:
                    CameraFollow.AttachedGamePlayer = "LeftPlayer3";
                    break;
                case GamePlayers.RedPlayer4:
                    CameraFollow.AttachedGamePlayer = "LeftPlayer4";
                    break;
                case GamePlayers.RedPlayer5:
                    CameraFollow.AttachedGamePlayer = "LeftPlayer5";
                    break;
                case GamePlayers.RedPlayer6:
                    CameraFollow.AttachedGamePlayer = "LeftPlayer6";
                    break;
                case GamePlayers.RedPlayer7:
                    CameraFollow.AttachedGamePlayer = "LeftPlayer7";
                    break;
                case GamePlayers.RedPlayer8:
                    CameraFollow.AttachedGamePlayer = "LeftPlayer8";
                    break;
                case GamePlayers.RedPlayer9:
                    CameraFollow.AttachedGamePlayer = "LeftPlayer9";
                    break;
                case GamePlayers.RedPlayer10:
                    CameraFollow.AttachedGamePlayer = "LeftPlayer10";
                    break;
            }
        }

        if (startCondition)
        {
            if (OVRInput.IsControllerConnected(controllerType) && OVRInput.GetDown(OVRInput.Button.Two))
            {
                ShowMiniature = false;
            }
            else if (OVRInput.IsControllerConnected(controllerType) && OVRInput.GetUp(OVRInput.Button.Two))
            {
                ShowMiniature = true;
            }

            if (((OVRInput.IsControllerConnected(controllerType) && OVRInput.GetDown(OVRInput.Button.One)) || Input.GetMouseButtonDown(0)) && PressButtonATimes == 0)
            {
                ControllerVibrate();

                PressButtonATimes++;
                showPlayers = true;
                ShowHidePlayers(showPlayers);
                ShowMiniature = true;
            }
            // if ((OVRInput.IsControllerConnected(controllerType) && OVRInput.Get(OVRInput.Button.One)) || Input.GetMouseButtonDown(0))  // TO-DO check if A has been pressed, if yes, show future, hide "Press A to show future" hint.
            else if ((((OVRInput.IsControllerConnected(controllerType) && OVRInput.GetDown(OVRInput.Button.One)) || Input.GetMouseButtonDown(0)) && showPlayers && PressButtonATimes == 1))  // TO-DO check if A has been pressed, if yes, show future, hide "Press A to show future" hint.
            {
                ControllerVibrate();

                // PressButtonATimes++;

                startEyeTracking = true;

                print("BUTTON ONE HAS BEEN PRESSED");
                VisButtonAHasBeenPressed = true;
                startTimer = true;
                if (temp_scenarioName.Contains("Heatmap"))
                {
                    showHeatmap = true;
                }
                else
                {
                    MovableFootball.conditionFutureAmount = int.Parse(temp_scenarioName.Substring(0, 1));
                    ShowArrow = true;
                    // MultiFuture.updateFutureFarDetails(MovableFootball.step_num, MovableFootball.multiFutureFar, 1);
                }
            }

            if ((showHeatmap || ShowArrow) && VisButtonAHasBeenPressed)
            {
                eyeTimer += Time.deltaTime;
                saveEyeDataAsCSV(ParticiantID, conditionName, eyeTimer, EyeInteraction.hit, currentSituations[situationNumber]);
            }

            if ((showHeatmap || ShowArrow) && ((OVRInput.IsControllerConnected(controllerType) && OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger)) || Input.GetMouseButtonDown(1)))
            {
                ControllerVibrate();

                triggerHasBeenPressed = true;
                startTimer = false; // When the trigger is being pressed, will not record the time any more, and the first element of CursorWorldPositions is the position while pressing.
                CursorWorldPositions.Add(GameObject.Find("FootballEngine").transform.TransformPoint(RaycasterCursorVisual.transform.position)); // Get and store cursor world positions
            }
            else
            {
                if (triggerHasBeenPressed && (showHeatmap || ShowArrow))
                {
                    // Check if trigger has been pressed, if yes, calculate average, store, jump to another situation


                    // print("EyeInteraction.EyeTrackingMiniatureTimes: " + EyeInteraction.EyeTrackingMiniatureTimes);
                    print("EyeInteraction.EyeTrackingMiniatureTimer: " + EyeInteraction.EyeTrackingMiniatureTimer);

                    saveGeneralDataAsCSV(ParticiantID, conditionName, currentSituations[situationNumber], timer, CursorWorldPositions,
                                EyeInteraction.EyeTrackingPitchPositions, EyeInteraction.EyeTrackingMiniatureTimer);

                    situationNumber++;

                    // Reset parameters
                    if (TimeLimit)
                        timer = totalTime;
                    else
                        timer = 0;
                    Timer.currentTime = timer;

                    eyeTimer = 0;
                    startEyeTracking = false;
                    triggerHasBeenPressed = false;
                    VisButtonAHasBeenPressed = false;
                    startTimer = false;
                    showHeatmap = false;
                    ShowArrow = false;
                    CursorWorldPositions = new List<Vector3>();
                    try
                    {
                        current_observer = setSituation(currentSituations[situationNumber]);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        StopPlayMode();
                    }
                    showPlayers = false;
                    ShowHidePlayers(showPlayers);
                    ShowMiniature = false;
                    PressButtonATimes = 0;
                }
            }

            if (startTimer)
            {
                if (TimeLimit)
                {
                    print("Timer: " + timer);
                    timer -= Time.deltaTime;

                    if (timer <= 0.001f)
                    {
                        saveGeneralDataAsCSV(ParticiantID, conditionName, currentSituations[situationNumber], timer, CursorWorldPositions,
                                    EyeInteraction.EyeTrackingPitchPositions, EyeInteraction.EyeTrackingMiniatureTimer);

                        situationNumber++;

                        timer = totalTime;
                        Timer.currentTime = timer;

                        eyeTimer = 0;
                        startEyeTracking = false;
                        triggerHasBeenPressed = false;
                        VisButtonAHasBeenPressed = false;
                        startTimer = false;
                        showHeatmap = false;
                        ShowArrow = false;
                        CursorWorldPositions = new List<Vector3>();
                        try
                        {
                            current_observer = setSituation(currentSituations[situationNumber]);
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            StopPlayMode();
                        }
                        showPlayers = false;
                        ShowHidePlayers(showPlayers);
                        ShowMiniature = false;
                        PressButtonATimes = 0;
                    }
                }
                else
                    timer += Time.deltaTime;
                Timer.currentTime = timer;
            }


            GameObject.Find("OVRCameraRig").transform.position = new Vector3(GameObject.Find(current_observer).transform.position.x, 0, GameObject.Find(current_observer).transform.position.z);
            GameObject.Find("OVRCameraRig").transform.rotation = GameObject.Find(current_observer).transform.rotation;
        }
    }


    private void saveGeneralDataAsCSV(int ParticipantID, string Condition, List<int> situation, float CompletionTime, List<Vector3> SelectedPositions, List<Vector3> EyeTrackingPitchPositions, float EyeTrackingMiniatureTimer)
    {
        // Get the current date
        DateTime currentDate = DateTime.Now;

        string observer;
        string highlightedPlayer;
        if (situation[0] > 10)
            observer = "LeftPlayer" + (situation[0] - 10).ToString();
        else
            observer = "RightPlayer" + situation[0].ToString();
        if (situation[1] > 10)
            highlightedPlayer = "LeftPlayer" + (situation[1] - 10).ToString();
        else
            highlightedPlayer = "RightPlayer" + situation[1].ToString();
        GameObject observerObject = GameObject.Find(observer);
        GameObject highlightedPlayerObject = GameObject.Find(highlightedPlayer);

        float ObserverTargetDistance = Vector3.Distance(observerObject.transform.position, highlightedPlayerObject.transform.position);
        float SelectedpositionTargetDistance = -1; // -1 indicates no distance
        if (SelectedPositions.Count != 0)
            SelectedpositionTargetDistance = Vector3.Distance(SelectedPositions[0], highlightedPlayerObject.transform.position);

        string filePath = "C:/Users/chenk/Desktop/First_Project/User_Study/Collected_Data_CSV/" + "Collected_PID_" + ParticipantID.ToString() + ".csv";

        bool fileExists = File.Exists(filePath);

        using (StreamWriter data = new StreamWriter(filePath, true))
        {
            if (!fileExists)
            {
                data.WriteLine($"{"Date"}, {"ParticipantID"}, {"Condition"}, {"Situation"}, {"CompletionTime"}, {"ObserverTargetDistance"}, {"SelectedpositionTargetDistance"}, {"EyeTrackingMiniatureTimer"}, {"SelectedPositions"}, {"ObserverObjectPosition"}, {"HighlightedPlayerObjectPosition"}");
            }
            // data.WriteLine($"{currentDate}, {ParticiantID},{Condition}, {situation[3]},{CompletionTime}, {ObserverTargetDistance}, {SelectedpositionTargetDistance}, {EyeTrackingMiniatureTimer}, {ConvertListVector3ToCSVString(SelectedPositions)}, {ConvertListVector3ToCSVString(EyeTrackingPitchPositions)},  {ConvertListVector3ToCSVString(new List<Vector3> { observerObject.transform.position })}, {ConvertListVector3ToCSVString(new List<Vector3> { highlightedPlayerObject.transform.position })}");
            data.WriteLine($"{currentDate}, {ParticipantID},{Condition}, {situation[3]},{CompletionTime}, {ObserverTargetDistance}, {SelectedpositionTargetDistance}, {EyeTrackingMiniatureTimer}, {ConvertListVector3ToCSVString(SelectedPositions)},  {ConvertListVector3ToCSVString(new List<Vector3> { observerObject.transform.position })}, {ConvertListVector3ToCSVString(new List<Vector3> { highlightedPlayerObject.transform.position })}");
        }
    }
    private void saveEyeDataAsCSV(int ParticipantID, string Condition, float time, RaycastHit hit, List<int> situation)
    {
        // Get the current date
        DateTime currentDate = DateTime.Now;

        string observer;
        string highlightedPlayer;
        if (situation[0] > 10)
            observer = "LeftPlayer" + (situation[0] - 10).ToString();
        else
            observer = "RightPlayer" + situation[0].ToString();
        if (situation[1] > 10)
            highlightedPlayer = "LeftPlayer" + (situation[1] - 10).ToString();
        else
            highlightedPlayer = "RightPlayer" + situation[1].ToString();

        GameObject observerObject = GameObject.Find(observer);
        GameObject highlightedPlayerObject = GameObject.Find(highlightedPlayer);

        GameObject head = GameObject.Find("OVRCameraRig");

        float ObserverTargetDistance = Vector3.Distance(observerObject.transform.position, highlightedPlayerObject.transform.position);
        float EyeTargetDistance = Vector3.Distance(hit.point, highlightedPlayerObject.transform.position);

        string filePath = "C:/Users/chenk/Desktop/First_Project/User_Study/User_Study_Eye_Data/" + "Eye_PID_" + ParticipantID.ToString() + "_" + Condition + ".csv";

        bool fileExists = File.Exists(filePath);

        if (hit.collider != null)
        {
            using (StreamWriter data = new StreamWriter(filePath, true))
            {
                if (!fileExists)
                {
                    data.WriteLine($"{"Date"},{"ParticiantID"},{"Condition"},{"Situation"},{"Time"},{"EyeTrackingPositions"},{"HitObject"},{"EyeTargetDistance"},{"ObserverTargetDistance"},{"ObserverObjectPosition"},{"OighlightedPlayerObject"}, {"HeadPosition"}, {"HeadRotation"}");
                }
                data.WriteLine($"{currentDate}, {ParticipantID}, {Condition}, {situation[3]}, {time}, {ConvertListVector3ToCSVString(new List<Vector3> { hit.point })}, {hit.collider.gameObject.name}, {EyeTargetDistance}, {ObserverTargetDistance}, {ConvertListVector3ToCSVString(new List<Vector3> { observerObject.transform.position })}, {ConvertListVector3ToCSVString(new List<Vector3> { highlightedPlayerObject.transform.position })}, {ConvertListVector3ToCSVString(new List<Vector3> { head.transform.position })}, {ConvertListVector3ToCSVString(new List<Vector3> { new Vector3(head.transform.rotation.x, head.transform.rotation.y, head.transform.rotation.z) })}");
            }
        }
    }
    private string ConvertListToCSV(List<int> dataList)
    {
        // Convert the list to a CSV-formatted string
        string csvData = string.Join(";", dataList);

        return csvData;
    }
    string ConvertListVector3ToCSVString(List<Vector3> list)
    {
        StringBuilder sb = new StringBuilder();

        // Append the list as a single column in the CSV string
        sb.Append("[");
        for (int i = 0; i < list.Count; i++)
        {
            Vector3 vector = list[i];
            sb.Append("[" + vector.x + ";" + vector.y + ";" + vector.z + "]");
            if (i < list.Count - 1)
                sb.Append("; ");
        }
        sb.Append("]");
        return sb.ToString();
    }
    // string ConvertListToCSVString(List<Vector3> list)
    // {
    //     StringBuilder sb = new StringBuilder();
    //     sb.Append("[");
    //     foreach (Vector3 vector in list)
    //     {
    //         sb.Append($"{vector.x}, {vector.y}, {vector.z}");
    //     }
    //     sb.Append("]");
    //     return sb.ToString();
    // }

    private string setSituation(List<int> situation) // [Observer, Highlighted, Step_number], blue(right) player:1-10, red(left) player:11-20
    {
        string observer;
        string highlightedPlayer;
        int stepNumber;

        stepNumber = situation[2];
        MovableFootball.step_num = stepNumber;

        if (situation[0] > 10)
            observer = "LeftPlayer" + (situation[0] - 10).ToString();
        else
            observer = "RightPlayer" + situation[0].ToString();
        if (situation[1] > 10)
            highlightedPlayer = "LeftPlayer" + (situation[1] - 10).ToString();
        else
            highlightedPlayer = "RightPlayer" + situation[1].ToString();

        GoldHaloEffect.createHighlightedObserver(observer);
        GoldHaloEffect.hideObserverPlayerObject(observer);
        GoldHaloEffect.createHighlightedPlayer(highlightedPlayer);
        return observer;
    }

    private void ShowHidePlayers(bool show)
    {
        if (show)
        {
            for (int i = 0; i < 11; i++)
            {

                GameObject.Find("RightPlayer" + i).transform.localScale = new Vector3(0.65f, 2.2f, 0.65f);
                GameObject.Find("LeftPlayer" + i).transform.localScale = new Vector3(0.65f, 2.2f, 0.65f);
            }
            GameObject.Find(current_observer).transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
        }
        else if (!show)
        {
            for (int i = 0; i < 11; i++)
            {
                GameObject.Find("RightPlayer" + i).transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
                GameObject.Find("LeftPlayer" + i).transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
            }
        }
    }

    private Vector3 CalculateAverage(List<Vector3> values)
    {
        Vector3 sum = Vector3.zero;
        int index = 0;
        int length = values.ToArray().Length;
        for (int i = 0; i < length / 3; i++)
        {
            index++;
            sum += values[i];
        }
        return sum / index;
    }

    private void ControllerVibrate()
    {
        // Trigger haptic feedback (vibration) on the controller
        OVRInput.SetControllerVibration(1.5f, 1.5f, OVRInput.Controller.Active);
        Invoke("StopVibration", 0.2f);
    }
    private void StopVibration()
    {
        // Stop the haptic feedback (vibration)
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.Active);
    }

    private void StopPlayMode()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Debug.Log("The condition has been finished.");
#endif
    }

}
