using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class DeveloperManager : MonoBehaviour
{
    VideoPlayer video;
    [SerializeField] Image FadeImage;

    void Start()
    {
        video = this.GetComponent<VideoPlayer>();

        StartCoroutine("VideoCo");
    }

    IEnumerator VideoCo()
    {
        yield return new WaitForSeconds(1.5f);

        video.Play();

        yield return new WaitForSeconds(1.5f);
        
        while (true)
        {
            yield return null;
            if (!video.isPlaying)
            {
                yield return new WaitForSeconds(0.5f);
                FadeImage.DOFade(1, 1f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    SceneManager.LoadScene("Scene_00_Title");
                });
                yield break;
            }
        }
    }
}
