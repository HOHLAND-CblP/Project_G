using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerButton : MonoBehaviour
{
    ArrowRailway arrow;

    private void Start()
    {
        arrow = transform.parent.GetComponent<ArrowRailway>();
    }


    private void Update()
    {
/*        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            if(Mathf.Abs(mousePos.x - transform.position.x) <= 0.5f && Mathf.Abs(mousePos.y - transform.position.y) <= 0.5f)
                arrow.ChangeOfDirectionArrow();
        }*/
    }

}
