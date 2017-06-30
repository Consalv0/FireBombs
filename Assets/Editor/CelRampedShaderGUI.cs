using UnityEditor;
using UnityEngine;
using System;

public class CelRampedShaderGUI : ShaderGUI {
	public enum BlendMode {
		Opaque,
		Cutout,
		Fade,
		Transparent
	}

	MaterialProperty color;
	MaterialProperty cutoutAlpha;
	MaterialProperty highlightColor;
	MaterialProperty shadowColor;

	MaterialProperty textureMap;
	MaterialProperty textureRamp;

	MaterialProperty outlineColor;
	MaterialProperty outlineWeight;

	MaterialProperty rimColor;
	MaterialProperty rimMin;
	MaterialProperty rimMax;

	MaterialProperty modeBlend;
	MaterialProperty srcBlend;
	MaterialProperty dstBlend;
	MaterialProperty zWrite;

	public void FindProperties(MaterialProperty[] props) {
		color = FindProperty("_Color", props);
		cutoutAlpha = FindProperty("_Cutoff", props);
		highlightColor = FindProperty("_HColor", props);
		shadowColor = FindProperty("_SColor", props);

		textureMap = FindProperty("_MainTex", props);
		textureRamp = FindProperty("_Ramp", props);

		outlineColor = FindProperty("_OutlineColor", props);
		outlineWeight = FindProperty("_OutlineWeight", props);

		rimColor = FindProperty("_RimColor", props);
		rimMax = FindProperty("_RimMax", props);
		rimMin = FindProperty("_RimMin", props);

		modeBlend = FindProperty("_Mode", props);
		srcBlend = FindProperty("_SrcBlend", props);
		dstBlend = FindProperty("_DstBlend", props);
		zWrite = FindProperty("_ZWrite", props);
	}

	public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties) {
		FindProperties(properties);
		Material material = materialEditor.target as Material;
		BlendMode render = (BlendMode)Enum.ToObject(typeof(BlendMode), Convert.ToInt32(modeBlend.floatValue));
		render = (BlendMode)EditorGUILayout.EnumPopup("Render Mode", (BlendMode)Enum.ToObject(typeof(BlendMode), Convert.ToInt32(modeBlend.floatValue)));
		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Maps And Color", EditorStyles.boldLabel);
		EditorGUILayout.BeginHorizontal();
		materialEditor.TexturePropertySingleLine(new GUIContent("Albedo", "Albedo (RGB) and Transparency (A)"), textureMap);
		EditorGUILayout.LabelField("Color", GUILayout.MinWidth(60));
		color.colorValue = materialEditor.ColorProperty(GUILayoutUtility.GetLastRect(), color, "");
		EditorGUILayout.EndHorizontal();
		EditorGUI.indentLevel += 1;
		if (render == BlendMode.Cutout)
			cutoutAlpha.floatValue = EditorGUILayout.Slider("Cutout Alpha", cutoutAlpha.floatValue, 0, 1);
		EditorGUILayout.BeginHorizontal();
		EditorGUIUtility.labelWidth = 36;
		EditorGUILayout.LabelField("Highlight", GUILayout.MinWidth(62));
		EditorGUIUtility.labelWidth = 0;
		highlightColor.colorValue = EditorGUILayout.ColorField(highlightColor.colorValue);
		EditorGUIUtility.labelWidth = 36;
		EditorGUILayout.LabelField("Shadow", GUILayout.MinWidth(62));
		EditorGUIUtility.labelWidth = 0;
		shadowColor.colorValue = EditorGUILayout.ColorField(shadowColor.colorValue);
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(20);
		EditorGUILayout.LabelField("");
		materialEditor.TextureScaleOffsetProperty(GUILayoutUtility.GetLastRect(), textureMap);
		EditorGUILayout.Space();
		EditorGUI.indentLevel += -1;

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("", GUILayout.MinWidth(105));
		materialEditor.TexturePropertyMiniThumbnail(GUILayoutUtility.GetLastRect(), textureRamp, "Light Ramp", "Albedo (RGB) and Transparency (A)");
		GUILayout.FlexibleSpace();
		EditorGUIUtility.labelWidth = 420;
		EditorGUILayout.LabelField("", GUILayout.MinWidth(60));
		EditorGUI.DrawPreviewTexture(GUILayoutUtility.GetLastRect(), textureRamp.textureValue ? textureRamp.textureValue : new Texture2D(1, 1, TextureFormat.ARGB32, false));
		EditorGUIUtility.labelWidth = 0;
		EditorGUILayout.EndHorizontal();
		EditorGUI.indentLevel += 1;
		GUILayout.Space(20);
		EditorGUILayout.LabelField("");
		materialEditor.TextureScaleOffsetProperty(GUILayoutUtility.GetLastRect(), textureRamp);
		EditorGUI.indentLevel += -1;
		EditorGUILayout.Space();

		modeBlend.floatValue = render.GetHashCode();
		SetupMaterialWithBlendMode(material, render);
			
		base.OnGUI(materialEditor, properties);
	}

	public static void SetupMaterialWithBlendMode(Material material, BlendMode blendMode) {
		switch (blendMode) {
			case BlendMode.Opaque:
				material.SetOverrideTag("RenderType", "Opaque");
				material.SetInt("_SrcBlend", 1);
				material.SetInt("_DstBlend", 0);
				material.SetInt("_ZWrite", 1);
				material.renderQueue = -1;
				break;
			case BlendMode.Cutout:
				material.SetOverrideTag("RenderType", "TransparentCutout");
				material.SetInt("_SrcBlend", 5);
				material.SetInt("_DstBlend", 10);
				material.SetInt("_ZWrite", 1);
				material.renderQueue = 2450;

				break;
			case BlendMode.Fade:
				material.SetOverrideTag("RenderType", "Transparent");
				material.SetInt("_SrcBlend", 5);
				material.SetInt("_DstBlend", 10);
				material.SetInt("_ZWrite", 0);
				material.renderQueue = 3000;
				break;
			case BlendMode.Transparent:
				material.SetOverrideTag("RenderType", "Transparent");
				material.SetInt("_SrcBlend", 1);
				material.SetInt("_DstBlend", 10);
				material.SetInt("_ZWrite", 0);
				material.renderQueue = 3000;
				break;
		}
	}
}
