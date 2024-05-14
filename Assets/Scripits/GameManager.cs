using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static Action onGameStarted;
    private bool isGameStarted;
    private float currentTimeScale;
    private int score;
    private int money;
    [SerializeField] private GameObject[] startObjects;
    [SerializeField] private GameObject playerModel;
    [SerializeField] private GameObject playerHat;
    [SerializeField] private GameObject playerIdle;
    [SerializeField] private GameObject cameraM;
    [SerializeField] private Transform inGameCameraPos;
    [SerializeField] private GameObject[] objectsForDeactivate;
    [SerializeField] private GameObject[] hats;
    private void Awake()
    {
        instance = this;
        currentTimeScale = Time.timeScale;
        if (PlayerPrefs.HasKey("Money"))
        {
            money = PlayerPrefs.GetInt("Money");
        }
        else
        {
            PlayerPrefs.SetInt("Money", 0);
            PlayerPrefs.Save();
        }
        CheckHats();
    }
    private void Start()
    {
        UIManager.instance.ShowMoney(money.ToString());
    }
    private void Update()
    {
        if (isGameStarted)
        {
            score += 1;
            UIManager.instance.ShowScore(score.ToString());
        }
    }
    public void CheckHats()
    {
        if (PlayerPrefs.HasKey("Buy1"))
        {
            foreach (GameObject hat in hats)
            {
                hat.SetActive(true);
            }
        }
    }
    public void StartGame()
    {
        isGameStarted = true;
        onGameStarted?.Invoke();
        Time.timeScale = 1f;
        playerModel.SetActive(true);
        playerHat.SetActive(true);
        playerIdle.SetActive(false);
        foreach(GameObject gameObject in startObjects)
        {
            gameObject.SetActive(true);
        }
        foreach(GameObject obj in objectsForDeactivate)
        {
            obj.SetActive(false);
        }
        cameraM.transform.DOMove(inGameCameraPos.position, 1f);
        cameraM.transform.DORotateQuaternion(inGameCameraPos.rotation, 1f);
    }
    public void AddMoney()
    {
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + 1);
        PlayerPrefs.Save();
    }
    public void PauseGame()
    {
        isGameStarted = false;
        Time.timeScale = 0f;
    }
    public void UnPauseGame()
    {
        isGameStarted = true;
        Time.timeScale = currentTimeScale;
    }
    public void EndGame()
    {
        isGameStarted = false;
        CheckBestScore();
        UIManager.instance.ShowLosePanel();
    }
    private void CheckBestScore()
    {
        if (PlayerPrefs.HasKey("BestScore"))
        {
            int tempBestScore = PlayerPrefs.GetInt("BestScore");
            if (tempBestScore > score)
            {
                UIManager.instance.ShowBestScore(tempBestScore.ToString());
            }
            else
            {
                UIManager.instance.ShowBestScore(score.ToString());
                PlayerPrefs.SetInt("BestScore", score);
                PlayerPrefs.Save();
            }
        }
        else
        {
            UIManager.instance.ShowBestScore(score.ToString());
            PlayerPrefs.SetInt("BestScore", score);
            PlayerPrefs.Save();
        }
    }
    public bool IsGameStarted()
    {
        return isGameStarted;
    }
}
