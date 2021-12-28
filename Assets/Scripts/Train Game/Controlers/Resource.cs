using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public string name;
    public Color32 color;
    public Sprite sprite;

    void Start()
    {
        Camera.main.gameObject.GetComponent<BuildingsGrid>().FiilRes((int)transform.position.x, (int)transform.position.y, this);
    }
}
