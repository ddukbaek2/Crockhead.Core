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
		/// 현재 인스턴스가 명시적인 생성인지 여부. (직접 Create() 호출)
		/// </summary>
		public bool IsExplicitCreated { private set; get; }

		/// <summary>
		/// 생성시 입력한 아규먼트.
		/// </summary>
		public object[] Arguments { private set; get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public SharedClass() : base()
		{
			IsExplicitCreated = false;
			Arguments = new object[0];

			// 이미 공유 인스턴스가 존재 할 경우 제외.
			if (SharedInstances.IsSet<TClass>())
				return;

			// 등록.
			SharedInstances.Set<TClass>((TClass)this);
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected virtual void OnCreate(params object[] arguments)
		{
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			// 현재 인스턴스가 공유 인스턴스가 아니면 제외.
			if (!SharedClass<TClass>.IsSharedInstance((TClass)this))
				return;

			// 등록 해제.
			SharedInstances.Unset<TClass>();
		}

		/// <summary>
		/// 생성.
		/// </summary>
		public static TClass Create(params object[] arguments)
		{
			if (SharedInstances.TryGet<TClass>(out var obj))
				return obj;

			obj = new TClass();
			obj.IsExplicitCreated = true;
			obj.Arguments = arguments;
			obj.OnCreate(arguments);
			return obj;
		}

		/// <summary>
		/// 대상 인스턴스가 공유 인스턴스인지 여부.
		/// </summary>
		public static bool IsSharedInstance(TClass obj)
		{
			// 공유 인스턴스의 존재 여부.
			if (!SharedInstances.IsSet<TClass>())
				return false;

			// 공유 인스턴스와 동일 인스턴스 여부.
			var instance = SharedInstances.Get<TClass>();
			if (instance != obj)
				return false;

			return true;
		}

		/// <summary>
		/// 해제.
		/// </summary>
		public static bool Dispose(TClass obj)
		{
			if (!SharedClass<TClass>.IsSharedInstance(obj))
				return false;

			Disposables.Dispose(obj);
			return true;
		}
	}
}