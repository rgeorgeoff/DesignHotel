using System.Runtime.Serialization;
using UnityEngine;

sealed class ColorSerializationSurrogate : ISerializationSurrogate
{
    // Method called to serialize a Color object
    public void GetObjectData(System.Object obj,
        SerializationInfo info, StreamingContext context)
    {
        Color c = (Color) obj;
        info.AddValue("a", c.a);
        info.AddValue("b", c.b);
        info.AddValue("g", c.g);
        info.AddValue("r", c.r);
    }

    // Method called to deserialize a Color object
    public System.Object SetObjectData(System.Object obj,
        SerializationInfo info, StreamingContext context,
        ISurrogateSelector selector)
    {
        Color c = (Color) obj;
        c.a = (float) info.GetValue("a", typeof(float));
        c.b = (float) info.GetValue("b", typeof(float));
        c.g = (float) info.GetValue("g", typeof(float));
        c.r = (float) info.GetValue("r", typeof(float));
        obj = c;
        return obj; // Formatters ignore this return value //Seems to have been fixed!
    }
}