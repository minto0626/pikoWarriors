using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : SingletonMonoBehaviour<GameMaster>
{
    [SerializeField] new Camera camera;
    [SerializeField] Vector3 setPlayerPos = new Vector3(0f, -3f, 0f);
    const int BULLET_MAX = 30;

    private void Start()
    {
        if (CheckInstance())
        {
            DontDestroyOnLoad(gameObject);
        }

        InputManager.Instance.Setup();
        CollisionManager.Instance.SetUp();
        ObjectPooler.Instance.SetUp();
        var b = MasterDataStore.Instance.GetObject(MasterDataStore.DataType.BULLET);
        var pool = ObjectPooler.Instance.CreatePool(b, BULLET_MAX);
        CollisionManager.Instance.AddList(pool);
        CreatePlayer();
        EnemyGenerator.Instance.Generate();
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
        var p = MasterDataStore.Instance.GetObject(MasterDataStore.DataType.PLAYER);
        var player = Instantiate(p, setPlayerPos, Quaternion.identity);
        CollisionManager.Instance.AddList(player.GetComponent<ObjectCollision>());
    }

    private void Update()
    {
        InputManager.Instance.OnUpdate();
        CollisionManager.Instance.OnUpdate();
    }
}
