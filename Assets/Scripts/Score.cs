using System;

public class Score : IEquatable<Score>, IComparable<Score> {

    public Score(int score, string playerInitial) {
        this.score = score;
        this.playerInitial = playerInitial;
        isNewScore = false;
    }

    public Score(int score, string playerInitial, bool isNewScore) {
        this.score = score;
        this.playerInitial = playerInitial;
        this.isNewScore = isNewScore;
    }

    public int score { get; set; }

    public string playerInitial { get; set; }

    public bool isNewScore { get; set; }

    public int CompareTo(Score other) {
        if (other == null) {
            return 1;
        } else {
            return -this.score.CompareTo(other.score);
        }
    }

    public bool Equals(Score other) {
        if (other == null) {
            return false;
        }

        Score objAsScore = other as Score;

        if (objAsScore == null) {
            return false;
        } else {
            return Equals(objAsScore);
        }
    }
}
