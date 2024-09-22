using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Hotel_Management__Debugging_
{
    public class DBAccess : DataBase
    {
        private static SqlConnection connection = new SqlConnection();
        private static SqlCommand command = new SqlCommand();
        private static SqlDataReader DbReader;
        private static SqlDataAdapter adapter = new SqlDataAdapter();
        public SqlTransaction DbTran;

        private static string strConnString = "Data Source=laptop-3mo76otm\\sqlexpress;Initial Catalog=Project;Integrated Security=True";



        //Class Methods:
        /*--------------------------------------------------------------------------------------------------------*/


        //Guest Related Methods Implementation

        public override void InsertGuestInfo(string firstname, string lastName, string phone_number, string id) // Inserting into Guest_info inside the database
        {

            SqlCommand insertCommand = new SqlCommand("insert into Guest_Info(FirstName,LastName,Phone_Number,Id) values(@FirstName, @LastName, @Phone_Number, @Id)");
            insertCommand.Parameters.Add(new SqlParameter("@Id", id));
            insertCommand.Parameters.Add(new SqlParameter("@Phone_Number", phone_number));
            insertCommand.Parameters.Add(new SqlParameter("@FirstName", firstname));
            insertCommand.Parameters.Add(new SqlParameter("@LastName", lastName));

            executeQuery(insertCommand);
        }

        public void UpdateGuestInfo(string id,string firstname, string lastName, string phone_number)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(strConnString))
                {
                    connection.Open();

                    // Update RoomNumber in Book_Reservation
                    SqlCommand command = new SqlCommand("UPDATE Guest_Info SET Phone_Number = @Phone_Number, FirstName = @FirstName, LastName = @LastName  WHERE Id = @Id", connection);
                    command.Parameters.Add(new SqlParameter("@Phone_Number", phone_number));
                    command.Parameters.Add(new SqlParameter("@FirstName", firstname));
                    command.Parameters.Add(new SqlParameter("@LastName", lastName));
                    command.Parameters.AddWithValue("@Id", id);

                    command.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public override void DeleteGuestInfo(string id)
        {
            try
            {
                SqlCommand deleteCommand = new SqlCommand("DELETE FROM Guest_Info WHERE Id = @Id");
                deleteCommand.Parameters.Add(new SqlParameter("@Id", id));

                executeQuery(deleteCommand);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the delete operation
                //throw ex;
                Console.WriteLine("Id not found!");
            }
        }

        public override void DeleteReservation(string id)
        {
            try
            {
                SqlCommand deleteCommand = new SqlCommand("DELETE FROM Book_Reservation WHERE Id = @Id");
                deleteCommand.Parameters.Add(new SqlParameter("@Id", id));

                executeQuery(deleteCommand);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the delete operation
                //throw ex;
                Console.WriteLine("Id not found!");
            }
        }


        /*--------------------------------------------------------------------------------------------------------*/


        //Reservation Methods Implementation
        public override void CheckInDate(DateOnly checkInDate, string Id)
        {
            // Convert DateOnly to DateTime using constructor
            DateTime date = new DateTime(checkInDate.Year, checkInDate.Month, checkInDate.Day);

            SqlCommand insertCommand1 = new SqlCommand("insert into Book_Reservation(Id, CheckIn) values(@Id, @CheckIn)");
            insertCommand1.Parameters.Add(new SqlParameter("@Id", Id));
            insertCommand1.Parameters.Add(new SqlParameter("@CheckIn", date)); // Use DateTime here

            executeQuery(insertCommand1);
        }

        public override void CheckOutDate(DateOnly DepartureDate, string id)
        {

            DateTime date = new DateTime(DepartureDate.Year, DepartureDate.Month, DepartureDate.Day);

            SqlCommand updateCommand = new SqlCommand("UPDATE Book_Reservation SET CheckOut = @CheckOut WHERE Id = @Id");
            updateCommand.Parameters.Add(new SqlParameter("@Id", id));
            updateCommand.Parameters.Add(new SqlParameter("@CheckOut", date)); // Use DateTime here

            executeQuery(updateCommand);
        }


        /*--------------------------------------------------------------------------------------------------------*/


        //Hotel Check In-Out Implementation
        public override void UpdateArrivalTime(TimeSpan arrivalTime, string id, string status) //Recording the time of arrival of the guest
        {

            SqlCommand updateCommand = new SqlCommand("UPDATE Book_Reservation SET Check_In_Time = @Check_In_Time, Status = @Status WHERE Id = @Id");
            updateCommand.Parameters.Add(new SqlParameter("@Id", id));
            updateCommand.Parameters.Add(new SqlParameter("@Check_In_Time", arrivalTime));
            updateCommand.Parameters.Add(new SqlParameter("@Status", status));

            executeQuery(updateCommand);

        }

        public override void UpdateDepartureTime(TimeSpan DepartureTime, string id)
        {
  
            SqlCommand updateCommand = new SqlCommand("UPDATE Book_Reservation SET Check_Out_Time = @Check_Out_Time WHERE Id = @Id");
            updateCommand.Parameters.Add(new SqlParameter("@Id", id));
            updateCommand.Parameters.Add(new SqlParameter("@Check_Out_Time", DepartureTime));

            executeQuery(updateCommand);

        }

        public override void UpdateArrivalStatus(string status, string id)
        {

            SqlCommand updateCommand = new SqlCommand("UPDATE Book_Reservation SET Status = @Status WHERE Id = @Id");
            updateCommand.Parameters.Add(new SqlParameter("@Id", id));
            updateCommand.Parameters.Add(new SqlParameter("@Status", status));

            executeQuery(updateCommand);
        }


        /*--------------------------------------------------------------------------------------------------------*/


        //Editing Reservation Implementation
        public override void UpdateCheckInDate(string id, DateOnly newCheckInDate)
        {

            DateTime date = new DateTime(newCheckInDate.Year, newCheckInDate.Month, newCheckInDate.Day);

            SqlCommand updateCommand = new SqlCommand("UPDATE Book_Reservation SET CheckIn = @CheckIn WHERE Id = @Id");
            updateCommand.Parameters.Add(new SqlParameter("@Id", id));
            updateCommand.Parameters.Add(new SqlParameter("@CheckIn", date));

            executeQuery(updateCommand);
        }

        public override void UpdateCheckOutDate(string id, DateOnly newCheckOutDate)
        {

            DateTime date = new DateTime(newCheckOutDate.Year, newCheckOutDate.Month, newCheckOutDate.Day);

            SqlCommand updateCommand = new SqlCommand("UPDATE Book_Reservation SET CheckOut = @CheckOut WHERE Id = @Id");
            updateCommand.Parameters.Add(new SqlParameter("@Id", id));
            updateCommand.Parameters.Add(new SqlParameter("@CheckOut", date));

            executeQuery(updateCommand);
        }


        /*--------------------------------------------------------------------------------------------------------*/


        //Occupying Rooms Implementation
        public override bool IsRoomOccupiedForDates(int roomNumber, DateTime checkInDateTime, DateTime checkOutDateTime)
        {
            //Given the Check In-Out dates, this Method Checks for the given Room number its Occupancy
            try
            {
                using (SqlConnection connection = new SqlConnection(strConnString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM Book_Reservation  WHERE RoomNumber = @RoomNumber " +
                                   "AND ((CheckIn <= @CheckOutDateTime AND CheckOut >= @CheckInDateTime) " +
                                   "OR (CheckIn >= @CheckInDateTime AND CheckIn <= @CheckOutDateTime))";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RoomNumber", roomNumber);
                        command.Parameters.AddWithValue("@CheckInDateTime", checkInDateTime);
                        command.Parameters.AddWithValue("@CheckOutDateTime", checkOutDateTime);

                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            { 
                throw ex;
            }
        }

        public override void UpdateRoomNumber(string id, int roomNumber, string roomtype)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(strConnString))
                {
                    connection.Open();

                    // Update RoomNumber in Book_Reservation
                    SqlCommand command = new SqlCommand("UPDATE Book_Reservation SET RoomNumber = @RoomNumber, RoomType = @RoomType WHERE Id = @Id", connection);
                    command.Parameters.AddWithValue("@RoomNumber", roomNumber);
                    command.Parameters.AddWithValue("@RoomType", roomtype);
                    command.Parameters.AddWithValue("@Id", id);

                    command.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void UpdateRoomNumber_Guestinfo(string id, int roomNumber)
        {
            // Same as the previous Method but for the Guest_info Table
            try
            {
                using (SqlConnection connection = new SqlConnection(strConnString))
                {
                    connection.Open();

                    // Update RoomNumber in Book_Reservation
                    SqlCommand command = new SqlCommand("UPDATE Guest_Info SET Room_Number = @Room_Number WHERE Id = @Id", connection);
                    command.Parameters.AddWithValue("@Room_Number", roomNumber);
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                // Handle exceptions here (log or throw, depending on your error handling strategy)
                throw ex;
            }
        }


        /*--------------------------------------------------------------------------------------------------------*/


        //Sever adn Database Connections classes
        public void createConn()
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.ConnectionString = strConnString;
                    connection.Open();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void closeConn()
        {
            connection.Close();
        }


        public int executeDataAdapter(DataTable UserTable, string strSelectSql)
        {
            try
            {
                if (connection.State == 0)
                {
                    createConn();
                }

                adapter.SelectCommand.CommandText = strSelectSql;
                adapter.SelectCommand.CommandType = CommandType.Text;
                SqlCommandBuilder DbCommandBuilder = new SqlCommandBuilder(adapter);


                string insert = DbCommandBuilder.GetInsertCommand().CommandText.ToString();
                string update = DbCommandBuilder.GetUpdateCommand().CommandText.ToString();
                string delete = DbCommandBuilder.GetDeleteCommand().CommandText.ToString();


                return adapter.Update(UserTable);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void readDatathroughAdapter(string query, DataTable UserTable)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    createConn();
                }

                command.Connection = connection;
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                adapter = new SqlDataAdapter(command);
                adapter.Fill(UserTable);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public SqlDataReader readDatathroughReader(string query)
        {
            //DataReader used to sequentially read data from a data source
            SqlDataReader reader;

            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    createConn();
                }

                command.Connection = connection;
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                reader = command.ExecuteReader();
                return reader;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int executeQuery(SqlCommand dbCommand)
        {
            try
            {
                if (connection.State == 0)
                {
                    createConn();
                }

                dbCommand.Connection = connection;
                dbCommand.CommandType = CommandType.Text;


                return dbCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }




        /*--------------------------------------------------------------------------------------------------------*/

        //Searching inside the client info table and returning variables

        public Reservation SearchCustomerById(string id)
        {
            try
            {
                // Create a new Reservation object to store the search result
                Reservation customer = new Reservation();

                // Open the database connection if it's not already open
                if (connection.State == ConnectionState.Closed)
                {
                    createConn();
                }

                // Define the SQL query for searching based on the Id (primary key)
                string query = "SELECT * FROM Guest_Info WHERE Id = @Id";

                // Create a SqlCommand object with the query and connection
                using (SqlCommand searchCommand = new SqlCommand(query, connection))
                {
                    // Add a parameter for the Id
                    searchCommand.Parameters.AddWithValue("@Id", id);

                    // Execute the command and read the data using SqlDataReader
                    using (SqlDataReader reader = searchCommand.ExecuteReader())
                    {
                        // Check if there is data to read
                        if (reader.Read())
                        {
                            // Fill the Reservation object with data from the database
                            customer.Id = reader["Id"].ToString();
                            customer.FirstName = reader["FirstName"].ToString();
                            customer.LastName = reader["SecondName"].ToString();
                            customer.PhoneNumber = reader["Phone_Number"].ToString();
                            customer.RoomNumber = Convert.ToInt32(reader["Room_Number"]);
                            // Other properties can be filled similarly
                        }
                    }
                }

                // Return the Reservation object containing the search result
                return customer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /*--------------------------------------------------------------------------------------------------------*/

    }

}