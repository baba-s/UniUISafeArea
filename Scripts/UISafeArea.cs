using UnityEngine;

namespace Kogane
{
	/// <summary>
	/// uGUI でセーフエリアに対応するためのコンポーネント
	/// </summary>
	[ExecuteAlways]
	[RequireComponent( typeof( RectTransform ) )]
	public sealed class UISafeArea : MonoBehaviour
	{
		//==============================================================================
		// 変数(SerializeField)
		//==============================================================================
		[SerializeField] private bool m_ignoreLeft   = default;
		[SerializeField] private bool m_ignoreRight  = default;
		[SerializeField] private bool m_ignoreTop    = default;
		[SerializeField] private bool m_ignoreBottom = default;

		//==============================================================================
		// 変数
		//==============================================================================
		private RectTransform m_rectTransformCache;
		private Rect          m_currentArea;
		private bool          m_isForce;

		//==============================================================================
		// プロパティ
		//==============================================================================
		private RectTransform RectTransform
		{
			get
			{
				if ( m_rectTransformCache == null )
				{
					m_rectTransformCache = GetComponent<RectTransform>();
				}

				return m_rectTransformCache;
			}
		}

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// 有効になった時に呼び出されます
		/// </summary>
		private void OnEnable()
		{
			Apply();
		}

#if UNITY_EDITOR

		/// <summary>
		/// 更新される時に呼び出されます
		/// </summary>
		private void Update()
		{
			if ( Application.isPlaying ) return;
			Apply( m_isForce );
			m_isForce = false;
		}

		/// <summary>
		/// パラメータが変更された時に呼び出されます
		/// </summary>
		private void OnValidate()
		{
			if ( Application.isPlaying ) return;
			m_isForce = true;
		}
#endif

		/// <summary>
		/// 現在の画面のセーフエリアに合わせて描画範囲を適用します
		/// </summary>
		private void Apply( bool isForce = false )
		{
			ApplyFrom( Screen.safeArea, isForce );
		}

		/// <summary>
		/// 指定されたセーフエリアに合わせて描画範囲を適用します
		/// </summary>
		private void ApplyFrom( Rect area, bool isForce = false )
		{
			if ( RectTransform == null ) return;
			if ( !isForce && m_currentArea == area ) return;

			var anchorMin = area.position;
			var anchorMax = area.position + area.size;

			anchorMin.x /= Screen.width;
			anchorMin.y /= Screen.height;
			anchorMax.x /= Screen.width;
			anchorMax.y /= Screen.height;

			if ( m_ignoreLeft )
			{
				anchorMin.x = 0f;
			}

			if ( m_ignoreRight )
			{
				anchorMax.x = 1f;
			}

			if ( m_ignoreTop )
			{
				anchorMax.y = 1f;
			}

			if ( m_ignoreBottom )
			{
				anchorMin.y = 0f;
			}

			RectTransform.anchoredPosition = Vector2.zero;
			RectTransform.sizeDelta        = Vector2.zero;
			RectTransform.anchorMin        = anchorMin;
			RectTransform.anchorMax        = anchorMax;

			m_currentArea = area;
		}
	}
}