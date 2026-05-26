/*
 * File: EditorWindowUtils.cs
 * Created: 24/05/2026, 2:25:09 PM
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEditor;

public static class EditorWindowUtils {
    // ------------------------------------------------------------------------
    // Functions
    // ------------------------------------------------------------------------
    public static T[] FindScriptableObjectData<T> () where T : ScriptableObject {
        string typeName = typeof(T).Name;
        string[] guids = AssetDatabase.FindAssets("t:" + typeName);
        List<T> objs = new List<T>();
        foreach(string guid in guids) {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Assert.IsNotNull(path);
            T obj =
                (T)AssetDatabase.LoadAssetAtPath(
                    path,
                    typeof(T)
                );
            Assert.IsNotNull(obj);
            objs.Add(obj);

            //Debug.Log("found ScriptableObject data: " + obj.ToString());
        }
        return objs.ToArray();
    }

    // ------------------------------------------------------------------------
    public static bool IEnumerableContentEqual<TSource> (IEnumerable<TSource> a, IEnumerable<TSource> b) {
        return a.OrderBy(o => o).SequenceEqual(b.OrderBy(o => o));
    }
}