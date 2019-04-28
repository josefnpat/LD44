using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    public static string typeName(object obj) {
        if(obj == null) return "null";
        return obj.GetType().ToString();
    }
}
