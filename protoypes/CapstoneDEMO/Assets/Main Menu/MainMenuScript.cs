using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [Header("UI Buttons")]
    [SerializeField] private Button tutorialButton;
    [SerializeField] private Button campaignButton;
    [SerializeField] private Button exitButton;

    [Header("Scene Names (must be in Build Settings)")]
    [SerializeField] private string tutorialScene = "SampleScene";
    [SerializeField] private string campaignScene = "CampaignMapScene";


    void Awake()
    {
        if (tutorialButton != null)
            tutorialButton.onClick.AddListener(OnTutorialPressed);
        else
            Debug.LogError("Tutorial Button not assigned on MainMenuScript.");

        if (campaignButton != null)
            campaignButton.onClick.AddListener(OnCampaignPressed);
        else
            Debug.LogError("Campaign Button not assigned on MainMenuScript.");

        if (exitButton != null)
            exitButton.onClick.AddListener(ExitButtonPressed);
        else
            Debug.LogError("Exit Button not assigned on MainMenuScript.");
    }

    public void OnTutorialPressed()
    {
        SceneManager.LoadScene(tutorialScene, LoadSceneMode.Single);
    }

    public void OnCampaignPressed()
    {
        SceneManager.LoadScene(campaignScene, LoadSceneMode.Single);
    }

    public void ExitButtonPressed()
    {
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}