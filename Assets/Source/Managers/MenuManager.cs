using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainWindow;
    [SerializeField] private GameObject newGameWindow;
    [SerializeField] private GameObject settingsWindow;
    [SerializeField] private GameObject exitWindow;

    [SerializeField] private Button newGameButton;
    [SerializeField] private Button newGameConfirmButton;
    [SerializeField] private Button newGameBackButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button settingsBackButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button exitConfirmButton;
    [SerializeField] private Button exitBackButton;

    [SerializeField] private TextMeshProUGUI mainMenuText;

    private void Awake()
    {
        mainWindow.SetActive(true);
        newGameWindow.SetActive(false);
        settingsWindow.SetActive(false);
        exitWindow.SetActive(false);

        newGameButton.onClick.AddListener(OnNewGameButtonClick);
        newGameConfirmButton.onClick.AddListener(OnNewGameConfirmButtonClick);
        newGameBackButton.onClick.AddListener(OnNewGameBackButtonClick);
        continueButton.onClick.AddListener(OnContinueButtonClick);
        settingsButton.onClick.AddListener(OnSettingsButtonClick);
        settingsBackButton.onClick.AddListener(OnSettingsBackButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);
        exitConfirmButton.onClick.AddListener(OnExitConfirmButtonClick);
        exitBackButton.onClick.AddListener(OnExitBackButtonClick);

        continueButton.enabled = !SerializeManager.GetNewGameState();
    }

    private void Update()
    {
        mainMenuText.rectTransform.localScale = Vector3.one + (new Vector3(Mathf.Sin(Time.time * 1.5f), Mathf.Sin(Time.time * 1.5f), Mathf.Sin(Time.time * 1.5f)) * 0.05f);
    }

    private void OnNewGameButtonClick()
    {
        mainWindow.SetActive(false);
        newGameWindow.SetActive(true);
        settingsWindow.SetActive(false);
        exitWindow.SetActive(false);
    }

    private void OnNewGameConfirmButtonClick()
    {
        SerializeManager.SetNewGameState(true);

        SceneManager.LoadScene(1);
    }

    private void OnNewGameBackButtonClick()
    {
        mainWindow.SetActive(true);
        newGameWindow.SetActive(false);
        settingsWindow.SetActive(false);
        exitWindow.SetActive(false);
    }

    private void OnContinueButtonClick()
    {
        SceneManager.LoadScene(1);
    }

    private void OnSettingsButtonClick()
    {
        mainWindow.SetActive(false);
        newGameWindow.SetActive(false);
        settingsWindow.SetActive(true);
        exitWindow.SetActive(false);
    }

    private void OnSettingsBackButtonClick()
    {
        mainWindow.SetActive(true);
        newGameWindow.SetActive(false);
        settingsWindow.SetActive(false);
        exitWindow.SetActive(false);
    }
    private void OnExitButtonClick()
    {
        mainWindow.SetActive(false);
        newGameWindow.SetActive(false);
        settingsWindow.SetActive(false);
        exitWindow.SetActive(true);
    }

    private void OnExitConfirmButtonClick()
    {
        Application.Quit();
    }

    private void OnExitBackButtonClick()
    {
        mainWindow.SetActive(true);
        newGameWindow.SetActive(false);
        settingsWindow.SetActive(false);
        exitWindow.SetActive(false);
    }
}
