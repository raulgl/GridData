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