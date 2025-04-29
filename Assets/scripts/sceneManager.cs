using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Function to load a scene by its build index
    public void LoadSceneByIndex(int sceneIndex)
    {
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.LogError($"Scene index {sceneIndex} is out of range.");
        }
    }

    // Function to load the first scene
    public void LoadScene1()
    {
        LoadSceneByIndex(0); // Replace 0 with the index of your first scene
    }

    // Function to load the second scene
    public void LoadScene2()
    {
        LoadSceneByIndex(1); // Replace 1 with the index of your second scene
    }

    // Function to quit the application
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Application Quit"); // This will only show in the editor
    }
}