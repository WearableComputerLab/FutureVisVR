using System.Globalization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// using Newtonsoft.Json;

public class MultiFuture : MonoBehaviour
{
    public static int futureNumber = 0;
    public static Dictionary<string, List<Dictionary<string, List<List<List<float>>>>>> stepMultiFuture;
    public static int gameDuration;
    public static List<GameObject> listCopyEventPlayer = new List<GameObject>();

    private static int temCurrentStep = -1;
    private static bool tempShowFuture = false;

    private static int EachPlayerNumber;

    public static void startFutureInfo(Dictionary<string, List<Dictionary<string, List<List<List<float>>>>>> _stepMultiFuture, int _gameDuration)
    {
        /***
            1. Receive New_Team from MovableFootball
            2. Calculate the number of future number
        ***/

        /*** Assign New_Team to New_Team of current class ***/
        stepMultiFuture = _stepMultiFuture;
        gameDuration = _gameDuration;

        /*** Create LineRenderer Object ***/
        GameObject emptyLineRenderer = new GameObject("EmptyLineRenderer");

        foreach (var eachStepMultiFuture in stepMultiFuture)
        {
            foreach (var eachFuture in eachStepMultiFuture.Value)
            {
                futureNumber++;
                foreach (var infoPair in eachFuture)
                {
                    /*** Create LineRenderer GameObject and attach LineRenderer component***/
                    if (infoPair.Key.Contains("TeamLocations"))
                    {
                        String rPlayer = infoPair.Key.Replace("TeamLocations", "Player"); /*** "Right" or "Left" ***/
                        String mPlayer = "m" + rPlayer;

                        EachPlayerNumber = infoPair.Value.Count;

                        for (int playerId = 0; playerId < infoPair.Value.Count; playerId++)
                        {
                            createLineRenderer(rPlayer + playerId.ToString(), emptyLineRenderer, futureNumber);
                            createLineRenderer(mPlayer + playerId.ToString(), emptyLineRenderer, futureNumber);
                        }
                    }

                    /*** Create a list of copyEventplayer, each copyEventPlayer in each future***/
                    listCopyEventPlayer.Add(new GameObject());
                }
            }
            print("future number: " + futureNumber);
            break;
        }
    }
    public static void updateFutureInfo(int currentStep, int farFuture, bool futureVisShow)
    {
        int futureNo = 0;
        int playerNumber = 0;

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
        // if (!futureVisShow)
        // {
        //     tempShowFuture = 0;
        // }
        if ((temCurrentStep != currentStep)) // || (futureVisShow && tempShowFuture == 0)
        {
            temCurrentStep = currentStep;
            // tempShowFuture = futureVisShow;

            foreach (var stepFuturesPair in stepMultiFuture)
            {
                if (stepFuturesPair.Key.Replace("Step", String.Empty).Equals(currentStep.ToString()))
                {
                    // print("Current Step: " + currentStep);
                    foreach (var eachFuture in stepFuturesPair.Value)
                    {
                        futureNo++;
                        List<Dictionary<int, Vector3>> passEventPositions = new List<Dictionary<int, Vector3>>();
                        List<Dictionary<int, Vector3>> shotEventPositions = new List<Dictionary<int, Vector3>>();

                        String eventPlayer = null;

                        foreach (var infoPair in eachFuture)
                        {
                            // if (infoPair.Key.Contains("RightTeamLocations"))
                            // print("infoPair key TeamLocations: " + infoPair.Key + "; if contains TeamLocations: " + infoPair.Key.Contains("TeamLocations"));
                            if (infoPair.Key.Contains("TeamLocations"))
                            {
                                String rPlayer = infoPair.Key.Replace("TeamLocations", "Player"); /*** "Right" or "Left" ***/
                                String mPlayer = "m" + rPlayer;

                                for (int playerId = 0; playerId < infoPair.Value.Count; playerId++)
                                {
                                    playerNumber++;
                                    List<Vector3> rPlayerPathPosition = new List<Vector3>();
                                    List<Vector3> mPlayerPathPosition = new List<Vector3>();

                                    // for (int step = currentStep; step < ((currentStep + farFuture) < gameDuration ? (currentStep + farFuture) : (gameDuration - currentStep)); step++)

                                    for (int step = 0; step < gameDuration; step++)
                                    {
                                        Vector3 rPlayerPosition = new Vector3(MovableFootball.scale_x(infoPair.Value[playerId][step][0]) / 1, -1.55f, MovableFootball.scale_z(infoPair.Value[playerId][step][1]) / 1);
                                        Vector3 mPlayerPosition = new Vector3(MovableFootball.scale_x(infoPair.Value[playerId][step][0]) / MovableFootball.scaleSize, -0.195f, MovableFootball.scale_z(infoPair.Value[playerId][step][1]) / MovableFootball.scaleSize);
                                        // Vector3 mPlayerPosition = new Vector3(MovableFootball.scale_x(infoPair.Value[playerId][step][0]) / MovableFootball.scaleSize, GameObject.Find(mPlayer).transform.position.y - 0.005f, MovableFootball.scale_z(infoPair.Value[playerId][step][1]) / MovableFootball.scaleSize);

                                        Vector3 mLocalPosition = GameObject.Find("MovableMiniature").transform.TransformPoint(mPlayerPosition);

                                        /*** Path ***/
                                        rPlayerPathPosition.Add(rPlayerPosition);
                                        // mPlayerPathPosition.Add(mPlayerPosition);
                                        mPlayerPathPosition.Add(mLocalPosition);

                                        /*** Event ***/
                                        float leftPlayer = eachFuture["Event"][step][0][0];
                                        float rightPlayer = eachFuture["Event"][step][1][0];
                                        float leftAction = eachFuture["Event"][step][0][1];
                                        float rightAction = eachFuture["Event"][step][1][1];

                                        if (leftPlayer.Equals(playerId.ToString()) || rightPlayer.Equals(playerId.ToString()))
                                        {
                                            /*** PlayerId with Event ***/
                                            if (leftPlayer.Equals(playerId.ToString()))
                                                eventPlayer = "LeftPlayer" + leftPlayer.ToString();
                                            else if (rightPlayer.Equals(playerId.ToString()))
                                                eventPlayer = "rightPlayer" + leftPlayer.ToString();

                                            if ((9 <= leftAction && leftAction <= 11) || (9 <= rightAction && rightAction <= 11))
                                            {
                                                Destroy(listCopyEventPlayer[futureNo]);
                                                Dictionary<int, Vector3> tempPosition = new Dictionary<int, Vector3>();
                                                tempPosition.Add(step, rPlayerPosition);
                                                passEventPositions.Add(tempPosition);
                                            }
                                            else if (leftAction == 12 || rightAction == 12)
                                            {
                                                Destroy(listCopyEventPlayer[futureNo]);

                                                Dictionary<int, Vector3> tempPosition = new Dictionary<int, Vector3>();
                                                tempPosition.Add(step, rPlayerPosition);
                                                shotEventPositions.Add(tempPosition);
                                            }
                                        }
                                    }

                                    lineDraw(rPlayer + playerId.ToString() + "LineRenderer" + futureNo, farFuture, rPlayerPathPosition.ToArray());
                                    lineDraw(mPlayer + playerId.ToString() + "LineRenderer" + futureNo, farFuture, mPlayerPathPosition.ToArray());
                                    // visFuturePath(rPlayer + playerId.ToString() + "LineRenderer" + futureNo, futureVisShow);
                                    // visFuturePath(mPlayer + playerId.ToString() + "LineRenderer" + futureNo, futureVisShow);

                                }
                            }

                            if (passEventPositions.Count != 0 && eventPlayer != null)
                            {
                                createEventPlayer(passEventPositions, currentStep, eventPlayer, listCopyEventPlayer[futureNo]);
                            }
                            else if (shotEventPositions.Count != 0 && eventPlayer != null)
                            {
                                createEventPlayer(shotEventPositions, currentStep, eventPlayer, listCopyEventPlayer[futureNo]);
                            }


                        }
                    }
                }
            }
        }

        if (tempShowFuture != futureVisShow)
        {
            tempShowFuture = futureVisShow;

            print("futureNo:" + futureNo);

            String rRightPlayer = "RightPlayer";
            String mRightPlayer = "m" + rRightPlayer;
            String rLeftPlayer = "LeftPlayer";
            String mLeftPlayer = "m" + rLeftPlayer;
            for (int playerId = 0; playerId < EachPlayerNumber; playerId++)
            {
                for (int eachFutureNumber = 1; eachFutureNumber <= futureNumber; eachFutureNumber++)  // start from RightPlayer0LineRenderer_1
                // for (int eachFutureNumber = 1; eachFutureNumber <= MovableFootball.multiFutureAmount; eachFutureNumber++)  // start from RightPlayer0LineRenderer_1
                {
                    visFuturePath(rRightPlayer + playerId.ToString() + "LineRenderer" + eachFutureNumber, futureVisShow);
                    visFuturePath(mRightPlayer + playerId.ToString() + "LineRenderer" + eachFutureNumber, futureVisShow);
                    visFuturePath(rLeftPlayer + playerId.ToString() + "LineRenderer" + eachFutureNumber, futureVisShow);
                    visFuturePath(mLeftPlayer + playerId.ToString() + "LineRenderer" + eachFutureNumber, futureVisShow);
                }
            }
        }
    }

