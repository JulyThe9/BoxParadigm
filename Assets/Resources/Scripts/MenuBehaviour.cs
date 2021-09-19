using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MenuBehaviour : MonoBehaviour {

	public void newGame()
    {
        LevelManager.DeserializeLevelFiles();
        SceneManager.LoadScene(GlobalVariables.playLevelsName);
    }

	public void mapEditor()
    {
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
