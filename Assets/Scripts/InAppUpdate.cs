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

            // Güncelleme durumu kontrolü
            // Burada AppUpdateOptions oluşturmanız gerekiyor
            var appUpdateOptions = AppUpdateOptions.ImmediateAppUpdateOptions();

            if (appUpdateInfoResult.UpdateAvailability == UpdateAvailability.UpdateAvailable &&
                appUpdateInfoResult.IsUpdateTypeAllowed(appUpdateOptions)) // Zorunlu güncelleme kontrolü
            {
                // Zorunlu güncelleme başlatılıyor
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
}