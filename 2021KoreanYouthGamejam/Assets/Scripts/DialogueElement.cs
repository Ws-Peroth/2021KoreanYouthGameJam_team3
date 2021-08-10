using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class DialogueElement
{
    public string name; // 캐릭터 이름
    public string[] txt; // 캐릭터 대사
    public string type; // 일반 대화 혹은 이벤트 대화
    public string image; // 이벤트 대화에서 표시할 일러스트
}

[Serializable]
public class DialogueElements
{
    public DialogueElement[] elements;
}