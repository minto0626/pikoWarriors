using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.System;

namespace Game
{
    /// <summary>
    /// ゲームの主体となるゲームオブジェクト。
    /// 基本的にゲームに関与するオブジェクトは、これを継承させる想定。
    /// </summary>
    public abstract class GameCharacter : MonoBehaviour, IObjectCollison
    {
        public int Layer { get; private set; } = -1;

        public virtual void OnHit(GameCharacter target) {}

        public bool IsActive => gameObject.activeSelf;

        public bool IsDestroy { get; private set; } = false;
        public void DestroyCharacter()
        {
            IsDestroy = true;
            gameObject.SetActive(false);
        }

        public virtual void Initialize(int layer)
        {
            Layer = layer;
            IsDestroy = false;
        }

        public virtual void OnUpdate() {}
    }
}