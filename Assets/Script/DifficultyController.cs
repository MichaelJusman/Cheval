using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty
{
    Easy,
    Hard
}

public class DifficultyController : Singleton<DifficultyController>
{

    public Difficulty difficulty;

    public void ChangeDifficulty(int _difficulty)
    {
        difficulty = (Difficulty)_difficulty;
        Setup();
    }

    public void Setup()
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                _GM.scoreMultiplier = 1;
                break;

            case Difficulty.Hard:
                _GM.scoreMultiplier = 2;
                break;
        }
    }

    public void ChangeDifficultyEasy()
    {
        difficulty = Difficulty.Easy;
    }
    
    public void ChangeDifficultyHard()
    {
        difficulty = Difficulty.Hard;
    }

}
