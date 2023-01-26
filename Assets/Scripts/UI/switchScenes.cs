using UnityEngine;
using UnityEngine.SceneManagement;

public class switchScenes : MonoBehaviour
{

    public string nextScene;
    public Vector2 PlayerPosition;
    public VectorValue storedPlayerPosition;

    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.CompareTag("Player")) {
            SaveManager.instance.switchedScene = true;
            storedPlayerPosition.startValue = PlayerPosition;
            SceneManager.LoadScene(nextScene);
            SaveManager.instance.Save();
        }
    }
}
