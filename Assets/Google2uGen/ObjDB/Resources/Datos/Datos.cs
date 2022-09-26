//----------------------------------------------
//    Google2u: Google Doc Unity integration
//         Copyright © 2015 Litteratus
//
//        This file has been auto-generated
//              Do not manually edit
//----------------------------------------------

using UnityEngine;
using System.Globalization;

namespace Google2u
{
	[System.Serializable]
	public class DatosRow : IGoogle2uRow
	{
		public string _Textos;
		public System.Collections.Generic.List<string> _Key = new System.Collections.Generic.List<string>();
		public float _PuntosPulsado;
		public float _PuntosSoltado;
		public Color32 _Color;
		public DatosRow(string __ID, string __Textos, string __Key, string __PuntosPulsado, string __PuntosSoltado, string __Color) 
		{
			_Textos = __Textos.Trim();
			{
				string []result = __Key.Split("|".ToCharArray(),System.StringSplitOptions.RemoveEmptyEntries);
				for(int i = 0; i < result.Length; i++)
				{
					_Key.Add( result[i].Trim() );
				}
			}
			{
			float res;
				if(float.TryParse(__PuntosPulsado, NumberStyles.Any, CultureInfo.InvariantCulture, out res))
					_PuntosPulsado = res;
				else
					Debug.LogError("Failed To Convert _PuntosPulsado string: "+ __PuntosPulsado +" to float");
			}
			{
			float res;
				if(float.TryParse(__PuntosSoltado, NumberStyles.Any, CultureInfo.InvariantCulture, out res))
					_PuntosSoltado = res;
				else
					Debug.LogError("Failed To Convert _PuntosSoltado string: "+ __PuntosSoltado +" to float");
			}
			{
				string [] splitpath = __Color.Split(",".ToCharArray(),System.StringSplitOptions.RemoveEmptyEntries);
				if(splitpath.Length != 3 && splitpath.Length != 4)
					Debug.LogError("Incorrect number of parameters for Color32 in " + __Color );
				byte []results = new byte[splitpath.Length];
				for(int i = 0; i < splitpath.Length; i++)
				{
					byte res;
					if(byte.TryParse(splitpath[i], NumberStyles.Any, CultureInfo.InvariantCulture, out res))
					{
						results[i] = res;
					}
					else 
					{
						Debug.LogError("Error parsing " + __Color + " Component: " + splitpath[i] + " parameter " + i + " of variable _Color");
					}
				}
				_Color.r = results[0];
				_Color.g = results[1];
				_Color.b = results[2];
				if(splitpath.Length == 4)
					_Color.a = results[3];
			}
		}

		public int Length { get { return 5; } }

		public string this[int i]
		{
		    get
		    {
		        return GetStringDataByIndex(i);
		    }
		}

		public string GetStringDataByIndex( int index )
		{
			string ret = System.String.Empty;
			switch( index )
			{
				case 0:
					ret = _Textos.ToString();
					break;
				case 1:
					ret = _Key.ToString();
					break;
				case 2:
					ret = _PuntosPulsado.ToString();
					break;
				case 3:
					ret = _PuntosSoltado.ToString();
					break;
				case 4:
					ret = _Color.ToString();
					break;
			}

			return ret;
		}

		public string GetStringData( string colID )
		{
			var ret = System.String.Empty;
			switch( colID )
			{
				case "_Textos":
					ret = _Textos.ToString();
					break;
				case "_Key":
					ret = _Key.ToString();
					break;
				case "_PuntosPulsado":
					ret = _PuntosPulsado.ToString();
					break;
				case "_PuntosSoltado":
					ret = _PuntosSoltado.ToString();
					break;
				case "_Color":
					ret = _Color.ToString();
					break;
			}

			return ret;
		}
		public override string ToString()
		{
			string ret = System.String.Empty;
			ret += "{" + "_Textos" + " : " + _Textos.ToString() + "} ";
			ret += "{" + "_Key" + " : " + _Key.ToString() + "} ";
			ret += "{" + "_PuntosPulsado" + " : " + _PuntosPulsado.ToString() + "} ";
			ret += "{" + "_PuntosSoltado" + " : " + _PuntosSoltado.ToString() + "} ";
			ret += "{" + "_Color" + " : " + _Color.ToString() + "} ";
			return ret;
		}
	}
	public class Datos :  Google2uComponentBase, IGoogle2uDB
	{
		public enum rowIds {
			A_1, A_2, A_3, B_1, B_2, B_3, C_1, C_2, C_3, D_1, D_2, D_3, E_1, E_2, E_3, F_1, F_2, F_3
			
		};
		public string [] rowNames = {
			"A_1", "A_2", "A_3", "B_1", "B_2", "B_3", "C_1", "C_2", "C_3", "D_1", "D_2", "D_3", "E_1", "E_2", "E_3", "F_1", "F_2", "F_3"
			
		};
		public System.Collections.Generic.List<DatosRow> Rows = new System.Collections.Generic.List<DatosRow>();
		public override void AddRowGeneric (System.Collections.Generic.List<string> input)
		{
			Rows.Add(new DatosRow(input[0],input[1],input[2],input[3],input[4],input[5]));
		}
		public override void Clear ()
		{
			Rows.Clear();
		}
		public IGoogle2uRow GetGenRow(string in_RowString)
		{
			IGoogle2uRow ret = null;
			try
			{
				ret = Rows[(int)System.Enum.Parse(typeof(rowIds), in_RowString)];
			}
			catch(System.ArgumentException) {
				Debug.LogError( in_RowString + " is not a member of the rowIds enumeration.");
			}
			return ret;
		}
		public IGoogle2uRow GetGenRow(rowIds in_RowID)
		{
			IGoogle2uRow ret = null;
			try
			{
				ret = Rows[(int)in_RowID];
			}
			catch( System.Collections.Generic.KeyNotFoundException ex )
			{
				Debug.LogError( in_RowID + " not found: " + ex.Message );
			}
			return ret;
		}
		public DatosRow GetRow(rowIds in_RowID)
		{
			DatosRow ret = null;
			try
			{
				ret = Rows[(int)in_RowID];
			}
			catch( System.Collections.Generic.KeyNotFoundException ex )
			{
				Debug.LogError( in_RowID + " not found: " + ex.Message );
			}
			return ret;
		}
		public DatosRow GetRow(string in_RowString)
		{
			DatosRow ret = null;
			try
			{
				ret = Rows[(int)System.Enum.Parse(typeof(rowIds), in_RowString)];
			}
			catch(System.ArgumentException) {
				Debug.LogError( in_RowString + " is not a member of the rowIds enumeration.");
			}
			return ret;
		}

	}

}
