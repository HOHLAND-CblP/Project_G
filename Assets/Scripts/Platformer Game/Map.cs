using UnityEngine;

public class Map : MonoBehaviour
{
    GameObject cam;

    private void Start()
    {
        cam = GameObject.Find("Main Camera");
    }

    public void GoHouse()
    {
        GamePrefs.inDialog = false;
        GamePrefs.id = 0;
        GamePrefs.inout = 2;
        cam.GetComponent<TrackingTheHero>().faded.SetActive(true);
    }

    public void GoStation()
    {
        GamePrefs.inDialog = false;
        GamePrefs.id = 1;
        GamePrefs.inout = 2;
        cam.GetComponent<TrackingTheHero>().faded.SetActive(true);
    }

    public void GoShop()
    {
        GamePrefs.inDialog = false;
        GamePrefs.id = 2;
        GamePrefs.inout = 2;
        cam.GetComponent<TrackingTheHero>().faded.SetActive(true);
    }

    public void GoHospital()
    {
        GamePrefs.inDialog = false;
        GamePrefs.id = 3;
        GamePrefs.inout = 2;
        cam.GetComponent<TrackingTheHero>().faded.SetActive(true);
    }

    public void GoHotel()
    {
        GamePrefs.inDialog = false;
        GamePrefs.id = 4;
        GamePrefs.inout = 2;
        cam.GetComponent<TrackingTheHero>().faded.SetActive(true);
    }

    public void GoSaloon()
    {
        GamePrefs.inDialog = false;
        GamePrefs.id = 5;
        GamePrefs.inout = 2;
        cam.GetComponent<TrackingTheHero>().faded.SetActive(true);
    }
}
