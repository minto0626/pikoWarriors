﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.System
{
    public class CollisionManager
    {
        /// <summary>
        /// コリジョンの種類
        /// </summary>
        public enum CollisionType
        {
            Circle = 0,
            Square,
            Capsule,
            Length
        }

        /// <summary>
        /// コリジョンが付いているオブジェクトのリスト
        /// </summary>
        List<GameCharacter> managedCollisonList;

        /// <summary>
        /// 管理する最大数
        /// </summary>
        const int MANAGE_CAPACITY = 256;

        Dictionary<int, int> masksByLayer;

        public CollisionManager()
        {
            managedCollisonList = new(MANAGE_CAPACITY);
            masksByLayer = new();
            InitMaskTable();
        }

        /// <summary>
        /// 管理するオブジェクトを追加
        /// </summary>
        /// <param name="obj"></param>
        public void AddList(GameCharacter obj)
        {
            if (managedCollisonList.Contains(obj)) { return; }
            managedCollisonList.Add(obj);
        }

        /// <summary>
        /// 管理しなくなったオブジェクトを削除
        /// </summary>
        /// <param name="obj"></param>
        public void Remove(GameCharacter obj)
        {
            if (!managedCollisonList.Contains(obj)) { return; }
            managedCollisonList.Remove(obj);
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void OnUpdate()
        {
            JudgmentUpdate();
        }

        /// <summary>
        /// レイヤーマスクテーブルの初期化
        /// </summary>
        void InitMaskTable()
        {
            for (var i = 0; i < 32; i++)
            {
                int mask = 0;
                for (var j = 0; j < 32; j ++)
                {
                    if(!Physics2D.GetIgnoreLayerCollision(i, j))
                    {
                        mask |= 1 << j;
                    }
                }

                masksByLayer.Add(i, mask);
            }
        }

        /// <summary>
        /// コリジョンの判定更新
        /// </summary>
        void JudgmentUpdate()
        {
            for (var i = 0; i < managedCollisonList.Count - 1; i++)
            {
                var current = managedCollisonList[i];
                if (current == null || !current.IsActive) { continue; }

                var currentLayer = 1 << current.Layer;
                var current_circle = current as ICircleCollison;

                for (var j = i + 1; j < managedCollisonList.Count; j++)
                {
                    var target = managedCollisonList[j];
                    if (target == null || !target.IsActive) { continue; }

                    // レイヤーマスクを見てビットが立っている物のみ当たる
                    if ((currentLayer & masksByLayer[target.Layer]) == 0) { continue; }

                    var target_circle = target as ICircleCollison;
                    bool isCtoC = current_circle != null & target_circle != null;
                    if (isCtoC && CircleToCircle(current_circle, target_circle))
                    {
                        current.OnHit(target);
                        target.OnHit(current);
                    }
                }
            }
        }

        /// <summary>
        /// 円と円
        /// </summary>
        /// <param name="current">比較元</param>
        /// <param name="target">比較対象</param>
        /// <returns>当たっているか否か</returns>
        bool CircleToCircle(ICircleCollison current, ICircleCollison target)
        {
            var currentRadius = current.Radius;
            var targetRadius = target.Radius;
            var distance = (current.Center - target.Center).sqrMagnitude;
            var calcDistance = Mathf.Pow(currentRadius + targetRadius, 2);

            return distance < calcDistance;
        }
    }
}
