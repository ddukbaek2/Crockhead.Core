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
		private RandomNumberGenerator m_RandomNumberGenerator;

		/// <summary>
		/// 바이트 배열 버퍼.
		/// </summary>
		private byte[] m_ByteBuffer;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public NumberIdentifiers() : base()
		{
			m_RandomNumberGenerator = RandomNumberGenerator.Create();

			var length = sizeof(ulong);
			m_ByteBuffer = new byte[length];
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			m_RandomNumberGenerator.Dispose();
			m_ByteBuffer = null;

			base.OnDispose(explicitDisposing);
		}

		/// <summary>
		/// 식별자 생성.
		/// </summary>
		protected override ulong CreateIdentifier()
		{
			m_RandomNumberGenerator.GetBytes(m_ByteBuffer);
			var identifier = BitConverter.ToUInt64(m_ByteBuffer);
			return identifier;
		}
	}
}