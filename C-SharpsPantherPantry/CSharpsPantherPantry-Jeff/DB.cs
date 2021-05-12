using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
/**
 * Author: Jeff Butler
 * Date Created:  20210419
 * Description:  This class connects to the databse cis234a_the_C-Sharps. the connections are internal to this class and
 *               is used by the methods that return results to the calling class.
 *TODO:  connect to the databse
 *TODO:  Any  sql statements or data validation methods
 *Modifications:  
 *  JB:  Added getSubscriberEmail method
 *  EL: Added methods: addNewUser, getUserByUsername, getAllUsernames 4/29/2021
 **/

namespace CSharpsPantherPantry_Jeff
{
    class DB
    {
        public DB()
        //constructor 
        {

        }

        //connection to the database
        protected SqlConnection SQLConnect = new SqlConnection
                (@"Data Source =cisdbss.pcc.edu;Initial Catalog=cis234a_the_C-Sharps;User ID=cis234a_the_C-Sharps;Password=Cis234A_Team_ the_C-Sharps_Spring_21_@(%");

        /**
         * method for testing connection string
         * <returns> List collection storing results from testdata table</returns>
         **/
        public DataTable getTestData()
        {
            // List<string> testData = new List<string>();
            DataTable testData = new DataTable();
            try
            {
                using (SqlDataAdapter sda = new SqlDataAdapter("SELECT FName, LName, email from TestData", SQLConnect))
                {
                    sda.Fill(testData);
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
            finally
            { SQLConnect.Close(); }

            return testData;
        }

        public List<string> getSubscriberEmailList()
        /**
         * 
         *<returns>returns a list of emails for all subscribers</returns> 
         **/
        {
            List<string> subscriberEmails = new List<string>();
            try
            {
                using (SqlCommand sqlcmd = new SqlCommand
                    ("select u.EmailAddress from Users u inner join Role r on u.RoleID = r.RoleID where r.Role = 'Subscriber';"))
                {
                    sqlcmd.CommandType = System.Data.CommandType.Text;
                    sqlcmd.Connection = SQLConnect;
                    SQLConnect.Open();
                    SqlDataReader rdr = null;
                    rdr = sqlcmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        subscriberEmails.Add(rdr["EmailAddress"].ToString());
                    }
                    return subscriberEmails;
                }
            }


            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);

            }
            finally
            {
                SQLConnect.Close();
            }
        }

