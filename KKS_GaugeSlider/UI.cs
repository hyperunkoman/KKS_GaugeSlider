//#define DBG_OUT
using System;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace KKS_GaugeSlider
{
	public class RadialSlider : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, ICanvasRaycastFilter
	{
		public float Thickness { get; set; }

		public float Radius { get; set; }

		public Vector2 CenterOffset { get; set; } = Vector2.zero;

		public Action<float> onClick { get; set; }

#if DBG_OUT
		private string m_lastStr;
#endif
		public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
		{
#if DBG_OUT
			var sb = new StringBuilder();
			sb.AppendFormat("isvalid[{0:X8}]:({1:F0},{2:F0})", this.GetInstanceID(), sp.x, sp.y);
#endif
			RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, sp, eventCamera, out sp);
			float num = Vector2.Distance(sp, CenterOffset);
			bool isValid = (Radius < num && num < Radius + Thickness);
#if DBG_OUT
			sb.AppendFormat("->({0:F0},{1:F0})-> dist:{2:F0} -> {3}", sp.x, sp.y, num, isValid);
			if (m_lastStr == null || m_lastStr.CompareTo(sb.ToString()) != 0)
			{
				m_lastStr = sb.ToString();
				Console.WriteLine(m_lastStr);
			}
#endif
			return isValid;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			GraphicRaycaster componentInParent = GetComponentInParent<GraphicRaycaster>();
			if (componentInParent != null)
			{
				RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, Input.mousePosition, componentInParent.eventCamera, out Vector2 localPoint);
				localPoint -= CenterOffset;
				float rate = Mathf.Atan2(0f - localPoint.x, localPoint.y) / Mathf.PI;
				if (onClick != null)
				{
#if DBG_OUT
					Console.WriteLine(string.Format("Click:{0:F4}", rate));
#endif
					onClick(rate);
				}
			}
		}
	}

	public class UI
	{
		public static GameObject CreateRadialSlider(GameObject parent)
		{
			GameObject gameObject = new GameObject("Radial Slider");
			Image obj = gameObject.AddComponent<Image>();
			obj.sprite = null;
			obj.type = Image.Type.Filled;
			obj.color = (Color.clear);
			obj.fillMethod = Image.FillMethod.Radial360;
			obj.fillOrigin = (0);
			obj.fillClockwise = (false);
			RectTransform component = parent.gameObject.GetComponent<RectTransform>();
			RectTransform component2 = gameObject.GetComponent<RectTransform>();
			component2.pivot = Vector2.up;
			component2.anchorMin = Vector2.up;
			component2.anchorMax = Vector2.up;
			component2.sizeDelta = new Vector2(component.sizeDelta.x, component.sizeDelta.y);
			component2.anchoredPosition = Vector2.zero;
			gameObject.AddComponent<RadialSlider>();
			gameObject.transform.SetParent(parent.transform, worldPositionStays: false);
			return gameObject;
		}
	}
}