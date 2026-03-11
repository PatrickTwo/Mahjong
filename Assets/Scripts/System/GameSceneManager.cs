using UnityEngine.SceneManagement;


public static class GameSceneManager
{
    public static string LobbyScene = "LobbyScene";
    public static string GameScene = "GameScene";

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
