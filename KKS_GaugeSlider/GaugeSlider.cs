using BepInEx;
using BepInEx.Logging;
using RuntimeUnityEditor.Core;
using UnityEngine;
using UnityEngine.UI;

namespace KKS_GaugeSlider
{
	[BepInPlugin("GaugeSlider", "Gauge Slider", "0.1")]
	[BepInDependency(RuntimeUnityEditorCore.GUID, RuntimeUnityEditorCore.Version)]
    [BepInDependency(KKAPI.KoikatuAPI.GUID, KKAPI.KoikatuAPI.VersionConst)]
    public class GaugeSlider : BaseUnityPlugin
    {
		internal static new ManualLogSource Logger;

		private HFlag m_hFlag;

		public GaugeSlider()
        {
			Logger = base.Logger;
		}

		public void Start()
        {
            KKAPI.MainGame.GameAPI.StartH += GameAPI_StartH;
			KKAPI.MainGame.GameAPI.EndH += GameAPI_EndH;
		}

        private void GameAPI_EndH(object sender, System.EventArgs e)
        {
			//Logger.LogDebug(string.Format("EndH"));
		}

        private void GameAPI_StartH(object sender, System.EventArgs e)
        {
			m_hFlag = Object.FindObjectOfType<HFlag>();
			//Logger.LogDebug(string.Format("StartH, flag=%b", m_hFlag != null));
			SetSlider_H();
		}

		private void SetSlider_H()
		{
			const float fgauge_size = 130.0f;
			const float fgauge_width = 11.0f;
			const float mgauge_size = 150.0f;
			const float mgauge_width = 10.0f;
			const float center_x_off = 22.0f;
			const float center_y_off = -38.0f;
			const float gauge_val_off = -0.04f;
			const float gauge_val_factor = 2.18f;
			Vector2 centerOffset = new Vector2(center_x_off, center_y_off);

			if (m_hFlag == null)
			{
				return;
			}
			GameObject gaugeFrame = GameObject.Find("Canvas/Gauge/Frame");
			if (gaugeFrame != null && !gaugeFrame.transform.Find("Radial Slider"))
			{
				GameObject femaleSlider = UI.CreateRadialSlider(gaugeFrame);
				RectTransform femaleTf = femaleSlider.GetComponent<RectTransform>();
				RadialSlider femaleUI = femaleSlider.GetComponent<RadialSlider>();
				femaleUI.Thickness = fgauge_width;
				femaleUI.Radius = fgauge_size;
				femaleUI.CenterOffset = centerOffset;
				femaleUI.onClick = delegate (float x)
				{
					//Logger.LogMessage(string.Format("Female gauge: {0}", x));
					x = (1.0f + x + gauge_val_off) * gauge_val_factor;
					m_hFlag.gaugeFemale = Mathf.Clamp(x * 100f, 0f, 99.9f);
				};
				femaleTf.anchoredPosition = Vector2.zero;
				GameObject maleSlider = UI.CreateRadialSlider(gaugeFrame);
				RectTransform maleTf = maleSlider.GetComponent<RectTransform>();
				RadialSlider maleUI = maleSlider.GetComponent<RadialSlider>();
				maleUI.Thickness = mgauge_width;
				maleUI.Radius = mgauge_size;
				maleUI.CenterOffset = centerOffset;
				maleUI.onClick = delegate (float x)
				{
					//Logger.LogMessage(string.Format("Male gauge: {0}", x));
					x = (1.0f + x + gauge_val_off) * gauge_val_factor;
					m_hFlag.gaugeMale = Mathf.Clamp(x * 100f, 0f, 99.9f);
				};
				maleTf.anchoredPosition = Vector2.zero;
				Image[] componentsInChildren = GameObject.Find("Canvas/Gauge/Female").GetComponentsInChildren<Image>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
                    componentsInChildren[i].raycastTarget = (false);
				}
				componentsInChildren = GameObject.Find("Canvas/Gauge/Male").GetComponentsInChildren<Image>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
                    componentsInChildren[i].raycastTarget = (false);
				}
				gaugeFrame.GetComponent<Image>().raycastTarget = (false);
				//GameObject gameObject4 = GameObject.Find("Canvas/face");
				//canvasH = gameObject4.transform.parent.GetComponent<Canvas>();
				//RectTransform component5 = gameObject4.GetComponent<RectTransform>();
				//UI.CreateButton(gameObject4, new Rect(0f, 0f, component5.sizeDelta.y, component5.sizeDelta.y), string.Empty, delegate
				//{
				//	GuiToggle();
				//}, Color.black, Color.clear, component5.sizeDelta.y * 0.9f);

				var faceRawImage = GameObject.Find("Canvas/face/RawImage");
                if (faceRawImage != null)
                {
					faceRawImage.GetComponent<RawImage>().raycastTarget = false;
				}
			}
		}

	}
}
