using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    FactoryPlayer factoryPlayer1;
    FactoryPlayer_2 factoryPlayer2;
    FactoryPlayer_3 factoryPlayer3;
    HouseScenePlayer housePlayer1;
    HouseScene2_Player housePlayer2;
    EvloutionPlayer evolutionPlayer;
    CityScenePlayer cityPlayer;
    CaveScenePlayer cavePlayer;

    public GameObject menuSet;
    public AudioSource ClickButtonAudio;
   
    public bool isStartScene;
    public bool isFactory_1;
    public bool isFactory_2;
    public bool isFactory_3;
    public bool isHouse_1;
    public bool isHouse_2;
    public bool isCity;
    public bool isCave;
    public bool isMain;

    public bool isLoading;
    public bool isStart;
    public bool isEnglish;

    public bool is2D;

    public AudioSource MainBGM;
    public AudioSource SFX;
    public GameObject mainUI;
   
    public GameObject AudioSettingUI;
    public GameObject Control_UI;
    public GameObject WarnningUI;
    public GameObject ExitUI;
    public GameObject LoadingUI;
   
    public LocaleManager LocaleManager;

    public GameObject canvasObjs;
    string playerData_Easy = "PlayerData_Easy.json";
    string playerData_Hard = "PlayerData_Hard.json";

    void Start()
    {
       
        if (File.Exists(playerData_Easy))
        {

            string jsonData = File.ReadAllText(playerData_Easy);
            PlayerData loadedData = JsonUtility.FromJson<PlayerData>(jsonData);

            isEnglish = loadedData.isEng;

        }
        else if (File.Exists(playerData_Hard))
        {
            string jsonData = File.ReadAllText(playerData_Hard);
            PlayerData loadedData = JsonUtility.FromJson<PlayerData>(jsonData);

            isEnglish = loadedData.isEng;

        }

        if (isEnglish)
        {
            if (LocaleManager != null)
            {
                PlayerData.isEnglish = true;
                LocaleManager = GetComponent<LocaleManager>();
                LocaleManager.ChangeLocale(0);
            }
        }

        else if (!isEnglish)
        {
            if (LocaleManager != null)
            {
                PlayerData.isEnglish = false;
                LocaleManager = GetComponent<LocaleManager>();
                LocaleManager.ChangeLocale(1);
            }
        }
    }

    public void SetKorean()
    {
        PlayerData.isEnglish = false;
    }
    public void SetEnglish()
    {

        PlayerData.isEnglish = true;
    }
    public void MainUIControlExit()
    {
          mainUI.SetActive(false);
    }
  
    public void GameExit()
    {
        Application.Quit();
    }

   
    public void AudioSettingScene()
    {
        AudioSettingUI.SetActive(true);
    }

    public void StartScene1()
    {
        Time.timeScale = 1f;
        LoadingSceneManager.LoadScene("StartScene");
    }
    public void StartScene2_Easy()
    {
        StartRealScene2_Easy();
    }

    public void StartScene2_Hard()
    {
        StartRealScene2_Hard();
    }
    public void StartRealScene2_Easy()
    {
        PlayerData.isEasyVer = true;
        
        if (File.Exists(playerData_Easy) || File.Exists(playerData_Hard))
        {
            if (File.Exists(playerData_Easy))
            {
                string jsonData = File.ReadAllText(playerData_Easy);
                PlayerData loadedData = JsonUtility.FromJson<PlayerData>(jsonData);

                GameSave.EasyLevel = loadedData.LevelChk;
            }
            LoadingSceneManager.LoadScene("Enter2DScene_Easy");

        }
        else
        {
            if (PlayerData.isEnglish)
            {
                if (LocaleManager != null)
                {
                    LocaleManager = GetComponent<LocaleManager>();
                    LocaleManager.ChangeLocale(0);
                }
            }
            else if (!PlayerData.isEnglish)
            {
                if (LocaleManager != null)
                {
                    LocaleManager = GetComponent<LocaleManager>();
                    LocaleManager.ChangeLocale(1);
                }
            }
            LoadingSceneManager.LoadScene("StartSceneShow");
        }
    }

    public void StartRealScene2_Hard()
    {
        PlayerData.isEasyVer = false;
        if (File.Exists(playerData_Hard) || File.Exists(playerData_Easy))
        {
            if (File.Exists(playerData_Hard))
            {
                string jsonData = File.ReadAllText(playerData_Hard);
                PlayerData loadedData = JsonUtility.FromJson<PlayerData>(jsonData);

                GameSave.HardLevel = loadedData.LevelChk;

            }
            LoadingSceneManager.LoadScene("Enter2DScene_Hard");
        }
        else
        {

            if (PlayerData.isEnglish)
            {
                if (LocaleManager != null)
                {
                    LocaleManager = GetComponent<LocaleManager>();
                    LocaleManager.ChangeLocale(0);
                }
            }
            else if (!PlayerData.isEnglish)
            {
                if (LocaleManager != null)
                {
                    LocaleManager = GetComponent<LocaleManager>();
                    LocaleManager.ChangeLocale(1);
                }
            }
            LoadingSceneManager.LoadScene("StartSceneShow");
           
        }
    }
    
    public void ReSetEveryThing()
    {
        isEnglish = true;

        if (LocaleManager != null)
        {
            LocaleManager = GetComponent<LocaleManager>();
            LocaleManager.ChangeLocale(0);
            PlayerData.isEnglish = true;
        }
        GameSave.HardLevel = 0;
        GameSave.EasyLevel = 0;
        if (File.Exists(playerData_Hard))
        {

            string jsonData_H = File.ReadAllText(playerData_Hard);
            PlayerData loadedData_H = JsonUtility.FromJson<PlayerData>(jsonData_H);
            loadedData_H.LevelChk = 0;

        }
        else if (File.Exists(playerData_Easy))
        {

            string jsonData_E = File.ReadAllText(playerData_Easy);
            PlayerData loadedData_E = JsonUtility.FromJson<PlayerData>(jsonData_E);
            loadedData_E.LevelChk = 0;
        }

        File.Delete(playerData_Hard);
        File.Delete(playerData_Easy);
    }
   
    
    public void Warnning()
    {
        if(WarnningUI != null)
        {
            WarnningUI.SetActive(true);
            canvasObjs.SetActive(false);
        }
            
    }
    public void WarnningExit()
    {
        WarnningUI.SetActive(false);
        canvasObjs.SetActive(true);
    }
   
    public void ExitShow()
    {
        if (ExitUI != null)
        {
            ExitUI.SetActive(true);
            canvasObjs.SetActive(false);
        }    
    }
    public void ExitEnd()
    {
        ExitUI.SetActive(false);
        canvasObjs.SetActive(true);
    }
   

    public void ClickButtonSound()
    {
        ClickButtonAudio.Play();
    }
}
