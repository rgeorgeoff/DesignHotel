using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataStoreManager : IManager
{
    void Save<T>(T obj, string savePath);
    T Load<T>(string savePath);
    void UpdatedModels(string savePath);
}