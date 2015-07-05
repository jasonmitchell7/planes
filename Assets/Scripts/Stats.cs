using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Stats
{
	public GameManager gm;

	private int baseScore;
	private int bonusScore;

	public int obstaclesAvoided;
	public int obstaclesDestroyed;

	private int lastSpeedIncrease;

	private int totalScore;

	public void ResetScore()
	{
		baseScore = 0;
		bonusScore = 0;
		totalScore = 0;
		obstaclesAvoided = 0;
		obstaclesDestroyed = 0;
		lastSpeedIncrease = 0;

		CalcScore();
	}


	public void CalcScore()
	{
		baseScore = obstaclesDestroyed*3 + obstaclesAvoided;

		if (baseScore - lastSpeedIncrease > 10 )
		{
			gm.RampGameSpeed();
			lastSpeedIncrease = baseScore;
			if (gm.planeLeft.gameObject.activeSelf & gm.planeRight.gameObject.activeSelf)
			{
				AddBonusPoints(5);
			}
		}

		totalScore = baseScore + bonusScore;
		gm.score.text = "Score: " + totalScore.ToString();
	}

	public void AddBonusPoints(int amount)
	{
		bonusScore += amount;
	}

	public int GetScore()
	{
		CalcScore();

		return totalScore;
	}

	public int GetHighScore()
	{
		if (!PlayerPrefs.HasKey("Highscore"))
		{
			PlayerPrefs.SetInt( "Highscore", 0 );
		}

		return PlayerPrefs.GetInt("Highscore");
	}

	public bool CheckNewHighScore()
	{
		if ( GetScore() > GetHighScore() )
		{
			PlayerPrefs.SetInt( "Highscore", GetScore() );
			return true;
		}
		else
		{
			return false;
		}
	}
	
}
