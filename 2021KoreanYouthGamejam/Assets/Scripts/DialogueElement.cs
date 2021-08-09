using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class DialogueElement
{
    public string name; // 캐릭터 이름
    public string[] txt; // 캐릭터 대사
    public bool end;
}

[Serializable]
public class DialogueElements
{
    public DialogueElement[] elements;
}