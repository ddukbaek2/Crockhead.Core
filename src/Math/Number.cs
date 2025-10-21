using System;
using System.Globalization;
using System.Numerics;
using System.Text;


namespace Crockhead.Core
{
	/// <summary>
	/// 정수와 실수가 포함된 임의정밀도를 가진 유리수. (BigInteger 기반)
	/// </summary>
	public readonly struct Number : IEquatable<Number>
	{
		/// <summary>
		/// 제로 프로퍼티.
		/// </summary>
		public static readonly Number Zero = new Number(0);

		/// <summary>
		/// 분자 프로퍼티.
		/// </summary>
		public BigInteger Numerator { get; }

		/// <summary>
		/// 분모 프로퍼티. (항상 0보다 크다)
		/// </summary>
		public BigInteger Denominator { get; }

		/// <summary>
		/// 0인지 여부 프로퍼티.
		/// </summary>
		public bool IsZero => Numerator.IsZero;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Number(BigInteger numerator) : this(numerator, BigInteger.One)
		{
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Number(BigInteger numerator, BigInteger denominator)
		{
			if (denominator.IsZero)
				throw new DivideByZeroException();

			if (denominator.Sign < 0)
			{
				numerator = BigInteger.Negate(numerator);
				denominator = BigInteger.Negate(denominator);
			}
			
			var greatest = BigInteger.GreatestCommonDivisor(BigInteger.Abs(numerator), denominator);
			Numerator = numerator / greatest;
			Denominator = denominator / greatest;
		}

		/// <summary>
		/// 동일 여부 반환.
		/// </summary>
		public bool Equals(Number other)
		{
			return Numerator.Equals(other.Numerator) && Denominator.Equals(other.Denominator);
		}

		/// <summary>
		/// 동일 여부 반환.
		/// </summary>
		public override bool Equals(object obj)
		{
			if (obj is Number other)
			{
				return Equals(other);
			}

			return false;
		}

		/// <summary>
		/// 해시값 반환.
		/// </summary>
		public override int GetHashCode()
		{
			return Numerator.GetHashCode() ^ Denominator.GetHashCode();
		}

		/// <summary>
		/// 문자열 변환.
		/// </summary>
		public override string ToString()
		{
			return $"{Numerator}/{Denominator}";
		}

		/// <summary>
		/// 출력 문자열 변환.
		/// </summary>
		public string ToDisplayString(int maxDigits = 40)
		{
			var numerator = BigInteger.Abs(Numerator);
			var denominator = Denominator;
			var integer = BigInteger.Divide(numerator, denominator);
			var remainder = numerator % denominator;
			var builder = new StringBuilder();
			if (Numerator.Sign < 0)
				builder.Append('-');

			builder.Append(integer.ToString(CultureInfo.InvariantCulture));

			if (remainder.IsZero)
				return builder.ToString();

			builder.Append('.');
			var count = 0;
			while (!remainder.IsZero && count < maxDigits)
			{
				remainder *= 10;
				var digit = remainder / denominator;
				remainder = remainder % denominator;
				builder.Append(digit.ToString());
				count++;
			}

			if (!remainder.IsZero)
				builder.Append("…");

			var trimCount = 0;
			for (var i = builder.Length - 1; i >= 0; --i)
			{
				var ch = builder[i];
				if (ch == '0')
				{
					trimCount++;
				}
				else
				{
					break;
				}
			}
			if (trimCount > 0)
			{
				builder.Length -= trimCount;
				if (builder[builder.Length - 1] == '.')
					builder.Length -= 1;
			}

			return builder.ToString();
		}

		/// <summary>
		/// 정수로 생성.
		/// </summary>
		public static Number Parse(long value)
		{
			return new Number(new BigInteger(value), BigInteger.One);
		}

		/// <summary>
		/// 소숫점이 존재하는 문자열로 생성.
		/// </summary>
		public static Number Parse(string text)
		{
			var dot = text.IndexOf('.');
			if (dot < 0)
			{
				var n = BigInteger.Parse(text, CultureInfo.InvariantCulture);
				return new Number(n, BigInteger.One);
			}
			var integerPart = text.Substring(0, dot);
			var fractionalPart = text.Substring(dot + 1);
			if (fractionalPart.Length == 0) fractionalPart = "0";

			var sign = 1;
			if (integerPart.StartsWith("+"))
			{
				integerPart = integerPart.Substring(1);
			}
			else if (integerPart.StartsWith("-"))
			{
				sign = -1;
				integerPart = integerPart.Substring(1);
			}

			var integerValue = integerPart.Length > 0 ? BigInteger.Parse(integerPart, CultureInfo.InvariantCulture) : BigInteger.Zero;
			var fractionalValue = BigInteger.Parse(fractionalPart, CultureInfo.InvariantCulture);
			var scale = BigInteger.Pow(10, fractionalPart.Length);
			var numerator = integerValue * scale + fractionalValue;
			if (sign < 0)
				numerator = BigInteger.Negate(numerator);
			return new Number(numerator, scale);
		}

		/// <summary>
		/// 비교 연산.
		/// </summary>
		public static bool operator ==(Number left, Number right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// 비교 연산.
		/// </summary>
		public static bool operator !=(Number left, Number right)
		{
			return !left.Equals(right);
		}

		/// <summary>
		/// 더하기 연산.
		/// </summary>
		public static Number operator +(Number left, Number right)
		{
			return new Number(left.Numerator * right.Denominator + right.Numerator * left.Denominator, left.Denominator * right.Denominator);
		}

		/// <summary>
		/// 빼기 연산.
		/// </summary>
		public static Number operator -(Number left, Number right)
		{
			return new Number(left.Numerator * right.Denominator - right.Numerator * left.Denominator, left.Denominator * right.Denominator);
		}

		/// <summary>
		/// 곱하기 연산.
		/// </summary>
		public static Number operator *(Number left, Number right)
		{
			return new Number(left.Numerator * right.Numerator, left.Denominator * right.Denominator);
		}

		/// <summary>
		/// 나누기 연산.
		/// </summary>
		public static Number operator /(Number left, Number right)
		{
			if (right.Numerator.IsZero)
				throw new DivideByZeroException();

			return new Number(left.Numerator * right.Denominator, left.Denominator * right.Numerator);
		}

		/// <summary>
		/// 나머지 연산.
		/// </summary>
		public static Number operator %(Number left, Number right)
		{
			if (right.Numerator.IsZero)
				throw new DivideByZeroException();

			var q = BigInteger.Divide(left.Numerator * right.Denominator, left.Denominator * right.Numerator);
			return left - new Number(q, BigInteger.One) * right;
		}
	}
}