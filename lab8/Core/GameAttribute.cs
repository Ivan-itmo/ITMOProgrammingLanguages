namespace Core;

using System;

[AttributeUsage(AttributeTargets.Class)]
public class GameAttribute : Attribute
{
    public string Name { get; }
    public GameAttribute(string name) => Name = name;
}