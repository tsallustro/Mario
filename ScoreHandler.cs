using System;

public class ScoreHandler
{
	private int score = 0;

	public ScoreHandler()
	{
	}

	public int GetScore()
    {
		return score;
    }

	public void IncreaseScore(int increase)
    {
		score += increase;
    }
}
