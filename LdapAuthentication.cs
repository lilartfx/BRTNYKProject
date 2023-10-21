using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using LdapForNet.Native;

namespace BRTNYKBNCProject;

using System;
using LdapForNet;

public abstract class LdapAuthentication
{
    public static DirectoryEntry? Authenticate(
        string username, string password, string ldapServer,
        string baseDn, string authAttribute, IEnumerable<string> allowedGroups,
        string readPrivilegeUser, string readPrivilegePass, SslConfig ssl
    )
    {
        using var connection = new LdapConnection();

        //uid=einstein,dc=example,dc=com

        var formattedUserName = authAttribute + "=" + username + "," + baseDn;
        
        Console.WriteLine(formattedUserName);

        if (ssl.Enabled)
        {
            try
            {
                connection.Connect(ldapServer, 636, Native.LdapSchema.LDAPS);
                connection.TrustAllCertificates();
                connection.SetClientCertificate(X509Certificate2.CreateFromPem(ssl.Cert));
            }
            catch (LdapException ex)
            {
                // Authentication failed
                throw new Exception(ex.Message);
            }
        }
        else
        {
            try
            {
                connection.Connect(ldapServer);
            }
            catch (LdapException ex)
            {
                // Authentication failed
                throw new Exception(ex.Message);
            }
        }


        try
        {
            connection.Bind(Native.LdapAuthMechanism.SIMPLE, formattedUserName, password);
        }
        catch
        {
            Console.WriteLine("Unauthorized Access!");
        }
        finally
        {
            connection.Bind(Native.LdapAuthMechanism.SIMPLE, readPrivilegeUser, readPrivilegePass);
        }

        foreach (var group in allowedGroups)
        {
            try
            {
                var searchRequest = new SearchRequest(
                    group, // Base DN of the LDAP directory
                    authAttribute + "=" + username, // Filter for the object to find
                    Native.LdapSearchScope.LDAP_SCOPE_SUB); // Search for the logged-in user by username

                // Perform the LDAP search
                if (connection.SendRequest(searchRequest) is SearchResponse searchResults &&
                    searchResults.Entries.Count > 0)
                {
                    return searchResults.Entries[0];
                }
            }
            catch
            {
                // ignored
            }
        }

        return new DirectoryEntry();
    }
}

