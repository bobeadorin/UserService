namespace UserService.AuthService
{
    public static class CookieTokenOptions
    {
        public static CookieOptions ProdRefreshTokenCookie = new CookieOptions()
        {
            HttpOnly = true,
            Secure = true,
            Expires = DateTime.UtcNow.AddDays(5),
            SameSite = SameSiteMode.Strict
        };

        public static CookieOptions ProdAccessTokenCookie = new CookieOptions()
        {
            HttpOnly = true,
            Secure = true,
            Expires = DateTime.UtcNow.AddMinutes(15),
            SameSite = SameSiteMode.Strict
        };

        public static CookieOptions DevRefreshTokenCookie = new CookieOptions()
        {
            HttpOnly = true,
            Secure = true,
            IsEssential = true,
            Expires = DateTimeOffset.UtcNow.AddDays(5),
            SameSite = SameSiteMode.None,
            Path = "/"

        };
        public static CookieOptions DevAccessTokenCookie = new CookieOptions()
        {
            HttpOnly = true,          
            IsEssential = true,
            Secure = true,
            Expires = DateTimeOffset.UtcNow.AddDays(5),
            SameSite = SameSiteMode.None,
            Path = "/"
        };

        public static CookieOptions DevRefreshTokenCookieLogout = new CookieOptions()
        {
            HttpOnly = true,
            Secure = true,
            IsEssential = true,
            Expires = DateTime.UtcNow.AddDays(0),
            SameSite = SameSiteMode.None,
            Path = "/"

        };
        public static CookieOptions DevAccessTokenCookieLogout = new CookieOptions()
        {
            HttpOnly = true,
            Secure = true,
            IsEssential = true,
            Expires = DateTime.UtcNow.AddMinutes(0),
            SameSite = SameSiteMode.None,
            Path = "/"
        };
    }
}
