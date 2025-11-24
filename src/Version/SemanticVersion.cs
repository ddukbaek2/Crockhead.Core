using System;
using System.Collections;
using System.Collections.Generic;


namespace Crockhead.Core
{
	/// <summary>
	/// 시멘틱 버전.
	/// <para>Major.Minor.Patch-Qualifier</para>
	/// </summary>
	public class SemanticVersion : Disposable
	{
		/// <summary>
		/// 버전 숫자 구분자.
		/// </summary>
		public const char Dot = '.';

		/// <summary>
		/// 버전 한정자 구분자.
		/// </summary>
		public const char Hyphen = '-';

		/// <summary>
		/// 주 버전 프로퍼티.
		/// <para>기존 버전과 호환되지 않는 대규모 변경이나 새로운 기능 추가.</para>
		/// </summary>
		public int Major { set; get; }

		/// <summary>
		/// 부 버전 프로퍼티.
		/// <para>기존 버전과 호환되면서 새로운 기능이 추가되거나 개선.</para>
		/// </summary>
		public int Minor { set; get; }

		/// <summary>
		/// 수 버전 프로퍼티.
		/// <para>기존 버전과 호환되면서 버그 수정이나 사소한 오류 수정 등 작은 변경.</para>
		/// </summary>
		public int Patch { set; get; }

		/// <summary>
		/// 버전 한정자 프로퍼티.
		/// <para>버전 뒤에 붙여 해당 버전의 특정 상태를 식별하는 분류 태그.</para>
		/// </summary>
		public string Qualifier { set; get; }

		/// <summary>
		/// 버전 문자열 프로퍼티.
		/// </summary>
		public string VersionString
		{
			set
			{
				Parse(value);
			}
			get
			{
				if (string.IsNullOrWhiteSpace(Qualifier))
				{
					return $"{Major}.{Minor}.{Patch}";
				}
				else
				{
					return $"{Major}.{Minor}.{Patch}-{Qualifier}";
				}
			}
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public SemanticVersion() : base()
		{
			Major = 0;
			Minor = 0;
			Patch = 0;
			Qualifier = string.Empty;
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public SemanticVersion(int major, int minor, int patch, string qualifier) : this()
		{
			Major = major;
			Minor = minor;
			Patch = patch;
			Qualifier = qualifier;
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public SemanticVersion(int major, int minor, int patch) : this(major, minor, patch, string.Empty)
		{
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public SemanticVersion(SemanticVersion version) : this()
		{
			if (version == null)
				throw new ArgumentNullException(nameof(version));

			Major = version.Major;
			Minor = version.Minor;
			Patch = version.Patch;
			Qualifier = version.Qualifier;
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public SemanticVersion(string versionString) : this()
		{
			try
			{
				// 해석 실패시 예외 던지기.
				Parse(versionString);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
		}

		/// <summary>
		/// 문자열 버전 데이터를 해석.
		/// </summary>
		public void Parse(string versionString)
		{
			if (string.IsNullOrWhiteSpace(versionString))
				throw new ArgumentNullException(nameof(versionString));

			versionString = versionString.Trim();

			// 버전 한정자 해석.
			Qualifier = string.Empty;
			var hyphenIndex = versionString.IndexOf(SemanticVersion.Hyphen);
			if (hyphenIndex != -1)
			{
				Qualifier = versionString.Substring(hyphenIndex + 1);
				versionString = versionString.Remove(hyphenIndex);
			}

			// 버전 숫자 해석.
			Major = 0;
			Minor = 0;
			Patch = 0;
			var numberStrings = versionString.Split(SemanticVersion.Dot);
			if (numberStrings.Length < 1 || numberStrings.Length > 3)
				throw new FormatException($"numberStrings.Length: {numberStrings.Length}");

			var number = 0;
			if (numberStrings.Length >= 1)
			{
				if (!int.TryParse(numberStrings[0], out number))
					throw new FormatException($"Invalid major version: '{numberStrings[0]}'");
				Major = number;
			}

			if (numberStrings.Length >= 2)
			{
				if (!int.TryParse(numberStrings[1], out number))
					throw new FormatException($"Invalid minor version: '{numberStrings[1]}'");
				Minor = number;
			}

			if (numberStrings.Length >= 3)
			{
				if (!int.TryParse(numberStrings[2], out number))
					throw new FormatException($"Invalid patch version: '{numberStrings[2]}'");
				Patch = number;
			}
		}

		/// <summary>
		/// 문자열 해석.
		/// </summary>
		public bool TryParse(string versionString)
		{
			try
			{
				Parse(versionString);
				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// 문자열 반환.
		/// </summary>
		public override string ToString()
		{
			return VersionString;
		}
	}
}