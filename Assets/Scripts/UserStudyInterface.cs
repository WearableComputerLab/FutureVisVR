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

    public enum Scenario
    {
        None,
        No_Time_Limit_2_Arrows, No_Time_Limit_5_Arrows, No_Time_Limit_Heatmap, Yes_Time_Limit_2_Arrows, Yes_Time_Limit_5_Arrows, Yes_Time_Limit_Heatmap
    }

    public Scenario scenario;
    private string scenarioName;
    // private int[][] currentSituations = new int[][];
    private List<List<int>> currentSituations = new List<List<int>>();
    private List<List<int>> No_Time_Limit_2_Arrows_Situations = new List<List<int>>();
    private List<List<int>> No_Time_Limit_5_Arrows_Situations = new List<List<int>>();
    private List<List<int>> No_Time_Limit_Heatmap_Situations = new List<List<int>>();
    private List<List<int>> Yes_Time_Limit_2_Arrows_Situations = new List<List<int>>();
    private List<List<int>> Yes_Time_Limit_5_Arrows_Situations = new List<List<int>>();
    private List<List<int>> Yes_Time_Limit_Heatmap_Situations = new List<List<int>>();


    private int situationNumber;
    string temp_scenarioName;

    private bool showHintPressA;
    private GUIStyle PressAGUIStyle;

    /*** Count Completion Time***/
    private float timer;
    private bool startTimer;

    /*** Countdown Timer ***/
    private bool TimeLimit;
    private float totalTime = 5f;

    private void Start()
    {
        /*** Step Num of Situations in Original Game ***/  // [Observer, Highlighted, Step_number], blue(right) player:1-10, red(left) player:11-20

        No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 15, 11, 0 });
        No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 20, 13, 489 });
        No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 7, 10, 384 });
        No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 3, 16, 12 });
        No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 1, 6, 60 });
        No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 12, 5, 234 });
        No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 10, 1, 108 });
        No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 2, 18, 144 });
        No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 16, 2, 213 });
        No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 8, 7, 150 });

        Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 15, 11, 0 });
        Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 20, 13, 489 });
        Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 7, 10, 384 });
        Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 3, 16, 12 });
        Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 1, 6, 60 });
        Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 12, 5, 234 });
        Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 10, 1, 108 });
        Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 2, 18, 144 });
        Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 16, 2, 213 });
        Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 8, 7, 150 });

        Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 15, 11, 0 });
        Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 20, 13, 489 });
        Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 7, 10, 384 });
        Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 3, 16, 12 });
        Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 1, 6, 60 });
        Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 12, 5, 234 });
        Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 10, 1, 108 });
        Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 2, 18, 144 });
        Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 16, 2, 213 });
        Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 8, 7, 150 });

        /*** Select Scenario from Unity Inspector ***/
        switch (scenario)
        {
            case Scenario.None:
                // TO-DO Hide Players
                break;
            case Scenario.No_Time_Limit_2_Arrows:
                // parseScenarioName("No_Time_Limit_2_Arrows");
                scenarioName = "No_Time_Limit_2Arrows";
                currentSituations = No_Time_Limit_2_Arrows_Situations;
                break;
            case Scenario.No_Time_Limit_5_Arrows:
                // parseScenarioName("No_Time_Limit_5_Arrows");
                scenarioName = "No_Time_Limit_5Arrows";
                currentSituations = No_Time_Limit_5_Arrows_Situations;
                break;
            case Scenario.No_Time_Limit_Heatmap:
                // parseScenarioName("No_Time_Limit_Heatmap");
                scenarioName = "No_Time_Limit_Heatmap";
                currentSituations = No_Time_Limit_Heatmap_Situations;
                break;
            case Scenario.Yes_Time_Limit_2_Arrows:
                // parseScenarioName("Yes_Time_Limit_2_Arrows");
                scenarioName = "Yes_Time_Limit_2Arrows";
                currentSituations = Yes_Time_Limit_2_Arrows_Situations;
                break;
            case Scenario.Yes_Time_Limit_5_Arrows:
                // parseScenarioName("Yes_Time_Limit_5_Arrows");
                scenarioName = "Yes_Time_Limit_5Arrows";
                currentSituations = Yes_Time_Limit_5_Arrows_Situations;
                break;
            case Scenario.Yes_Time_Limit_Heatmap:
                // parseScenarioName("Yes_Time_Limit_Heatmap");
                scenarioName = "Yes_Time_Limit_Heatmap";
                currentSituations = Yes_Time_Limit_Heatmap_Situations;
                break;
        }

        /*** Parse Scenario Name ***/
        int underscoreIndex = scenarioName.IndexOf("_");
        if (underscoreIndex != -1)
        {
            if (scenarioName.Substring(0, underscoreIndex) == "No")
            {
                TimeLimit = false;
                timer = 0;
            }
            else if (scenarioName.Substring(0, underscoreIndex) == "Yes")
            {
                TimeLimit = true;
                timer = totalTime;
            }
        }
        temp_scenarioName = scenarioName;
        int temp_underscoreIndex = temp_scenarioName.IndexOf("_");
        while (temp_underscoreIndex != -1)
        {
            temp_scenarioName = temp_scenarioName.Remove(0, temp_underscoreIndex + 1);
            temp_underscoreIndex = temp_scenarioName.IndexOf("_");
        }


        miniMapObject = GameObject.Find("MovableMiniature").transform;
        controllerType = OVRInput.Controller.RTouch;

        RaycasterCursorVisual = GameObject.Find("RaycasterCursorVisual");
        CursorWorldPositions = new List<Vector3>();

        // triggerPressedFlag = 0;
        // tem_triggerPressedFlag = 0;
        triggerHasBeenPressed = false;

        /*** Show Hint: Please Press A to Start and Show Future Visualization ***/
        // showHintPressA = true;
        // PressAGUIStyle = new GUIStyle();
        // PressAGUIStyle.fontSize = 60;
        // PressAGUIStyle.normal.textColor = Color.red;

        // timer = 0;
        startTimer = false;

        situationNumber = 0;
        setSituation(currentSituations[situationNumber]);
        showHeatmap = false;
        ShowArrow = false;
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
                    GoldHaloEffect.createHightedPlayer(null);
                    break;
                case GamePlayers.BluePlayer0:
                    GoldHaloEffect.createHightedPlayer("RightPlayer0");
                    break;
                case GamePlayers.BluePlayer1:
                    GoldHaloEffect.createHightedPlayer("RightPlayer1");
                    break;
                case GamePlayers.BluePlayer2:
                    GoldHaloEffect.createHightedPlayer("RightPlayer2");
                    break;
                case GamePlayers.BluePlayer3:
                    GoldHaloEffect.createHightedPlayer("RightPlayer3");
                    break;
                case GamePlayers.BluePlayer4:
                    GoldHaloEffect.createHightedPlayer("RightPlayer4");
                    break;
                case GamePlayers.BluePlayer5:
                    GoldHaloEffect.createHightedPlayer("RightPlayer5");
                    break;
                case GamePlayers.BluePlayer6:
                    GoldHaloEffect.createHightedPlayer("RightPlayer6");
                    break;
                case GamePlayers.BluePlayer7:
                    GoldHaloEffect.createHightedPlayer("RightPlayer7");
                    break;
                case GamePlayers.BluePlayer8:
                    GoldHaloEffect.createHightedPlayer("RightPlayer8");
                    break;
                case GamePlayers.BluePlayer9:
                    GoldHaloEffect.createHightedPlayer("RightPlayer9");
                    break;
                case GamePlayers.BluePlayer10:
                    GoldHaloEffect.createHightedPlayer("RightPlayer10");
                    break;
                case GamePlayers.RedPlayer0:
                    GoldHaloEffect.createHightedPlayer("LeftPlayer0");
                    break;
                case GamePlayers.RedPlayer1:
                    GoldHaloEffect.createHightedPlayer("LeftPlayer1");
                    break;
                case GamePlayers.RedPlayer2:
                    GoldHaloEffect.createHightedPlayer("LeftPlayer2");
                    break;
                case GamePlayers.RedPlayer3:
                    GoldHaloEffect.createHightedPlayer("LeftPlayer3");
                    break;
                case GamePlayers.RedPlayer4:
                    GoldHaloEffect.createHightedPlayer("LeftPlayer4");
                    break;
                case GamePlayers.RedPlayer5:
                    GoldHaloEffect.createHightedPlayer("LeftPlayer5");
                    break;
                case GamePlayers.RedPlayer6:
                    GoldHaloEffect.createHightedPlayer("LeftPlayer6");
                    break;
                case GamePlayers.RedPlayer7:
                    GoldHaloEffect.createHightedPlayer("LeftPlayer7");
                    break;
                case GamePlayers.RedPlayer8:
                    GoldHaloEffect.createHightedPlayer("LeftPlayer8");
                    break;
                case GamePlayers.RedPlayer9:
                    GoldHaloEffect.createHightedPlayer("LeftPlayer9");
                    break;
                case GamePlayers.RedPlayer10:
                    GoldHaloEffect.createHightedPlayer("LeftPlayer10");
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


        if ((OVRInput.IsControllerConnected(controllerType) && OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger)) || Input.GetMouseButtonDown(1))
        {
            triggerHasBeenPressed = true;
            // TO-DO Get and store cursor world positions
            CursorWorldPositions.Add(GameObject.Find("FootballEngine").transform.TransformPoint(RaycasterCursorVisual.transform.position));
        }
        else
        {
            if (triggerHasBeenPressed && (showHeatmap || ShowArrow))
            {
                // TO-DO check if trigger has been pressed, if yes, calculate average, store, jump to another situation

                situationNumber++;


                // int minutes = Mathf.FloorToInt(timer / 60f);
                // int second = Mathf.FloorToInt(timer % 60f);

                storeAsCSV(ParticiantID, scenarioName, timer, CursorWorldPositions, CalculateAverage(CursorWorldPositions));

                if (TimeLimit)
                    timer = totalTime;
                else
                    timer = 0;

                triggerHasBeenPressed = false;
                CursorWorldPositions = new List<Vector3>();
                try
                {
                    setSituation(currentSituations[situationNumber]);
                    startTimer = false;
                    showHeatmap = false;
                    ShowArrow = false;
                }
                catch (ArgumentOutOfRangeException)
                {
                    StopPlayMode();
                }
            }

            if ((OVRInput.IsControllerConnected(controllerType) && OVRInput.Get(OVRInput.Button.One)) || Input.GetMouseButtonDown(0))  // TO-DO check if A has been pressed, if yes, show future, hide "Press A to show future" hint.
            {
                print("BUTTON ONE HAS BEEN PRESSED");
                showHintPressA = false;
                startTimer = true;
                if (temp_scenarioName.Contains("Heatmap"))
                {
                    showHeatmap = true;
                }
                else
                {
                    MovableFootball.multiFutureAmount = int.Parse(temp_scenarioName.Substring(0, 1));
                    ShowArrow = true;
                }

            }
        }

        if (startTimer)
        {
            if (TimeLimit)
            {
                print("Timer: " + timer);
                timer -= Time.deltaTime;

                if (timer <= 0.01f)
                {
                    situationNumber++;

                    storeAsCSV(ParticiantID, scenarioName, timer, CursorWorldPositions, CalculateAverage(CursorWorldPositions));

                    timer = totalTime;

                    triggerHasBeenPressed = false;
                    CursorWorldPositions = new List<Vector3>();
                    try
                    {
                        setSituation(currentSituations[situationNumber]);
                        startTimer = false;
                        showHeatmap = false;
                        ShowArrow = false;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        StopPlayMode();
                    }
                }
            }
            else
                timer += Time.deltaTime;
        }
        // else
        // {
        //     if (TimeLimit)
        //         timer = totalTime;
        //     else
        //         timer = 0;
        // }
    }


    // private void OnGUI()
    // {
    //     if (showHintPressA)
    //     {
    //         // GUI.Label(new Rect(0, 0, Screen.width, Screen.height * 0.1f), "Please Press A to Start", PressAGUIStyle);
    //         GUI.Label(new Rect(Screen.width / 2 - 400, 20, 200, 40), "Please Press A to Start and Show Future Visualization", PressAGUIStyle);
    //     }
    // }

    private void storeAsCSV(int ParticiantID, string Situation, float CompletionTime, List<Vector3> SelectedPositions, Vector3 AveragePosition)
    {
        string filePath = "C:/Users/chenk/Desktop/First_Project/User_Study/Collected_Data_CSV.csv";
        //     using (StreamWriter data = new StreamWriter(filePath, true))
        //     {
        //         data.WriteLine($"{ParticiantID},{Situation},{CompletionTime},{SelectedPositions}, {AveragePosition}");
        //     }
        Debug.Log("Participant ID: " + ParticiantID + " Situation: " + Situation + " CompletionTime: " + CompletionTime + " AveragePosition: " + AveragePosition);
    }

    private void setSituation(List<int> situation) // [Observer, Highlighted, Step_number], blue(right) player:1-10, red(left) player:11-20
    {
        string observer;
        string highlightedPlayer;
        int stepNumber;

        if (situation[0] > 10)
            observer = "LeftPlayer" + (situation[0] - 10).ToString();
        else
            observer = "RightPlayer" + situation[0].ToString();
        if (situation[1] > 10)
            highlightedPlayer = "LeftPlayer" + (situation[1] - 10).ToString();
        else
            highlightedPlayer = "RightPlayer" + situation[1].ToString();

        stepNumber = situation[2];

        MovableFootball.step_num = stepNumber;
        GoldHaloEffect.createHightedPlayer(highlightedPlayer);
        GameObject.Find("OVRCameraRig").transform.position = new Vector3(GameObject.Find(observer).transform.position.x, 0, GameObject.Find(observer).transform.position.z);
        GameObject.Find("OVRCameraRig").transform.rotation = GameObject.Find(observer).transform.rotation;

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

    private void StopPlayMode()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Debug.Log("The condition has been finished.");
#endif
    }

}
