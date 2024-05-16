using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : Singleton<LevelData>
{
    [SerializeField] int currentLevel = 0;

    public void SetCurrentLevel(int level)
    {
        currentLevel = level;
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }
}
