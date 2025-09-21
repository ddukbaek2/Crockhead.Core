using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Crockhead.Core
{
	/// <summary>
	/// 통보자.
	/// <para>[NotifiableMethod(typeof(Notification))]void OnNotification(Notification){}</para>
	/// </summary>
	public class Notifier : Registry<INotifiable>
	{
		/// <summary>
		/// 메서드 정보 목록. (캐시)
		/// <para>{ Key: typeof(INotifiable), Value: { Key: typeof(INotification), Value: MethodInfo[] } }</para>
		/// </summary>
		private Dictionary<Type, Dictionary<Type, List<MethodInfo>>> m_CachedDictionary;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Notifier() : base()
		{
			m_CachedDictionary = new Dictionary<Type, Dictionary<Type, List<MethodInfo>>>();
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			m_CachedDictionary.Clear();

			base.OnDispose(explicitDisposing);
		}

		/// <summary>
		/// 등록.
		/// </summary>
		public override bool Register(INotifiable target)
		{
			// 등록.
			if (!base.Register(target))
				return false;

			var targetType = target.GetType();

			// 대상 객체의 등록 여부 확인.
			if (m_CachedDictionary.ContainsKey(targetType))
				return true;

			var notifiableMethodInfos = new Dictionary<Type, List<MethodInfo>>();
			var notifiableMethodAttributeType = typeof(NotifiableMethodAttribute);
			var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

			// 대상 객체의 모든 메서드를 조사.
			foreach (var methodInfo in targetType.GetMethods(bindingFlags))
			{
				//// 메서드에 통보 특성 확인.
				//if (!methodInfo.IsDefined(notifiableMethodAttributeType))
				//	continue;

				foreach (var attribute in methodInfo.GetCustomAttributes(notifiableMethodAttributeType, true))
				{
					var notifiableMethodAttribute = attribute as NotifiableMethodAttribute;

					// 특성에 지정된 통보 내용 타입 존재 여부 확인.
					var notificationType = notifiableMethodAttribute.NotificationType;
					if (notificationType == null)
						continue;

					// 특성에 지정된 통보 내용 타입의 INotification 상속 확인.
					if (!typeof(INotification).IsAssignableFrom(notificationType))
						continue;

					// 인자가 1개 초과인 경우는 제외.
					// 인자는 1개 혹은 0개.
					var parameterInfos = methodInfo.GetParameters();
					if (parameterInfos.Length > 1)
						continue;

					// 인자가 1개일 경우 통보 내용 타입과 일치하는지 체크.
					if (parameterInfos.Length == 1)
					{
						var parameterInfo = parameterInfos[0];
						if (!parameterInfo.ParameterType.IsAssignableFrom(notificationType))
							continue;
					}

					// 등록.
					if (notifiableMethodInfos.TryGetValue(notificationType, out var methodInfos))
					{
						methodInfos.Add(methodInfo);
					}
					else
					{
						methodInfos = new List<MethodInfo>();
						methodInfos.Add(methodInfo);
						notifiableMethodInfos.Add(notificationType, methodInfos);
					}
				}
			}

			m_CachedDictionary.Add(targetType, notifiableMethodInfos);
			return true;
		}

		/// <summary>
		/// 대상 지정 통보.
		/// </summary>
		public void Send(INotifiable target, INotification notification)
		{
			// 객체 유효성 확인.
			if (target == null || notification == null)
				return;

			// 등록 여부 확인.
			if (!IsRegistered(target))
				return;

			// 대상 타입으로 등록된 메서드 목록 찾기.
			var targetType = target.GetType();
			if (!m_CachedDictionary.TryGetValue(targetType, out var notifiableMethodInfos))
				return;

			// 통보 내용 타입으로 등록된 메서드 정보 찾기.
			var notificationType = notification.GetType();
			if (!notifiableMethodInfos.TryGetValue(notificationType, out var methodInfos))
				return;

			// 한 클래스 내에 동일 통보 내용 메서드가 여러개가 있어도 전부 통보.
			foreach (var methodInfo in methodInfos)
			{
				try
				{
					// 인자가 없는 이벤트 수신 + 데이터 전달 목적.
					var parameterInfos = methodInfo.GetParameters();
					if (parameterInfos.Length == 1)
					{
						methodInfo.Invoke(target, new object[] { notification });
					}
					// 인자가 없는 이벤트 수신 목적.
					else
					{
						methodInfo.Invoke(target, null);
					}
				}
				catch
				{
				}
			}
		}

		/// <summary>
		/// 전체 통보.
		/// </summary>
		public void Notify(INotification notification)
		{
			// 등록된 객체의 중간 변경에 의한 컬렉션 오류 방지 스냅샷.
			var targets = Values.ToArray();

			foreach (var target in targets)
			{
				Send(target, notification);
			}
		}
	}
}