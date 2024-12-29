//AppUpdateManager �ξ� ������Ʈ ���� 
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
    //����Ƽ �÷��� �ξ۰� Play API �� ����� ó���ϴ� AppUpdateManager Ŭ����

    //�� ������Ʈ �۾�

    //
    /*    private void Awake()
        {
            //������Ʈ �ϰ� �󸶳� �������� ��� �ϼ� Ȯ�� �ϴ� ���
            // var stalenessDays = appUpdateInfoOperation.GetResult();
            StartCoroutine(CheckForUpdate());
        }*/
    /// <summary>
    /// Google.Play.AppUpdate Ŭ���� 
    /// AppUpdateInfo : ���� ������Ʈ ���뼺 �� ��ġ ������� ���� ����
    /// AppUpdateManager : �� ���� ������Ʈ�� ��û�ϰ� �����ϴ� �۾�
    /// AppUpdateOptions : AppUpdateType�� �����Ͽ� �� �� ������Ʈ�� �����ϴ� �� ���Ǵ� �ɼ��Դϴ�.
    /// AppUpdateRequest : ���� ���� �� �� ������Ʈ�� ����͸��ϴ� �� ���Ǵ� ����� ���� ���� ����Դϴ�.(?)
    /// </summary>
    /// <returns></returns>
    //������Ʈ ��� ���� ���θ� Ȯ��
    public static IEnumerator CheckForUpdate()
    {
        appUpdateManager = new AppUpdateManager();
        PlayAsyncOperation<AppUpdateInfo, AppUpdateErrorCode> appUpdateInfoOperation;
        appUpdateInfoOperation = appUpdateManager.GetAppUpdateInfo();
        // Wait until the asynchronous operation completes.
        //�񵿱� �۾��� �Ϸ�� ������ ��ٸ��ϴ�.
        yield return appUpdateInfoOperation;

        // var appUpdateOptions = AppUpdateOptions.FlexibleAppUpdateOptions();

        if (appUpdateInfoOperation.IsSuccessful)
        {
            // Check AppUpdateInfo's UpdateAvailability, UpdatePriority,
            // IsUpdateTypeAllowed(), etc. and decide whether to ask the user
            // to start an in-app update.

            /*AppUpdateInfo�� UpdateAvailability, UpdatePriority,
            IsUpdateTypeAllowed() �� �� ����ڿ��� ��û���� ���θ� ����
            �� �� ������Ʈ�� �����մϴ�.*/

            var appUpdateInfoResult = appUpdateInfoOperation.GetResult();
            //  AppUpdateManager.StartUpdate() ������Ʈ�� ��û 
            //  ������Ʈ�� ��û�ϱ� ���� �ֽ� AppUpdataeInfo�� �ִ��� Ȯ���ؾ���
            //  UpdateAvailability ,�� �� ������Ʈ�� ���� ���뼺 �����Դϴ�.
            if (appUpdateInfoResult.UpdateAvailability == UpdateAvailability.UpdateAvailable)
            {
                // Debug.Log("start updateAble");
                //������Ʈ ��� ����
                /// AppUpdateOptions : AppUpdateType�� �����Ͽ� �� �� ������Ʈ�� �����ϴ� �� ���Ǵ� �ɼ��Դϴ�.
                //var appUpdateOptions = AppUpdateOptions.FlexibleAppUpdateOptions(); //������ ������Ʈ ó�� AppUpdateType =0
                var appUpdateOptions = AppUpdateOptions.ImmediateAppUpdateOptions(); //��� ������Ʈ ó�� AppUpdateType =1

                //������ ������Ʈ ������ ���� �� �� ������Ʈ ����.

                var startUpdateRequest = appUpdateManager.StartUpdate(
                    //appUpdateInfoOperation.GetResult()�� ����� ������
                    appUpdateInfoResult,
                    //������Ʈ ó�� ����� ������
                    appUpdateOptions);

                //updateStatusLog.text = $"{startUpdateRequest.Status}";
                // �� �� ������Ʈ api ����
                //https://developer.android.com/reference/unity/namespace/Google/Play/AppUpdate#appupdatestatus
                //0 : Unknown 1:Pending 2:Downloading 3:Downloaded 4:Installing 5:Installed 6:Failed 7:Canceled
                yield return startUpdateRequest;
            }
            //������Ʈ�� ����� �� ����
            else if (appUpdateInfoResult.UpdateAvailability == UpdateAvailability.UpdateNotAvailable)
            {
                //updateStatus_test2.text = "������Ʈ ��� �Ұ�";
                yield return (int)UpdateAvailability.UpdateNotAvailable;  // 1
            }
            else
            {
                //updateStatus_test2.text = "Else";
                yield return (int)UpdateAvailability.Unknown; // 0
            }


            //   var startUpdateRequest = appUpdateManager.StartUpdate(
            //       // PlayAsync Operation���� ��ȯ�� ����Դϴ�.��� ��������().
            //       appUpdateInfoResult,
            //       //��û�� �� �� ������Ʈ �� �ش� �Ű� ������ �����ϴ� �� ������Ʈ �ɼ��� �����Ǿ����ϴ�.
            //       appUpdateOptions);

        }
        else
        {
            // Log appUpdateInfoOperation.Error.
            //appUpdateInfoOperation�� ����մϴ�.����.
        }
    }
}