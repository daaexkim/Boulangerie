using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class GPGSManager
{
    static GPGSManager inst = new GPGSManager();
    public static GPGSManager Inst => inst;

    void Init()
    {
        var config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }
    public void Login(Action<bool, ILocalUser> onLoginSuccess = null)
    {
        Init();
        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptAlways, (success) =>
        {
            onLoginSuccess?.Invoke(success == SignInStatus.Success, Social.localUser);
        });
    }

    public void ShowAllLeaderboardUI() =>
        Social.ShowLeaderboardUI();
    private void ReportLeaderboard(string gpgsId, long score, Action<bool> onReported = null) =>
            Social.ReportScore(score, gpgsId, success => onReported?.Invoke(success));
    public void ReportLeaderboard(GameMode mode, int score)
    {
        switch(mode)
        {
            case GameMode.Bebe:
                ReportLeaderboard(GPGSIds.leaderboard_bb, score);
                break;
            case GameMode.Jeune:
                ReportLeaderboard(GPGSIds.leaderboard_jeune, score);
                break;
            case GameMode.Adulte:
                ReportLeaderboard(GPGSIds.leaderboard_adulte, score);
                break;
        }
    }
}
