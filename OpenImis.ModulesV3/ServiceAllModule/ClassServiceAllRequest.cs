using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenImis.DB.SqlServer;
using System.Data.Common;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Xml;
using System.Diagnostics;

namespace OpenImis.ModulesV3.ServiceAllModule
{
   public class ClassServiceAllRequest
    {

        public ClassServiceAllRequest()
        {
        }

        public IEnumerable<Dictionary<string, object>> SerializeDr()
        {

            var results = new List<Dictionary<string, object>>();
            var results_list_all = new List<Dictionary<string,  object>>();
            
            try
            {

                using (var imisContext = new ImisDB())
                {
                    var sql = "SELECT * From tblServices ORDER BY ServiceID";
                    DbConnection connection = imisContext.Database.GetDbConnection();
                    using (DbCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        if (connection.State.Equals(ConnectionState.Closed)) connection.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            var cols = new List<string>(); // create a list of string
                            for (var i = 0; i < reader.FieldCount; i++)
                                cols.Add(reader.GetName(i));
                            cols.Add("SubService");
                            cols.Add("SubItems");

                            while (reader.Read())
                            {
                                var getId = Convert.ToString(reader["ServiceID"]); //convert the ID got into string
                                results_list_all.Add(SerializeRow_SubRow_Dr(cols, reader, getId)); // add in the list, the results of serialized data from executed request

                            }
                            reader.Close();
                        }
                    }
                }
            }
                catch (Exception ex) 
            {
                Console.Write(ex.Message);
            }
            return results_list_all;
        }


        private Dictionary<string,  object> SerializeRow_SubRow_Dr(List <string> cols,
                                                       DbDataReader dr, string id)
        {
            var results = new Dictionary<string, object>();
            var result_lis_sub_req_serv = new List<Dictionary<string, object>>();
            var result_lis_sub_req_item = new List<Dictionary<string, object>>();
            var listcheck_id_services = new List<bool>();

            listcheck_id_services = CheckServiceId(id);

            foreach (var col in cols)
             {
                if (col == "SubService" && listcheck_id_services[1])
                {
                    try
                    {
                        using (var imisContext = new ImisDB())
                        {
                            var sql = "SELECT * From tblServiceContainedPackage where tblServiceContainedPackage.ServiceLinked =" + id;
                            DbConnection connection = imisContext.Database.GetDbConnection();
                            using (DbCommand cmd = connection.CreateCommand())
                            {
                                cmd.CommandText = sql;
                                if (connection.State.Equals(ConnectionState.Closed)) connection.Open();

                                using (var reader = cmd.ExecuteReader())
                                {
                                    var cols_sub = new List<string>();
                                    for (var i = 0; i < reader.FieldCount; i++)
                                        cols_sub.Add(reader.GetName(i)); // insert field column within the list of string
                                    while (reader.Read())
                                        result_lis_sub_req_serv.Add(SerializeRowDr(cols_sub, reader));

                                    results.Add(col, result_lis_sub_req_serv); // add within the Dictionary the serialize data where key is the list of field and the value is a dictionary.

                                    reader.Close();
                                }
                            }

                        }                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }

                }
                            
                else if(col == "SubItems" && listcheck_id_services[0]) // In the column is ItemID
                {
                    try
                    {
                        using (var imisContext = new ImisDB())
                        {
                            var sql = "SELECT * From tblProductContainedPackage where tblProductContainedPackage.ServiceId =" + id;
                            DbConnection connection = imisContext.Database.GetDbConnection();
                            using (DbCommand cmd = connection.CreateCommand())
                            {
                                cmd.CommandText = sql;
                                if (connection.State.Equals(ConnectionState.Closed)) connection.Open();

                                using (var reader = cmd.ExecuteReader())
                                {
                                    var cols_sub = new List<string>();
                                    for (var i = 0; i < reader.FieldCount; i++)
                                        cols_sub.Add(reader.GetName(i)); // insert field column within the list of string
                                    while (reader.Read())
                                        result_lis_sub_req_item.Add(SerializeRowDr(cols_sub, reader));
                                    results.Add(col, result_lis_sub_req_item); // add within the Dictionary the serialize data where key is the list of field and the value is a dictionary.

                                    reader.Close();

                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
 
                }
                       
                else
                    if (col!= "SubService" && col!= "SubItems")
                        results.Add(col, dr[col]);      
             }
            
            return results;
        }

        private Dictionary<string, object> SerializeRowDr(IEnumerable<string> cols,
                                                        DbDataReader dr) // function to serialize sub data requested
        {
            var result = new Dictionary<string, object>();
            foreach (var col in cols)
                result.Add(col, dr[col]);
            return result;
        }

        private List<bool> CheckServiceId(string id)
        {
            var resultBool = new List<Boolean>();
            resultBool.Add(false); resultBool.Add(false);
            try
            {
                using (var imisContext = new ImisDB())
                {
                    var sql = "SELECT * From tblProductContainedPackage where tblProductContainedPackage.ServiceID =" + id;
                    DbConnection connection = imisContext.Database.GetDbConnection();
                    using (DbCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        if (connection.State.Equals(ConnectionState.Closed)) connection.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read() && Convert.ToString(reader["ServiceID"]) == id)
                                resultBool[0] = true;

                            reader.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            try
            {
                using (var imisContext = new ImisDB())
                {
                    var sql = "SELECT * From tblServiceContainedPackage where tblServiceContainedPackage.ServiceLinked =" + id;
                    DbConnection connection = imisContext.Database.GetDbConnection();
                    using (DbCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        if (connection.State.Equals(ConnectionState.Closed)) connection.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read() && Convert.ToString(reader["ServiceLinked"]) == id)
                                resultBool[1] = true;

                            reader.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return resultBool;
        }

    }
}
