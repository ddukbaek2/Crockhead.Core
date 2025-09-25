using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;


namespace Crockhead.Core
{
	/// <summary>
	/// 문자열 타입의 고유 식별자 생성기.
	/// </summary>
	public class StringIdentifiers : Identifiers<string>
	{
		/// <summary>
		/// 사용 할 문자 목록.
		/// </summary>
		public const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

		/// <summary>
		/// 문자열 생성기.
		/// </summary>
		private readonly StringBuilder m_StringBuilder;

		/// <summary>
		/// 사용할 문자 목록.
		/// </summary>
		private readonly string m_Chars;

		/// <summary>
		/// 최소 문자열 길이.
		/// </summary>
		private readonly int m_MinLength;

		/// <summary>
		/// 최소 문자열 길이.
		/// </summary>
		private readonly int m_MaxLength;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public StringIdentifiers(int minLength, int maxLength, string chars = StringIdentifiers.Chars) : base()
		{
			// 최소값이 1보다 작거나 최소값이 최대값보다 큰 경우.
			if (minLength < 1 || minLength > maxLength)
				throw new ArgumentException();

			m_StringBuilder = new StringBuilder(maxLength);
			m_Chars = chars;
			m_MinLength = minLength;
			m_MaxLength = maxLength;
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public StringIdentifiers(int length, string chars = StringIdentifiers.Chars) : this(length, length, chars)
		{
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			base.OnDispose(explicitDisposing);
		}

		/// <summary>
		/// 식별자 생성.
		/// </summary>
		protected override string CreateIdentifier()
		{
			var length = RandomNumberGenerator.GetInt32(m_MinLength, m_MaxLength + 1);
			m_StringBuilder.Clear();
			for (var i = 0; i < length; ++i)
			{
				var randomCharIndex = RandomNumberGenerator.GetInt32(0, Chars.Length);
				var ch = m_Chars[randomCharIndex];
				m_StringBuilder.Append(ch);
			}

			var identifier = m_StringBuilder.ToString();
			return identifier;
		}
	}
}