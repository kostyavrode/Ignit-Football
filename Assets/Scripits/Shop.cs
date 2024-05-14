using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public int cost1;
    public int cost2;
    public GameObject buyButton1;
    public GameObject buyButton2;
    private void OnEnable()
    {
        CheckBuy();
    }
    private void CheckBuy()
    {
        if (PlayerPrefs.HasKey("Buy1"))
        {
            buyButton1.SetActive(false);
        }
        if (PlayerPrefs.HasKey("Buy2"))
        {
            buyButton2.SetActive(false);
        }
    }
    public void Buy1()
    {
        if (PlayerPrefs.GetInt("Money")>=cost1)
        {
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - cost1);
            PlayerPrefs.SetString("Buy1","true");
            PlayerPrefs.Save();
            CheckBuy();
            GameManager.instance.CheckHats();
        }
    }
    public void Buy2()
    {
        if (PlayerPrefs.GetInt("Money") >= cost2)
        {
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - cost2);
            PlayerPrefs.SetString("Buy2", "true");
            PlayerPrefs.Save();
            CheckBuy();
        }
    }
}
