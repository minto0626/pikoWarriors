using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// Resourceの参照を持つクラス
    /// ※全体的に手直しが必要
    /// </summary>
    public class MasterDataStore : SingletonMonoBehaviour<MasterDataStore>
    {
        public enum DataType
        {
            INVALID = -1,
            PLAYER,
            ENEMY,
            BULLET,
            OVER_BATH,
            MAX,
        }

        [SerializeField] GameObject playerPref;
        [SerializeField] GameObject enemyPref;
        [SerializeField] GameObject bulletPref;
        [SerializeField] GameObject overBathPref;

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

                case DataType.OVER_BATH:
                    return overBathPref;
            }
            return null;
        }
    }
}
