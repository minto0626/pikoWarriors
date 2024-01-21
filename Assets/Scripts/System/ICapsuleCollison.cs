using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// オブジェクトにカプセル形の衝突判定を持たせるインターフェース
    /// </summary>
    public interface ICapsuleCollison
    {
        float Radius { get; }
        Vector2 StartSegment { get; }
        Vector2 EndSegment { get; }
    }
}