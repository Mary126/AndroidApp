using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MenuController : MonoBehaviour
{
    public Button StartButton;
    public Button ShowPrivacyScreen;
    public GameObject PrivacyPanel;
    public GameObject PrivacyScreen;
    public GameObject MainWindow;
    public Button AgreeToPrivacyButton;
    public GameObject AgreeSwitch;
    public Button WebViewButton;
    
    private void AgreeToPrivacySwitch()
    {
        if (PlayerPrefs.GetInt("HasAgreedToPrivacy") == 0)
        {
            PlayerPrefs.SetInt("HasAgreedToPrivacy", 1);
            AgreeSwitch.transform.position = new Vector3(AgreeSwitch.transform.position.x + 0.3f, AgreeSwitch.transform.position.y, AgreeSwitch.transform.position.z);
            AgreeToPrivacyButton.GetComponent<Image>().color = Color.green;
            PrivacyPanel.SetActive(false);
            PrivacyScreen.SetActive(false);
            MainWindow.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetInt("HasAgreedToPrivacy", 0);
            AgreeSwitch.transform.position = new Vector3(AgreeSwitch.transform.position.x - 0.3f, AgreeSwitch.transform.position.y, AgreeSwitch.transform.position.z);
            AgreeToPrivacyButton.GetComponent<Image>().color = Color.grey;
        }
    }
    private void ShowPrivacyPolicy()
    {
        MainWindow.SetActive(false);
        PrivacyScreen.SetActive(true);
    }
    private void OpenWebView()
    {
        SceneManager.LoadScene("WebView");
    }
    private void Play()
    {
        if (PlayerPrefs.GetInt("HasAgreedToPrivacy") == 1)
        {

            SceneManager.LoadScene("Game");
        }
        else
        {
            PrivacyPanel.SetActive(true);
        }
    }
    private void Awake()
    {
        StartButton.onClick.AddListener(Play);
        ShowPrivacyScreen.onClick.AddListener(ShowPrivacyPolicy);
        AgreeToPrivacyButton.onClick.AddListener(AgreeToPrivacySwitch);
        WebViewButton.onClick.AddListener(OpenWebView);
        if (!PlayerPrefs.HasKey("HasAgreedToPrivacy"))
        {
            PlayerPrefs.SetInt("HasAgreedToPrivacy", 0);
        }
    }
    
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (PrivacyPanel.activeSelf == true || PrivacyScreen.activeSelf == true)
            {
                PrivacyPanel.SetActive(false);
                PrivacyScreen.SetActive(false);
                MainWindow.SetActive(true);
            }
            else
            {
                Application.Quit();
            }
        }
    }
}
