﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Hospital_Management.Areas.User.Data;
using Hospital_Management.Areas.User.Models;
using Hospital_Management.Areas.Doctor.Data;
using System.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
namespace Hospital_Management.Controllers
{
    public class UserController : Controller
    {
        user_layer obj_user = new user_layer();
        string connection = "Data Source=_aimerstar_\\HARSHILMSSQL;Initial Catalog=HospitalDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";

        private IConfiguration configuration;
        public UserController(IConfiguration configuration)
        {
            this.configuration = configuration;            
        }
        
        public IActionResult Index()
        {
            List<usermodel> users = new List<usermodel>();
            users = obj_user.GetAllUser().ToList();
            return View(users);
        }
        public ActionResult AddUser()
        {

            return View();
        }
        [HttpPost]
        public IActionResult AddUser([Bind] usermodel user)
        {
            obj_user.AddUser(user, HttpContext);
            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult Login()
        {
            Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            Response.Headers.Add("Pragma", "no-cache");
            Response.Headers.Add("Expires", "0");
            return View();
        }
        public ActionResult Login(string email, string password)
        {
            using (var conn = new SqlConnection(connection))
            {
                conn.Open();
                using (var command = new SqlCommand("Sp_Login", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);

                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        var roleName = reader["Name"] as string;
                        var roleId = reader["Role_ID"];

                        if (!string.IsNullOrEmpty(roleName) && roleId != null)
                        {
                            reader.Close();
                            var claims = new[]
                            {
                        new Claim(ClaimTypes.Email, email),
                        new Claim(ClaimTypes.Role, roleName)
                    };

                            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
                            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                            var token = new JwtSecurityToken(
                                issuer: "hospital.example.com",
                                audience: "https://myhospitalapp.example.com",
                                claims: claims,
                                expires: DateTime.Now.AddHours(1),
                                signingCredentials: credentials
                            );

                            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                            HttpContext.Session.SetString("roleid", roleId.ToString());
                            HttpContext.Session.SetString("loginemail", email);
                            HttpContext.Session.SetString("loginpassword", password);


                            var com = new SqlCommand("sp_GetUserId", conn);
                            com.CommandType = CommandType.StoredProcedure;
                            com.Parameters.AddWithValue("@RoleID", roleId);
                            com.Parameters.AddWithValue("@Email", email);
                            var userId = com.ExecuteScalar();
                            var cam = new SqlCommand("sp_GetUserName", conn);
                            cam.CommandType = CommandType.StoredProcedure;
                            cam.Parameters.AddWithValue("@RoleID", roleId);
                            cam.Parameters.AddWithValue("@Email", email);
                            var name = cam.ExecuteScalar();
                            HttpContext.Session.SetString("userid", userId.ToString());
                            HttpContext.Session.SetString("name", name.ToString());

                            string dateString = DateTime.Now.ToString("yyyy-MM-dd");

                            if (roleName == "Doctor")
                            {
                                return RedirectToAction("DoctorList", "Doctor", new { area = "Doctor", email = email, Token = tokenString, roleId = roleId });
                            }
                            else if (roleName == "Patient")
                            {
                                return RedirectToAction("PatientDashboard", "Patient", new { area = "Patient", email = email, Token = tokenString, roleId = roleId });
                            }
                            else if (roleName == "Staff")
                            {
                                return RedirectToAction("StaffDashboard", "Staff", new { area = "Staff", Token = tokenString});
                            }
                            else if (roleName == "Admin")
                            {
                                return RedirectToAction("AdminDashboard", "Admin", new { area = "Admin", Token = tokenString });
                            }
                        }

                        return RedirectToAction("Login");

                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Invalid credentials";
                        return View("Login");
                    }
                }
            }
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("roleid");
            HttpContext.Session.Remove("loginemail");
            HttpContext.Session.Remove("loginpassword");
            HttpContext.Session.Remove("userid");
            HttpContext.Session.Remove("name");

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

             foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }

            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            return Content("<!DOCTYPE html><html><head><title>Logging out...</title><script>setTimeout(function() { window.location.reload(true); setTimeout(function() { window.location.href = '/Home/Index'; }, ); }, 2000);</script></head><body></body></html>", "text/html");
        }

        //public void UpdateUser(usermodel user)
        //{
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(connection))
        //        {
        //            using (SqlCommand cmd = new SqlCommand("UPDATE User_tbl SET Name = @Name, Email = @Email, Password = @Password, Role_ID = @Role_ID WHERE User_ID = @UserID", con))
        //            {
        //                cmd.Parameters.AddWithValue("@UserID", user.UserID);
        //                cmd.Parameters.AddWithValue("@Name", user.UserName);
        //                cmd.Parameters.AddWithValue("@Email", user.Email);
        //                cmd.Parameters.AddWithValue("@Password", user.Password);
        //                cmd.Parameters.AddWithValue("@Role_ID", user.RoleId);

        //                con.Open();
        //                cmd.ExecuteNonQuery();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle the exception here, you might want to log it or throw it further.
        //        Console.WriteLine("Error: " + ex.Message);
        //    }
        //}

        public void DeleteUser(int userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM User_tbl WHERE User_ID = @UserID", con))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here, you might want to log it or throw it further.
                Console.WriteLine("Error: " + ex.Message);
            }
        }


    }
}
