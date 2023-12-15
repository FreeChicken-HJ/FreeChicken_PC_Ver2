using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CityMap_CountDown : MonoBehaviour
{
    public TextMeshProUGUI text;
    public bool isCity;
    public int CountValue;
    public bool isFin;
    public bool isStart;
    public bool isStop;
    public AudioSource countDownSound;
    private Color originalColor;

    void Start()
    {
        originalColor = text.color;
    }

    void Update()
    {
        if (isStart)
        {
            isStart = false;
            isStop = false;
            text.gameObject.SetActive(true);
            StartCoroutine(CountDown());
        }   
        
    }
    IEnumerator CountDown()
    {
        if (!isCity)
        {
            if (countDownSound != null)
            {
                countDownSound.Play();
            }
        }
        while (CountValue > 0 && !isStop)
        {
           
            text.text = CountValue.ToString();
            if (CountValue <= 10)
            {
                text.color = Color.red;
            }

            yield return new WaitForSeconds(1f);
            CountValue--;
        }
        if (isCity)
        {
            text.text = "GO!";
        }
        yield return new WaitForSeconds(.2f);
        if (!isCity)
        {
            if (countDownSound != null)
            {
                countDownSound.Stop();
            }
        }

        text.color = originalColor;
        if (!isStop && !isCity) isFin = true;
        text.gameObject.SetActive(false);
       
    }
}
