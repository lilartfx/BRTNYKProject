using System;
using System.IO;
using System.Windows;
using LdapForNet;
using YamlDotNet.Serialization;

namespace BRTNYKBNCProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static DirectoryEntry? TryLogin(string username, string password)
        {
            const string filePath = "./config.yaml";
            Config config;
            
            var deserializer = new DeserializerBuilder().Build();
            var yamlText = File.ReadAllText(filePath);

            try
            {
                config = deserializer.Deserialize<Config>(yamlText);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading YAML file: " + ex.Message);
                Console.WriteLine(ex.StackTrace); // Print the stack trace
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }
                return null;
            }

            
            try
            {
                return LdapAuthentication.Authenticate(username, password, config.Ldap.Connection.Host,
                    config.Ldap.Connection.BaseDn, config.Ldap.Auth.Attribute, config.Ldap.Groups.AllowedGroups, config.Ldap.Connection.User,
                    config.Ldap.Connection.Pass, config.Ldap.Connection.Ssl);
            }
            catch (Exception ex)
            {
                // Authentication failed
                throw new Exception(ex.Message);
            }
        }
    }
}