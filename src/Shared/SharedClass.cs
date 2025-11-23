namespace Crockhead.Core
{
	/// <summary>
	/// 공유 클래스.
	/// </summary>
	public class SharedClass<TClass> : Disposable where TClass : SharedClass<TClass>, new()
	{
		/// <summary>
		/// 공유 클래스 프로퍼티.
		/// </summary>
		public static TClass Instance => Create();

		/// <summary>
		/// 생성됨.
		/// </summary>
		public SharedClass() : base()
		{
			// 이미 동일 타입의 공유 인스턴스가 존재 할 경우 제외.
			// 이후는 IsShared(this)로 판별.
			if (SharedInstances.IsSet<TClass>())
			{
				return;
			}

			// 등록.
			SharedInstances.Set<TClass>((TClass)this);
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			// 현재 인스턴스가 공유 인스턴스가 아니면 제외.
			if (!IsShared())
				return;

			// 등록 해제.
			SharedInstances.Unset<TClass>();
		}

		/// <summary>
		/// 현재 인스턴스가 공유 인스턴스인지 여부.
		/// </summary>
		public bool IsShared()
		{
			// 혹시 객체가 파괴된 경우는 실패처리.
			if (Disposables.IsDisposed(this))
				return false;

			var shared = IsShared((TClass)this);
			return shared;
		}

		/// <summary>
		/// 생성.
		/// </summary>
		public static TClass Create()
		{
			if (SharedInstances.TryGet<TClass>(out var sharedInstance))
				return sharedInstance;

			sharedInstance = new TClass();
			return sharedInstance;
		}

		/// <summary>
		/// 공유 인스턴스가 생성 되었는지 여부.
		/// </summary>
		public static bool IsCreated()
		{
			var created = SharedInstances.IsSet<TClass>();
			return created;
		}

		/// <summary>
		/// 대상 인스턴스가 공유 인스턴스인지 여부.
		/// </summary>
		public static bool IsShared(TClass obj)
		{
			// 공유 인스턴스의 존재 여부.
			if (!SharedInstances.IsSet<TClass>())
				return false;

			// 공유 인스턴스와 동일 인스턴스 여부.
			var sharedInstance = SharedInstances.Get<TClass>();
			if (sharedInstance != obj)
				return false;

			return true;
		}

		/// <summary>
		/// 해제.
		/// </summary>
		public static bool Dispose(TClass obj)
		{
			if (!SharedClass<TClass>.IsShared(obj))
				return false;

			Disposables.Dispose(obj);
			return true;
		}
	}
}