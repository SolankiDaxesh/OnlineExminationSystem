//using Microsoft.Data.SqlClient;
using OESRegistraionAPI.IRepository;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http.Headers;

namespace OESRegistraionAPI.Repository
{
    public class RgistrationRepository : IRgistrationRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private string _key;
        private string _host;
        private string _url;
        public RgistrationRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("ConnectionString").ToString();
            _key = _configuration.GetValue<string>("RapidApi:X-RapidAPI-Key").ToString();
            _host = _configuration.GetValue<string>("RapidApi:X-RapidAPI-Host").ToString();
            _url = _configuration.GetValue<string>("RapidApi:Url").ToString();

        }

        public DataTable VerifyUserLogin(string Enrollment, string password)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new(_connStr))
                {
                    using (SqlCommand cmd = new("[DevelopmentDB].[dbo].[USP_AccountManagment]", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Action", SqlDbType.Int).Value = 1;
                        cmd.Parameters.Add("@Enrollment", SqlDbType.VarChar).Value = Enrollment;
                        cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = password;
                        var messageParameter = new SqlParameter
                        {
                            ParameterName = "@message",
                            SqlDbType = SqlDbType.VarChar,
                            Size = 50,
                            Direction = ParameterDirection.Output
                        };

                        cmd.Parameters.Add(messageParameter);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(dt);
                        SendWhatsAppMessage(dt);

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public async Task<string> SendWhatsAppMessage(DataTable dt)
        {
            string str = string.Empty;
            string Password = string.Empty;
            var client = new HttpClient();

            for (int i = 0; i < dt.Rows.Count; i++) 
            {
                Password = dt.Rows[i]["COL1"].ToString();
            }
            try
            {
               
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(_url),
                    Headers =
    {
        { "X-RapidAPI-Key", _key },
        { "X-RapidAPI-Host",_host },
    },
                    Content = new StringContent("{\r\n    \"text\": \"Greetings from University! Your New Password for login into exam portal is: "+ Password + " .Kindly Do not share your password.\"\r\n}")
                    {
                        Headers =
        {
            ContentType = new MediaTypeHeaderValue("application/json")
        }
                    }
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(body);
                }
            }
            catch(Exception ex) 
            {
                throw ex;
            }
            return Password;
        }
    }
}
