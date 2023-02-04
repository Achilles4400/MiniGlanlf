using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject successLevelUI;

    public void CompleteLevel()
    {
        Debug.Log("Level Complete!");
        successLevelUI.SetActive(true);
    }
}
