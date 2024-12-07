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
}
