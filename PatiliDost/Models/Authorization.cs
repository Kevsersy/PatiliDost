namespace PatiliDost.Models
{
    public class Authorization
    {
        public enum Roles
        {
            Administrator,
            Moderator,
            User
        }

        public static string DefaultRole => Roles.User.ToString();
    }



}
