using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

static class BinaryFormaterWithSurrogates
{
    public static BinaryFormatter GetBFInstance()
    {
        BinaryFormatter bf = new BinaryFormatter();
        SurrogateSelector ss = new SurrogateSelector();

        //Add all surrogates to the surrogate selector
        Vector3IntSerializationSurrogate v3Iss = new Vector3IntSerializationSurrogate();
        ss.AddSurrogate(typeof(Vector3Int),
            new StreamingContext(StreamingContextStates.All),
            v3Iss);

        Vector3SerializationSurrogate v3ss = new Vector3SerializationSurrogate();
        ss.AddSurrogate(typeof(Vector3),
            new StreamingContext(StreamingContextStates.All),
            v3ss);

        Vector2SerializationSurrogate v2ss = new Vector2SerializationSurrogate();
        ss.AddSurrogate(typeof(Vector2),
            new StreamingContext(StreamingContextStates.All),
            v2ss);

        ColorSerializationSurrogate colorss = new ColorSerializationSurrogate();
        ss.AddSurrogate(typeof(Color),
            new StreamingContext(StreamingContextStates.All),
            colorss);

        //Have the formatter use our surrogate selector
        bf.SurrogateSelector = ss;
        return bf;
    }
}