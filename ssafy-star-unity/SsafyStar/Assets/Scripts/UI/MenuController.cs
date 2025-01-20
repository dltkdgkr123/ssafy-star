using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Windows;
using JetBrains.Annotations;
using Unity.VisualScripting.Antlr3.Runtime;



public class MenuController : MonoBehaviour
{
    public static MenuController Instance;

    [Header("Mute Button")]
    [SerializeField]
    private Sprite muteSprite;
    [SerializeField]
    private Sprite unmuteSprite;
    private bool muted;

    [Header("WebGL")]
    private string _nickname = "";
    private string _token = "";

    [Header("NickNameUI")]
    [SerializeField]
    private GameObject panelNickname;
    [SerializeField]
    private TMP_Text textNickname;
    [SerializeField]
    private TMP_InputField textResult;

    [Header("Warning")]
    [SerializeField]
    private GameObject panelWarning;

    private UIDocument doc;
    private UnityEngine.UIElements.Button btnMute;
    private VisualElement cardPlay;
    private VisualElement cardGuest;

    private bool isGuest = false;

    [DllImport("__Internal")]
    private static extern void GetUser(int accessNumber);

    private void Awake()
    {
        Instance = this;

        doc = GetComponent<UIDocument>();

        btnMute = doc.rootVisualElement.Q<UnityEngine.UIElements.Button>("ButtonMute");
        cardPlay = doc.rootVisualElement.Q<VisualElement>("CardPlay");
        cardGuest = doc.rootVisualElement.Q<VisualElement>("CardGuest");

        cardPlay.AddManipulator(new Clickable(BtnPlayOnClicked));
        cardGuest.AddManipulator(new Clickable(BtnGuestOnClicked));
        btnMute.clicked += BtnMuteOnClicked;
    }

    private void Update()
    {
        if(panelNickname.activeSelf)
        {
            if(UnityEngine.Input.GetKeyDown(KeyCode.Return))
            {
                CheckDuplicate();
            }
        }
    }

    //Unity���� �÷��� ��ư�� �������� ����Ǵ� �Լ�
    private void BtnPlayOnClicked()
    {
        isGuest = false;
        //webGL���� react�� ���� ����
#if UNITY_WEBGL == true && UNITY_EDITOR == false
    GetUser(100);
#endif
#if UNITY_EDITOR == true
        panelWarning.SetActive(true);
#endif
    }

    //Unity���� �Խ�Ʈ�� �÷��� ��ư�� �������� ����Ǵ� �Լ�
    private void BtnGuestOnClicked()
    {
        isGuest = true;
        OpenNickNamePanel();
    }

    //React���� �α��� ������ �����ֱ� ���� �����ϴ� �Լ�
    public void GetLogin(string token)
    {
        if (token != "")
        {
            //Debug.Log("�α��� �� email:" + token);
            _token = token;
        }
        else
        {
            //Debug.Log("�α��� ���� ����");
            panelWarning.SetActive(true);
            _token = "";
        }
    }

    //React���� �г��� ������ �����ֱ� ���� �����ϴ� �Լ�
    public void GetNickname(string nickname = "")
    {
        if (nickname == "")
        {
            OpenNickNamePanel();
        }
        else
        {
            _nickname = nickname;
            PlayerPrefs.SetString("Nickname", _nickname);
            SceneManager.LoadScene("Lobby");
        }
    }

    public void OpenNickNamePanel()
    {
        panelNickname.SetActive(true);
    }

    public void CheckDuplicate()
    {
        _nickname = textNickname.text;
        if (_nickname == "")
        {
            textResult.gameObject.SetActive(true);
            textResult.text = "�г����� �Է����ּ���";
        }
        else if (_nickname.Length > 10)
        {
            textResult.gameObject.SetActive(true);
            textResult.text = "10���� �̳��� �Է����ּ���";
            _nickname = "";
        }
        else
        {
            StartCoroutine(Request.Instance.ApiGetRequest($"/app/user/nickname/check-duplicate?nickname={_nickname}",
                                                          _nickname, "nickname", _token));
        }
    }

    public void PrintError(string message)
    {
        if (message == "�ߺ�")
        {
            textResult.gameObject.SetActive(true);
            textResult.text = "�ߺ��� �г����Դϴ�.";
        }
        else if (message == "����")
        {
            textResult.gameObject.SetActive(true);
            textResult.text = "�г��� ��Ͽ� �����߽��ϴ�.";
        }
    }

    public void SetNickName()
    {
        _nickname = textNickname.text;

        if (isGuest)
        {
            PlayerPrefs.SetString("Nickname",_nickname);
            SceneManager.LoadScene("Lobby");
        }
        else
        {
            StartCoroutine(Request.Instance.ApiPatchRequest("/app/user", _nickname, "nickname", _token));
        }
    }

    private void SetText(string message)
    {
        if (!textResult.IsActive())
        {
            textResult.gameObject.SetActive(true);
        }

        textResult.text = message;
    }

    private void BtnMuteOnClicked()
    {
        muted = !muted;
        var bg = btnMute.style.backgroundImage;
        bg.value = Background.FromSprite(muted ? muteSprite : unmuteSprite);
        btnMute.style.backgroundImage = bg;

        AudioListener.volume = muted ? 0 : 1;
    }

    public void CloseGameObject(GameObject go)
    {
        go.SetActive(false);
    }
}
