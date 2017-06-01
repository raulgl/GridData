/*
 * Creado por SharpDevelop.
 * Usuario: ics_raul
 * Fecha: 05/05/2017
 * Hora: 13:29
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;



namespace SQLFrame
{
	/// <summary>
	/// ESta clase es el Core de la libreria
	/// </summary>
	public class Core
	{
		protected ArrayList groups;//array donde estan todos los groupby
    	protected string tipo;//tipo de informe que se va a generar:"html","csv","pdf"
    	protected static int pos_file=0;//posicion actual dentro de la linea del informe
    	protected static Dictionary<string, string> json;//objeto json parseado
    	protected DataTable result;
    	/// <summary>
    	/// Funcion que devuelve el DataTable resultado del proceso
    	/// </summary>
    	public DataTable resultado
    	{
        	get
        	{
            	return result;
        	}
        
    	}
    	/// <summary>
    	/// Constructura que a partir del fichero json pasado por parametro 
    	/// construye la infraestructura del ArrayList de groupby con todos los group
    	/// by que hay en el json y para cada uno se crea y añade un sumatorio para cada
    	/// sumatorio y contador del json
    	/// </summary>
    	/// <param name="formato">no utilizado de momento</param>
    	/// <param name="filepdf">no utilizado de momento</param>
    	/// <param name="fileconfig">json que se parsea</param>
		public Core(string formato,string filepdf,string fileconfig)
		{
			result = GridData.queryresult.Clone();
			tipo=formato;
			groups = new ArrayList();
			System.Web.Script.Serialization.JavaScriptSerializer serializer = new
            System.Web.Script.Serialization.JavaScriptSerializer();
            json = new Dictionary<string, string>();
            json = serializer.Deserialize<Dictionary<string, string>>(filepdf);
            Dictionary<string, Object> config = new Dictionary<string, Object>();
            StreamReader r = new StreamReader(fileconfig);
            string sjson = r.ReadToEnd();            	
            config = serializer.Deserialize<Dictionary<string, Object>>(sjson);
            ArrayList lgroup = (ArrayList)config["groupby"];
			ArrayList lsumatorios = (ArrayList)config["sumatorio"];
			ArrayList lcontadores = (ArrayList)config["contador"];
			
			foreach(Dictionary<string, Object> group in lgroup){
				Groupby g = new Groupby(group["nombre"].ToString(),Convert.ToInt32(group["posicion"]),group["class"].ToString(),group["total"].ToString());
				foreach(Dictionary<string, Object> sumador in lsumatorios){
					
					g.ad(sumador["nombre"].ToString(),Convert.ToInt32(sumador["posicion"].ToString()),false);
				}
				foreach(Dictionary<string, Object> contador in lcontadores){
					
					g.ad(contador["nombre"].ToString(),Convert.ToInt32(contador["posicion"].ToString()),true);
				}
				groups.Add(g);
			}
		}
		/// <summary>
		/// Anyade una row de un datarow al datatable de destino.Tambien mira si es diferente en algunos de los
		/// campos que estan en los group by.Esto quiere decir que antes poner los sumatorios de los group by 
		/// anteriores y ponerlos a 0.
		/// </summary>
		/// <param name="row"></param>
		public void ad(DataRow row){
			DataRow dr = result.NewRow();
			dr.ItemArray = row.ItemArray;
			int i=0;
        	bool reseteado=false;
        	ArrayList groupsprintar = new ArrayList();
        	/*Buscamos para cada group by corresponde con la columna de la row
        	 * cuando la encuentra mira si el contenido de la row es igual al campo actual 
        	 * o no.Si no lo es pone ese grupo a printar y actualiza el actual del grupo al
        	 * contenido de la columna.Si lo es suma los valores del row a los sumatorios del
        	 * group by*/
        	while(i<groups.Count && !reseteado){
            	int j=0;
            	bool encontrado=false;            
            	while(j<row.ItemArray.Length && !encontrado){
            		Groupby group = (Groupby)groups[i];
                	if(group.mismo(GridData.queryresult.Columns[j].ColumnName)){
                    	encontrado=true;                    
                    	if(group.is_reset(row[j].ToString())){
                        	reseteado=true;
                        	groupsprintar.Add(group);
                        	group.set_actual(row[j].ToString());
                    	}
                    	else{
                        	group.sum(row);
                    	}
                	}
                	j++;
            	}
            	i++;
        	}
        	/*Com los group by que estan mas abajo automaticamente los ponemos
        	 * en el grupo de printar y actualizamos el campo actualizar al de la row*/
        	Groupby groupnuevo = (Groupby)groups[0];
        	groupnuevo.sum(row);
        	while(i<groups.Count){
            	Groupby group = (Groupby)groups[i];
            	groupsprintar.Add(group);           
            	bool encontrado=false;
            	int j=0;
            	while(j<row.ItemArray.Length && !encontrado){
                	Groupby group2 = (Groupby)groups[i];
                	if(group2.mismo(GridData.queryresult.Columns[j].ColumnName)){
                    	encontrado=true;
                    	group2.set_actual(row[j].ToString());
                	}
                	j++;
            	}
            	i++;
        	}
        	printar(groupsprintar,0,row);
        	result.Rows.Add(dr);
		}
		/// <summary>
		/// printa los groupsby que estan en el groupsprintar a partir de la po
		/// sicion i de este
		/// </summary>
		/// <param name="groupsprintar"></param>
		/// <param name="i"></param>
		/// <param name="row"></param>
		public void printar(ArrayList groupsprintar, int i,DataRow row ){
			if(i<groupsprintar.Count){
            	i++;
            	printar(groupsprintar,i,row);
            	i--;
            	Groupby group = (Groupby)groupsprintar[i];
            	group.print_dt(result);
            	group.reset();
            	if(row!=null){
            		group.sum(row);
            	}
        	}
		}
		public void printar_todos(){
        	printar(groups,0,null);
    	}
		public void printar_a_partir(int i){
        	printar(groups,i,null);
    	}
		public void printar_solo(int i){
        	Groupby group = (Groupby)groups[i];        
        	group.print_dt(result);
        	group.reset();
    	}
	}
}
