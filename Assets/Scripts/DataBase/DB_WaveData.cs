using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.System;

namespace Game.DB
{
    [CreateAssetMenu(menuName = "データベース/ウェーブ情報")]
    public class DB_WaveData : ScriptableObject
    {
        [SerializeField]
        List<WaveData> data;

        public List<WaveData> Data => this.data;
    }
}