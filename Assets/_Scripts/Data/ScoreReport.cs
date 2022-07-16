using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreReport {
    private bool _isSolved;
    private int _finalScoredPoints;
    private int _originalPointsToScore;

    public ScoreReport(bool isSolved, int finalScoredPoints, int originalPointsToScore) {
        _isSolved = isSolved;
        _finalScoredPoints = finalScoredPoints;
        _originalPointsToScore = originalPointsToScore;
    }

    public void SetIsSolved(bool isSolved) {
        _isSolved = isSolved;
    }

    public bool IsSolved() {
        return _isSolved;
    }

    public void SetFinalScoredPoints(int points) {
        _finalScoredPoints = points;
    }

    public int GetFinalScoredPoints() {
        return _finalScoredPoints;
    }

    public void SetOriginalPointsToScore(int points) {
        _originalPointsToScore = points;
    }

    public int GetOriginalPointsToScore() {
        return _originalPointsToScore;
    }
}
