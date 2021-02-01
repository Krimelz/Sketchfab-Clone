using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoadAssetBundles : MonoBehaviour
{
    public string url = "http://192.168.0.101:25565/";
    public string gameObjectsBundleName = "gameobjects";
    public uint version = 0;
    public Dropdown dropdownList;
    
    private AssetBundle models;
    private GameObject currentGameObject;

    void Start()
    {
        StartCoroutine(LoadAsset());
    }

    IEnumerator LoadAsset()
    {
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url + gameObjectsBundleName, version);

        yield return request.SendWebRequest();

        models = DownloadHandlerAssetBundle.GetContent(request);

        List<string> list = new List<string>();
        list.AddRange(models.GetAllAssetNames());
        dropdownList.AddOptions(list);
    }

    public void SpawnModel()
    {
        if (currentGameObject != null)
        {
            Destroy(currentGameObject);
        }

        currentGameObject = Instantiate(models.LoadAsset<GameObject>(dropdownList.captionText.text), transform.position, transform.rotation);
    }
}
