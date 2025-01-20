using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class LogText
{
    [TextArea]
    public string log;
}

public class Dialogue : MonoBehaviour
{
    static private string[] dialogue;
    bool firstTalk = false;

    [Header("���� ��ư")]
    [SerializeField]
    private GameObject questionPanel;
    [SerializeField]
    private Button questionButton1;
    [SerializeField]
    private Button questionButton2;

    [Header("��ȭ��")]
    private int count = 0; // ��ȭ�� �󸶳� ���� �Ǿ����� Ȯ��
    [SerializeField]
    TMP_Text txt_dialogue;
    static public bool isDialogue = true;
    static public int npcTalkCnt = 0;


    private void Awake()
    {
        dialogue = new String[100];

        //ó�� intro ���
        dialogue[0] = "�ȳ��ϼ��� �÷��̾�� �ݰ�����";
        dialogue[1] = "���ǽ�Ÿ�� ��Ÿ���� ������ ���Ű� ȯ���ؿ�";
        dialogue[2] = "end";

        //main ���
        dialogue[3] = "[������ ���ƴٴϴ� ��...]";
        dialogue[4] = "���Ͻô� ������ ������ �ּ���";
        dialogue[5] = "end";

        dialogue[6] = "";

        //����1
        dialogue[7] = "<���۹��>�� �ñ��Ͻñ���";
        dialogue[8] = "�̵� :����Ű�� asdw�� ����ؼ� �̵��� �� �ֽ��ϴ�." +
                      "\n    �����̽��ٷ� ����, shift�� �޸� �� �־��" +
                      "\n ȸ�� : ȭ���� �����ڸ��� ���콺�� �̵��� ������";
        dialogue[9] = "M�� �����ø� ��ü ���� Ȯ�� �� �� �־��";
        dialogue[10] = "���Ͻô� ����� �ִٸ� �������� īī��ä�η� �������ּ���!";
        dialogue[11] = "end";

        //����2
        dialogue[12] = "�׷� ���� �ð� ��������";
        dialogue[13] = "end";

        QuestionSetActive(false);
    }

    private void Start()
    {
        NextDialogue();
    }

    public void TalkNPC()
    {
        QuestionSetActive(true);
        count = 3;
        txt_dialogue.text = dialogue[count];
    }

    private void NextDialogue()
    {
        txt_dialogue.text = dialogue[count];
        count++;
    }

    public void ClickQuestionControl()
    {
        isDialogue = true;
        QuestionSetActive(false);
        count = 7;
        npcTalkCnt = 0;
        if(count<dialogue.Length)
        {
            NextDialogue();
        }
    }

    public void ClickQuestionExit()
    {
        npcTalkCnt = 1;
    }

    public void QuestionSetActive(bool set)
    {
        questionPanel.gameObject.SetActive(set);
        questionButton1.gameObject.SetActive(set);
        questionButton2.gameObject.SetActive(set);
    }

    private void Update()
    {
        if(npcTalkCnt == 1 & firstTalk == false)
        {
            TalkNPC();
            firstTalk = true;
        }
        else if(npcTalkCnt == 2 )
        {
            QuestionSetActive(false);
            firstTalk = false;
        }

        if(isDialogue && count != 3)
        {
            if (dialogue[count].Equals("end"))
            {
                if(count<dialogue.Length)
                {
                    NextDialogue();
                }
            }
        }
    }
}
