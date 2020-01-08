using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using DG.Tweening;

public class ImageLoader : MonoBehaviour
{
    public JsonParser jsonParser;

    public GameObject imagesParent;
    public GameObject imagePrefab;
    private bool allImagesLoaded;
    public RectTransform contentRectTransform;
    private bool reachedBottom;
    private bool reachedTop;
    public GameObject loadingInfo;

    private void Start()
    {
        loadingInfo.SetActive(true);
        foreach(Transform child in imagesParent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public async void loadImage(string _imageUrl, string _webUrl)
    {
        Texture2D texture = await GetRemoteTexture(_imageUrl);

        GameObject newImage = Instantiate(imagePrefab) as GameObject;
        newImage.transform.SetParent(imagesParent.transform);
        newImage.transform.localPosition = Vector3.zero;
        newImage.transform.localRotation = Quaternion.identity;
        newImage.transform.localScale = Vector3.one;
        newImage.GetComponent<ImageDetails>()._webUrl = _webUrl;
        newImage.GetComponent<RectTransform>().sizeDelta = new Vector2(texture.width, texture.height);
        newImage.GetComponent<RawImage>().texture = texture;
    }

    public static async Task<Texture2D> GetRemoteTexture(string url)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            //begin request:
            var asyncOp = www.SendWebRequest();

            //await until it's done: 
            while (asyncOp.isDone == false)
            {
                await Task.Delay(1000 / 30);//30 hertz
            }

            //read results:
            if (www.isNetworkError || www.isHttpError)
            {
                //log error:
#if DEBUG
                Debug.Log($"{ www.error }, URL:{ www.url }");
#endif

                //nothing to return on error:
                return null;
            }
            else
            {
                //return valid results:
                return DownloadHandlerTexture.GetContent(www);
            }
        }
    }

    private void Update()
    {
        if (!allImagesLoaded && imagesParent.transform.childCount == jsonParser.data.Count)
        {
            loadingInfo.SetActive(false);
            allImagesLoaded = true;
            reachedTop = true;
        }

        if (allImagesLoaded)
        {
            if (contentRectTransform.localPosition.y >= 0 && contentRectTransform.localPosition.y < contentRectTransform.sizeDelta.y - 2600f)
            {               
                if (reachedTop &&!reachedBottom)
                {                                                       
                    autoScroll(true);
                }     
            }
            else
            {                                       
                if (!reachedTop&&reachedBottom)
                {                                                     
                    autoScroll(false);
                }                 
            }
        }
    }

    public void autoScroll(bool up)
    {
        reachedTop = false;
        reachedBottom = false;                             

        if (!up)
        {                                 
            contentRectTransform.DOAnchorPosY(2f, 5f).OnComplete(()=>reachedDestination(true,false));
        }
        else
        {                                                      
            contentRectTransform.DOAnchorPosY(contentRectTransform.sizeDelta.y - 2500f, 5f).OnComplete(() => reachedDestination(false,true));
        }
    }

    public void reachedDestination(bool initial, bool final)
    {                                                                                  
        reachedTop = initial;
        reachedBottom = final;
    }
}