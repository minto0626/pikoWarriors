using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : SingletonMonoBehaviour<GameMaster>
{
    [SerializeField] GameObject playerPrefab;
    GameObject playerObj;
    [SerializeField] Vector3 setPlayerPos = new Vector3(0f, -3f, 0f);
    [SerializeField] GameObject bulletPrefab;
    const int BULLET_MAX = 30;

    private void Start()
    {
        if (CheckInstance())
        {
            DontDestroyOnLoad(gameObject);
        }

        ObjectPooler.Instance.SetUp();
        ObjectPooler.Instance.CreatePool(bulletPrefab, BULLET_MAX);
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
