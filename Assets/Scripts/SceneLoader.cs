using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public OVROverlay overlay_Background;
    public OVROverlay overlay_LoadingText;

    #region Singleton
    //Implementing Basic Singleton Pattern
    public static SceneLoader instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    public void LoadScene(string sceneName)
    {
        StartCoroutine(ShowOverlayLoad(sceneName));
    }

    IEnumerator ShowOverlayLoad(string sceneName)
    {

        overlay_Background.enabled = true;
        overlay_LoadingText.enabled = true;

        GameObject centerEyeAnchor = GameObject.Find("CenterEyeAnchor");
        overlay_LoadingText.gameObject.transform.position = centerEyeAnchor.transform.position + new Vector3(0f, 0f, 3f);

        //Waiting some seconds to prevent pop tp new scene
        yield return new WaitForSeconds(5f);

        //Load scene and wait till complete
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        //Disabling the overlays again
        overlay_Background.enabled = false;
        overlay_LoadingText.enabled = false;

        yield return null;
    }

}
