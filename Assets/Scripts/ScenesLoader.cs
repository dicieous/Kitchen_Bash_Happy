using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ScenesLoader
{
    public enum Scene
    {
        MainMenuScene,
        LoadingScene,
        GameScene,
    }
    
    private static Scene tragetScene;
    
    public static void Load(Scene targetScene)
    {
        ScenesLoader.tragetScene = targetScene;

        SceneManager.LoadScene(Scene.LoadingScene.ToString());
       
        
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(tragetScene.ToString());
    }
}
