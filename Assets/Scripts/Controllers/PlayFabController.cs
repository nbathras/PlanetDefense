using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class PlayFabController : MonoBehaviour {
    public static PlayFabController Instance;

    private const string CUSTOMER_ID = "GettingStartedGuide";

    private const string HIGH_SCORES_KEY = "HighScores";

    private LoginResult loginResult;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    public void Start() {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId)) {
            /*
            titleId to your own titleId from PlayFab Game Manager.
            */
            PlayFabSettings.staticSettings.TitleId = "4E4A3";
        }

        PlayFabClientAPI.LoginWithCustomID(
            new LoginWithCustomIDRequest {
                CustomId = CUSTOMER_ID, CreateAccount = true
            }, 
            OnLoginSuccess, 
            OnLoginFailure
       );
    }

    private void OnLoginSuccess(LoginResult result) {
        // Store result
        loginResult = result;
    }

    private string ListToString(string prefix, List<Score> list) {
        for (int i = 0; i < list.Count; i++) {
            prefix += list[i].score + "," + list[i].playerInitial + "&&";
        }

        return prefix;
    }

    public void SaveHighScore(int score, string playerInitials) {
        if (score < 0 || playerInitials.Length != 3) {
            throw new Exception("Save High Score Parameters were incorrect");
        }

        Score newScore = new Score(score, playerInitials);

        PlayFabClientAPI.GetUserData(new GetUserDataRequest() {
            PlayFabId = loginResult.PlayFabId,
            Keys = null
        }, resultGet => {
            List<Score> highScoresList = new List<Score>();
            if (resultGet.Data.ContainsKey(HIGH_SCORES_KEY)) {
                string highScoresString = resultGet.Data[HIGH_SCORES_KEY].Value.ToString();
                highScoresList = ConvertHighScoresStringToList(highScoresString);
            }

            highScoresList.Add(newScore);
            highScoresList.Sort();

            if (highScoresList.Count > 10) {
                highScoresList.RemoveRange(10, highScoresList.Count - 10);
            }

            string value = ConvertHighScoresListToString(highScoresList);

            Debug.Log("Value: " + value);

            PlayFabClientAPI.UpdateUserData(
                new UpdateUserDataRequest() {
                    Data = new Dictionary<string, string>() {
                                {HIGH_SCORES_KEY, value}
                    }
                },
                resultSet => Debug.Log("Successfully updated user data"),
                error => {
                    Debug.Log("Got error setting user data Ancestor to Arthur");
                    Debug.Log(error.GenerateErrorReport());
                }
            );
        }, (error) => {
            Debug.Log("Got error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
        });
    }

    public void UpdateScoreTextFields(TextMeshProUGUI rankedListText, TextMeshProUGUI scoreListText, TextMeshProUGUI nameListText) {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() {
            PlayFabId = loginResult.PlayFabId,
            Keys = null
        }, resultGet => {
            List<Score> highScoresList = new List<Score>();
            if (resultGet.Data.ContainsKey(HIGH_SCORES_KEY)) {
                string highScoresString = resultGet.Data[HIGH_SCORES_KEY].Value.ToString();
                highScoresList = ConvertHighScoresStringToList(highScoresString);
            }

            rankedListText.SetText("");
            scoreListText.SetText("");
            nameListText.SetText("");
            for (int i = 0; i < highScoresList.Count; i++) {
                rankedListText.text += (i + 1).ToString() + ".\n";
                scoreListText.text += highScoresList[i].score.ToString() + "\n";
                nameListText.text += highScoresList[i].playerInitial + "\n";
            }

        }, (error) => {
            Debug.Log("Got error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
        });
    }

    public void UpdateScoreTextFieldsNewScore(int newScoreInt, TextMeshProUGUI rankedListText, TextMeshProUGUI scoreListText, TextMeshProUGUI nameListText) {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() {
            PlayFabId = loginResult.PlayFabId,
            Keys = null
        }, resultGet => {
            List<Score> highScoresList = new List<Score>();
            if (resultGet.Data.ContainsKey(HIGH_SCORES_KEY)) {
                string highScoresString = resultGet.Data[HIGH_SCORES_KEY].Value.ToString();
                highScoresList = ConvertHighScoresStringToList(highScoresString);
            }

            Score newScore = new Score(newScoreInt, "???", true);
            highScoresList.Add(newScore);

            rankedListText.SetText("");
            scoreListText.SetText("");
            nameListText.SetText("");
            for (int i = 0; i < highScoresList.Count; i++) {
                if (highScoresList[i].isNewScore) {
                    rankedListText.text += "<b><color=red>" + (i + 1).ToString() + ".</color></b>\n";
                    scoreListText.text += "<b><color=red>" + highScoresList[i].score.ToString() + "</color></b>\n";
                    nameListText.text += "<b><color=red>" + highScoresList[i].playerInitial + "</color></b>\n";
                } else {
                    rankedListText.text += (i + 1).ToString() + ".\n";
                    scoreListText.text += highScoresList[i].score.ToString() + "\n";
                    nameListText.text += highScoresList[i].playerInitial + "\n";
                }
            }

        }, (error) => {
            Debug.Log("Got error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
        });
    }

    private List<Score> ConvertHighScoresStringToList(string highScoresString) {
        List<Score> highScoresList = new List<Score>();

        string[] highScoresSplit = highScoresString.Split('|');
        if (highScoresSplit.Length > 10) {
            throw new Exception("This should not be");
        }

        for (int i = 0; i < highScoresSplit.Length; i++) {
            string[] entrySplit = highScoresSplit[i].Split(',');
            if (entrySplit.Length != 2) {
                throw new Exception("Split error");
            }

            highScoresList.Add(new Score(int.Parse(entrySplit[0]), entrySplit[1]));
        }

        return highScoresList;
    }

    private string ConvertHighScoresListToString(List<Score> highScoresList) {
        if (highScoresList.Count > 10) {
            throw new Exception("To many scores being saved");
        }

        string highScoresString = "";
        for (int i = 0; i < highScoresList.Count; i++) {
            string score = highScoresList[i].score.ToString();
            string playerInitial = highScoresList[i].playerInitial;
            if (i == highScoresList.Count - 1) {
                highScoresString += score + "," + playerInitial;
            } else {
                highScoresString += score + "," + playerInitial + "|";
            }
        }

        return highScoresString;
    }

    private static DateTime ConvertFromUnixTimestamp(double timestamp) {
        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        return origin.AddSeconds(timestamp);
    }

    private static double ConvertToUnixTimestamp(DateTime date) {
        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        TimeSpan diff = date.ToUniversalTime() - origin;
        return Math.Floor(diff.TotalSeconds);
    }

    private void OnLoginFailure(PlayFabError error) {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }
}
