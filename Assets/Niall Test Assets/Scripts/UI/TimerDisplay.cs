using UnityEngine;
using TMPro;

/// <summary>
/// Displays the out-of-water timer on the UI.
/// Responds to timer events to update display.
/// </summary>
public class TimerDisplay : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("TextMeshPro element for timer display")]
    public TextMeshProUGUI timerText;

    [Header("Display Settings")]
    [Tooltip("Format string for timer display")]
    public string timerFormat = "Out of water: {0:F1}s / {1:F1}s";

    [Tooltip("Text shown when player is safe in water")]
    public string safeText = "In water - safe!";

    [Tooltip("Color of timer text in normal state")]
    public Color normalColor = Color.white;

    [Tooltip("Color of timer text in danger state")]
    public Color dangerColor = Color.red;

    private TextMeshProUGUI cachedText;
    private float maxTime = 3f;

    void OnEnable()
    {
        if (timerText == null)
        {
            timerText = GetComponent<TextMeshProUGUI>();
        }
        cachedText = timerText;
    }

    /// <summary>
    /// Updates timer display with current time out of water.
    /// </summary>
    public void UpdateDisplay(float timeOutOfWater)
    {
        if (cachedText == null) return;

        cachedText.text = string.Format(timerFormat, timeOutOfWater, maxTime);
        cachedText.color = normalColor;
    }

    /// <summary>
    /// Sets the max time for display reference.
    /// </summary>
    public void SetMaxTime(float max)
    {
        maxTime = max;
    }

    /// <summary>
    /// Shows safe message when player is in water.
    /// </summary>
    public void ShowSafeMessage()
    {
        if (cachedText == null) return;

        cachedText.text = safeText;
        cachedText.color = normalColor;
    }

    /// <summary>
    /// Shows danger state with red color.
    /// </summary>
    public void ShowDanger()
    {
        if (cachedText == null) return;

        cachedText.color = dangerColor;
    }

    /// <summary>
    /// Returns to normal color when danger ends.
    /// </summary>
    public void HideDanger()
    {
        if (cachedText == null) return;

        cachedText.color = normalColor;
    }
}