    public static void updateFutureAmountFar(int currentStep, int farFuture, float futureDetails, bool futureVisShow)
    {
        int futureNo = 0;
        int playerNumber = 0;
        foreach (var stepFuturesPair in stepMultiFuture)
        {
            if (stepFuturesPair.Key.Replace("Step", String.Empty).Equals(currentStep.ToString()))
            {
                // print("Current Step: " + currentStep);
                foreach (var eachFuture in stepFuturesPair.Value)
                {
                    futureNo++;
                    List<Dictionary<int, Vector3>> passEventPositions = new List<Dictionary<int, Vector3>>();
                    List<Dictionary<int, Vector3>> shotEventPositions = new List<Dictionary<int, Vector3>>();
                    foreach (var infoPair in eachFuture)
                    {
                        // if (infoPair.Key.Contains("RightTeamLocations"))
                        // print("infoPair key TeamLocations: " + infoPair.Key + "; if contains TeamLocations: " + infoPair.Key.Contains("TeamLocations"));
                        if (infoPair.Key.Contains("TeamLocations"))
                        {
                            String rPlayer = infoPair.Key.Replace("TeamLocations", "Player"); /*** "Right" or "Left" ***/
                            String mPlayer = "m" + rPlayer;
                            for (int playerId = 0; playerId < infoPair.Value.Count; playerId++)
                            {
                                playerNumber++;
                                List<Vector3> rPlayerPathPosition = new List<Vector3>();
                                List<Vector3> mPlayerPathPosition = new List<Vector3>();
                                for (int step = 0; step < MovableFootball.multiFutureFar; step++)
                                {
                                    Vector3 rPlayerPosition = new Vector3(MovableFootball.scale_x(infoPair.Value[playerId][step][0]) / 1, -1.55f, MovableFootball.scale_z(infoPair.Value[playerId][step][1]) / 1);
                                    Vector3 mPlayerPosition = new Vector3(MovableFootball.scale_x(infoPair.Value[playerId][step][0]) / MovableFootball.scaleSize, -0.195f, MovableFootball.scale_z(infoPair.Value[playerId][step][1]) / MovableFootball.scaleSize);

                                    Vector3 mLocalPosition = GameObject.Find("MovableMiniature").transform.TransformPoint(mPlayerPosition);
                                    /*** Path ***/
                                    rPlayerPathPosition.Add(rPlayerPosition);
                                    // mPlayerPathPosition.Add(mPlayerPosition);
                                    mPlayerPathPosition.Add(mLocalPosition);
                                }
                                lineDraw(rPlayer + playerId.ToString() + "LineRenderer" + futureNo, farFuture, futureDetails, rPlayerPathPosition.ToArray());
                                lineDraw(mPlayer + playerId.ToString() + "LineRenderer" + futureNo, farFuture, futureDetails, mPlayerPathPosition.ToArray());
                            }
                        }
                    }
                }
            }
        }
        String rRightPlayer = "RightPlayer";
        String mRightPlayer = "m" + rRightPlayer;
        String rLeftPlayer = "LeftPlayer";
        String mLeftPlayer = "m" + rLeftPlayer;
        for (int playerId = 0; playerId < EachPlayerNumber; playerId++)
        {
            if (futureVisShow)
            {
                if (MovableFootball.multiFutureAmount == futureNumber)
                {
                    for (int eachFutureNumber = 1; eachFutureNumber <= futureNumber; eachFutureNumber++)  // start from RightPlayer0LineRenderer_1
                    {
                        visFuturePath(rRightPlayer + playerId.ToString() + "LineRenderer" + eachFutureNumber, futureVisShow);
                        visFuturePath(mRightPlayer + playerId.ToString() + "LineRenderer" + eachFutureNumber, futureVisShow);
                        visFuturePath(rLeftPlayer + playerId.ToString() + "LineRenderer" + eachFutureNumber, futureVisShow);
                        visFuturePath(mLeftPlayer + playerId.ToString() + "LineRenderer" + eachFutureNumber, futureVisShow);
                    }
                }
                if (MovableFootball.multiFutureAmount < futureNumber)
                {
                    for (int eachFutureNumber = 1; eachFutureNumber <= MovableFootball.multiFutureAmount; eachFutureNumber++)  // start from RightPlayer0LineRenderer_1
                    {
                        visFuturePath(rRightPlayer + playerId.ToString() + "LineRenderer" + eachFutureNumber, true);
                        visFuturePath(mRightPlayer + playerId.ToString() + "LineRenderer" + eachFutureNumber, true);
                        visFuturePath(rLeftPlayer + playerId.ToString() + "LineRenderer" + eachFutureNumber, true);
                        visFuturePath(mLeftPlayer + playerId.ToString() + "LineRenderer" + eachFutureNumber, true);
                    }
                    for (int eachFutureNumber = MovableFootball.multiFutureAmount + 1; eachFutureNumber <= futureNumber; eachFutureNumber++)  // start from RightPlayer0LineRenderer_1
                    {
                        visFuturePath(rRightPlayer + playerId.ToString() + "LineRenderer" + eachFutureNumber, false);
                        visFuturePath(mRightPlayer + playerId.ToString() + "LineRenderer" + eachFutureNumber, false);
                        visFuturePath(rLeftPlayer + playerId.ToString() + "LineRenderer" + eachFutureNumber, false);
                        visFuturePath(mLeftPlayer + playerId.ToString() + "LineRenderer" + eachFutureNumber, false);
                    }
                }
            }
            else
            {
                for (int eachFutureNumber = 1; eachFutureNumber <= futureNumber; eachFutureNumber++)  // start from RightPlayer0LineRenderer_1
                {
                    visFuturePath(rRightPlayer + playerId.ToString() + "LineRenderer" + eachFutureNumber, futureVisShow);
                    visFuturePath(mRightPlayer + playerId.ToString() + "LineRenderer" + eachFutureNumber, futureVisShow);
                    visFuturePath(rLeftPlayer + playerId.ToString() + "LineRenderer" + eachFutureNumber, futureVisShow);
                    visFuturePath(mLeftPlayer + playerId.ToString() + "LineRenderer" + eachFutureNumber, futureVisShow);
                }
            }
        }
    }

