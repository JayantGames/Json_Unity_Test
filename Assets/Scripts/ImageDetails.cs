using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageDetails : MonoBehaviour
{                             
    public string _webUrl;

    public void onClickHandler()
    {
        Application.OpenURL(_webUrl);
    }
}
