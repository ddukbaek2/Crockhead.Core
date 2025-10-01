using System.Threading;
using System.Threading.Tasks;


namespace Crockhead.Core
{
	/// <summary>
	/// 비동기 오퍼레이션.
	/// </summary>
	public class AsyncOperation : Operation
	{
		/// <summary>
		/// 생성됨.
		/// </summary>
		public AsyncOperation() : base()
		{
		}

		/// <summary>
		/// 자신의 작업이 끝날 때까지 비동기 대기.
		/// </summary>
		public async virtual void WaitForCompletion()
		{
			if (!IsStarted)
				return;

			await Task.Run(() =>
			{
				while (!IsCompleted)
				{
					Thread.Sleep(1);
				}
			});
		}
	}
}
