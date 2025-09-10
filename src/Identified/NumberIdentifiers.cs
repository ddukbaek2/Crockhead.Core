using System;
using System.Security.Cryptography;


namespace Crockhead.Core
{
	/// <summary>
	/// ulong 타입의 고유 식별자 생성기.
	/// </summary>
	public class NumberIdentifiers : Identifiers<ulong>
	{
		/// <summary>
		/// 랜덤 생성기.
		/// </summary>
		private RandomNumberGenerator m_Random;

		/// <summary>
		/// 임시 바이트 배열 버퍼.
		/// </summary>
		private byte[] m_TempBytes;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public NumberIdentifiers() : base()
		{
			m_Random = RandomNumberGenerator.Create();
			m_TempBytes = new byte[sizeof(ulong)];
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			m_Random.Dispose();
			m_TempBytes = null;

			base.OnDispose(explicitDisposing);
		}

		/// <summary>
		/// 식별자 생성.
		/// </summary>
		protected override ulong CreateIdentifier()
		{
			m_Random.GetBytes(m_TempBytes);
			//var identifier = BitConverter.ToUInt64(m_TempBytes);
			var identifier = BitConverter.ToUInt64(m_TempBytes, 0);
			return identifier;
		}
	}
}