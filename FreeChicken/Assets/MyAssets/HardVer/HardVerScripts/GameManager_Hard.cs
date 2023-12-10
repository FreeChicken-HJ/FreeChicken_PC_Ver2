using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager_Hard : MonoBehaviour
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
    public bool isEasyVer;

    public AudioSource MainBGM;
    public AudioSource SFX;
    public GameObject mainUI;
   
    public GameObject AudioSettingUI;
    public GameObject Control_UI;
    public GameObject WarnningUI;
    public GameObject ExitUI;
    public GameObject LoadingUI;
   
    public LocaleManager LocaleManager;
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
        else if(File.Exists("PlayerData_Hard.json"))
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
                //isFactory_1 = true;
            }
            else if(factoryPlayer2 != null)
            {
                menuSet.SetActive(true);
                factoryPlayer2.BGM.Pause();
                Time.timeScale = 0f;
                factoryPlayer2.isTalk = true;
                //isFactory_2 = true;
            }
            else if(factoryPlayer3!=null)
            {
                menuSet.SetActive(true);
                factoryPlayer3.BGM.Pause();
                Time.timeScale = 0f;
                factoryPlayer3.isTalk = true;
                //isFactory_3 = true;
            }
            else if(housePlayer1!=null)
            {
                menuSet.SetActive(true);
                housePlayer1.mainAudio.Pause();
                housePlayer1.runAudio.Pause();
                Time.timeScale = 0f;
                housePlayer1.isTalk = true;
                //isHouse_1 = true;
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
                cityPlayer.bgm.Pause();
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
    public void ContinueGame()
    {
       
        menuSet.SetActive(false);
        Time.timeScale = 1;

        if (isFactory_1 && factoryPlayer1 != null)
        {
            Cursor.visible = false;
            factoryPlayer1.mainAudio.UnPause();
            factoryPlayer1.runAudio.UnPause();
            factoryPlayer1.isTalk = false;
        }
        else if (isFactory_2 && factoryPlayer2 != null)
        {
            Cursor.visible = false;
            factoryPlayer2.BGM.UnPause();
            factoryPlayer2.isTalk = false;
        }
        else if (isFactory_3 && factoryPlayer3 != null)
        {
            Cursor.visible = false;
            factoryPlayer3.BGM.UnPause();
            factoryPlayer3.isTalk = false;
        }
        else if (isHouse_1 && housePlayer1 != null)
        {
            Cursor.visible = false;
            housePlayer1.mainAudio.UnPause();
            housePlayer1.runAudio.UnPause();
            housePlayer1.isTalk = false;
        }
        else if (isHouse_2 && housePlayer2 != null)
        {
            Cursor.visible = false;
            housePlayer2.mainAudio.UnPause();
            housePlayer2.runAudio.UnPause();
            housePlayer2.isTalk1 = false;
            housePlayer2.isTalk2 = false;
        }
        else if (isHouse_2 && evolutionPlayer != null)
        {
            Cursor.visible = false;
            evolutionPlayer.mainAudio.UnPause();
            evolutionPlayer.runAudio.UnPause();
            evolutionPlayer.isTalk2 = false;
        }
        else if (isCity && cityPlayer != null)
        {
            Cursor.visible = false;
            cityPlayer.bgm.UnPause();
            cityPlayer.startAudio.UnPause();
            cityPlayer.isAllStop = false;
        }
        else if (isCave && cavePlayer != null)
        {
            Cursor.visible = false;
            cavePlayer.mainAudio.UnPause();
            cavePlayer.isTalk = false;
        }

      
        else if (isMain)
        {

            if (MainBGM != null)
            {
                //Cursor.visible = true;
                MainBGM.UnPause();
            }
        }
    }
      
    public void GameExit()
    {

        Application.Quit();
    }
    public void Enter()
    {
        LoadingSceneManager.LoadScene("Enter2DScene_Hard");
      
    }
    public void Enter2dScene()
    {

        Time.timeScale = 1f;
        MemoryCount.memCount = 0;

        LoadingSceneManager.LoadScene("Enter2DScene_Hard");
    }
   
    public void AudioSettingScene()
    {
        AudioSettingUI.SetActive(true);
    }

    public void StartScene()
    {
        Time.timeScale = 1f;
        LoadingSceneManager.LoadScene("StartScene");
    }
    public void StartScene_2DEnter()
    {
        
        Invoke("StartRealScene2", 0.35f);
        
    }
    public void StartRealScene2()
    {
        if (File.Exists("PlayerData_Hard.json"))
        {
            string jsonData = File.ReadAllText("PlayerData_Hard.json");
            PlayerData loadedData = JsonUtility.FromJson<PlayerData>(jsonData);

            GameSave.HardLevel = loadedData.LevelChk;


            LoadingSceneManager.LoadScene("Enter2DScene_Hard");
           
          
        }
        else
        {

            if (isEnglish)
            {
                if (LocaleManager != null)
                {
                    LocaleManager = GetComponent<LocaleManager>();
                    LocaleManager.ChangeLocale(0);
                }
            }
            else if (!isEnglish)
            {
                if (LocaleManager != null)
                {
                    LocaleManager = GetComponent<LocaleManager>();
                    LocaleManager.ChangeLocale(1);
                }
            }
            LoadingSceneManager.LoadScene("Enter2DScene_Hard");
           
        }
    }
    public void Enter2DExit()
    {
        PlayerData playerData = new PlayerData();
        playerData.LevelChk = GameSave.HardLevel;
        
        if (PlayerData.isEnglish)
        {
            playerData.isEng = true;
        }
        else if(!PlayerData.isEnglish)   
        {
            playerData.isEng = false;
        }
        string json = JsonUtility.ToJson(playerData);

        File.WriteAllText("PlayerData_Hard.json", json);


        
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
        if (File.Exists("PlayerData_Hard.json") || File.Exists("PlayerData_Easy.json"))
        {

            string jsonData_H = File.ReadAllText("PlayerData_Hard.json");
            PlayerData loadedData_H = JsonUtility.FromJson<PlayerData>(jsonData_H);
            loadedData_H.LevelChk = 0;

            string jsonData_E = File.ReadAllText("PlayerData_Easy.json");
            PlayerData loadedData_E = JsonUtility.FromJson<PlayerData>(jsonData_E);
            loadedData_E.LevelChk = 0;
        }
        File.Delete("PlayerData_Hard.json");
        File.Delete("PlayerData_Easy.json");
       
    }
   
    public void Controls()
    {
        Control_UI.SetActive(true);
    }
    public void Warnning()
    {
        if(WarnningUI != null) WarnningUI.SetActive(true);
    }
    public void WarnningExit()
    {
        WarnningUI.SetActive(false);
    }
    public void ControlsExit()
    {
        Control_UI.SetActive(false);
    }
  
    public void ExitShow()
    {
        if (ExitUI != null) ExitUI.SetActive(true);
       
    }
    public void ExitEnd()
    {
        ExitUI.SetActive(false);
    }
    public void ReplayGame()
    {
        Time.timeScale = 1f;
        if (isFactory_1)
        {
            MemoryCount.memCount = 0;
            LoadingSceneManager.LoadScene("FactoryScene_1_Hard");
            
        }
        else if (isFactory_2)
        {
            if (MemoryCount.memCount >= 2)
            {
                MemoryCount.memCount = 2;
            }
            LoadingSceneManager.LoadScene("FactoryScene_2_Hard");
        }
        else if (isFactory_3)
        {
            if (MemoryCount.memCount >= 4)
            {
                MemoryCount.memCount = 4;
            }
            LoadingSceneManager.LoadScene("FactoryScene_3_Hard");
        }
        else if (isHouse_1)
        {
            MemoryCount.memCount = 0;
            LoadingSceneManager.LoadScene("HouseScene_1_Hard");

        }
        else if (isHouse_2)
        {
            LoadingSceneManager.LoadScene("HouseScene_2_Hard");
        }
        else if (isCity)
        {
            LoadingSceneManager.LoadScene("CityScene_Hard");
        }
        else if (isCave)
        {
            LoadingSceneManager.LoadScene("CaveScene_Hard");
        }
    }

    public void ControlsUI()
    {
        if (isMain)
        {
            if(mainUI != null)
            {
                mainUI.gameObject.SetActive(true);
            }
        }
    }

    public void ClickButtonSound()
    {
        ClickButtonAudio.Play();
    }
}