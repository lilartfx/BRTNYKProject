using System.DirectoryServices;
using LdapForNet.Native;

namespace BRTNYKBNCProject;

using System;
using LdapForNet;

public abstract class LdapAuthentication
{
    public static DirectoryEntry? Authenticate(
        string username, string password, string ldapServer, string baseDn,
        string authAttribute, string allowedGroup,
        string readPrivilegeUser, string readPrivilegePass
    )
    {
        using var connection = new LdapConnection();

        //uid=einstein,dc=example,dc=com

        var formattedUserName = authAttribute + "=" + username + "," + baseDn;

        try
        {
            connection.Connect(ldapServer);
            connection.Bind(Native.LdapAuthMechanism.SIMPLE, readPrivilegeUser, readPrivilegePass);
            connection.Bind(Native.LdapAuthMechanism.SIMPLE, formattedUserName, password);
            {
                var searchRequest = new SearchRequest(
                    baseDn, // Base DN of the LDAP directory
                    authAttribute + "=" + username, // Filter for the object to find
                    Native.LdapSearchScope.LDAP_SCOPE_SUB); // Search for the logged-in user by username

                // Perform the LDAP search
                if (connection.SendRequest(searchRequest) is SearchResponse searchResults &&
                    searchResults.Entries.Count > 0)
                {
                    return searchResults.Entries[0];
                }

                return new DirectoryEntry();
            }
        }
        catch (LdapException ex)
        {
            // Authentication failed
            throw new Exception(ex.Message);
        }
    }
}