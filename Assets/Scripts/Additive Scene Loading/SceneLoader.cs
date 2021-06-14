using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private string SceneName = "Additive Scene 2";

    private AsyncOperation LoadLevelOperation = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null && LoadLevelOperation == null)
        {
            LoadLevelOperation = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
        }
    }
}
