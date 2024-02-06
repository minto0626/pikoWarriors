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
        /// キャラクター生成用のパラメータ
        /// </summary>
        public class GameWaveData
        {
            /// <summary>
            /// 生成時間
            /// </summary>
            public float Time { get; }
            /// <summary>
            /// 生成するキャラクタ
            /// </summary>
            public ObjectType ObjType { get; }
            /// <summary>
            /// 生成する座標
            /// </summary>
            public Vector3 Pos { get; }
            public GameWaveData(float time, ObjectType objType, Vector3 pos)
            {
                Time = time;
                ObjType = objType;
                Pos = pos;
            }
        }

        /// <summary>
        /// 生成用パラメータのリスト
        /// </summary>
        GameWaveData[] waveDataArray =
        {
            new(2f, ObjectType.Enemy_Mon, new Vector3(-5f, 3f, 0f)),
            new(2f, ObjectType.Enemy_Mon, new Vector3(0f, 3f, 0f)),
            new(2f, ObjectType.Enemy_Mon, new Vector3(5f, 3f, 0f)),
        };

        /// <summary>
        /// 見ている生成用パラメータリストの場所
        /// </summary>
        int waveDataIndex = 0;

        /// <summary>
        /// ウェーブのタイマー
        /// </summary>
        float waveInTimeer = 0f;

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
}