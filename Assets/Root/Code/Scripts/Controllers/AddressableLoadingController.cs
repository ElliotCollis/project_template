using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableLoadingController : MonoBehaviour
{
    [SerializeField] private AssetReference loadingCanvasPrefab;

    public bool Ready => ready;
    private bool ready;

    private GameObject loadingCanvas;

    public void Load()
    {
        StartCoroutine(LoadAsync());
    }

    public IEnumerator LoadAsync()
    {
        var handle = Addressables.InstantiateAsync(loadingCanvasPrefab, transform);

        yield return handle;  // wait for async call completion

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            loadingCanvas = handle.Result;
            loadingCanvas.SetActive(false);
            ready = true;
        }
    }

    public void Unload()
    {
        ready = false;
        Addressables.ReleaseInstance(loadingCanvas); // will decrement refence count
    }

    public void Show()
    {
        if (ready)
        {
            loadingCanvas.SetActive(true);
        }
    }

    public void Hide()
    {
        if (ready)
        {
            loadingCanvas.SetActive(false);
        }
    }

}
