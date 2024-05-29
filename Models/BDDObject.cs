using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;

namespace Docteur.Models
{
    public class BDDObject
    {
        public virtual string tableName()
        {
            return "";
        }

        public virtual List<string> GetTableColumns(DbConnection connection)
        {
            List<string> columnNames = new List<string>();

            string query = $"SELECT * FROM {tableName()}";
            //Console.WriteLine(query);
            using (var command = connection.CreateCommand())//new NpgsqlCommand(query, connection))
            {
                command.CommandText = query;
                command.Connection = connection;
                using (var reader = command.ExecuteReader())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string columnName = reader.GetName(i);
                        columnNames.Add(columnName);
                    }
                }
            }

            return columnNames;
        }

        public PropertyInfo GetProperty(String argName)
        {
            Type type = this.GetType();
            //Console.WriteLine(argName);
            PropertyInfo propriete = type.GetProperty(Util.upperedName(argName), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            return propriete;
        }

        public String ToValueForInsert(Object arg)
        {
            if (arg is null)
            {
                return "NULL";
            }
            String ans = "'" + arg.ToString() + "'";

            if (arg is int || arg is double)
            {
                ans = arg.ToString().Replace(",", ".");
            }

            return ans;
        }

        public String ScriptInsertValues(DbConnection connection)
        {
            List<String> columnNames = this.GetTableColumns(connection);
            String ans = "";

            for (int i = 0; i < columnNames.Count; i++)
            {
                //Console.WriteLine(Util.upperedName(columnNames[i]));
                PropertyInfo prop = GetProperty(Util.upperedName(columnNames[i]));
                if (Attribute.GetCustomAttribute(prop, typeof(Identity)) == null)
                {
                    if (Attribute.GetCustomAttribute(prop, typeof(Default)) != null)
                    {
                        ans += "default";
                    }
                    else
                    {
                        if (prop.PropertyType == typeof(DateTime))
                        {
                            Console.WriteLine(((DateTime)prop.GetGetMethod().Invoke(this, null)).ToString("yyyy-MM-dd hh:mm:ss"));
                            ans += ToValueForInsert(((DateTime)prop.GetGetMethod().Invoke(this, null)).ToString("yyyy-MM-dd hh:mm:ss"));
                        }
                        else
                        {
                            ans += ToValueForInsert(prop.GetGetMethod().Invoke(this, null));
                        }
                    }
                    if (i != columnNames.Count - 1)
                    {
                        ans += ",";
                    }
                }
            }



            return ans;
        }

        public void Find(DbConnection con)
        {
            bool isNewConnexion = false;

            // Create a new connection if the provided connection is null
            if (con == null)
            {
                con = new Connect().getConnectPostgres();
                isNewConnexion = true;
            }
            try
            {
                List<String> attributsName = GetTableColumns(con);
                string query = "SELECT * FROM " + tableName() + " WHERE id = " + this.GetProperty("Id").GetGetMethod().Invoke(this, null);
                using (var command = con.CreateCommand())//new NpgsqlCommand(query, con))
                {
                    command.CommandText = query;
                    command.Connection = con;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PropertyInfo prop = null;
                            foreach (String attributName in attributsName)
                            {
                                prop = GetProperty(Util.upperedName(attributName));
                                if (reader.IsDBNull(reader.GetOrdinal(attributName)) == false)
                                {
                                    prop.GetSetMethod().Invoke(this, new object[] { reader.GetValue(reader.GetOrdinal(attributName)) });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (isNewConnexion == true)
                    con.Close();
            }
        }

        public double Sum(DbConnection connection, string attribut, string filtre)
        {
            bool isNewConnexion = false;

            // Create a new connection if the provided connection is null
            if (connection == null)
            {
                connection = new Connect().getConnectPostgres();
                isNewConnexion = true;
            }
            if (filtre == null)
            {
                filtre = "";
            }

            string query = "SELECT SUM(" + attribut + ") FROM " + tableName() + " " + filtre;
            using (var command = connection.CreateCommand())
            {
                command.CommandText = query;
                command.Connection = connection;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return reader.IsDBNull(0) ? 0 : reader.GetDouble(0);
                    }
                }
            }

            // Close the connection if it was created within this method
            if (isNewConnexion == true)
            {
                connection.Close();
            }

            return 0;
        }

        public double Avg(DbConnection connection, string attribut, string filtre)
        {
            bool isNewConnexion = false;

            // Create a new connection if the provided connection is null
            if (connection == null)
            {
                connection = new Connect().getConnectPostgres();
                isNewConnexion = true;
            }
            if (filtre == null)
            {
                filtre = "";
            }

            string query = "SELECT AVG(" + attribut + ") FROM " + tableName() + " " + filtre;
            using (var command = connection.CreateCommand())
            {
                command.CommandText = query;
                command.Connection = connection;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return reader.IsDBNull(0) ? 0 : reader.GetDouble(0);
                    }
                }
            }

            // Close the connection if it was created within this method
            if (isNewConnexion == true)
            {
                connection.Close();
            }

            return 0;
        }


        public double Max(DbConnection connection, string attribut, string filtre)
        {
            bool isNewConnexion = false;

            // Create a new connection if the provided connection is null
            if (connection == null)
            {
                connection = new Connect().getConnectPostgres();
                isNewConnexion = true;
            }
            if (filtre == null)
            {
                filtre = "";
            }

            string query = "SELECT MAX(" + attribut + ") FROM " + tableName() + " " + filtre;
            using (var command = connection.CreateCommand())
            {
                command.CommandText = query;
                command.Connection = connection;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return reader.IsDBNull(0) ? 0 : reader.GetDouble(0);
                    }
                }
            }

            // Close the connection if it was created within this method
            if (isNewConnexion == true)
            {
                connection.Close();
            }

            return 0;
        }

        public List<Object> Find(DbConnection connection, string filtre)
        {
            bool isNewConnexion = false;

            // Create a new connection if the provided connection is null
            if (connection == null)
            {
                connection = new Connect().getConnectPostgres();
                isNewConnexion = true;
            }
            if (filtre == null)
            {
                filtre = "";
            }

            List<Object> ans = new List<Object>();
            List<String> attributsName = GetTableColumns(connection);

            string query = "SELECT * FROM " + tableName() + " " + filtre;
            //Console.WriteLine(query);
            using (var command = connection.CreateCommand())
            {
                command.CommandText = query;
                command.Connection = connection;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Object objectInstance = Activator.CreateInstance(this.GetType());
                        PropertyInfo prop = null;
                        foreach (String attributName in attributsName)
                        {
                            prop = GetProperty(Util.upperedName(attributName));
                            if (reader.IsDBNull(reader.GetOrdinal(attributName)) == false)
                            {
                                prop.GetSetMethod().Invoke(objectInstance, new object[] { reader.GetValue(reader.GetOrdinal(attributName)) });
                            }
                        }
                        ans.Add(objectInstance);
                    }
                }
            }

            // Close the connection if it was created within this method
            if (isNewConnexion == true)
            {
                connection.Close();
            }

            return ans;
        }

        public void Update(DbConnection connection, DbTransaction transaction, string[] properties)
        {
            bool isNewConnexion = false;

            // Create a new connection if the provided connection is null
            if (connection == null)
            {
                connection = new Connect().getConnectPostgres();
                isNewConnexion = true;
            }

            List<String> columnNames = GetTableColumns(connection);
            String key = columnNames[0];

            using (DbCommand command = connection.CreateCommand())
            {
                command.Connection = connection;
                if (transaction != null)
                {
                    command.Transaction = transaction;
                }
                int rowsAffected = 0;

                foreach (string col in properties)
                {
                    // Invoke the method and return the result
                    PropertyInfo prop = GetProperty(col);
                    if (prop != null)
                    {
                        string updateQuery = $"UPDATE {tableName()} SET {col} = '{prop.GetGetMethod().Invoke(this, null).ToString()}' WHERE {key} = {GetProperty(Util.upperedName(key)).GetGetMethod().Invoke(this, null)}";

                        command.CommandText = updateQuery;

                        //Console.WriteLine(updateQuery);

                        rowsAffected += command.ExecuteNonQuery();
                    }
                }
            }

            // Close the connection if it was created within this method
            if (isNewConnexion == true)
            {
                connection.Close();
            }
        }

        public PropertyInfo GetIdentityProperty()
        {
            Type type = this.GetType();
            PropertyInfo[] proprietes = type.GetProperties();
            foreach (var item in proprietes)
            {
                if (Attribute.GetCustomAttribute(item, typeof(Identity)) != null)
                {
                    return item;
                }
            }
            return null;
        }

        public void Insert(DbConnection connection, DbTransaction transaction)
        {
            bool isNewConnexion = false;
            if (connection == null)
            {
                connection = new Connect().getConnectPostgres();
                isNewConnexion = true;
            }

            String key = GetTableColumns(connection)[0];
            try
            {
                using (DbCommand command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    if (transaction != null)
                    {
                        command.Transaction = transaction;
                    }
                    int rowsAffected = 0;

                    string identity = null;
                    PropertyInfo prop = this.GetIdentityProperty();
                    if (prop != null)
                    {
                        identity = prop.Name;
                    }
                    // Invoke the method and return the result
                    String columns = Util.scriptInsertColumns(GetTableColumns(connection), identity);
                    String values = ScriptInsertValues(connection);

                    string query = $"insert into {tableName()} ({columns}) values ({values})";
                    Console.WriteLine(query);
                    System.Diagnostics.Debug.WriteLine("***" + query);

                    command.CommandText = query;

                    rowsAffected += command.ExecuteNonQuery();
                }
            }
            finally
            {
                if (isNewConnexion == true)
                {
                    connection.Close();
                }
            }
        }
    }
}
