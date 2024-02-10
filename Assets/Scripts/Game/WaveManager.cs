using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// ウェーブ管理クラス
    /// </summary>
    public class WaveManager
    {
        /// <summary>
        /// 生成用パラメータのリスト
        /// </summary>
        WaveData[] waveDataArray;

        /// <summary>
        /// 見ている生成用パラメータリストの場所
        /// </summary>
        int waveDataIndex = 0;

        /// <summary>
        /// ウェーブのタイマー
        /// </summary>
        float waveInTimeer = 0f;

        public WaveManager(WaveData[] waveData)
        {
            this.waveDataArray = waveData;
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        public void OnUpdate()
        {
            if (waveDataIndex >= waveDataArray.Length)
            {
                return;
            }

            if (waveDataArray[waveDataIndex].Time <= waveInTimeer)
            {
                var createTime = waveDataArray[waveDataIndex].Time;
                while (waveDataArray[waveDataIndex].Time == createTime)
                {
                    var waveData = waveDataArray[waveDataIndex];
                    var chara = GameMaster.Instance.CharacterManager.CreateChara(waveData.ObjType);
                    chara.transform.position = waveData.Pos;
                    waveDataIndex++;

                    if (waveDataIndex >= waveDataArray.Length)
                    {
                        return;
                    }
                }
            }

            waveInTimeer += Time.deltaTime;
        }
    }

    /// <summary>
    /// キャラクター生成用のパラメータ
    /// </summary>
    [Serializable]
    public class WaveData
    {
        [SerializeField]
        float time;
        /// <summary>
        /// 生成時間
        /// </summary>
        public float Time => this.time;

        [SerializeField]
        ObjectType objectType;
        /// <summary>
        /// 生成するキャラクタ
        /// </summary>
        public ObjectType ObjType => this.objectType;

        [SerializeField]
        Vector3 pos;
        /// <summary>
        /// 生成する座標
        /// </summary>
        public Vector3 Pos => this.pos;
        public WaveData(float time, ObjectType objType, Vector3 pos)
        {
            this.time = time;
            this.objectType = objType;
            this.pos = pos;
        }
    }
}