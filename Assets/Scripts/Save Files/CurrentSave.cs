using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CurrentSave
{
    public string SaveFile { get; set; }

    private static readonly Lazy<CurrentSave>
        lazy =
        new Lazy<CurrentSave>
            (() => new CurrentSave());

    public static CurrentSave Instance { get { return lazy.Value; } }

    private CurrentSave()
    {
    }
}