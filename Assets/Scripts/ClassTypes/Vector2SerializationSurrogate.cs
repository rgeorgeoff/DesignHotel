using System.Runtime.Serialization;
using UnityEngine;

sealed class Vector2SerializationSurrogate : ISerializationSurrogate
{
    // Method called to serialize a Vector2 object
    public void GetObjectData(System.Object obj,
        SerializationInfo info, StreamingContext context)
    {
        Vector2 v2 = (Vector2) obj;
        info.AddValue("x", v2.x);
        info.AddValue("y", v2.y);
    }

    // Method called to deserialize a Vector2 object
    public System.Object SetObjectData(System.Object obj,
        SerializationInfo info, StreamingContext context,
        ISurrogateSelector selector)
    {
        Vector2 v3 = (Vector2) obj;
        v3.x = (int) info.GetValue("x", typeof(int));
        v3.y = (int) info.GetValue("y", typeof(int));
        obj = v3;
        return obj; // Formatters ignore this return value //Seems to have been fixed!
    }
}