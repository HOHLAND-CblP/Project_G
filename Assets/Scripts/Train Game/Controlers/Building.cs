 using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Building : MonoBehaviour
{
    public Vector2Int Size;
    public GameObject cellBackground;

    private void Awake()
    {
        Camera.main.GetComponent<GameControler>().AddBuildings(gameObject);
    }


    public void ClickOnTheCell()
    {
        if (GetComponent<ArrowRailway>())
        {
            GetComponent<ArrowRailway>().ChangeOfDirectionArrow();
        }
    }

    private void Start()
    {
        transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0);
    }
}