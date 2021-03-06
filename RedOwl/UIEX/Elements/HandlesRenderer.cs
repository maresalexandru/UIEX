﻿#pragma warning disable 0649 // UXMLReference variable declared but not assigned to.
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_2019_1_OR_NEWER
using UnityEngine.UIElements;
using UnityEngine.UIElements.StyleEnums;
#else
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;
#endif

namespace RedOwl.Editor
{
	public interface IHandlesBezier
	{
		IEnumerable<Tuple<Vector2, Vector2, Color, float>> GetBezierPoints();
	}
	
	public class HandlesRenderer : RedOwlVisualElement
	{
		public new class UxmlFactory : UxmlFactory<HandlesRenderer> {}
		
		private IHandlesBezier data;
		
		private IMGUIContainer container;
		
		public HandlesRenderer() : base() {}
		
		public void Load(IHandlesBezier data) { this.data = data; }
	    
		[UICallback(1, true)]
		private void CreateUI()
		{
			container = new IMGUIContainer(UpdateUI);
			container.style.overflow = Overflow.Visible;
			Add(container);
		}
	    
		private void UpdateUI()
		{
			if (data != null) DrawHandlesBezier();
		}
		
		private void DrawHandlesBezier()
		{
			Vector2 start;
			Vector2 startTangent = Vector2.right;
			Vector2 end;
			Vector2 endTangent = Vector2.left;
			float factor = 40;
			foreach (var point in data.GetBezierPoints())
			{
				start = container.WorldToLocal(point.Item1);
				end = container.WorldToLocal(point.Item2);
				//TODO: Turn this into a 2 curve drawing where you draw from
				// start to midpoint and then end to midpoint where midpoint is end - start
				Handles.DrawBezier(
					start,
					end,
					start + startTangent * factor,
					end + endTangent * factor,
					point.Item3,
					null,
					point.Item4
				);
			}
		}
	}
}
