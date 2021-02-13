using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : SingletonMonoBehaviour<GameMaster>
{
    [SerializeField] new Camera camera;
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

    public Vector3 GetCameraTopLeft()
    {
        var tl = camera.ScreenToWorldPoint(Vector3.zero);
        tl.Scale(new Vector3(1f, -1f, 1f));
        return tl;
    }

    public Vector3 GetCameraBottomRight()
    {
        var br = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));
        br.Scale(new Vector3(1f, -1f, 1f));
        return br;
    }

    void CreatePlayer()
    {
        var player = Instantiate(playerPrefab, setPlayerPos, Quaternion.identity);
    }

    private void Update()
    {
        
    }
}
