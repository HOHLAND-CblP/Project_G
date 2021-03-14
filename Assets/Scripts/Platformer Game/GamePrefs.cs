using UnityEngine;
public static class GamePrefs
{
    public static int inout = -1;
    public static int id = -1;
    public static int amountOfFood = 1;
    public static int[] countsOfKindOfFood = { 0, 0, 1, 0, 0, 0 };
    public static int amountOfHealth = 0;
    public static int[] countsOfKindOfHealth = { 2, 0, 0 };
    public static int countOfDialog = 0;
    public static int chapter = 1;
    public static int day = 2;
    public static int chooseButton = -1;
    public static int runnerLevel;
    public static int countOfPlotMoment = 0;
    public static int countOfPlots = 1;

    public static GameObject currentLevel;
    public static GameObject currentRunnerLevel;

    public static double discount = 0;

    public static bool inDialog = false;
    public static bool headAche = false;
    public static bool usbFLASH = false;

    public static string nameForMainNote = "";
    public static string textForMainNote = "";

    public static string nameForAdd1Note = "";
    public static string textForAdd1Note = "";

    public static string nameForAdd2Note = "";
    public static string textForAdd2Note = "";

    public static bool prologCrutch1 = false;
    public static bool prologCrutch2 = false;
}
