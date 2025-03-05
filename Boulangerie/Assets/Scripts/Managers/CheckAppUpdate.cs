using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Google.Play.AppUpdate;
using Google.Play.Common;

public class CheckAppUpdate : Singleton<CheckAppUpdate>
{
    AppUpdateManager appUpdateManager;
    private void Start()
    {
        Debug.Log("앱 업데이트 확인");
#if UNITY_EDITOR
#elif UNITY_ANDROID
        StartCoroutine(CheckForUpdate());
#endif
    }

    public IEnumerator CheckForUpdate()
    {
        appUpdateManager = new AppUpdateManager();

        PlayAsyncOperation<AppUpdateInfo, AppUpdateErrorCode> appUpdateInfoOperation =
            appUpdateManager.GetAppUpdateInfo();

        yield return appUpdateInfoOperation;

        if (appUpdateInfoOperation.IsSuccessful)
        {
            var appUpdateInfoResult = appUpdateInfoOperation.GetResult();

            if (appUpdateInfoResult.UpdateAvailability == UpdateAvailability.UpdateAvailable)
            {
                var appUpdateOptions = AppUpdateOptions.ImmediateAppUpdateOptions();
                var startUpdateRequest = appUpdateManager.StartUpdate(appUpdateInfoResult, appUpdateOptions);

                while (!startUpdateRequest.IsDone)
                {
                    if (startUpdateRequest.Status == AppUpdateStatus.Downloading)
                        Debug.Log("업데이트 다운로드가 진행 중");
                    else if (startUpdateRequest.Status == AppUpdateStatus.Downloaded)
                        Debug.Log("업데이트 완전히 다운로드 완료");
                    yield return null;
                }

                var result = appUpdateManager.CompleteUpdate();
                while (!result.IsDone)
                    yield return new WaitForEndOfFrame();
                yield return (int)startUpdateRequest.Status;
            }
            else if (appUpdateInfoResult.UpdateAvailability == UpdateAvailability.UpdateNotAvailable)
            {
                Debug.Log("업데이트 없음");
                yield return (int)UpdateAvailability.UpdateNotAvailable;
            }
            else
            {
                Debug.Log("정보 없음");
                yield return (int)UpdateAvailability.Unknown;
            }
        }
        else
            Debug.Log("Error");
    }
}