using System;
using System.Collections.Generic;
using System.Security.Cryptography;


namespace Crockhead.Core
{
	/// <summary>
	/// 제네릭 정수 타입의 고유 식별자 생성기.
	/// </summary>
	public class NumberIdentifiers<TNumber> : Identifiers<TNumber>
	{
		/// <summary>
		/// 정수 타입 목록.
		/// </summary>
		public readonly static HashSet<Type> NumberTypes = new HashSet<Type>()
		{
			typeof(sbyte),
			typeof(short),
			typeof(int),
			typeof(long),
			typeof(byte),
			typeof(ushort),
			typeof(uint),
			typeof(ulong),
		};

		/// <summary>
		/// 정수 타입의 크기 목록.
		/// </summary>
		public readonly static Dictionary<Type, int> NumberTypeSizes = new Dictionary<Type, int>()
		{
			{ typeof(sbyte), sizeof(sbyte) },
			{ typeof(short), sizeof(short) },
			{ typeof(int), sizeof(int) },
			{ typeof(long), sizeof(long) },
			{ typeof(byte), sizeof(byte) },
			{ typeof(ushort), sizeof(ushort) },
			{ typeof(uint), sizeof(uint) },
			{ typeof(ulong), sizeof(ulong) },
		};


		/// <summary>
		/// 정수 타입의 변환 콜백 목록.
		/// </summary>
		public readonly static Dictionary<Type, Func<byte[], TNumber>> NumberTypeConverters = new Dictionary<Type, Func<byte[], TNumber>>()
		{
			{ typeof(sbyte), (bytes) => (TNumber)(object)(sbyte)bytes[0] },
			{ typeof(short), (bytes) => (TNumber)(object)BitConverter.ToInt16(bytes) },
			{ typeof(int), (bytes) => (TNumber)(object)BitConverter.ToInt32(bytes) },
			{ typeof(long), (bytes) => (TNumber)(object)BitConverter.ToInt64(bytes) },
			{ typeof(byte), (bytes) => (TNumber)(object)bytes[0] },
			{ typeof(ushort), (bytes) => (TNumber)(object)BitConverter.ToUInt16(bytes) },
			{ typeof(uint), (bytes) => (TNumber)(object)BitConverter.ToUInt32(bytes) },
			{ typeof(ulong), (bytes) => (TNumber)(object)BitConverter.ToUInt64(bytes) },
		};


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

			var type = typeof(TNumber);
			if (!NumberIdentifiers<TNumber>.NumberTypeSizes.TryGetValue(type, out var length))
				throw new NotSupportedException("Not Supported Number Type.");

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
		protected override TNumber OnCreateIdentifier()
		{
			var type = typeof(TNumber);
			if (!NumberTypeConverters.TryGetValue(type, out var converter))
				throw new NotSupportedException("Not Supported Number Type.");

			m_RandomNumberGenerator.GetBytes(m_ByteBuffer);
			var identifier = converter.Invoke(m_ByteBuffer);
			return identifier;
		}
		
		/// <summary>
		/// 복제.
		/// </summary>
		public NumberIdentifiers<TNumber> Clone()
		{
			var clonable = this as ICloneable;
			var identifiers = (NumberIdentifiers<TNumber>)clonable.Clone();
			return identifiers;
		}
	}
}