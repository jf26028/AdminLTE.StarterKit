using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace AdminLTE.StarterKit.Infrastructure.Extensions
{
	public static class TempDataExtensions
	{
		public static void FlashDanger(this ITempDataDictionary tempData, string message)
		{
			CreateCookieWithFlashMessage(tempData, Notification.Danger, message);
		}

		public static void FlashWarning(this ITempDataDictionary tempData, string message)
		{
			CreateCookieWithFlashMessage(tempData, Notification.Warning, message);
		}

		public static void FlashSuccess(this ITempDataDictionary tempData, string message)
		{
			CreateCookieWithFlashMessage(tempData, Notification.Success, message);
		}

		public static void FlashInfo(this ITempDataDictionary tempData, string message)
		{
			CreateCookieWithFlashMessage(tempData, Notification.Info, message);
		}

		private static void CreateCookieWithFlashMessage(ITempDataDictionary tempData, Notification notification, string message)
		{
			tempData["flash." + notification.ToString()] = message;
		}

		private enum Notification
		{
			Danger,
			Warning,
			Success,
			Info
		}
	}
}
