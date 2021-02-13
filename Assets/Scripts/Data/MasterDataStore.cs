using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterDataStore : SingletonMonoBehaviour<MasterDataStore>
{
    public enum DataType
    {
        INVALID = -1,
        PLAYER,
        ENEMY,
        BULLET,
        MAX
    }

    [SerializeField] GameObject playerPref;
    [SerializeField] GameObject enemyPref;
    [SerializeField] GameObject bulletPref;

    void Start()
    {
        if (CheckInstance())
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public GameObject GetObject(DataType type)
    {
        switch (type)
        {
            case DataType.PLAYER:
                return playerPref;

            case DataType.ENEMY:
                return enemyPref;

            case DataType.BULLET:
                return bulletPref;
        }
        return null;
    }
}
