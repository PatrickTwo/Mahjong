using UnityEngine.SceneManagement;


public class SceneNameConst
{
    public static string LobbyScene = "LobbyScene";
    public static string GameScene = "GameScene";
}
public static class GameSceneManager
{

    #region 加载场景
    public static void LoadGameScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
    {
        SceneManager.LoadScene(sceneName, loadSceneMode);
    }
    #endregion
    #region 卸载场景
    public static void UnloadGameScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }
    #endregion
}
