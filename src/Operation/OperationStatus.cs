namespace Crockhead.Core
{
	/// <summary>
	/// 명령 상태.
	/// </summary>
	public enum OperationStatus
	{
		/// <summary>
		/// 시작 전 대기.
		/// </summary>
		Wait = 0,

		/// <summary>
		/// 시작되어 동작 중.
		/// </summary>
		Running,

		/// <summary>
		/// 작업 완료 및 성공됨.
		/// </summary>
		Succeeded,

		/// <summary>
		/// 작업 중 실패됨.
		/// </summary>
		Faulted,

		/// <summary>
		/// 작업 중 취소됨.
		/// </summary>
		Canceled,
	}
}