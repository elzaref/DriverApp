using DriverAPI.Models;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DriverAPI.Repository
{
    public class DriverRepository:IDriverRepository
    {
        private readonly IConfiguration _config;
        private readonly string _connectionstr;
        public DriverRepository(IConfiguration config)
        {
            _config= config;
            _connectionstr= _config.GetValue<string>("ConnectionStrings:Default");
        }
        public List<Driver> GetAllDrivers()
        {
            //string strConString = _config.GetValue<string>("ConnectionStrings:Default");
            List<Driver> list = new List<Driver>();

            using (SqliteConnection con = new SqliteConnection(_connectionstr))
            {
                con.Open();
                SqliteCommand cmd = new SqliteCommand("Select * from driver ORDER BY FirstName, LastName", con);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Driver()
                        {
                            Id = Convert.ToInt32( reader["Id"]),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Email = reader["Email"].ToString(),
                            PhoneNumber = reader["PhoneNumber"].ToString(),
                        });
                    }

                }
            }
            return list;
        }
        public int InsertDriver(Driver driver)
        {
            //string strConString = _config.GetValue<string>("ConnectionStrings:Default");

            using (SqliteConnection con = new SqliteConnection(_connectionstr))
            {
                con.Open();
                string query = "Insert into driver (FirstName, LastName,Email,PhoneNumber) values(@FirstName, @LastName , @Email,@PhoneNumber)";
                SqliteCommand cmd = new SqliteCommand(query, con);
                cmd.Parameters.AddWithValue("@FirstName", driver.FirstName);
                cmd.Parameters.AddWithValue("@LastName", driver.LastName);
                cmd.Parameters.AddWithValue("@Email", driver.Email);
                cmd.Parameters.AddWithValue("@PhoneNumber", driver.PhoneNumber);
                return cmd.ExecuteNonQuery();
            }
        }
        public Driver GetDriverByID(int Id)
        {
            using (SqliteConnection con = new SqliteConnection(_connectionstr))
            {
                con.Open();
                SqliteCommand cmd = new SqliteCommand("Select * from driver where Id=" + Id, con);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Driver()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Email = reader["Email"].ToString(),
                            PhoneNumber = reader["PhoneNumber"].ToString(),
                        };
                    }

                }
            }
            return null;
        }
        public int UpdateDriver(Driver driver)
        {
            using (SqliteConnection con = new SqliteConnection(_connectionstr))
            {
                con.Open();
                string query = "Update driver SET FirstName=@FirstName, LastName=@LastName , Email=@Email , PhoneNumber=@PhoneNumber where Id="+driver.Id;
                SqliteCommand cmd = new SqliteCommand(query, con);
                cmd.Parameters.AddWithValue("@FirstName", driver.FirstName);
                cmd.Parameters.AddWithValue("@LastName", driver.LastName);
                cmd.Parameters.AddWithValue("@Email", driver.Email);
                cmd.Parameters.AddWithValue("@PhoneNumber", driver.PhoneNumber);
                return cmd.ExecuteNonQuery();
            }
        }
        public int DeleteDriver(int Id)
        {
            using (SqliteConnection con = new SqliteConnection(_connectionstr))
            {
                con.Open();
                string query = "Delete from driver where Id=@Id";
                SqliteCommand cmd = new SqliteCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", Id);
                return cmd.ExecuteNonQuery();
            }
        }
        
    }
}
