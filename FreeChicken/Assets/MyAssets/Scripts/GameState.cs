using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameState : MonoBehaviour
{
    
    public bool isF;
    public bool isH;
    public bool isCi;
    public bool isCa;
    public bool isEasy;
    public AudioSource BGM;
   
    public AudioSource ClickSound;
    
    public void OnMouseDown()
    {
        if (isF)
        {
            ClickSound.Play();
            FactoryScenePlay();
           
        }
        else if (isH)
        {
            ClickSound.Play();
            HouseScenePlay();
            
        }
        else if (isCi)
        {
            ClickSound.Play();
            CityScenePlay();
          
            
        }
        else if (isCa)
        {
            ClickSound.Play();
            CaveScenePlay();
        }
    }  
  
    public void FactoryScenePlay()
    {
        if (isEasy)
        {
            LoadingSceneManager.LoadScene("FactoryScene_1_Easy");
        }
        else
        {
            LoadingSceneManager.LoadScene("FactoryScene_1_Hard");
        }
    }
    public void HouseScenePlay()
    {

        if (isEasy)
        {
            LoadingSceneManager.LoadScene("HouseScene_1_Easy");
        }
        else
        {
            LoadingSceneManager.LoadScene("HouseScene_1_Hard");
        }
    }
    public void CityScenePlay()
    {

        if (isEasy)
        {
            LoadingSceneManager.LoadScene("CityScene_Easy");
        }
        else
        {
            LoadingSceneManager.LoadScene("CityScene_Hard");
        }

    }
    public void CaveScenePlay()
    {
        if (isEasy)
        {
            LoadingSceneManager.LoadScene("CaveScene_Easy");
        }
        else
        {
            LoadingSceneManager.LoadScene("CaveScene_Hard");
        }
    }
}
