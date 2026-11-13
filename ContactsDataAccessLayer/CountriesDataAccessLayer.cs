using System;
using System.Data;
using System.Data.SqlClient;

namespace CountriesDataAccessLayer
{
    static public class CountriesDataAccess
    {
        static string connectionString = "Server=localhost,1433;Database=ContactsDB;User Id=sa;Password=Sa123456;";

        public struct stCountry
        {
            public int ID;
            public string Name, Code, PhoneCode;

            public stCountry()
            {
            }

            public stCountry(int ID, string Name, string PhoneCode, string Code)
            {
                this.ID = ID;
                this.Name = Name;
                this.Code = Code;
                this.PhoneCode = PhoneCode;
            }
        }

        static public bool Find(string CountryName, ref stCountry c)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand command = new SqlCommand(@"SELECT CountryID, CountryName, Code, PhoneCode FROM Countries WHERE CountryName = @Name", connection);
            command.Parameters.AddWithValue("@Name", CountryName);

            bool is_found = false;
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    is_found = true;
                    c.ID = (int)reader["CountryID"];
                    c.Name = reader["CountryName"].ToString();
                    c.Code = reader["Code"].ToString();
                    c.PhoneCode = reader["PhoneCode"].ToString();
                }

                reader.Close();
            }
            finally
            {
                connection.Close();
            }

            return is_found;
        }

        static public bool Find(int CountryID, ref stCountry c)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand command = new SqlCommand(@"SELECT CountryID, CountryName, Code, PhoneCode FROM Countries WHERE CountryID= @ID", connection);
            command.Parameters.AddWithValue("@ID", CountryID);

            bool is_found = false;
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    is_found = true;
                    c.ID = (int)reader["CountryID"];
                    c.Name = (string)reader["CountryName"];
                    c.Code = (string)reader["Code"];
                    c.PhoneCode = (string)reader["PhoneCode"];
                }

                reader.Close();
            }
            finally
            {
                connection.Close();
            }

            return is_found;
        }

        static public int Add(stCountry c)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand command = new SqlCommand(
              @"INSERT INTO Countries(CountryName, Code, PhoneCode)
                  values (@Name, @Code, @PhoneCode);
                SELECT SCOPE_IDENTITY();",
              connection);

            command.Parameters.AddWithValue("@Name", c.Name);
            command.Parameters.AddWithValue("@Code", c.Code);
            command.Parameters.AddWithValue("@PhoneCode", c.PhoneCode);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                int insertedID;

                if (result != null && int.TryParse(result.ToString(), out insertedID))
                {
                    c.ID = insertedID;
                }
            }
            finally
            {
                connection.Close();
            }
            return c.ID;
        }

        static public bool Update(stCountry c)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(
                @"UPDATE Countries
                    SET CountryName = @Name, Code = @Code, CountryCode= @PhoneCode
                    WHERE CountryID = @ID"
                , connection);

            command.Parameters.AddWithValue("@ID", c.ID);
            command.Parameters.AddWithValue("@Name", c.Name);
            command.Parameters.AddWithValue("@Code", c.Code);
            command.Parameters.AddWithValue("@PhoneCode", c.PhoneCode);
            int affectedRows;
            try
            {
                connection.Open();
                affectedRows = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                connection.Close();
            }

            return (affectedRows > 0);
        }

        static public bool Delete(int id)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand command = new SqlCommand("DELETE Countries WHERE CountryID = @ID", connection);
            command.Parameters.AddWithValue("@ID", id);

            int affectedRows;
            try
            {
                connection.Open();
                affectedRows = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                connection.Close();
            }

            return (affectedRows > 0);
        }
        static public bool Delete(string Name)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand command = new SqlCommand("DELETE Countries WHERE CountryName= @Name", connection);
            command.Parameters.AddWithValue("@Name", Name);

            int affectedRows;
            try
            {
                connection.Open();
                affectedRows = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                connection.Close();
            }

            return (affectedRows > 0);
        }

        static public bool isFound(int id)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand command = new SqlCommand(@"SELECT Found = 1 FROM Countries WHERE CountryID = @ID", connection);
            command.Parameters.AddWithValue("@ID", id);
            bool found;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                found = reader.HasRows;
                reader.Close();
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                connection.Close();
            }

            return found;
        }

        static public bool isFound(string Name)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand command = new SqlCommand(@"SELECT Found = 1 FROM Countries WHERE CountryName = @Name", connection);
            command.Parameters.AddWithValue("@Name", Name);
            bool found;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                found = reader.HasRows;
                reader.Close();
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                connection.Close();
            }

            return found;
        }

        static public DataTable GetAllCountries()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("SELECT * FROM Countries", connection);


            DataTable dt = new DataTable();
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }

                reader.Close();
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }
    }
}
