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
	/// Description of Sumatorio.
	/// </summary>
	public class Sumatorio
	{
		string nombre; //nombre del campo de la BD y del campo del json
    	int posicion;//en que posicion se printara en el csv o en el html
    	bool escontador;// es contador o sumador
    	double total;//total de este sumatorio
		public Sumatorio( string nom, int pos, bool escont)
		{
			nombre=nom;
			posicion=pos;
			escontador=escont;
			total=0;
			
		}
		public void add(string num){
        
        	if(escontador){
            	total++;
        	}
        	else{
				total+=Convert.ToDouble(num);
        	}
    	}
		public void reset(){
        	total=0;
    	}
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
		public void get_Sumatorio(DataRow dr){
			dr[posicion]=total;
		}
	}
}
