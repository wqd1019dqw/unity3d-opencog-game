
/// Unity3D OpenCog World Embodiment Program
/// Copyright (C) 2013  Novamente			
///
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU Affero General Public License as
/// published by the Free Software Foundation, either version 3 of the
/// License, or (at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU Affero General Public License for more details.
///
/// You should have received a copy of the GNU Affero General Public License
/// along with this program.  If not, see <http://www.gnu.org/licenses/>.

#region Usings, Namespaces, and Pragmas

using System.Collections;
using OpenCog.Attributes;
using OpenCog.Extensions;
using ImplicitFields = ProtoBuf.ImplicitFields;
using ProtoContract = ProtoBuf.ProtoContractAttribute;
using Serializable = System.SerializableAttribute;

//The private field is assigned but its value is never used
#pragma warning disable 0414

#endregion

namespace OpenCog
{

/// <summary>
/// The OpenCog OCInventoryGUI.
/// </summary>
#region Class Attributes

[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
[OCExposePropertyFields]
[Serializable]
	
#endregion
public class OCInventoryGUI : OCMonoBehaviour
{

	//---------------------------------------------------------------------------

	#region Private Member Data

	//---------------------------------------------------------------------------
	
	private OpenCog.BlockSet.OCBlockSet _blockSet;
	private OpenCog.Builder.OCBuilder _builder;

	private bool _show = false;
	// Member is not static, but a variable with the same name is used in DrawList
	private UnityEngine.Vector2 scrollPosition = UnityEngine.Vector3.zero;
			
	//---------------------------------------------------------------------------

	#endregion

	//---------------------------------------------------------------------------

	#region Accessors and Mutators

	//---------------------------------------------------------------------------
		

			
	//---------------------------------------------------------------------------

	#endregion

	//---------------------------------------------------------------------------

	#region Public Member Functions

	//---------------------------------------------------------------------------

	public void Awake () {
		Map map = (Map) GameObject.FindObjectOfType( typeof(Map) );
		_blockSet = map.GetBlockSet();
		GameObject player = GameObject.FindGameObjectWithTag( "Player" );
		_builder = (Builder) player.GetComponent<Builder>();
	}
	
	public void Update () {
		if( Input.GetKeyDown(KeyCode.E) && GameStateManager.IsPlaying ) {
			_show = !_show;
			Screen.showCursor = _show;
		}
		if(GameStateManager.IsPause) show = false;
	}
	
	public void OnGUI() {
		if(_show) {
			UnityEngine.Rect window = new UnityEngine.Rect(0, 0, Screen.width*0.5f, Screen.height*0.6f);
			window.center = new Vector2(Screen.width, Screen.height)/2f;
			GUILayout.Window(0, window, DoInventoryWindow, "Inventory");
		}
	}

	//---------------------------------------------------------------------------

	#endregion

	//---------------------------------------------------------------------------

	#region Private Member Functions

	//---------------------------------------------------------------------------
	
	private void DoInventoryWindow(int windowID) {
		Block selected = builder.GetSelectedBlock();
		selected = DrawInventory(_blockSet, ref _scrollPosition, selected);
		_builder.SetSelectedBlock(selected);
    }

	private static OpenCog.BlockSet.BaseBlockSet.OCBlock DrawInventory(OpenCog.BlockSet.OCBlockSet blockSet, ref UnityEngine.Vector2 scrollPosition, OpenCog.BlockSet.BaseBlockSet.OCBlock selected) {
		scrollPosition = GUILayout.BeginScrollView(scrollPosition);
		for(int i=0, y=0; i<blockSet.GetBlockCount(); y++) {
			GUILayout.BeginHorizontal();
			for(int x=0; x<8; x++, i++) {
				Block block = blockSet.GetBlock(i);
				if( DrawBlock(block, block == selected && selected != null) ) {
					selected = block;
				}
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndScrollView();
		return selected;
	}
	
	private static bool DrawBlock(OpenCog.BlockSet.BaseBlockSet.OCBlock block, bool selected) {
		UnityEngine.Rect rect = GUILayoutUtility.GetAspectRect(1f);
		
		if(selected) GUI.Box(rect, GUIContent.none);
		
		UnityEngine.Vector3 center = rect.center;
		rect.width -= 8;
		rect.height -= 8;
		rect.center = center;
		
		if(block != null) return block.DrawPreview(rect);
		return false;
	}
			
	//---------------------------------------------------------------------------

	#endregion

	//---------------------------------------------------------------------------

	#region Other Members

	//---------------------------------------------------------------------------		

	

	//---------------------------------------------------------------------------

	#endregion

	//---------------------------------------------------------------------------

}// class OCInventoryGUI

}// namespace OpenCog




