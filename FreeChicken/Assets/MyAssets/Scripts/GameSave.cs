using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class GameSave : MonoBehaviour
{
    public bool isChk;
    public static bool isFactory;
    public static bool isHouse;
    public static bool isCity;
    public static bool isCave;
    public GameObject Factory; // 1
    public GameObject House;   // 2
    public GameObject City;   // 3
    public GameObject Cave;   // 4

    public AudioSource ShowSound;
    public GameObject[] Objects;
    public ParticleSystem ShowParticle_1;
    public ParticleSystem ShowParticle_2;
    public ParticleSystem ShowParticle_3;
    public ParticleSystem ShowParticle_4;

    public static int HardLevel;
    public static int EasyLevel;
    public bool isExist;
    public bool isEasy;
    private void Start()
    {
        Cursor.visible = true;
        if (!isEasy && File.Exists("PlayerData_Hard.json"))
        {
            isExist = true;

            string jsonData = File.ReadAllText("PlayerData_Hard.json");
            PlayerData loadedData = JsonUtility.FromJson<PlayerData>(jsonData);
           
            if (loadedData.LevelChk >= HardLevel)
            {
                HardLevel = loadedData.LevelChk;
            }
        }
        else if (isEasy && File.Exists("PlayerData_Easy.json"))
        {
            isExist = true;
           
            string jsonData = File.ReadAllText("PlayerData_Easy.json");
            PlayerData loadedData = JsonUtility.FromJson<PlayerData>(jsonData);
            if (loadedData.LevelChk >= EasyLevel)
            {
                EasyLevel = loadedData.LevelChk;
            }
            
        }
        if (!isEasy)
        {
            for (int i = 1; i < HardLevel; i++)
            {
                Objects[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 1; i < EasyLevel; i++)
            {
                Objects[i].SetActive(true);
            }
        }
        ShowSound.Play();
    }



    public void Update()
    {
        if (isEasy)
        {
            if(EasyLevel == 0 && !isChk)
            {           
                ShowParticle_1.Play();                
                isChk = true;
            }
            if (EasyLevel == 2 && !isChk)
            {
                House.SetActive(true);
               
                ShowParticle_2.Play();
                SetFile();
                isChk = true;
            }
            if (EasyLevel == 3 && !isChk)
            {
                House.SetActive(true);
                City.SetActive(true);
               
                ShowParticle_3.Play();
                SetFile();
                isChk = true;
            }
            if (EasyLevel == 4 && !isChk)
            {
                House.SetActive(true);
                City.SetActive(true);
                Cave.SetActive(true);
              
                ShowParticle_4.Play();
                SetFile();
                isChk = true;
            }
        }
        else
        {
            if (HardLevel == 0 && !isChk)
            {
                ShowParticle_1.Play();
                isChk = true;
            }
            if (HardLevel == 2 && !isChk)
            {
                House.SetActive(true);
              
                ShowParticle_2.Play();
                SetFile();
                isChk = true;
            }
            if (HardLevel == 3 && !isChk)
            {
                House.SetActive(true);
                City.SetActive(true);
                
                ShowParticle_3.Play();
                SetFile();
                isChk = true;
            }
            if (HardLevel == 4 && !isChk)
            {
                House.SetActive(true);
                City.SetActive(true);
                Cave.SetActive(true);
               
                ShowParticle_4.Play();
                SetFile();
                isChk = true;
            }
        }
    }
    public void SetFile()
    {
        if (isEasy)
        {
            PlayerData playerData = new PlayerData();
            playerData.LevelChk = EasyLevel;

            if (PlayerData.isEnglish)
            {
                playerData.isEng = true;
            }
            else if (!PlayerData.isEnglish)
            {
                playerData.isEng = false;
            }
            string json = JsonUtility.ToJson(playerData);

            File.WriteAllText("PlayerData_Easy.json", json);
        }
        else
        {
            PlayerData playerData = new PlayerData();
            playerData.LevelChk = HardLevel;

            if (PlayerData.isEnglish)
            {
                playerData.isEng = true;
            }
            else if (!PlayerData.isEnglish)
            {
                playerData.isEng = false;
            }
            string json = JsonUtility.ToJson(playerData);

            File.WriteAllText("PlayerData_Hard.json", json);
        }
    }
}