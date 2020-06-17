using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json.Linq;

public class JsonParser : MonoBehaviour{

    public static T[] FromJson<T>(string json){

        string newJson = "{ \"array\": " + json + "}";
        JToken jToken = JToken.Parse(newJson);
        Wrapper<T> wrapper = jToken.ToObject<Wrapper<T>>();
        return wrapper.array;
    }

    [System.Serializable]
    private class Wrapper<T>
    {

        public T[] array;
    }
}
