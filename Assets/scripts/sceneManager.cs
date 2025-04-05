using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Reference to UI panels
    public GameObject Panel_selection;
    public GameObject Panel_intro;

    // Dictionary to map scene names to their build indices
    private readonly Dictionary<string, int> sceneIndices = new Dictionary<string, int>
    {
        { "Starting Menu", 0 },
        { "Main Menu", 1 },
        { "place on plane", 2 },
        { "place on image", 3 },
        { "Language Testing", 4 },
        { "Quiz", 5 }, // Added for the new scene
        { "test simulation", 6 },
        { "simulation", 7 }
    };

    void Start()
    {
        // Optionally, initialize any default state here
    }

    void Update()
    {
        // No updates needed in this case
    }

    // Function to load a scene by name
    public void LoadScene(string sceneName)
    {
        if (sceneIndices.ContainsKey(sceneName))
        {
            SceneManager.LoadScene(sceneIndices[sceneName]);
        }
        else
        {
            Debug.LogError($"Scene '{sceneName}' not found in the build settings.");
        }
    }

    // Specific scene loading methods for convenience
    public void LoadSceneIntro()
    {
        LoadScene("Starting Menu");
    }

    public void LoadSceneRack()
    {
        LoadScene("Main Menu");
    }

    public void LoadScenePlaceOnPlane()
    {
        LoadScene("place on plane");
    }

    public void LoadScenePlaceOnImage()
    {
        LoadScene("place on image");
    }

    public void LoadSceneLanguageTesting()
    {
        LoadScene("Language Testing");
    }

    public void LoadSceneQuiz()
    {
        LoadScene("Quiz"); // New method for the Quiz scene
    }

    public void LoadSceneTestSimulation()
    {
        LoadScene("test simulation");
    }

    public void LoadSceneSimulation()
    {
        LoadScene("simulation");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LanguageSelection()
    {
        // Logic to open language selection scene
        LoadScene("Language Testing"); // Assuming "Language Testing" is the language selection scene
    }

    public void StartScene()
    {
        Panel_intro.SetActive(false);
        Panel_selection.SetActive(true);
    }

    public void Back()
    {
        Panel_intro.SetActive(true);
        Panel_selection.SetActive(false);
    }
}