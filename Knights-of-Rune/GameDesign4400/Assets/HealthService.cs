using UnityEngine;
using System;

public class HealthService : MonoBehaviour
{
    public static HealthService Instance;

    public int Max = 100;
    public int Current { get; private set; }

    public event Action<int,int> OnChanged; // (current, max)

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Current = Mathf.Clamp(Current == 0 ? Max : Current, 0, Max);
        OnChanged?.Invoke(Current, Max);
    }

    public void SetMax(int max, bool keepRatio = true)
    {
        max = Mathf.Max(1, max);
        if (keepRatio && Max > 0) Current = Mathf.RoundToInt((float)Current / Max * max);
        Max = max;
        Set(Current);
    }

    public void Damage(int amount) => Set(Current - amount);
    public void Heal(int amount)   => Set(Current + amount);

    public void Set(int value)
    {
        int clamped = Mathf.Clamp(value, 0, Max);
        if (clamped == Current) return;
        Current = clamped;
        OnChanged?.Invoke(Current, Max);
    }
}
