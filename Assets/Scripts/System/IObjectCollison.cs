using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// オブジェクトに衝突判定を持たせるインターフェース
    /// </summary>
    public interface IObjectCollison
    {
        int Layer { get; }
        void OnHit(GameCharacter target);
    }
}