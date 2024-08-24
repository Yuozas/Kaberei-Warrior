using MessagePack;
using System;
[MessagePackObject]
public class SaveFile
{
    [Key(0)]
    public DateTime Timestamp { get; set; }
    [Key(1)]
    public float PlayerPositionX;
    [Key(2)]
    public float PlayerPositionY;
    [Key(3)]
    public string CurrentScene;

}
