using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dataset : ScriptableObject
{
    private static List<int> stepNumWithFuture = new List<int>();
    public static int stepToCaulate;
    public static int fixedGameDuration;
    public static List<List<int>> No_Time_Limit_2_Arrows_Trials = new List<List<int>>();
    public static List<List<int>> No_Time_Limit_5_Arrows_Trials = new List<List<int>>();
    public static List<List<int>> No_Time_Limit_Heatmap_Trials = new List<List<int>>();
    public static List<List<int>> Yes_Time_Limit_2_Arrows_Trials = new List<List<int>>();
    public static List<List<int>> Yes_Time_Limit_5_Arrows_Trials = new List<List<int>>();
    public static List<List<int>> Yes_Time_Limit_Heatmap_Trials = new List<List<int>>();

    // public static List<List<int>> ShuffledTrials = new List<List<int>>();

    public static void startDataset()
    {
        /*** Step Num of Situations in Original Game: [Observer, Highlighted, Step_number], blue(right) player:1-10, red(left) player:11-20

        Dataset (Situation) dimension:
        1. Distance between observer and target: Far (10m-20m) and Close (0m-10m)
        2. Scattered level of target's multi-future: Dense (0d-90d) and Scattered (90d-180d)
        3. Density around the target within 10m: Dense (>2) and Scattered (<=2)

        So we will have 8 permutation sets of (Distance, Scattered level of target's multi-future, Density around the target within 10m):
        (Far, Dense, Dense)
        (Far, Dense, Scattered)
        (Far, Scattered, Dense)
        (Far, Scattered, Scattered)
        (Close, Dense, Dense)
        (Close, Dense, Scattered)
        (Close, Scattered, Dense)
        (Close, Scattered, Scattered)

        Specs:
        1. Each condition (6 in total): No_2_A, N_5_A, N_Hm, Y_2_A, Y_5_A, Y_Hm contains 8*5 = 40 trials. (8 permutation sets, 5 trials for each permutation set)
        2. We will have different 5*8*6=240 situations in total for each participant. They are expected to complete all situations in 40 minutes.
        3. Each permutation set has 5 trials. Each participant will complete 30 trials for each permutation set.

        ***/

        No_Time_Limit_2_Arrows_Trials.Add(new List<int> { 1, 2, 0, 0 });
        No_Time_Limit_2_Arrows_Trials.Add(new List<int> { 3, 1, 0, 1 });
        No_Time_Limit_2_Arrows_Trials.Add(new List<int> { 10, 12, 0, 2 });
        No_Time_Limit_5_Arrows_Trials.Add(new List<int> { 1, 2, 0, 0 });
        No_Time_Limit_5_Arrows_Trials.Add(new List<int> { 3, 1, 0, 1 });
        No_Time_Limit_5_Arrows_Trials.Add(new List<int> { 10, 12, 0, 2 });
        No_Time_Limit_Heatmap_Trials.Add(new List<int> { 1, 2, 0, 0 });
        No_Time_Limit_Heatmap_Trials.Add(new List<int> { 3, 1, 0, 1 });
        No_Time_Limit_Heatmap_Trials.Add(new List<int> { 10, 12, 0, 2 });

        Yes_Time_Limit_2_Arrows_Trials.Add(new List<int> { 1, 2, 0, 0 });
        Yes_Time_Limit_2_Arrows_Trials.Add(new List<int> { 3, 1, 0, 1 });
        Yes_Time_Limit_2_Arrows_Trials.Add(new List<int> { 10, 12, 0, 2 });
        Yes_Time_Limit_5_Arrows_Trials.Add(new List<int> { 1, 2, 0, 0 });
        Yes_Time_Limit_5_Arrows_Trials.Add(new List<int> { 3, 1, 0, 1 });
        Yes_Time_Limit_5_Arrows_Trials.Add(new List<int> { 10, 12, 0, 2 });
        Yes_Time_Limit_Heatmap_Trials.Add(new List<int> { 1, 2, 0, 0 });
        Yes_Time_Limit_Heatmap_Trials.Add(new List<int> { 3, 1, 0, 1 });
        Yes_Time_Limit_Heatmap_Trials.Add(new List<int> { 10, 12, 0, 2 });


        // (Far, Dense, Dense)
        // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 4, 6, 2560 });
        // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 8, 11, 0 });
        // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 4, 17, 2050 });
        // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 17, 7, 1030 });
        // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 4, 6, 520 });
        // // (Far, Dense, Scattered)
        // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 4, 1, 2570 });
        // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 12, 18, 2060 });
        // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 8, 3, 1040 });
        // // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 14, 8, 530 });
        // // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 11, 6, 20 });
        // // (Far, Scattered, Dense)
        // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 3, 10, 1980 });
        // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 10, 9, 1560 });
        // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 4, 6, 1260 });
        // // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 5, 11, 540 });
        // // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 11, 15, 30 });
        // // (Far, Scattered, Scattered)
        // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 1, 6, 1200 });
        // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 1, 6, 690 }); // here
        // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 10, 8, 1570 });
        // // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 18, 12, 1060 });
        // // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 10, 8, 40 });
        // // (Close, Dense, Dense)
        // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 6, 10, 2600 });
        // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 16, 4, 2090 });
        // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 8, 14, 1580 });
        // // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 8, 17, 1070 });
        // // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 13, 9, 560 });
        // // (Close, Dense, Scattered)
        // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 12, 20, 50 });
        // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 11, 3, 2610 });
        // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 1, 9, 2100 });
        // // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 19, 2, 1590 });
        // // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 4, 3, 1080 });
        // // (Close, Scattered, Dense)
        // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 18, 2, 2620 });
        // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 18, 9, 2110 });
        // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 6, 11, 1600 });
        // // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 7, 19, 1090 });
        // // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 13, 2, 580 });
        // // (Close, Scattered, Scattered)
        // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 6, 20, 1620 });
        // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 11, 7, 1110 });
        // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 10, 19, 600 });
        // // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 10, 7, 2650 });
        // // No_Time_Limit_2_Arrows_Situations.Add(new List<int> { 8, 6, 90 });

        // // (Far, Dense, Dense)
        // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 11, 10, 2120 });
        // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 11, 18, 60 });
        // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 19, 12, 10 });
        // // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 11, 5, 2140 });
        // // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 4, 5, 1120 });
        // // (Far, Dense, Scattered)
        // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 5, 18, 610 });
        // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 15, 17, 100 });
        // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 7, 13, 2660 });
        // // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 4, 20, 2150 });
        // // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 17, 11, 1130 });
        // // (Far, Scattered, Dense)
        // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 15, 20, 2670 });
        // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 8, 14, 110 });
        // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 9, 13, 2160 });
        // // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 10, 15, 630 });
        // // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 9, 11, 2680 });
        // // (Far, Scattered, Scattered)
        // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 18, 17, 2170 });
        // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 14, 4, 1660 });
        // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 5, 12, 1150 });
        // // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 18, 16, 640 });
        // // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 17, 11, 2690 });
        // // (Close, Dense, Dense)
        // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 2, 19, 130 });
        // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 17, 19, 2180 });
        // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 11, 9, 1670 });
        // // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 9, 5, 1160 });
        // // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 10, 7, 2190 });
        // // (Close, Dense, Scattered)
        // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 8, 5, 1680 });
        // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 2, 12, 1170 });
        // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 7, 2, 150 });
        // // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 3, 7, 2710 });
        // // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 16, 18, 2200 });
        // // (Close, Scattered, Dense)
        // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 11, 10, 2120 });
        // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 3, 6, 1690 });
        // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 18, 16, 1180 });
        // // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 18, 2, 670 });
        // // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 8, 17, 160 });
        // // (Close, Scattered, Scattered)
        // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 7, 10, 2210 });
        // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 12, 13, 1700 });
        // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 17, 12, 1190 });
        // // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 3, 12, 680 });
        // // No_Time_Limit_5_Arrows_Situations.Add(new List<int> { 18, 15, 2730 });

        // // (Far, Dense, Dense)
        // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 19, 20, 2300 });
        // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 18, 12, 1710 });
        // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 5, 15, 2740 });
        // // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 19, 13, 180 });
        // // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 3, 2, 2230 });
        // // (Far, Dense, Scattered)
        // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 14, 12, 1720 });
        // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 14, 20, 1210 });
        // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 11, 16, 700 });
        // // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 13, 12, 190 });
        // // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 3, 7, 2750 });
        // // // (Far, Scattered, Dense)
        // // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 13, 19, 2240 });
        // // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 5, 9, 1730 });
        // // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 5, 2, 1220 });
        // // // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 18, 13, 2760 });
        // // // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 15, 5, 2250 });
        // // // (Far, Scattered, Scattered)
        // // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 18, 9, 1740 });
        // // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 20, 18, 1230 });
        // // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 2, 1, 2770 });
        // // // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 5, 12, 210 });
        // // // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 11, 13, 2260 });
        // // // (Close, Dense, Dense)
        // // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 17, 6, 1750 });
        // // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 14, 4, 730 });
        // // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 18, 17, 220 });
        // // // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 19, 8, 2780 });
        // // // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 4, 11, 1760 });
        // // // (Close, Dense, Scattered)
        // // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 2, 14, 1250 });
        // // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 20, 9, 230 });
        // // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 11, 14, 1260 });
        // // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 17, 3, 750 });
        // // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 11, 16, 240 });
        // // (Close, Scattered, Dense)
        // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 6, 10, 2800 });
        // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 15, 12, 2290 });
        // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 4, 9, 760 });
        // // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 3, 1, 2810 });
        // // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 12, 6, 250 });
        // // (Close, Scattered, Scattered)
        // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 18, 7, 1790 });
        // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 1, 3, 1280 });
        // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 8, 13, 260 });
        // // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 3, 10, 2820 });
        // // No_Time_Limit_Heatmap_Situations.Add(new List<int> { 3, 8, 2310 });




        // // (Far, Dense, Dense)
        // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 9, 18, 1800 });
        // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 19, 3, 1290 });
        // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 1, 8, 780 });
        // // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 12, 15, 2830 });
        // // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 3, 4, 270 });
        // // (Far, Dense, Scattered)
        // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 3, 2, 2320 });
        // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 16, 7, 1810 });
        // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 19, 4, 1300 });
        // // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 13, 11, 790 });
        // // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 12, 11, 2840 });
        // // (Far, Scattered, Dense)
        // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 12, 7, 280 });
        // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 3, 8, 2330 });
        // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 11, 7, 1820 });
        // // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 3, 6, 1310 });
        // // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 13, 17, 290 });
        // // (Far, Scattered, Scattered)
        // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 6, 2, 2850 });
        // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 4, 20, 1320 });
        // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 16, 7, 810 });
        // // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 7, 3, 2350 });
        // // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 15, 10, 1840 });
        // // (Close, Dense, Dense)
        // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 8, 11, 1330 });
        // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 12, 10, 820 });
        // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 12, 16, 310 });
        // // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 3, 16, 2870 });
        // // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 5, 17, 2360 });
        // // (Close, Dense, Scattered)
        // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 2, 19, 1850 });
        // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 1, 20, 1340 });
        // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 3, 1, 830 });
        // // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 1, 6, 2880 });
        // // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 10, 12, 320 });
        // // (Close, Scattered, Dense)
        // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 10, 4, 2370 });
        // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 16, 12, 1350 });
        // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 15, 4, 840 });
        // // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 4, 8, 2890 });
        // // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 19, 6, 1879 });
        // // (Close, Scattered, Scattered)
        // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 16, 12, 1360 });
        // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 1, 12, 340 });
        // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 3, 5, 1370 });
        // // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 2, 12, 860 });
        // // Yes_Time_Limit_2_Arrows_Situations.Add(new List<int> { 19, 3, 2910 });




        // // (Far, Dense, Dense)
        // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 3, 20, 350 });
        // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 16, 8, 1890 });
        // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 10, 1, 1380 });
        // // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 4, 13, 870 });
        // // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 5, 9, 360 });
        // // (Far, Dense, Scattered)
        // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 18, 9, 2920 });
        // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 10, 8, 1900 });
        // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 18, 20, 1390 });
        // // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 4, 13, 880 });
        // // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 14, 7, 2930 });
        // // (Far, Scattered, Dense)
        // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 17, 19, 370 });
        // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 4, 3, 2420 });
        // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 20, 19, 2400 });
        // // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 20, 9, 1400 });
        // // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 16, 20, 890 });
        // // (Far, Scattered, Scattered)
        // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 4, 6, 380 });
        // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 11, 3, 2940 });
        // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 10, 11, 2430 });
        // // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 6, 15, 1920 });
        // // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 9, 12, 1410 });
        // // (Close, Dense, Dense)
        // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 8, 10, 900 });
        // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 20, 18, 390 });
        // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 11, 13, 2440 });
        // // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 3, 11, 1930 });
        // // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 4, 2, 910 });
        // // (Close, Dense, Scattered)
        // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 2, 20, 2450 });
        // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 14, 5, 1430 });
        // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 4, 7, 2970 });
        // // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 15, 11, 410 });
        // // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 4, 5, 2460 });
        // // (Close, Scattered, Dense)
        // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 10, 12, 1440 });
        // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 7, 8, 930 });
        // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 3, 15, 420 });
        // // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 10, 16, 2980 });
        // // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 7, 15, 2470 });
        // // (Close, Scattered, Scattered)
        // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 12, 16, 1960 });
        // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 5, 1, 940 });
        // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 19, 16, 430 });
        // // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 3, 9, 2990 });
        // // Yes_Time_Limit_5_Arrows_Situations.Add(new List<int> { 7, 14, 2480 });



        // // (Far, Dense, Dense)
        // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 13, 2, 1970 });
        // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 9, 5, 1460 });
        // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 6, 7, 3000 });
        // // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 5, 1, 2490 });
        // // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 18, 8, 1980 });
        // // (Far, Dense, Scattered)
        // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 9, 20, 1470 });
        // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 3, 2, 960 });
        // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 11, 7, 450 });
        // // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 4, 12, 1990 });
        // // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 13, 10, 1480 });
        // // // (Far, Scattered, Dense)
        // // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 20, 9, 970 });
        // // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 4, 11, 460 });
        // // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 18, 16, 2510 });
        // // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 14, 7, 2000 });
        // // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 11, 8, 1490 });
        // // // (Far, Scattered, Scattered)
        // // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 15, 9, 980 });
        // // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 10, 6, 470 });
        // // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 11, 2, 2520 });
        // // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 3, 8, 2010 });
        // // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 6, 10, 1500 });
        // // (Close, Dense, Dense)
        // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 15, 17, 990 });
        // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 3, 16, 480 });
        // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 9, 10, 2530 });
        // // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 14, 8, 1510 });
        // // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 6, 10, 1000 });
        // // (Close, Dense, Scattered)
        // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 1, 9, 2540 });
        // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 6, 14, 2030 });
        // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 4, 14, 500 });
        // // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 8, 10, 2040 });
        // // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 19, 14, 510 });
        // // // (Close, Scattered, Dense)
        // // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 3, 6, 1610 });
        // // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 13, 9, 1100 });
        // // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 12, 18, 590 });
        // // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 1, 12, 2640 });
        // // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 4, 13, 2130 });
        // // // (Close, Scattered, Scattered)
        // // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 1, 5, 2900 });
        // // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 17, 1, 1020 });
        // // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 10, 18, 1530 });
        // // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 13, 2, 2720 });
        // // Yes_Time_Limit_Heatmap_Situations.Add(new List<int> { 14, 7, 1450 });
    }

    public static void shuffleDataset(List<List<int>> condition)
    {
        int n = condition.Count;

        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            List<int> temp = condition[k];
            condition[k] = condition[n];
            condition[n] = temp;
        }
    }

}
