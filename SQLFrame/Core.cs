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
	/// Description of Core.
	/// </summary>
	public class Core
	{
		protected ArrayList groups;//array donde estan todos los groupby
    	protected string tipo;//tipo de informe que se va a generar:"html","csv","pdf"
    	protected static int pos_file=0;//posicion actual dentro de la linea del informe
    	protected static Dictionary<string, string> json;//objeto json parseado
    	protected DataTable result;
    	public DataTable resultado
    	{
        	get
        	{
            	return result;
        	}
        
    	}
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
		
		public void ad(DataRow row){
			DataRow dr = result.NewRow();
			dr.ItemArray = row.ItemArray;
			int i=0;
        	bool reseteado=false;
        	ArrayList groupsprintar = new ArrayList();
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
