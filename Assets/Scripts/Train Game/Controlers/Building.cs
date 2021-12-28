 using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    [HideInInspector]
    public GameObject cellBackground;

    [Header("Can You Destroy This Building")]
    [SerializeField]
    bool canDestroy;

    private void Awake()
    {
        Camera.main.GetComponent<GameControler>().AddBuildings(gameObject);
    }


    public void ClickOnTheCell()
    {
        if (GetComponent<ArrowRailway>())
        {
            GetComponent<ArrowRailway>().ChangeOfDirectionArrow();
            return;
        }
        if (GetComponent<Factory>())
        {
            GetComponent<Factory>().ClickOnCell();
        }
    }

    private void Start()
    {
        transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0);
    }

    public bool CanDestroy()
    {
        return canDestroy;
    }
}