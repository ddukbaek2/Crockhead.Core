using Crockhead.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;


namespace Crockhead.Logging
{
	/// <summary>
	/// 로그 처리자.
	/// </summary>
	public class Logger : Disposable, ILogger
	{
		/// <summary>
		/// 기본 기록 데이터.
		/// </summary>
		public readonly struct Entry : ILogEntry
		{
			/// <summary>
			/// 기록 일자.
			/// </summary>
			public DateTime DateTime { get; }

			/// <summary>
			/// 내용.
			/// </summary>
			public string Message { get; }

			/// <summary>
			/// 예외.
			/// </summary>
			public Exception Exception { get; }

			/// <summary>
			/// 생성됨.
			/// </summary>
			public Entry(DateTime dateTime, string text, Exception exception)
			{
				DateTime = dateTime;
				Message = text;
				Exception = exception;
			}
		}


		/// <summary>
		/// 기본 형식 지정자.
		/// </summary>
		public class Formatter : ILogFormatter
		{
			/// <summary>
			/// 양식에 맞춘 문자열로 변환.
			/// </summary>
			string ILogFormatter.Format(ILogger logger, ILogEntry entry)
			{
				var name = logger.Name;
				var timestamp = entry.DateTime.ToString("yyyy-MM-dd HH:mm:ss:FFF");

				if (entry.Exception == null)
				{
					return $"[{timestamp}][{logger.Name}] {entry.Message}";
				}
				else
				{
					return $"[{timestamp}][{logger.Name}] {entry.Exception}";
				}
			}
		}


		/// <summary>
		/// 락.
		/// </summary>
		private object m_Locked;

		/// <summary>
		/// 이름.
		/// </summary>
		private string m_Name;

		/// <summary>
		/// 사용 여부.
		/// </summary>
		private bool m_IsEnabled;

		/// <summary>
		/// 기록 처리자 목록.
		/// </summary>
		private List<ILogWriter> m_Writers;

		/// <summary>
		/// 형식 지정자.
		/// </summary>
		private ILogFormatter m_Formatter;

		/// <summary>
		/// 이름 프로퍼티.
		/// </summary>
		public string Name => m_Name;

		/// <summary>
		/// 사용 여부 프로퍼티.
		/// </summary>
		public bool IsEnabled => m_IsEnabled;

		/// <summary>
		/// 인터페이스 변환 프로퍼티.
		/// </summary>
		public ILogger AsLogger => (ILogger)this;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Logger(string name) : base()
		{
			m_Locked = new object();
			m_Name = name;
			m_IsEnabled = true;
			m_Writers = new List<ILogWriter>();
			Loggers.AddLogger(this);
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Logger(string name, ILogWriter writer = null, ILogFormatter formatter = null) : this(name)
		{
			m_Locked = new object();
			m_Name = name;
			m_IsEnabled = true;
			m_Writers = new List<ILogWriter>();
			AddWriter(writer);
			SetFormatter(formatter);
			Loggers.AddLogger(this);
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			Loggers.RemoveLogger(m_Name);
			Disposables.Dispose(m_Formatter);
			m_Formatter = null;

			m_Writers.Clear();
			m_Writers = null;
		}

		/// <summary>
		/// 사용 여부 설정.
		/// </summary>
		public void SetEnable(bool enabled)
		{
			m_IsEnabled = enabled;
		}

		/// <summary>
		/// 형식 지정자 설정.
		/// </summary>
		public void SetFormatter(ILogFormatter formatter)
		{
			m_Formatter = formatter;
			if (m_Formatter == null)
				m_Formatter = new Formatter();
		}

		/// <summary>
		/// 기록 처리자 추가.
		/// </summary>
		public void AddWriter(ILogWriter writer)
		{
			if (writer == null)
				return;

			if (m_Writers.Contains(writer))
				return;

			m_Writers.Add(writer);
		}

		/// <summary>
		/// 기록 처리자 제거.
		/// </summary>
		public void RemoveWriter(ILogWriter writer)
		{
			if (writer == null)
				return;

			if (!m_Writers.Contains(writer))
				return;

			m_Writers.Remove(writer);
		}

		/// <summary>
		/// 기록 처리자 전체 제거.
		/// </summary>
		public void RemoveAllWriters()
		{
			m_Writers.Clear();
		}

		/// <summary>
		/// 기록 처리자 포함 여부.
		/// </summary>
		public bool ContainsWriter(ILogWriter writer)
		{
			if (writer == null)
				return false;

			return m_Writers.Contains(writer);
		}

		/// <summary>
		/// 기록.
		/// </summary>
		void ILogger.Log(ILogEntry entry)
		{
			if (entry == null)
				return;

			if (m_IsEnabled)
				return;

			try
			{
				var message = m_Formatter.Format(this, entry);

				var writers = default(ILogWriter[]);
				lock (m_Locked)
				{
					writers = m_Writers.ToArray();
				}

				foreach (var writer in writers)
				{
					writer.Write(this, message);
				}
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// 기록.
		/// </summary>
		public void Log(string text)
		{
			var record = new Entry(DateTime.Now, text, null);
			AsLogger.Log(record);
		}

		/// <summary>
		/// 기록.
		/// </summary>
		public void Log(Exception exception)
		{
			var entry = new Entry(DateTime.Now, string.Empty, exception);
			AsLogger.Log(entry);
		}
	}
}