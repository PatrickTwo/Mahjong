using Mahjong;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    public TilePool tilePool;
    public Player player;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        player = new Player(1, "Player 1");

        tilePool = new TilePool();
        tilePool.Initialize();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.UnloadSceneAsync("LobbyScene");
        }
    }
}
