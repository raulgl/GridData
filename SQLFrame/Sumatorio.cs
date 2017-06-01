/*
 * Creado por SharpDevelop.
 * Usuario: ics_raul
 * Fecha: 27/04/2017
 * Hora: 16:45
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Data;

namespace SQLFrame
{
	/// <summary>
	/// Representa para cada group by el sumatorio del campo nombre de la bd
	/// </summary>
	public class Sumatorio
	{
		string nombre; //nombre del campo de la BD y del campo del json
    	int posicion;//en que posicion se printara en el csv o en el html
    	bool escontador;// es contador o sumador
    	double total;//total de este sumatorio
    	/// <summary>
    	/// Constructor
    	/// </summary>
    	/// <param name="nom">campo de la BD que se va a sumar o contar</param>
    	/// <param name="pos">posicion donde se va a poner el sumatorio</param>
    	/// <param name="escont">es contador o sumador</param>
		public Sumatorio( string nom, int pos, bool escont)
		{
			nombre=nom;
			posicion=pos;
			escontador=escont;
			total=0;
			
		}
		/// <summary>
		/// suma num al sumatorio o añade uno al sumatorio dependiendo si el sumatorio
		/// es sumatorio o contador
		/// </summary>
		/// <param name="num"></param>
		public void add(string num){
        
        	if(escontador){
            	total++;
        	}
        	else{
				total+=Convert.ToDouble(num);
        	}
    	}
		/// <summary>
		/// pone el sumatorio a 0
		/// </summary>
		public void reset(){
        	total=0;
    	}
		/// <summary>
		/// Mira dentro del datarow dr que campo es igual al nomb del sumatorio
		/// y cuando lo encuentra hace un add de su valor
		/// </summary>
		/// <param name="dr"></param>
		/// <returns></returns>
		public bool same(System.Data.DataRow dr){
        	int i=0;
        	bool encontrado=false;
        
        	while(i<dr.ItemArray.Length && !encontrado){
        		if(dr.Table.Columns[i].ColumnName.CompareTo(nombre)==0){
        			encontrado=true;
        			add(dr[i].ToString());
        		
        		}
           		i++;
        	}
        	return encontrado;
        
    	}
		/// <summary>
		/// devuelve dr pero con el sumatorio en el campo posicion
		/// </summary>
		/// <param name="dr"></param>
		public void get_Sumatorio(DataRow dr){
			dr[posicion]=total;
		}
	}
}
