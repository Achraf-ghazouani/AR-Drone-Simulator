using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Panel_selection;
    public GameObject Panel_intro;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Function to load scene with index 1
    public void LoadSceneWithIndex1()
    {
        SceneManager.LoadScene(1);
    }

    // Function to load scene with index 2
    public void LoadSceneWithIndex2()
    {
        SceneManager.LoadScene(2);
    }
     public void LoadSceneIntro()
    {
        SceneManager.LoadScene(0);
    }
      public void LoadSceneRack()
    {
        SceneManager.LoadScene(3);
    }
    public void quit()
    {
        Application.Quit();
    }
    public void languageSelection()
    {
        //open scene language selection

    }

    public void startscene()
    {
        Panel_intro.SetActive(false);
        Panel_selection.SetActive(true);
    }
    public void back()
    {
        Panel_intro.SetActive(true);
        Panel_selection.SetActive(false);
    }
}