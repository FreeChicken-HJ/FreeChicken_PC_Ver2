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

    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            factoryPlayer1 = GameObject.FindGameObjectWithTag("Player").GetComponent<FactoryPlayer>();
            factoryPlayer2 = GameObject.FindGameObjectWithTag("Player").GetComponent<FactoryPlayer_2>();
            factoryPlayer3 = GameObject.FindGameObjectWithTag("Player").GetComponent<FactoryPlayer_3>();
            housePlayer1 = GameObject.FindGameObjectWithTag("Player").GetComponent<HouseScenePlayer>();
            housePlayer2 = GameObject.FindGameObjectWithTag("Player").GetComponent<HouseScene2_Player>();
            evolutionPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<EvloutionPlayer>();
            cityPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<CityScenePlayer>();
            cavePlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<CaveScenePlayer>();
        }

        if (File.Exists("PlayerData_Easy.json"))
        {

            string jsonData = File.ReadAllText("PlayerData_Easy.json");
            PlayerData loadedData = JsonUtility.FromJson<PlayerData>(jsonData);

            isEnglish = loadedData.isEng;

        }
        else if (File.Exists("PlayerData_Hard.json"))
        {
            string jsonData = File.ReadAllText("PlayerData_Hard.json");
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
   
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && !isLoading && !isStart)
        {
            ClickButtonAudio.Play();
            Cursor.visible = true;

            if(factoryPlayer1 != null)
            {
                menuSet.SetActive(true);
                factoryPlayer1.mainAudio.Pause();
                factoryPlayer1.runAudio.Pause();
                Time.timeScale = 0f;
                factoryPlayer1.isTalk = true;
            }
            else if(factoryPlayer2 != null)
            {
                menuSet.SetActive(true);
                factoryPlayer2.BGM.Pause();
                Time.timeScale = 0f;
                factoryPlayer2.isTalk = true;
            }
            else if(factoryPlayer3!=null)
            {
                menuSet.SetActive(true);
                factoryPlayer3.BGM.Pause();
                Time.timeScale = 0f;
                factoryPlayer3.isTalk = true;
            }
            else if(housePlayer1!=null)
            {
                menuSet.SetActive(true);
                housePlayer1.mainAudio.Pause();
                housePlayer1.runAudio.Pause();
                Time.timeScale = 0f;
                housePlayer1.isTalk = true;
            }
            else if(housePlayer2!=null)
            {
                menuSet.SetActive(true);
                housePlayer2.mainAudio.Pause();
                housePlayer2.runAudio.Pause();
                Time.timeScale = 0f;
                housePlayer2.isTalk1 = true;
                housePlayer2.isTalk2 = true;
                //isHouse_2 = true;
            }
            else if(evolutionPlayer!=null)
            {
                menuSet.SetActive(true);
                evolutionPlayer.mainAudio.Pause();
                evolutionPlayer.runAudio.Pause();
                Time.timeScale = 0f;
                evolutionPlayer.isTalk2 = true;
                //isHouse_2 = true;
            }
            else if(cityPlayer!=null)
            {
                menuSet.SetActive(true);
                //cityPlayer.BGM.Pause();
                cityPlayer.startAudio.Pause();
                Time.timeScale = 0f;
                cityPlayer.isAllStop= true;
                //isCity = true;
            }
            else if(cavePlayer!=null)
            {
                menuSet.SetActive(true);
                cavePlayer.mainAudio.Pause();
                Time.timeScale = 0f;
                cavePlayer.isTalk= true;
                //isCave = true;
            }
            else if(isMain)
            {
                menuSet.SetActive(true);
                Time.timeScale = 0f;
                /*if (MainBGM != null)
                {
                    MainBGM.Pause();
                }*/
                
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
        
        if (File.Exists("PlayerData_Easy.json") || File.Exists("PlayerData_Hard.json"))
        {
            if (File.Exists("PlayerData_Easy.json"))
            {
                string jsonData = File.ReadAllText("PlayerData_Easy.json");
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
        if (File.Exists("PlayerData_Hard.json") || File.Exists("PlayerData_Easy.json"))
        {
            if (File.Exists("PlayerData_Hard.json"))
            {
                string jsonData = File.ReadAllText("PlayerData_Hard.json");
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
            //LoadingSceneManager.LoadScene("StartSceneShow");
            LoadingSceneManager.LoadScene("Enter2DScene_Hard");

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
        if (File.Exists("PlayerData_Hard.json"))
        {

            string jsonData_H = File.ReadAllText("PlayerData_Hard.json");
            PlayerData loadedData_H = JsonUtility.FromJson<PlayerData>(jsonData_H);
            loadedData_H.LevelChk = 0;

        }
        else if (File.Exists("PlayerData_Easy.json"))
        {

            string jsonData_E = File.ReadAllText("PlayerData_Easy.json");
            PlayerData loadedData_E = JsonUtility.FromJson<PlayerData>(jsonData_E);
            loadedData_E.LevelChk = 0;
        }

        File.Delete("PlayerData_Hard.json");
        File.Delete("PlayerData_Easy.json");
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
