using System.Collections;
using System.Collections.Generic;
using Game.System;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 衝突するものだけ破壊する壁
    /// </summary>
    public class DisappearWall : GameCharacter, ISquareCollison
    {
        [SerializeField]
        Vector2 size = Vector2.zero;

        public Vector2 Center => transform.position;
        public Vector2 Size => size;
        public override void OnHit(GameCharacter target)
        {
            target.DestroyCharacter();
        }

        public override void Initialize(int layer)
        {
            base.Initialize(layer);
        }

        public void SetSize(float x, float y)
        {
            size.x = x;
            size.y = y;
        }
    }
}
