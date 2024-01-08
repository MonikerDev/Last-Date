using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.SceneManagement;

public class TriggerCutscene : MonoBehaviour
{
	public string sceneName;

    [YarnCommand("loadCutscene")]
    public void LoadCutscene()
	{
		SceneManager.LoadScene(sceneName);
	}
}
