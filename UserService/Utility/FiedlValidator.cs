using System.Text.RegularExpressions;

namespace UserService.Utility
{
    public class FiedlValidator
    {
        public static class FieldValidator
        {
            public static bool IsUsernameValid(string username)
            {
                if (username == null) return false;
                if (username.Length < 5) return false;
                return true;
            }

            public static bool IsPasswordValid(string password)
            {
                if (password == null) return false;

                if (password.Length > 5 &&
                    Regex.IsMatch(password, @"\d") &&
                    _containsUpperCase(password) &&
                    _containsSpecialChar(password)
                    )
                {
                    return true;
                }

                return false;
            }

            public static bool IsEmailValid(string password)
            {
                if (password == null) return false;

                if (password != null &&
                    password.Length > 10 &&
                    password.Contains('@') &&
                    password.Contains('.')
                    )
                {
                    return true;
                }
                return false;
            }

            private static bool _containsUpperCase(string word)
            {

                foreach (char c in word)
                {
                    if (char.IsUpper(c))
                    {
                        return true;
                    }
                }
                return false;

            }

            private static bool _containsSpecialChar(string word)
            {
                string pattern = @"[^a-zA-Z0-9\s]";

                return Regex.IsMatch(word, pattern);
            }
        }
    }
}
