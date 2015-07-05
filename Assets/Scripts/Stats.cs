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

	public void ResetScore()
	{
		baseScore = 0;
		bonusScore = 0;
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
				bonusScore += 5;
			}
		}

		int totalScore = baseScore + bonusScore;
		gm.score.text = "Score: " + totalScore.ToString();
	}
	
}
