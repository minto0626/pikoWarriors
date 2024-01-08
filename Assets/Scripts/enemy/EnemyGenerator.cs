using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.System
{
    public class EnemyGenerator
    {
        GameObject enemyObj;

        public void LoadPrefab()
        {
            enemyObj = MasterDataStore.Instance.GetObject(MasterDataStore.DataType.ENEMY);
        }

        public GameObject Generate(Vector2 generatePos)
        {
            var obj = GameObject.Instantiate(enemyObj, generatePos, Quaternion.identity);
            return obj;
        }
    }
}
