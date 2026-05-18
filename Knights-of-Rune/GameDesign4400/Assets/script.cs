using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    [Header("UI (auto-detected if left blank)")]
    public Slider slider;          // child Slider
    public Image fill;             // child "Fill" image

    private Color initialFillColor;

    void Awake()
    {
        if (!slider) slider = GetComponentInChildren<Slider>(true);
        if (!fill && slider)
        {
            var imgs = slider.GetComponentsInChildren<Image>(true);
            foreach (var img in imgs)
                if (img.name.ToLower().Contains("fill")) { fill = img; break; }
        }

        if (slider && fill && slider.fillRect == null)
            slider.fillRect = fill.rectTransform;

        if (fill) initialFillColor = fill.color;

        if (slider)
        {
            slider.minValue = 0;
            slider.wholeNumbers = false;
        }

        Debug.Log($"[HB] Awake. slider={(slider?slider.name:"null")} fill={(fill?fill.name:"null")}");
    }

    void OnEnable()
    {
    SceneManager.activeSceneChanged += OnSceneChanged;
    StartCoroutine(WaitForHealthService());
    }

private IEnumerator WaitForHealthService()
    {
    // Wait until HealthService.Instance exists
    while (HealthService.Instance == null)
        yield return null;

    HealthService.Instance.OnChanged += OnServiceChanged;
    OnServiceChanged(HealthService.Instance.Current, HealthService.Instance.Max);
    }

    void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
        if (HealthService.Instance != null)
            HealthService.Instance.OnChanged -= OnServiceChanged;
    }

    void OnSceneChanged(Scene a, Scene b)
    {
        if (HealthService.Instance != null)
            OnServiceChanged(HealthService.Instance.Current, HealthService.Instance.Max);
    }

    private void OnServiceChanged(int current, int max)
    {
        if (!slider)
        {
            Debug.LogWarning("[HB] No Slider found at OnServiceChanged.");
            return;
        }

        // Re-attach Fill if Unity lost the reference across scenes
        if (fill && slider.fillRect == null)
            slider.fillRect = fill.rectTransform;

        slider.maxValue = max;
        slider.value = Mathf.Clamp(current, 0, max);

        if (fill) fill.color = initialFillColor;

        // Force a layout/graphics refresh in case the Canvas was just re-enabled
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(slider.transform as RectTransform);

        Debug.Log($"[HB] UI updated: {current}/{max}. Slider.value={slider.value}, fillRectSet={(slider.fillRect!=null)}");
    }

    // Compatibility (not used in the service pattern but safe to keep)
    public void SetMaxHealth(int max)  => HealthService.Instance?.SetMax(max);
    public void SetHealth(int value)   => HealthService.Instance?.Set(value);
}
