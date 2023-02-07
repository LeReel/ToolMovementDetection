using UnityEngine;
 
public class CustomNamedArrayAttribute : PropertyAttribute
{
    public readonly string[] names;
    public CustomNamedArrayAttribute(string[] names) { this.names = names; }
}