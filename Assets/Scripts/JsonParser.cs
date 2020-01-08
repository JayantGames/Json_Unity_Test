using System.Collections;
using System.Collections.Generic;
using UnityEngine;   
using System.IO;
using System;
using UnityEngine.UI;

public class JsonParser : MonoBehaviour
{
    public ImageLoader imageLoader;
    public string jsonDataFileName = "JsonFile.json";
    public List<Datum> data;

    private void Start()
    {                              

        string filePath = Path.Combine(Application.streamingAssetsPath, jsonDataFileName);
        string dataAsJson;

        TextAsset txtAsset = (TextAsset)Resources.Load("JsonFile", typeof(TextAsset));
        dataAsJson = txtAsset.text;
        RootObject rootObjectdata = JsonUtility.FromJson<RootObject>(dataAsJson);
        data = rootObjectdata.data;

        startLoadingImages();
    }        
    
    public void startLoadingImages()
    {
        foreach(Datum datum in data)
        {
            imageLoader.loadImage(datum.imageurl, datum.weburl);
        }                        
    }    
}

[Serializable]
public class Datum
{
    public string imageurl;
    public string weburl; 
}
[Serializable]
public class RootObject
{
    public List<Datum> data;
}
