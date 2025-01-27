//AppUpdateManager 인앱 업데이트 지원 
using Google.Play.AppUpdate;
using Google.Play.Common;
using System.Collections;
using UnityEngine;

/// <summary>
/// https://developer.android.com/reference/unity/class/Google/Play/AppUpdate/AppUpdateInfo#clientversionstalenessdays
/// https://developer.android.com/reference/unity/namespace/Google/Play/AppUpdate
/// </summary>
public static class CheckAppUpdate
{
    static AppUpdateManager appUpdateManager = null;
    //유니티 플러그 인앱과 Play API 간 통신을 처리하는 AppUpdateManager 클래스

    //앱 업데이트 작업

    //
    /*    private void Awake()
        {
            //업데이트 하고 얼마나 지났는지 경과 일수 확인 하는 방법
            // var stalenessDays = appUpdateInfoOperation.GetResult();
            StartCoroutine(CheckForUpdate());
        }*/
    /// <summary>
    /// Google.Play.AppUpdate 클래스 
    /// AppUpdateInfo : 앱의 업데이트 가용성 및 설치 진행률에 대한 정보
    /// AppUpdateManager : 앱 내에 업데이트를 요청하고 시작하는 작업
    /// AppUpdateOptions : AppUpdateType을 포함하여 앱 내 업데이트를 구성하는 데 사용되는 옵션입니다.
    /// AppUpdateRequest : 진행 중인 앱 내 업데이트를 모니터링하는 데 사용되는 사용자 지정 수율 명령입니다.(?)
    /// </summary>
    /// <returns></returns>
    //업데이트 사용 가능 여부를 확인
    public static IEnumerator CheckForUpdate()
    {
        appUpdateManager = new AppUpdateManager();
        PlayAsyncOperation<AppUpdateInfo, AppUpdateErrorCode> appUpdateInfoOperation;
        appUpdateInfoOperation = appUpdateManager.GetAppUpdateInfo();
        // Wait until the asynchronous operation completes.
        //비동기 작업이 완료될 때까지 기다립니다.
        yield return appUpdateInfoOperation;

        // var appUpdateOptions = AppUpdateOptions.FlexibleAppUpdateOptions();

        if (appUpdateInfoOperation.IsSuccessful)
        {
            // Check AppUpdateInfo's UpdateAvailability, UpdatePriority,
            // IsUpdateTypeAllowed(), etc. and decide whether to ask the user
            // to start an in-app update.

            /*AppUpdateInfo의 UpdateAvailability, UpdatePriority,
            IsUpdateTypeAllowed() 등 및 사용자에게 요청할지 여부를 결정
            앱 내 업데이트를 시작합니다.*/

            var appUpdateInfoResult = appUpdateInfoOperation.GetResult();
            //  AppUpdateManager.StartUpdate() 업데이트를 요청 
            //  업데이트를 요청하기 전에 최신 AppUpdataeInfo가 있는지 확인해야함
            //  UpdateAvailability ,앱 내 업데이트에 대한 가용성 정보입니다.
            if (appUpdateInfoResult.UpdateAvailability == UpdateAvailability.UpdateAvailable)
            {
                // Debug.Log("start updateAble");
                //업데이트 사용 가능
                /// AppUpdateOptions : AppUpdateType을 포함하여 앱 내 업데이트를 구성하는 데 사용되는 옵션입니다.
                //var appUpdateOptions = AppUpdateOptions.FlexibleAppUpdateOptions(); //유연한 업데이트 처리 AppUpdateType =0
                var appUpdateOptions = AppUpdateOptions.ImmediateAppUpdateOptions(); //즉시 업데이트 처리 AppUpdateType =1

                //지정된 업데이트 유형에 대해 앱 내 업데이트 시작.

                var startUpdateRequest = appUpdateManager.StartUpdate(
                    //appUpdateInfoOperation.GetResult()의 결과를 가져옴
                    appUpdateInfoResult,
                    //업데이트 처리 방식을 가져옴
                    appUpdateOptions);

                //updateStatusLog.text = $"{startUpdateRequest.Status}";
                // 앱 내 업데이트 api 상태
                //https://developer.android.com/reference/unity/namespace/Google/Play/AppUpdate#appupdatestatus
                //0 : Unknown 1:Pending 2:Downloading 3:Downloaded 4:Installing 5:Installed 6:Failed 7:Canceled
                yield return startUpdateRequest;
            }
            //업데이트를 사용할 수 없음
            else if (appUpdateInfoResult.UpdateAvailability == UpdateAvailability.UpdateNotAvailable)
            {
                //updateStatus_test2.text = "업데이트 사용 불가";
                yield return (int)UpdateAvailability.UpdateNotAvailable;  // 1
            }
            else
            {
                //updateStatus_test2.text = "Else";
                yield return (int)UpdateAvailability.Unknown; // 0
            }


            //   var startUpdateRequest = appUpdateManager.StartUpdate(
            //       // PlayAsync Operation에서 반환한 결과입니다.결과 가져오기().
            //       appUpdateInfoResult,
            //       //요청된 앱 내 업데이트 및 해당 매개 변수를 정의하는 앱 업데이트 옵션이 생성되었습니다.
            //       appUpdateOptions);

        }
        else
        {
            // Log appUpdateInfoOperation.Error.
            //appUpdateInfoOperation을 기록합니다.오류.
        }
    }
}