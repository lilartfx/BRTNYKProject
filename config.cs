using System;
using System.IO;
using YamlDotNet.Serialization;

namespace BRTNYKBNCProject
{
    public class Config
    {
        public LdapConfig Ldap { get; set; } = null!;
        public OpenIdConfig OpenId { get; set; } = null!;
        public SamlConfig Saml { get; set; } = null!;
    }

    public class LdapConfig
    {
        public bool Enabled { get; set; }
        public ConnectionConfig Connection { get; set; } = null!;
        public GroupsConfig Groups { get; set; } = null!;
        public AuthConfig Auth { get; set; } = null!;
    }

    public class ConnectionConfig
    {
        public string Host { get; set; } = null!;
        public string User { get; set; } = null!;
        public string Pass { get; set; } = null!;
        public string BaseDn { get; set; } = null!;
        public SslConfig Ssl { get; set; } = null!;
    }

    public class SslConfig
    {
        public bool Enabled { get; set; }
        public string Cert { get; set; } = null!;
    }

    public class GroupsConfig
    {
        public string Allowedgroups { get; set; } = null!;
    }

    public class AuthConfig
    {
        public string Attribute { get; set; } = null!;
    }

    public class OpenIdConfig
    {
        public bool Enabled { get; set; }
    }

    public class SamlConfig
    {
        public bool Enabled { get; set; }
    }
}