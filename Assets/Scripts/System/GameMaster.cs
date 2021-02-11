using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : SingletonMonoBehaviour<GameMaster>
{
    [SerializeField] GameObject playerPrefab;
    GameObject playerObj;
    [SerializeField] Vector3 setPlayerPos = new Vector3(0f, -3f, 0f);

    private void Start()
    {
        if (CheckInstance())
        {
            DontDestroyOnLoad(this);
        }

        CreatePlayer();
    }

    void CreatePlayer()
    {
        var player = Instantiate(playerPrefab, setPlayerPos, Quaternion.identity);
    }

    private void Update()
    {
        
    }
}
