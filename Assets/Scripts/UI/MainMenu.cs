using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button play;
    [SerializeField] Button quit;

    private void Awake()
    {
        play.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(1);
        });
        quit.onClick.AddListener(() => {
            Application.Quit();
        });
    }
}
