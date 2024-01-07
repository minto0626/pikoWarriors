using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// オブジェクトに円型の衝突判定を持たせるインターフェース
    /// </summary>
    public interface ICircleCollison
    {
        Vector2 Center { get; }
        float Radius { get; }
    }
}