        /**
         * writes into the notification that a message was sent.
         * <param>Subject, dateSent, countSentTo, Userid></param>
         * userid is the person id of the person that sent the email
         **/
        public void writeNotificationLog(string subject, DateTime dateSent, int countSentTo, int userid)
        {
            try
            {
                using (SqlCommand sqlcmd = new SqlCommand("INSERT into Notifications(Subject, Date, numberSent, userid)" +
                    "values(@subject, @date, @numberSent, @userid);"))
                {
                    sqlcmd.Connection = SQLConnect;
                    SQLConnect.Open();
                    sqlcmd.Parameters.AddWithValue("@subject", subject);
                    sqlcmd.Parameters.AddWithValue("@date", dateSent);
                    sqlcmd.Parameters.AddWithValue("@numberSent", countSentTo);
                    sqlcmd.Parameters.AddWithValue("@userid", userid);
                    sqlcmd.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
            finally
            {
                SQLConnect.Close();
            }
        }

        /**<summary> Collects a list of all the usernames on the database</summary>
         * <returns>A list of Usernames. All Should be unique.</returns>
         */
        public List<string> getAllUsernames()
        {
            List<string> usernames = new List<string>();
            try
            {
                using (SqlCommand sqlcmd = new SqlCommand
                    ("select UserName from Users;"))
                {
                    sqlcmd.CommandType = System.Data.CommandType.Text;
                    sqlcmd.Connection = SQLConnect;
                    SQLConnect.Open();
                    SqlDataReader rdr = null;
                    rdr = sqlcmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        usernames.Add(rdr["username"].ToString());
                    }
                    return usernames;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
            finally
            {
                SQLConnect.Close();
            }
        }

        /**<summary>gets a subscriber object by the corresponding username</summary>
         * <param name="username">the username of the subscriber</param>
         * <returns>the subscriber with the matching username</returns>
         */
        //public Subscriber getUserByUsername(string username)
        //{
        //    try
        //    {
        //        using (SqlCommand sqlCommand = new SqlCommand(
        //            "SELECT FirstName, LastName, EmailAddress, PhoneNumber, UserName, Salt, Password, RoleID, UserID FROM Users " +
        //            "WHERE UserName = @Uname"))
        //        {
        //            SqlParameter unameParam = new SqlParameter();
        //            unameParam.ParameterName = "@Uname";
        //            unameParam.Value = username;
        //            sqlCommand.Parameters.Add(unameParam);

        //            sqlCommand.CommandType = System.Data.CommandType.Text;
        //            sqlCommand.Connection = SQLConnect;
        //            SQLConnect.Open();

        //            SqlDataReader reader = sqlCommand.ExecuteReader();
        //            DataTable results = new DataTable();
        //            Subscriber subscriber = new Subscriber();
        //            while (reader.Read())
        //            {
        //                subscriber.setFirstName(reader["FirstName"].ToString());
        //                subscriber.setLastName(reader["LastName"].ToString());
        //                subscriber.setPhoneNumber(reader["PhoneNumber"].ToString());
        //                subscriber.setUsernameFromDB(reader["UserName"].ToString());
        //                subscriber.setSaltFromDB(reader["Salt"].ToString());
        //                subscriber.setPasswordFromDB(reader["Password"].ToString());
        //                subscriber.setEmail(reader["EmailAddress"].ToString());
        //                subscriber.setRoleID(int.Parse(reader["RoleID"].ToString()));
        //                subscriber.setUserIDfromDB(int.Parse(reader["UserID"].ToString()));
        //            }

        //            reader.Close();
        //            return subscriber;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ArgumentException("Error fetching data: " + ex.Message);
        //    }
        //    finally
        //    {
        //        SQLConnect.Close();
        //    }
        //}

        /**
         * <summary>Takes a subscriber with full informaiton and adds them to the database</summary>
         * <param name="subscriber">the subscriber to be registered</param>
         */
        //public void addNewUser(Subscriber subscriber)
        //{
        //    try
        //    {
        //        using (SqlCommand sqlCommand = new SqlCommand())
        //        {

        //            string nonQueryString = "INSERT INTO Users (LastName, FirstName, PhoneNumber, UserName, Salt, Password, RoleID, EmailAddress) " +
        //                    "Values(@Last, @First, @Phone, @Uname, @Salt, @Password, @Role, @Email)";
        //            sqlCommand.CommandText = nonQueryString;

        //            SqlParameter lastParam = new SqlParameter("@Last", SqlDbType.NVarChar, 50);
        //            SqlParameter firstParam = new SqlParameter("@First", SqlDbType.NVarChar, 50);
        //            SqlParameter phoneParam = new SqlParameter("@Phone", SqlDbType.NVarChar, 15);
        //            SqlParameter unameParam = new SqlParameter("@Uname", SqlDbType.NVarChar, 50);
        //            SqlParameter saltParam = new SqlParameter("@Salt", SqlDbType.NVarChar, 50);
        //            SqlParameter passParam = new SqlParameter("@Password", SqlDbType.NVarChar, 255);
        //            SqlParameter roleParam = new SqlParameter("@Role", SqlDbType.Int, 4);
        //            SqlParameter emailParam = new SqlParameter("@Email", SqlDbType.NVarChar, 255);

        //            lastParam.Value = subscriber.getLastName();
        //            firstParam.Value = subscriber.getFirstName();
        //            phoneParam.Value = subscriber.getPhoneNumber();
        //            unameParam.Value = subscriber.getUsername();
        //            saltParam.Value = subscriber.getSavedSalt();
        //            passParam.Value = subscriber.getHashedPassword();
        //            roleParam.Value = subscriber.getRoleID();
        //            emailParam.Value = subscriber.getEmail();

        //            sqlCommand.Parameters.Add(lastParam);
        //            sqlCommand.Parameters.Add(firstParam);
        //            sqlCommand.Parameters.Add(phoneParam);
        //            sqlCommand.Parameters.Add(unameParam);
        //            sqlCommand.Parameters.Add(saltParam);
        //            sqlCommand.Parameters.Add(passParam);
        //            sqlCommand.Parameters.Add(roleParam);
        //            sqlCommand.Parameters.Add(emailParam);


        //            sqlCommand.CommandType = System.Data.CommandType.Text;
        //            sqlCommand.Connection = SQLConnect;
        //            SQLConnect.Open();
        //            sqlCommand.Prepare();
        //            sqlCommand.ExecuteNonQuery();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        SQLConnect.Close();
        //    }
        //}

        public DataTable getNotficationsData()
        {
            // List<string> testData = new List<string>();
            DataTable notifcationsData = new DataTable();
            try
            {
                using (SqlDataAdapter sda = new SqlDataAdapter("SELECT Subject, CAST(Date AS CHAR(10))AS 'Date Sent', CAST(Users.FirstName + ' ' + Users.LastName AS CHAR(20)) AS 'Author', numberSent AS 'Notifications Sent' from Notifications inner join Users ON Users.UserID = Notifications.userid;", SQLConnect))
                {
                    sda.Fill(notifcationsData);
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
            finally
            { SQLConnect.Close(); }

            return notifcationsData;
        }




        public DataTable getDateFilterNotficationsData(String From, String To)
        {
            // List<string> testData = new List<string>();
            DataTable notifcationsDataFilter = new DataTable();
            try
            {
                using (SqlDataAdapter sda = new SqlDataAdapter("SELECT Subject, CAST(Date AS CHAR(10))AS 'Date Sent', CAST(Users.FirstName + ' ' + Users.LastName AS CHAR(20)) AS 'Author', numberSent AS 'Notifications Sent'" +
                    " from Notifications inner join Users ON Users.UserID = Notifications.userid WHERE Date BETWEEN @From AND @To;", SQLConnect))

                {
                    sda.SelectCommand.Parameters.AddWithValue("@From", From);
                    sda.SelectCommand.Parameters.AddWithValue("@To", To);
                    sda.Fill(notifcationsDataFilter);

                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
            finally
            { SQLConnect.Close(); }

            return notifcationsDataFilter;
        }
    }
}

