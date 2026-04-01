using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public UIDocument mainMenuUIDocument;
    private VisualElement rootVisualElement;

    private Button startButton;
    private Button settingsButton;
    private Button exitButton;

    void Awake() {
        // Get the root of the UI document
        rootVisualElement = mainMenuUIDocument.rootVisualElement;

        startButton = rootVisualElement.Q<Button>("StartButton");
        settingsButton = rootVisualElement.Q<Button>("SettingsButton");
        exitButton = rootVisualElement.Q<Button>("ExitButton");

        startButton.clicked += OnStartButtonClicked;
        settingsButton.clicked += OnSettingsButtonClicked;
        exitButton.clicked += OnExitButtonClicked;
    }

    private void OnStartButtonClicked() {
        Debug.Log("Start Button Clicked");
        SceneManager.LoadScene("MainLoop"); 
    }

    private void OnSettingsButtonClicked() {
        Debug.Log("Settings Button Clicked");
        // Open the settings menu or navigate to the settings screen here
    }

    private void OnExitButtonClicked() {
        Debug.Log("Exit Button Clicked");
        // Exit the application
        Application.Quit();
    }

    void OnDestroy() {
        startButton.clicked -= OnStartButtonClicked;
        settingsButton.clicked -= OnSettingsButtonClicked;
        exitButton.clicked -= OnExitButtonClicked;
    }
}
