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
        Debug.Log("�� ������Ʈ Ȯ��");
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
                        Debug.Log("������Ʈ �ٿ�ε尡 ���� ��");
                    else if (startUpdateRequest.Status == AppUpdateStatus.Downloaded)
                        Debug.Log("������Ʈ ������ �ٿ�ε� �Ϸ�");
                    yield return null;
                }

                var result = appUpdateManager.CompleteUpdate();
                while (!result.IsDone)
                    yield return new WaitForEndOfFrame();
                yield return (int)startUpdateRequest.Status;
            }
            else if (appUpdateInfoResult.UpdateAvailability == UpdateAvailability.UpdateNotAvailable)
            {
                Debug.Log("������Ʈ ����");
                yield return (int)UpdateAvailability.UpdateNotAvailable;
            }
            else
            {
                Debug.Log("���� ����");
                yield return (int)UpdateAvailability.Unknown;
            }
        }
        else
            Debug.Log("Error");
    }
}