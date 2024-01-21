using System.Collections;
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
                var current_square = current as ISquareCollison;
                var current_capsule = current as ICapsuleCollison;

                for (var j = i + 1; j < managedCollisonList.Count; j++)
                {
                    var target = managedCollisonList[j];
                    if (target == null || !target.IsActive) { continue; }

                    // レイヤーマスクを見てビットが立っている物のみ当たる
                    if ((currentLayer & masksByLayer[target.Layer]) == 0) { continue; }

                    var target_circle = target as ICircleCollison;
                    bool isCtoC = current_circle != null & target_circle != null;
                    if (isCtoC && CircleAndCircle(current_circle, target_circle))
                    {
                        current.OnHit(target);
                        target.OnHit(current);
                        continue;
                    }

                    bool isSqutoC = current_square != null & target_circle != null;
                    if (isSqutoC && CircleAndSquare(target_circle, current_square))
                    {
                        current.OnHit(target);
                        target.OnHit(current);
                        continue;
                    }

                    var target_square = target as ISquareCollison;
                    bool isCtoSqu = current_circle != null & target_square != null;
                    if (isCtoSqu && CircleAndSquare(current_circle, target_square))
                    {
                        current.OnHit(target);
                        target.OnHit(current);
                        continue;
                    }

                    var target_capsule = target as ICapsuleCollison;
                    bool isCtoCap = current_circle != null & target_capsule != null;
                    if (isCtoCap && CircleAndCapsule(current_circle, target_capsule))
                    {
                        current.OnHit(target);
                        target.OnHit(current);
                        continue;
                    }

                    bool isCaptoC = current_capsule != null & target_circle != null;
                    if (isCaptoC && CircleAndCapsule(target_circle, current_capsule))
                    {
                        current.OnHit(target);
                        target.OnHit(current);
                        continue;
                    }

                    // 衝突判定メモ
                    {
                        // // カプセルとカプセル
                        // if(cForm == 1 && tForm == 1){
                        //     if(CaptoCap_Collision(current.pos,target.pos,
                        //                             current.radius, target.radius,
                        //                             current.pos + current.vSeg, target.pos + target.vSeg,
                        //                             current.pos - current.vSeg, target.pos - target.vSeg)){
                                
                        //         current.c = true;
                        //         target.c = true;
                        //     }
                        // }
                    }
                }
            }
        }

        /// <summary>
        /// 円と円
        /// </summary>
        /// <param name="circle1">円1</param>
        /// <param name="circle2">円2</param>
        /// <returns>当たっているか否か</returns>
        bool CircleAndCircle(ICircleCollison circle1, ICircleCollison circle2)
        {
            var currentRadius = circle1.Radius;
            var targetRadius = circle2.Radius;
            var distance = (circle1.Center - circle2.Center).sqrMagnitude;
            var calcDistance = Mathf.Pow(currentRadius + targetRadius, 2);

            return distance < calcDistance;
        }

        /// <summary>
        /// 円と矩形
        /// </summary>
        /// <param name="circle">円</param>
        /// <param name="square">矩形</param>
        /// <returns>当たっているか否か</returns>
        bool CircleAndSquare(ICircleCollison circle, ISquareCollison square)
        {
            var sqrLength = 0f;
            for (var i = 0; i < 2; i++)
            {
                var point = circle.Center[i];
                var squareMin = square.Center[i] + square.Size[i] * -0.5f;
                var squareMax = square.Center[i] + square.Size[i] * 0.5f;

                if (point < squareMin)
                {
                    sqrLength += (point - squareMin) * (point - squareMin);
                }
                if (point > squareMax)
                {
                    sqrLength += (point - squareMax) * (point - squareMax);
                }
            }

            if (sqrLength == 0f)
            {
                return true;
            }

            return sqrLength <= circle.Radius * circle.Radius;
        }

        /// <summary>
        /// 円とカプセル
        /// </summary>
        /// <param name="circle">円</param>
        /// <param name="capsule">カプセル</param>
        /// <returns>当たったか否か</returns>
        bool CircleAndCapsule(ICircleCollison circle, ICapsuleCollison capsule)
        {
            // 二つの円をつなぐ線分との近傍点を求める
            var startToCircle = circle.Center - capsule.StartSegment;
            var startToEnd = capsule.EndSegment - capsule.StartSegment;
            var nearLength = Vector2.Dot(startToEnd.normalized, startToCircle);
            var nearLengthRate = nearLength / startToEnd.magnitude;
            var nearPoint = capsule.StartSegment + startToEnd * Mathf.Clamp01(nearLengthRate);

            // 円とカプセルとの最短距離を求める
            float sqrDistance;

            // 近傍点が線分上になく、start寄りにある
            if (nearLengthRate < 0)
            {
                sqrDistance = startToCircle.sqrMagnitude;
            }
            // 近傍点が線分上になく、end寄りにある
            else if (nearLengthRate > 1)
            {
                var endToCircle = circle.Center - capsule.EndSegment;
                sqrDistance = endToCircle.sqrMagnitude;
            }
            // 近傍点が線分上にある
            else
            {
                var nearToCircle = circle.Center - nearPoint;
                sqrDistance = nearToCircle.sqrMagnitude;
            }

            return sqrDistance - (circle.Radius + capsule.Radius) * (circle.Radius + capsule.Radius) <= 0;
        }

