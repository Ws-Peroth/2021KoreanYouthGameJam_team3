using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Event
{
    public string name = "";
    public string[] text;
}

[Serializable]
public class Events
{
    public Event[] elements;
}