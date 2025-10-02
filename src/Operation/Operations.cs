using System.Threading;
using System.Threading.Tasks;


namespace Crockhead.Core
{
	/// <summary>
	/// 오퍼레이션 유틸리티.
	/// </summary>
	public static class Operations
	{
		///// <summary>
		///// 자신의 작업이 끝날 때까지 비동기 대기.
		///// </summary>
		//public async static void Wait(IOperation operation)
		//{
		//	if (!operation.IsStarted)
		//		return;

		//	await Task.Run(() =>
		//	{
		//		while (!operation.IsCompleted)
		//		{
		//			Thread.Sleep(1);
		//		}
		//	});
		//}

		///// <summary>
		///// 자신의 작업이 끝날 때까지 비동기 대기.
		///// </summary>
		//public async static void Wait(IAsyncOperation operation)
		//{
		//	if (!operation.IsStarted)
		//		return;
			
		//	await operation;
		//}

		///// <summary>
		///// 자신의 작업이 끝날 때까지 비동기 대기.
		///// </summary>
		//public async static Task<TResult> Wait<TResult>(IAsyncOperation<TResult> operation)
		//{
		//	return await operation;
		//}
	}
}
