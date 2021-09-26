using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayLevelsGuiManager : MonoBehaviour
{
    private PlayerMovement playerMovement = null;
    private MouseLook mouseLook = null;

    private GameObject levelCompletedPanel = null;
    private GameObject gameCompletedPanel = null;
    private GameObject crosshairCanvas = null;

    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag(ObjectTypes.playerTagName).GetComponent<PlayerMovement>();
        mouseLook = GameObject.FindGameObjectWithTag(ObjectTypes.mainCameraTagName).GetComponent<MouseLook>();

        levelCompletedPanel = GameObject.Find(GlobalVariables.levelCompletedPanelName);
        levelCompletedPanel.SetActive(false);

        gameCompletedPanel = GameObject.Find(GlobalVariables.gameCompletedPanelName);
        gameCompletedPanel.SetActive(false);

        crosshairCanvas = GameObject.Find(GlobalVariables.crosshairCanvasName);
    }

    // Same as in EditorUI - see if can be unified
    public void OnLevelCompleted(bool gameCompleted)
    {
        Cursor.lockState = CursorLockMode.None;

        playerMovement.enabled = false;
        mouseLook.enabled = false;

        crosshairCanvas.SetActive(false);

        if (gameCompleted)
        {
            gameCompletedPanel.SetActive(true);
        }
        else
        {
            levelCompletedPanel.SetActive(true);
        }
    }

    public void NextLevel()
    {
        BHWrapper.IncreaseLevel();
        SceneManager.LoadScene(GlobalVariables.playLevelsName);
    }

    public void BackToMenu()
    {
        // TODO: find out what happens to the current scene when you are loading a new one
        SceneManager.LoadScene(GlobalVariables.startMenuName);
    }
}
