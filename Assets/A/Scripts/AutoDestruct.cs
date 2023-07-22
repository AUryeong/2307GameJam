using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestruct : MonoBehaviour
{
    public float maxDuration;
    private float duration;

    private void OnEnable()
    {
        duration = 0;
    }

    private void Update()
    {
        duration += Time.deltaTime;
        if(duration >= maxDuration)
        {
            gameObject.SetActive(false);
        }
    }
}
