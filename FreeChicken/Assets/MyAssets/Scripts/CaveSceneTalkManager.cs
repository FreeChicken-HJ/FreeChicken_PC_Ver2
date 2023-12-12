using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Cinemachine;

public class CaveSceneTalkManager : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject nextText;

    public Queue<string> sentences;
    private string currentSentences;
    public bool isTyping;

    public static CaveSceneTalkManager instance;
    public GameObject NpcImage;
    public GameObject PlayerImage;
    public bool isNPCImage;
    public bool isPlayerImage;
    CaveScenePlayer Player;
    public bool isTalkEnd;
    public AudioSource TalkSound;
    public AudioSource ClickButtonSound;

    public CinemachineVirtualCamera NpcCam;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        sentences = new Queue<string>();
        isTalkEnd = false;
        isPlayerImage = true;
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<CaveScenePlayer>();
        Player.isTalk = true;
        Cursor.visible = true;

        NpcImage.SetActive(false);
        PlayerImage.SetActive(false);
    }

    public void OndiaLog(string[] lines)
    {
        sentences.Clear();

        foreach (string line in lines)
        {
            sentences.Enqueue(line);
        }
    }

    public void NextSentence()
    {
        if (sentences.Count != 0)
        {
            currentSentences = sentences.Dequeue();
            isTyping = true;
            nextText.SetActive(false);
            TalkSound.Play();
            StartCoroutine(Typing(currentSentences));
        }

        if (sentences.Count == 0)
        {
            Destroy(instance.gameObject);
            if (NpcCam != null) { NpcCam.Priority = -100; }

            Player.isTalk = false;
            Cursor.visible = false;
        }
    }

    void ChangeImage()
    {
        if (isNPCImage)
        {
            isNPCImage = false;
            NpcImage.gameObject.SetActive(false);
            PlayerImage.gameObject.SetActive(true);
            isPlayerImage = true;
        }
        else if (isPlayerImage)
        {
            isNPCImage = true;
            NpcImage.gameObject.SetActive(true);
            PlayerImage.gameObject.SetActive(false);
            isPlayerImage = false;
        }
    }

    IEnumerator Typing(string line)
    {
        text.text = "";
        foreach (char ch in line.ToCharArray())
        {
            text.text += ch;
            yield return new WaitForSeconds(0.0001f);
        }
    }

    void Update()
    {
        if (text.text.Equals(currentSentences))
        {
            nextText.SetActive(true);
            isTyping = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isTyping)
            {
                
                NextSentence();
                ClickButtonSound.Play();
                ChangeImage();
            }
        }
    }
    public void OnClick()
    {
        if (!isTyping)
        {

            NextSentence();
            ClickButtonSound.Play();
            if (sentences.Count == 0)
            {
                NpcImage.SetActive(false);
                PlayerImage.SetActive(false);
            }
            ChangeImage();

        }
    }

}
