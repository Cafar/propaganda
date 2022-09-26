using UnityEngine;
using UnityEditor;

namespace Google2u
{
	[CustomEditor(typeof(Datos))]
	public class DatosEditor : Editor
	{
		public int Index = 0;
		public int _Key_Index = 0;
		public override void OnInspectorGUI ()
		{
			Datos s = target as Datos;
			DatosRow r = s.Rows[ Index ];

			EditorGUILayout.BeginHorizontal();
			if ( GUILayout.Button("<<") )
			{
				Index = 0;
			}
			if ( GUILayout.Button("<") )
			{
				Index -= 1;
				if ( Index < 0 )
					Index = s.Rows.Count - 1;
			}
			if ( GUILayout.Button(">") )
			{
				Index += 1;
				if ( Index >= s.Rows.Count )
					Index = 0;
			}
			if ( GUILayout.Button(">>") )
			{
				Index = s.Rows.Count - 1;
			}

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "ID", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.LabelField( s.rowNames[ Index ] );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "_Textos", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.TextField( r._Textos );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			if ( r._Key.Count == 0 )
			{
			    GUILayout.Label( "_Key", GUILayout.Width( 150.0f ) );
			    {
			    	EditorGUILayout.LabelField( "Empty Array" );
			    }
			}
			else
			{
			    GUILayout.Label( "_Key", GUILayout.Width( 130.0f ) );
			    if ( _Key_Index >= r._Key.Count )
				    _Key_Index = 0;
			    if ( GUILayout.Button("<", GUILayout.Width( 18.0f )) )
			    {
			    	_Key_Index -= 1;
			    	if ( _Key_Index < 0 )
			    		_Key_Index = r._Key.Count - 1;
			    }
			    EditorGUILayout.LabelField(_Key_Index.ToString(), GUILayout.Width( 15.0f ));
			    if ( GUILayout.Button(">", GUILayout.Width( 18.0f )) )
			    {
			    	_Key_Index += 1;
			    	if ( _Key_Index >= r._Key.Count )
		        		_Key_Index = 0;
				}
				EditorGUILayout.TextField( r._Key[_Key_Index] );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "_PuntosPulsado", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.FloatField( (float)r._PuntosPulsado );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "_PuntosSoltado", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.FloatField( (float)r._PuntosSoltado );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "_Color", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.ColorField( r._Color );
			}
			EditorGUILayout.EndHorizontal();

		}
	}
}
