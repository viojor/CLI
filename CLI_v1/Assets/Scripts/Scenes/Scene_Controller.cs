using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Controller : MonoBehaviour{

    public const string LEVEL = "Level";
    public const string LEVEL_MENU = "Levels Menu";
    public const string GAME_OVER = "Game Over";
    public const string MAIN_MENU = "Main Menu";
    public const string WIN_GAME = "Win Game";

    public static void ChangeScene(string scene_name){

        SceneManager.LoadScene(scene_name);
    }
}
