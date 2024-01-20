using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// オブジェクトに矩形の衝突判定を持たせるインターフェース
    /// </summary>
    public interface ISquareCollison
    {
        Vector2 Center { get; }
        Vector2 Size { get; }
    }
}