using System.Data.SqlClient;
using System.Data;

namespace ContactsDataAccessLayer
{
    public static class ContactsDataAccess
    {
        public struct stContact
        {
            public int ID = -1;
            public string FirstName = "",
            LastName = "",
            Email = "",
            Phone = "",
            Address = "";
            public int CountryID = -1;

            public stContact() { }

            public stContact(int ID, string FirstName, string LastName, string Email, string Phone, string Address, int CountryID)
            {
                this.ID = ID;
                this.FirstName = FirstName;
                this.LastName = LastName;
                this.Email = Email;
                this.Phone = Phone;
                this.Address = Address;
                this.CountryID = CountryID;
            }
        }
        static string connectionString = "Server=localhost,1433;Database=ContactsDB;User Id=sa;Password=Sa123456;";

        static public bool Find(int ID, ref stContact search)
        {
            bool is_found = false;

            SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand command = new SqlCommand(@"SELECT * FROM Contacts WHERE ContactID = @ID"
                , connection);

            command.Parameters.AddWithValue("@ID", ID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    is_found = true;
                    search = new stContact((int)reader["ContactID"], (string)reader["FirstName"], (string)reader["LastName"], (string)reader["Email"], (string)reader["Phone"], (string)reader["Address"], (int)reader["CountryID"]);
                }
                else
                {
                    is_found = false;
                    search = new stContact();
                }

                reader.Close();
            }
            finally
            {
                connection.Close();
            }

            return is_found;
        }

        static public int Add(stContact contact)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand command = new SqlCommand(
                @"INSERT INTO Contacts (FirstName, LastName, Email, Phone, Address, CountryID)
              values (@FirstName, @LastName, @Email, @Phone, @Address, @CountryID);
              SELECT SCOPE_IDENTITY();", connection);

            command.Parameters.AddWithValue("@FirstName", contact.FirstName);
            command.Parameters.AddWithValue("@LastName", contact.LastName);
            command.Parameters.AddWithValue("@Email", contact.Email);
            command.Parameters.AddWithValue("@Phone", contact.Phone);
            command.Parameters.AddWithValue("@Address", contact.Address);
            command.Parameters.AddWithValue("@CountryID", contact.CountryID);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                int InsertedID;
                if (result != null && int.TryParse(result.ToString(), out InsertedID))
                    contact.ID = InsertedID;
            }
            finally
            {
                connection.Close();
            }

            return contact.ID;
        }

        static public bool Update(stContact updatedContact)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand command = new SqlCommand(
                @"UPDATE Contacts
                  SET FirstName = @FirstName,
                      LastName = @LastName,
                      Email = @Email,
                      Address = @Address,
                      Phone = @Phone,
                      CountryID = @CountryID
                  WHERE ContactID = @ID",
                connection);

            command.Parameters.AddWithValue("@FirstName", updatedContact.FirstName);
            command.Parameters.AddWithValue("@LastName", updatedContact.LastName);
            command.Parameters.AddWithValue("@Email", updatedContact.Email);
            command.Parameters.AddWithValue("@Address", updatedContact.Address);
            command.Parameters.AddWithValue("@Phone", updatedContact.Phone);
            command.Parameters.AddWithValue("@CountryID", updatedContact.CountryID);
            command.Parameters.AddWithValue("@ID", updatedContact.ID);

            int rowsAffected = 0;
            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }

        static public bool Delete(int ID)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand command = new SqlCommand("DELETE FROM Contacts WHERE ContactID = @ID", connection);

            command.Parameters.AddWithValue("@ID", ID);

            int rowsAffected = 0;

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                connection.Close();
            }

            return rowsAffected > 0;
        }

        static public DataTable GetAllContacts()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand command = new SqlCommand("SELECT * FROM Contacts", connection);

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

        static public bool isFound(int id)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("SELECT Found = 1 From Contacts", connection);
            bool is_found;
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                is_found = reader.HasRows;
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

            return is_found;
        }
    }
}
