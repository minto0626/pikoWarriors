using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : SingletonMonoBehaviour<EnemyGenerator>
{
    [SerializeField] GameObject[] generatePos;
    GameObject enemyObj;

    const int GENERATE_MAX = 3;

    void Start()
    {
        if (CheckInstance())
        {
            DontDestroyOnLoad(gameObject);
        }

        enemyObj = MasterDataStore.Instance.GetObject(MasterDataStore.DataType.ENEMY);
    }

    public void Generate()
    {
        for (var index = 0; index < GENERATE_MAX; index++)
        {
            var obj = Instantiate(enemyObj, generatePos[index].transform.position, Quaternion.identity);
        }
    }
}
