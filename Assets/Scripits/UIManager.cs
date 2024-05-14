using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] private TMP_Text scoreBar;
    [SerializeField] private TMP_Text bestScoreBar;
    [SerializeField] private TMP_Text moneyBar;
    [SerializeField] private GameObject soundButtonOn;
    [SerializeField] private GameObject soundButtonOff;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject inGamePanel;
    [SerializeField] public GameObject[] elements;
    [SerializeField] private GameObject blackWindow;
    [SerializeField] private AudioSource source;
    public UniWebView uniWebView;
    public void CloseUI()
    {
        source.Pause();
        foreach (GameObject obj in elements)
        {
            obj.SetActive(false);
        }
        blackWindow.SetActive(true);

    }
    private void Awake()
    {
        instance = this;
        if (!PlayerPrefs.HasKey("Sound"))
        {
            PlayerPrefs.SetString("Sound", "true");
            PlayerPrefs.Save();
        }
        CheckSound();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            CloseUI();
        }
    }
    public void StartGame()
    {
        GameManager.instance.StartGame();
    }
    public void CheckSound()
    {
        if (PlayerPrefs.GetString("Sound")=="true")
        {
            audioSource.Play();
            soundButtonOff.SetActive(true);
            soundButtonOn.SetActive(false);
        }
        else
        {
            audioSource.Pause();
            soundButtonOff.SetActive(false);
            soundButtonOn.SetActive(true);
        }
    }
    public void SoundOff()
    {
        PlayerPrefs.SetString("Sound", "false");
        PlayerPrefs.Save();
    }
    public void SoundOn()
    {
        PlayerPrefs.SetString("Sound", "true");
        PlayerPrefs.Save();
    }
    public void ShowMoney(string money)
    {
        moneyBar.text = money;
    }
    public void PauseGame()
    {
        GameManager.instance.PauseGame();
    }
    public void UnPauseGame()
    {
        GameManager.instance.UnPauseGame();
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ShowScore(string score)
    {
        scoreBar.text = score;
    }
    public void ShowBestScore(string bestScore)
    {
        bestScoreBar.text = bestScore;
    }
    public void ShowLosePanel()
    {
        StartCoroutine(WaitToShowLosePanel());
    }
    private IEnumerator WaitToShowLosePanel()
    {
        yield return new WaitForSeconds(2f);
        losePanel.SetActive(true);
        inGamePanel.SetActive(false);
    }
    public void ShowPrivacy(string url)
    {
        var webviewObject = new GameObject("UniWebview");
        uniWebView = webviewObject.AddComponent<UniWebView>();
        uniWebView.Frame = new Rect(0, 0, Screen.width, Screen.height);
        uniWebView.SetShowToolbar(true, false, true, true);
        uniWebView.Load(url);
        uniWebView.Show();
    }
}
