using System.Collections;
using System.Collections.Generic;
using Mahjong;
using UnityEngine;

public class Test : MonoBehaviour
{
    public TilePool tilePool;
    public Player player;

    private void Awake()
    {
        player = new Player(1, "Player 1");

        tilePool = new TilePool();
        tilePool.Initialize();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            List<MahjongTile> tiles = tilePool.DrawTiles(13);
            player.AddTiles(tiles);
            Debug.Log(string.Join(", ", player.GetHandTiles()));
        }
    }
}
