namespace Crockhead.Core
{
	/// <summary>
	/// 공유 클래스.
	/// </summary>
	public abstract class SharedClass<TClass> : Disposable where TClass : SharedClass<TClass>, new()
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
		/// 생성됨.
		/// </summary>
		public SharedClass() : base()
		{
			// 중복생성의 경우 제외.
			if (SharedInstances.IsSet<TClass>())
			{
				//Disposables.Dispose(this);
				return;
			}
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			if (explicitDisposing)
			{
				if (!SharedInstances.IsSet<TClass>())
					return;

				var sharedInstance = SharedInstances.Get<TClass>();
				if (sharedInstance != this)
					return;

				SharedInstances.Unset<TClass>();
			}
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected abstract void OnCreate(params object[] arguments);

		/// <summary>
		/// 생성.
		/// </summary>
		public static TClass Create(params object[] arguments)
		{
			if (SharedInstances.TryGet<TClass>(out var sharedInstance))
				return sharedInstance;

			sharedInstance = new TClass();
			SharedInstances.Set<TClass>(sharedInstance);
			sharedInstance.OnCreate(arguments);
			return sharedInstance;
		}
	}
}