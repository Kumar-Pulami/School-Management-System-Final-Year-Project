namespace _19033684_Kumar_Pulami.Services
{
    public class DatabaseAccess
    {
        private static IConfiguration configuration;
        public static String GetConnection()
        {
            var buidler = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            configuration = buidler.Build();
            return configuration.GetConnectionString("myConnection");
        }
    }
}
