// LoadingScreen.cs
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Slider progressBar;
    [SerializeField] private float minimumDisplayTime = 1f;


    private void Start()
    {
        string nextScene = PendingLoad.NextSceneName;
       
       
        if (string.IsNullOrEmpty(nextScene))
        {
            // Fallback if no scene specified
            SceneLoader.Instance.LoadScene("MainMenu");
            
            return;
        }

        StartCoroutine(LoadSequence(nextScene));
    }



    private IEnumerator LoadSequence(string sceneName)
    {
        float timer = 0f;

        SceneLoader.Instance.LoadScene(sceneName);


        while (SceneLoader.Instance.isLoading || timer < minimumDisplayTime)
        {
            progressBar.value = Mathf.Clamp01(SceneLoader.Instance.loadingProgress);
           
            timer += Time.deltaTime;


            yield return null;
        }
    }
}
