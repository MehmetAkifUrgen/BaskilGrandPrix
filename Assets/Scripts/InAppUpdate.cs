using System.Collections;
using UnityEngine;
using Google.Play.AppUpdate;
using Google.Play.Common;

public class InAppUpdate : MonoBehaviour
{
    private AppUpdateManager appUpdateManager;

    void Start()
    {
        appUpdateManager = new AppUpdateManager();
        StartCoroutine(CheckForUpdate());
    }

    private IEnumerator CheckForUpdate()
    {
        var appUpdateInfoOperation = appUpdateManager.GetAppUpdateInfo();
        yield return appUpdateInfoOperation;

        if (appUpdateInfoOperation.IsSuccessful)
        {
            var appUpdateInfoResult = appUpdateInfoOperation.GetResult();

            // Güncelleme seçeneklerini belirliyoruz (zorunlu veya esnek ayrımı yapılmadan her durumda güncelleme yapılacak)
            var appUpdateOptions = AppUpdateOptions.ImmediateAppUpdateOptions(); // Hangi güncelleme türü olduğuna bakılmaksızın her zaman Immediate kullanılıyor.

            // Güncelleme mevcutsa güncellemeyi başlat
            if (appUpdateInfoResult.UpdateAvailability == UpdateAvailability.UpdateAvailable)
            {
                StartCoroutine(StartImmediateUpdate(appUpdateInfoResult));
            }
            else
            {
                Debug.Log("No update available. The game continues.");
            }
        }
        else
        {
            Debug.LogError("Error checking for update.");
        }
    }

    private IEnumerator StartImmediateUpdate(AppUpdateInfo appUpdateInfo)
    {
        var appUpdateOptions = AppUpdateOptions.ImmediateAppUpdateOptions();
        var startUpdateRequest = appUpdateManager.StartUpdate(appUpdateInfo, appUpdateOptions);
        yield return startUpdateRequest;

        // Güncelleme durumunu kontrol et
        if (startUpdateRequest.Status == AppUpdateStatus.Failed)
        {
            Debug.LogError("Update failed.");
        }
        else if (startUpdateRequest.Status == AppUpdateStatus.Installed)
        {
            Debug.Log("Update installed successfully. Restarting app...");
            appUpdateManager.CompleteUpdate();
        }
    }
    IEnumerator StartFlexibleUpdate()
    {
        var appUpdateInfoOperation = appUpdateManager.GetAppUpdateInfo();
        var appUpdateInfoResult = appUpdateInfoOperation.GetResult();
        // Creates an AppUpdateOptions defining a flexible in-app
        // update flow and its parameters.
        var appUpdateOptions = AppUpdateOptions.FlexibleAppUpdateOptions();
        // Creates an AppUpdateRequest that can be used to monitor the
        // requested in-app update flow.
        var startUpdateRequest = appUpdateManager.StartUpdate(
          // The result returned by PlayAsyncOperation.GetResult().
          appUpdateInfoResult,
          // The AppUpdateOptions created defining the requested in-app update
          // and its parameters.
          appUpdateOptions);

        while (!startUpdateRequest.IsDone)
        {
            // For flexible flow,the user can continue to use the app while
            // the update downloads in the background. You can implement a
            // progress bar showing the download status during this time.
            yield return null;
        }

    }

    IEnumerator CompleteFlexibleUpdate()
    {
        var result = appUpdateManager.CompleteUpdate();
        yield return result;

        // If the update completes successfully, then the app restarts and this line
        // is never reached. If this line is reached, then handle the failure (e.g. by
        // logging result.Error or by displaying a message to the user).
    }
}
