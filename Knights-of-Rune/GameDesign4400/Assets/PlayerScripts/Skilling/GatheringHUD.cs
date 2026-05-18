using System.Collections.Generic;
using UnityEngine;

public class ResourceMiningHUD : MonoBehaviour
{
    [Header("Sources")]
    public List<ResourceTilemapMinerPerCell> miners = new List<ResourceTilemapMinerPerCell>();

    [Header("HUD")]
    public bool showDebugHud = true;
    public Vector2 debugHudPos = new Vector2(12, 12);

    private void OnGUI()
    {
        if (!showDebugHud || miners == null || miners.Count == 0) return;

        // Choose the miner with max progress (so HUD doesn't double)
        ResourceTilemapMinerPerCell best = null;
        float bestProgress = 0f;

        foreach (var m in miners)
        {
            if (m == null) continue;
            if (!m.CurrentTargetCell.HasValue) continue;

            float p = m.CurrentProgressSeconds;
            if (p > bestProgress)
            {
                bestProgress = p;
                best = m;
            }
        }

        string line;
        if (best != null)
        {
            float p = Mathf.Min(best.CurrentProgressSeconds, best.MineTimeSeconds);
            line = $"Gathering {best.ResourceLabel}: {p:0.00} / {best.MineTimeSeconds:0.00} s";
        }
        else
        {
            line = "Gathering: idle";
        }

        var rect = new Rect(debugHudPos.x, debugHudPos.y, 300f, 28f);
        GUI.Box(rect, GUIContent.none);
        GUI.Label(new Rect(rect.x + 8, rect.y + 5, rect.width - 16, rect.height - 10), line);
    }
}
