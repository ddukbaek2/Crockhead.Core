namespace Crockhead.Core
{
	/// <summary>
	/// 공유 클래스.
	/// </summary>
	public class SharedClass<TClass> : Disposable where TClass : SharedClass<TClass>, new()
	{
		/// <summary>
		/// 생성 되었는지 여부 프로퍼티.
		/// </summary>
		public static bool IsCreated => SharedInstances.IsSet<TClass>();

		/// <summary>
		/// 공유 클래스 프로퍼티.
		/// </summary>
		public static TClass Instance => Create();

		/// <summary>
		/// 클래스 타입의 이름 프로퍼티.
		/// </summary>
		public static string ClassName => typeof(TClass).Name;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public SharedClass() : base()
		{
			// 이미 동일 타입의 공유 인스턴스가 존재 할 경우 제외.
			// 이후는 IsShared(this)로 판별.
			if (SharedInstances.IsSet<TClass>())
				return;

			SharedInstances.Set<TClass>((TClass)this);
			OnCreate();
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected sealed override void OnDispose(bool explicitDisposing)
		{
			if (!SharedInstances.TryGet<TClass>(out var sharedInstance))
				return;

			if (sharedInstance != this)
				return;

			SharedInstances.Unset<TClass>();
			OnDispose();
		}

		/// <summary>
		/// 생성됨. (공유 인스턴스 전용)
		/// </summary>
		protected virtual void OnCreate()
		{
		}

		/// <summary>
		/// 해제됨. (공유 인스턴스 전용)
		/// </summary>
		protected virtual void OnDispose()
		{
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
		/// 해제.
		/// </summary>
		public static void Dispose()
		{
			if (!SharedInstances.TryGet<TClass>(out var sharedInstance))
				return;

			if (Disposables.IsDisposed(sharedInstance))
				return;

			Disposables.Dispose(sharedInstance);
		}
	}
}