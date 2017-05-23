/*
 * Creado por SharpDevelop.
 * Usuario: ics_raul
 * Fecha: 02/05/2017
 * Hora: 12:18
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Data;

namespace SQLFrame
{
	/// <summary>
	/// Description of groupby.
	/// </summary>
	public class Groupby
	{
		protected string nombre;//nombre del group by en el json
    	protected int posicion;//posicion en que se escribe dentro de una linea en el html o en el csv
    	protected System.Collections.ArrayList sumatorios;//array de sumatorio para ese group by
    	protected string actual;//actual dato que tiene el campo del group by en la BD
    	protected string clas;//class html a la hora de printar el group by 
    	protected string total;//atributo del json para ver donde se imprime en el pdf
		public Groupby(string nom,int pos){
    		nombre=nom.Trim();
        	posicion=pos;
        	sumatorios = new System.Collections.ArrayList();
        	clas = "";
        	total = "";
    	}
    	public Groupby(string nom,int pos,string clashtml,string totalpdf){
        	nombre=nom.Trim();
        	posicion=pos;
        	sumatorios = new System.Collections.ArrayList();
        	clas = clashtml;
        	total = totalpdf;
    	}
    	public void ad(string nombre,int posicion,bool contador){
        	Sumatorio suma = new Sumatorio(nombre,posicion,contador);
        	sumatorios.Add(suma);
    	}
    	public void reset(){
    		foreach (Sumatorio suma in sumatorios){
            	suma.reset();
        	}
    	}
    	public void set_actual(string act){
        	actual = act;
    	}
    	public bool mismo(string nom){
        	return nombre.CompareTo(nom)==0;
    	}
    	public bool is_reset(string valor){
        	if(actual==null){
            	actual = valor;
            	return false;
        	}
    		if(actual.CompareTo(valor)!=0){
            	return true;
        	}
        	else{
            	return false;
        	}
    	}
    	public void sum(DataRow row){
        	foreach (Sumatorio sum in sumatorios){
            	sum.same(row);          
        	}       
    	}
    	public void print_dt(DataTable dt){
    		DataRow dr = dt.NewRow();
    		dr[posicion]="TOTAL";
    		foreach (Sumatorio sum in sumatorios){
    			sum.get_Sumatorio(dr);
        	} 
    		dt.Rows.Add(dr);
    	}
    	
    	
	}
}
