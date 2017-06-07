/*
 * Creado por SharpDevelop.
 * Usuario: ics_raul
 * Fecha: 27/04/2017
 * Hora: 16:43
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Collections.Generic;
using System.Data;

namespace SQLFrame
{
	/// <summary>
	/// ES la entrada principal de la libreria
	/// </summary>
	
	public class GridData
	{
		public static DataTable queryresult;
		/// <summary>
		/// Es la funcion principal
		/// </summary>
		/// <param name="input">DataTable original</param>
		/// <param name="formato">no utilizado de momento</param>
		/// <param name="filepdf">no utilizado de momento</param>
		/// <param name="fileconfig">json que se utiliza para ver de que va
		/// riables y para que variables se quieren los subtotales </param>
		/// <returns>Devuelve el Datatable con los subttotales ya metidos.
		/// </returns>
		public static DataTable printar(DataTable input,string formato,string filepdf,string fileconfig){
			queryresult=input;
			Core c = new Core(formato,filepdf,fileconfig);
			foreach(DataRow row in input.Rows){
				c.ad(row);
			}
			c.printar_todos();
			return c.resultado;
			
		}
		
	}
}