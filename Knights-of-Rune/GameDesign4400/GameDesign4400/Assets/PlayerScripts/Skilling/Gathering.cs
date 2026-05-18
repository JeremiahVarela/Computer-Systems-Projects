using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ResourceTilemapMinerPerCell : MonoBehaviour
{
    [Header("Player Detection")]
    public Transform player;
    public float detectionRadius = 1.5f;

    [Header("Mining Settings")]
    public float mineTime = 3f;       // seconds to mine
    public float respawnTime = 30f;   // per-cell cooldown
    public string resourceLabel = "Ore"; // e.g., "Ore", "Wood"

    private Tilemap tilemap;

    private struct PendingRespawn
    {
        public TileBase tile;
        public float respawnAt;
    }

    private readonly Dictionary<Vector3Int, PendingRespawn> pending = new();

    // Mining progress state (polled by HUD)
    public Vector3Int? CurrentTargetCell { get; private set; } = null;
    public float CurrentProgressSeconds { get; private set; } = 0f; // 0..mineTime
    public float MineTimeSeconds => mineTime;
    public string ResourceLabel => resourceLabel;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    private void Update()
    {
        if (player == null || tilemap == null) return;

        // 1) Handle respawns
        if (pending.Count > 0)
        {
            float now = Time.time;
            List<Vector3Int> toRespawn = null;
            foreach (var kv in pending)
            {
                if (now >= kv.Value.respawnAt)
                {
                    (toRespawn ??= new List<Vector3Int>()).Add(kv.Key);
                }
            }
            if (toRespawn != null)
            {
                foreach (var cell in toRespawn)
                {
                    var data = pending[cell];
                    if (!tilemap.HasTile(cell) && data.tile != null)
                        tilemap.SetTile(cell, data.tile);

                    pending.Remove(cell);
                }
            }
        }

        // 2) Find nearest available resource cell
        Vector3Int playerCell = tilemap.WorldToCell(player.position);
        Vector3Int? nearest = null;
        float nearestDist = Mathf.Infinity;

        for (int dx = -2; dx <= 2; dx++)
        {
            for (int dy = -2; dy <= 2; dy++)
            {
                var cell = playerCell + new Vector3Int(dx, dy, 0);
                if (!tilemap.HasTile(cell)) continue;
                if (pending.ContainsKey(cell)) continue;

                float dist = Vector2.Distance(player.position, tilemap.GetCellCenterWorld(cell));
                if (dist <= detectionRadius && dist < nearestDist)
                {
                    nearest = cell;
                    nearestDist = dist;
                }
            }
        }

        if (nearest.HasValue)
        {
            if (CurrentTargetCell.HasValue && CurrentTargetCell.Value == nearest.Value)
            {
                CurrentProgressSeconds += Time.deltaTime;
                if (CurrentProgressSeconds >= mineTime)
                {
                    MineCell(nearest.Value);
                    CurrentTargetCell = null;
                    CurrentProgressSeconds = 0f;
                }
            }
            else
            {
                CurrentTargetCell = nearest.Value;
                CurrentProgressSeconds = 0f;
            }
        }
        else
        {
            CurrentTargetCell = null;
            CurrentProgressSeconds = 0f;
        }
    }

    private void MineCell(Vector3Int cell)
    {
        if (!tilemap.HasTile(cell) || pending.ContainsKey(cell)) return;

        var original = tilemap.GetTile(cell);
        tilemap.SetTile(cell, null);

        pending[cell] = new PendingRespawn
        {
            tile = original,
            respawnAt = Time.time + respawnTime
        };

        Debug.Log($"[{resourceLabel}] Mined at {cell}. Respawns in {respawnTime} s.");
    }
}


