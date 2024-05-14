using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeIncreaser : MonoBehaviour
{
    private float currentTimeScale;
    private bool isCan;
    private float time;
    public void IncreaseTime()
    {
        currentTimeScale = Time.timeScale;
        Time.timeScale +=Time.timeScale;
        StartCoroutine(WaitForStopTimeIncrease());
        Debug.Log("Vizval  coroutine");
        GameManager.instance.IncreaseTime();
    }
    private void Update()
    {
        if (isCan)
        {
            time += Time.deltaTime;
            Debug.Log("ETO");
            if (time>5)
            {
                Time.timeScale = currentTimeScale;
                isCan = false;
                time = 0;
                
            }
        }
    }
    private IEnumerator WaitForStopTimeIncrease()
    {
        yield return new WaitForSeconds(5);
        Time.timeScale = currentTimeScale;
        Debug.Log("Vremya");
    }
}
