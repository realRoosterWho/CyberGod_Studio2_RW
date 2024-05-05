using UnityEngine;
using UnityEngine.UI;

public class GeneralCountdown_Logic : MonoBehaviour
{
    [SerializeField]
    public Scrollbar countdownScrollbar;

    [SerializeField]
    private float maxCountdownTime;

    private void Start()
    {
        // Set the max value and current value of the slider to the max countdown time
        countdownScrollbar.size = 1;
        countdownScrollbar.value = 1;
    }

    private void Update()
    {
        // Decrease the current value of the slider every frame to simulate the countdown
        countdownScrollbar.size -= Time.deltaTime / maxCountdownTime;
    }
}