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
	/// Representa cada agrupacion de datos para hacer los subtotales que se
	/// recoje del json.
	/// </summary>
	public class Groupby
	{
		protected string nombre;//nombre del group by en el json
    	protected int posicion;//posicion en que se escribe dentro de una linea en el html o en el csv
    	protected System.Collections.ArrayList sumatorios;//array de sumatorio para ese group by
    	protected string actual;//actual dato que tiene el campo del group by en la BD
    	protected string clas;//class html a la hora de printar el group by 
    	protected string total;//atributo del json para ver donde se imprime en el pdf
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="nom">campo de la BD del que hacemos el group by</param>
		/// <param name="pos">Posicion donde se empezarán a mostrarse los subtotal </param>
    	public Groupby(string nom,int pos){
    		nombre=nom.Trim();
        	posicion=pos;
        	sumatorios = new System.Collections.ArrayList();
        	clas = "";
        	total = "TOTAL";
    	}
		/// <summary>
		/// De momento no se utiliza
		/// </summary>
		/// <param name="nom"></param>
		/// <param name="pos"></param>
		/// <param name="clashtml"></param>
		/// <param name="totalpdf"></param>
    	public Groupby(string nom,int pos,string clashtml,string totalpdf){
        	nombre=nom.Trim();
        	posicion=pos;
        	sumatorios = new System.Collections.ArrayList();
        	clas = clashtml;
        	total = totalpdf;
    	}
		/// <summary>
		/// Añade un sumatorio a la array list de sumatorios
		/// </summary>
		/// <param name="nombre">campo de la BD del que se hace la suma</param>
		/// <param name="posicion">posicion donde se mostrara la suma</param>
		/// <param name="contador">nos dice si es la suma o solo se cuenta el numero de fila </param>
    	public void ad(string nombre,int posicion,bool contador){
        	Sumatorio suma = new Sumatorio(nombre,posicion,contador);
        	sumatorios.Add(suma);
    	}
		/// <summary>
		/// ponemos a 0 todos los sumatorios de group by
		/// </summary>
    	public void reset(){
    		foreach (Sumatorio suma in sumatorios){
            	suma.reset();
        	}
    	}
		/// <summary>
		/// ponemos como valor actual del campo del group by act
		/// </summary>
		/// <param name="act"></param>
    	public void set_actual(string act){
        	actual = act;
    	}
		/// <summary>
		/// nos dice si nom es el campo de la bd del group by 
		/// </summary>
		/// <param name="nom"></param>
		/// <returns></returns>
    	public bool mismo(string nom){
        	return nombre.CompareTo(nom)==0;
    	}
		/// <summary>
		/// nos dice si el valor del campo del group by ha cambiado
		/// </summary>
		/// <param name="valor"></param>
		/// <returns></returns>
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
		/// <summary>
		/// Suma en cada sumatorio el campo del row que corresponde
		/// </summary>
		/// <param name="row"></param>
    	public void sum(DataRow row){
        	foreach (Sumatorio sum in sumatorios){
            	sum.same(row);          
        	}       
    	}
		/// <summary>
		/// printa en el DataTable un datarow con los subtotales de este groupby
		/// </summary>
		/// <param name="dt"></param>
    	public void print_dt(DataTable dt){
    		DataRow dr = dt.NewRow();
    		dr[posicion]=total;
    		foreach (Sumatorio sum in sumatorios){
    			sum.get_Sumatorio(dr);
        	} 
    		dt.Rows.Add(dr);
    	}
    	
    	
	}
}
