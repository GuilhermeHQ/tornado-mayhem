using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] float startingTime = 10f;
    [SerializeField] Text countdownText;
    [SerializeField] private SoundPlayer soundPlayer;

    void Start()
    {
    }
    
    // void Update()
    // {
    //     currentTime -= 1 * Time.deltaTime;
    //     countdownText.text = currentTime.ToString("0");
    //
    //     if (currentTime <= 10)
    //     {
    //         countdownText.color = Color.red;
    //     }        
    //
    //     if (currentTime <=0)
    //     {
    //         currentTime = 0;
    //     }
    //
    // }

    private void UpdateTimerText(TimeSpan timeLeft)
    {
        
        countdownText.text = timeLeft.Seconds.ToString("0");
        if (timeLeft <= TimeSpan.FromSeconds(4))
        {
            soundPlayer.PlaySound(timeLeft.Seconds.ToString("0"));
            countdownText.color = Color.red;
        }
    }

    public void StartTimer(float timeInSeconds, Action onFinishTimer)
    {
        var endTime = DateTime.Now + TimeSpan.FromSeconds(timeInSeconds);

        StartCoroutine(CountTimer(endTime, UpdateTimerText, onFinishTimer));
    }

    private IEnumerator CountTimer(DateTime endTime, Action<TimeSpan> onUpdate, Action onFinishTimer)
    {
        while (true)
        {
            var timeLeft = endTime - DateTime.Now;
            
            onUpdate?.Invoke(timeLeft);
            
            yield return new WaitForSeconds(1);

            if (timeLeft <= TimeSpan.Zero)
            {
                onFinishTimer?.Invoke();
                yield break;
            }
            
        }
    }
    
    

}