// 衝突判定メモ
        // /// <summary>
        // /// カプセルとカプセルの当たり判定
        // /// </summary>
        // /// <param name="cPos">比較元の中心座標</param>
        // /// <param name="tPos">比較対象の中心座標</param>
        // /// <param name="cr">比較元の半径</param>
        // /// <param name="tr">比較対象の半径</param>
        // /// <param name="cTop">比較元の始点座標</param>
        // /// <param name="tTop">比較対象の始点座標</param>
        // /// <param name="cBot">比較元の終点座標</param>
        // /// <param name="tBot">比較対象の終点座標</param>
        // /// <returns></returns>
        // bool CaptoCap_Collision(Vector3 cPos, Vector3 tPos, float cr, float tr, Vector3 cTop, Vector3 tTop, Vector3 cBot, Vector3 tBot){
        //     // 始点から終点を結ぶベクトル
        //     var V1 = cBot - cTop;
        //     var V2 = tBot - tTop;

        //     // 算出された最近点と向かい合うベクトルとの距離
        //     float length1 = CalcVector(V1, cTop,tTop);
        //     float length2 = CalcVector(V1, cTop,tBot);
        //     float length3 = CalcVector(V2, tTop,cTop);
        //     float length4 = CalcVector(V2, tTop,cBot);

        //     // 最短距離を算出
        //     float dis = CompLength(length1, length2, length3 ,length4);
            
        //     // 距離を比較
        //     if(dis <= (cr + tr) * (cr + tr)){
        //         return true;
        //     }else{
        //         return false;
        //     }
        // }

        // /// <summary>
        // /// ベクトル上にある最近点を算出
        // /// </summary>
        // /// <param name="a"></param>
        // /// <param name="b"></param>
        // /// <returns></returns>
        // float CalcVector(Vector3 vec, Vector3 a, Vector3 b){
        //     var vec2 = b - a;

        //     var t1 = Vector3.Dot(vec2, vec) / vec2.sqrMagnitude;

        //     if(t1 >= 1.0f){
        //         // 終点が最近点
        //         t1 = 1.0f;
        //     }else if(t1 <= 0.0f){
        //         // 始点が最近点
        //         t1 = 0.0f;
        //     }

        //     var p1 = a + vec * t1;

        //     return (b - p1).sqrMagnitude;
        // }

        // /// <summary>
        // /// ４つの値から一番短いものを返す
        // /// </summary>
        // /// <param name="a"></param>
        // /// <param name="b"></param>
        // /// <param name="c"></param>
        // /// <param name="d"></param>
        // /// <returns></returns>
        // float CompLength(float a, float b, float c, float d){
        //     float[] work1 = {a,b,c,d};
        //     float work2 = a;

        //     for(int i = 1; i < work1.Length; i++){
        //         if(work1[i] < work2){
        //             work2 = work1[i];
        //         }          
        //     }
        //     return work2;
        // }
    }
}
