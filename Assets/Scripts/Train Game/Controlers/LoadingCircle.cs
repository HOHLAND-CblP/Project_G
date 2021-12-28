using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingCircle : MonoBehaviour
{
    [Header("Circle Sprite Renderer")]
    [SerializeField]
    Image srCircle;

    float time;
    float pastTense;
    Transform trackingObj;

    public void NewLoad(float time, Transform trackingObj)
    {
        this.trackingObj = trackingObj;
        this.time = time;

        StartCoroutine(TimeToLoad(time));
        enabled = true;
    }
    
    void Update()
    {
        transform.position = trackingObj.position;

        pastTense += Time.deltaTime;

        srCircle.fillAmount = pastTense/time;
    }

    IEnumerator TimeToLoad(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