    public static void createEventPlayer(List<Dictionary<int, Vector3>> EventPositions, int currentStep, String eventPlayer, GameObject copyEventPlayer)
    {
        copyEventPlayer = Instantiate(GameObject.Find(eventPlayer), EventPositions.Last().First().Value, Quaternion.identity);
        copyEventPlayer.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

        if (copyEventPlayer.GetComponent<LineRenderer>() != null ||
            copyEventPlayer.GetComponent<CapsuleCollider>() != null
            // || copyEventPlayer.GetComponent<EyeFuture>() != null
            )
        {
            Destroy(copyEventPlayer.GetComponent<LineRenderer>());
            Destroy(copyEventPlayer.GetComponent<CapsuleCollider>());
            // Destroy(copyEventPlayer.GetComponent<EyeFuture>());
        }
        if (currentStep >= EventPositions.Last().First().Key - 1)
            Destroy(copyEventPlayer);
    }


    public static void visFuturePath(String Player, bool show)
    {
        if (GameObject.Find(Player).GetComponent<LineRenderer>() != null)
        {
            LineRenderer line = GameObject.Find(Player).GetComponent<LineRenderer>();
            line.enabled = show;
        }
    }

    public static void visFutureEvent()
    {

    }


    public static void createLineRenderer(String objectName, GameObject emptyLineRenderer, int futureNumber)
    {
        /***
            1. LineRenderer game object name example with two future possibilities:
                "Player"
                    "RightPlayer1LineRenderer1"
                    "RightPlayer1LineRenderer2"
                "mPlayer"
                    "mRightPlayer1LineRenderer1"
                    "mRightPlayer1LineRenderer2"
            2. Each LineRenderer game object contains one LineRender component.
        ***/

        GameObject Player = GameObject.Find(objectName);
        GameObject lr = Instantiate(emptyLineRenderer);
        lr.name = objectName + "LineRenderer" + futureNumber.ToString();
        lr.transform.SetParent(Player.transform, true);
        lr.AddComponent<LineRenderer>();
        LineRenderer line = lr.GetComponent<LineRenderer>();
        line.enabled = false;
    }


