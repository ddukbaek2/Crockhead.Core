using System;


namespace Crockhead.Core
{
	/// <summary>
	/// 연결된 파일 경로를 수식하는 특성.
	/// </summary>
	public class FilePathAttribute : Attribute
	{
		/// <summary>
		/// 값.
		/// </summary>
		private string m_Value;

		/// <summary>
		/// 사용 여부.
		/// </summary>
		private bool m_IsEnabled;

		/// <summary>
		/// 값 프로퍼티.
		/// </summary>
		public string Value => m_Value;

		/// <summary>
		/// 사용 여부 프로퍼티.
		/// </summary>
		public bool IsEnabled => m_IsEnabled;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public FilePathAttribute(string path, bool enabled = true) : base()
		{
			m_Value = path;
			m_IsEnabled = enabled;
		}
	}
}