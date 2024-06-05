namespace EliteSoftTask.Http.Responses;

using System;
using System.Collections.Generic;

public class FreeIpaUserRpcResponse
{
    public UserDetails Result { get; set; }
    public string Value { get; set; }
    public object Summary { get; set; }

    public class UserDetails
    {
        public List<string> Cn { get; set; }
        public List<string> DisplayName { get; set; }
        public List<string> Initials { get; set; }
        public List<string> Gecos { get; set; }
        public List<string> ObjectClass { get; set; }
        public List<string> IpaUniqueId { get; set; }
        public List<string> IpantSecurityIdentifier { get; set; }
        public List<string> Mail { get; set; }
        public List<string> HomeDirectory { get; set; }
        public List<string> KrbCanonicalName { get; set; }
        public List<string> GivenName { get; set; }
        public List<string> UserClass { get; set; }
        public List<string> LoginShell { get; set; }
        public List<string> Uid { get; set; }
        public List<string> UidNumber { get; set; }
        public List<string> Sn { get; set; }
        public List<string> KrbPrincipalName { get; set; }
        public List<string> GidNumber { get; set; }
        public bool NsAccountLock { get; set; }
        public bool HasPassword { get; set; }
        public bool HasKeytab { get; set; }
        public bool Preserved { get; set; }
        public List<string> MemberOfGroup { get; set; }
        public string Dn { get; set; }
    }

    public class DateTimeWrapper
    {
        public DateTime __datetime__ { get; set; }
    }
}
