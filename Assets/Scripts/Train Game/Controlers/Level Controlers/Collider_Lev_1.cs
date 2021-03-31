using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collider_Lev_1 : MonoBehaviour
{
    public bool FA; // First Action
    public bool SA; // Second Action

    public bool isMainWay;

    private Level1 levelControler;

    private void Start()
    {
        levelControler = transform.GetComponentInParent<Level1>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Train")
        {
            if (FA)
            {
                if (isMainWay)
                {
                    levelControler.ColFirstAction(0);
                }
                else
                {
                    levelControler.ColFirstAction(1);
                }
            }

            if (SA)
            {
                levelControler.ColSecondAction();
            }
        }
    }
}
