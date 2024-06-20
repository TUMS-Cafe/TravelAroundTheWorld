using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBar : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI nameTxt;
    [SerializeField] TypeEffect talk;
    string[] str = { "HI", "HIHIHI", "HI HI HI H I HIHI " };

    void Start()
    {
        ActiveDialogue(0, "JPB", ref str);
    }

    
    void Update()
    {
        
    }

    void ActiveDialogue(int idx, string nameData, ref string[] talkData)
    {
        if(nameData == null && talkData == null)
        {
            //데이터 안가져와졌으므로 데이터 관리하는 곳에서 해당 부분에 맞는 데이터 가져오기
        }

        nameTxt.text = nameData;

        for(int i=0; i<talkData.Length; i++)
        {
            talk.SetMsg(talkData[i]);
            //특정 키 누르면 다음 대화 진행 가능.
            //대화 스킵 여부는 논의 후 결정 (InputSystem에 그러면 추후 추가해야 함)
        }

        
        
    }


    //상호작용하면 대화창 활성화
    /*
     * UI Manager에서 플레이어와 다른 오브젝트간 충돌 / 특정 대화 이벤트 발생시 활성화
     dialogueBar.SetAcvite(true);
     */
    
}
