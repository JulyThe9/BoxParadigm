using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MenuBehaviour : MonoBehaviour {

	public void newGame()
    {
        // TODO: perhaps create a method ResetLevels() or OnNewGame() in terms of levels
        BHWrapper.BHoldersClear();
        BHWrapper.ResetCurLevel();
        LevelManager.DeserializeLevelFiles();
        SceneManager.LoadScene(GlobalVariables.playLevelsName);
    }

	public void mapEditor()
    {
        BHWrapper.BHoldersClear();
        BHWrapper.ResetCurLevel();
        SceneManager.LoadScene(GlobalVariables.mapEditorName);
	}

	public void settings()
    {
	}

	public void quit()
    {
		Application.Quit();
	}

	public void mark(GameObject menuEntry)
    {
         menuEntry.GetComponent<Text>().alignment = TextAnchor.UpperCenter;
	}

	public void unmark(GameObject menuEntry)
    {
        menuEntry.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
    }
		

}
