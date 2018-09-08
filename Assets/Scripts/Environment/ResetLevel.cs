using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevel : nextLevel {
    
    private void Start()
    {        
        nextSceneName = SceneManager.GetActiveScene().name;
    }
}
