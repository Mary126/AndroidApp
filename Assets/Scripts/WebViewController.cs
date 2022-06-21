using UnityEngine.SceneManagement;
using UnityEngine;

public class WebViewController : MonoBehaviour
{
    private void Awake()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            SceneManager.LoadScene("Game");
        }
        else
        {
            Application.OpenURL("https://www.google.com/");
            SceneManager.LoadScene("Menu");
        }
    }
}