    public static void lineDraw(String playerName, int farFuture, Vector3[] positions)
    {
        LineRenderer line = GameObject.Find(playerName).GetComponent<LineRenderer>();
        line.positionCount = farFuture;
        Material lineMaterial = Resources.Load<Material>("Material/Arrow");
        lineMaterial.mainTextureScale = new Vector2(farFuture / 2, 1);

        line.material = lineMaterial;
        if (playerName.Contains("m"))
        {
            line.startWidth = 0.01f;
            line.endWidth = 0.01f;
        }
        else
        {
            line.startWidth = 0.5f;
            line.endWidth = 0.5f;
        }
        line.SetPositions(positions);
    }
    public static void lineDraw(String playerName, int farFuture, float futureDetails, Vector3[] positions)
    {
        LineRenderer line = GameObject.Find(playerName).GetComponent<LineRenderer>();
        line.positionCount = farFuture;
        Material lineMaterial = Resources.Load<Material>("Material/Arrow");
        lineMaterial.mainTextureScale = new Vector2(farFuture * futureDetails, 1);

        line.material = lineMaterial;
        if (playerName.Contains("m"))
        {
            line.startWidth = 0.01f;
            line.endWidth = 0.01f;
        }
        else
        {
            line.startWidth = 0.5f;
            line.endWidth = 0.5f;
        }
        line.SetPositions(positions);
    }


    public static void futurePathVis(string rPlayer, bool show)
    {
        if (GameObject.Find(rPlayer).GetComponent<LineRenderer>() != null)
        {
            LineRenderer line = GameObject.Find(rPlayer).GetComponent<LineRenderer>();
            line.enabled = show;
        }
    }
}

