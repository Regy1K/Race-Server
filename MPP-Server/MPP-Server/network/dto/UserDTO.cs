using System;

namespace MPP_Server.network.dto
{
    [Serializable]
    public class UserDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public UserDTO() { }

        public UserDTO(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public override string ToString() => $"UserDTO {{ Username = \"{Username}\", Password = \"***\" }}";
    }
}