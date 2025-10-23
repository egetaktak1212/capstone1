using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuCode : MonoBehaviour
{
    [Header("UI Buttons")]
    [SerializeField] private Button tutorialButton;
    [SerializeField] private Button campaignButton;

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
    }

    public void OnTutorialPressed()
    {
        SceneManager.LoadScene(tutorialScene, LoadSceneMode.Single);
    }

    public void OnCampaignPressed()
    {
        SceneManager.LoadScene(campaignScene, LoadSceneMode.Single);
    }
}
