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
            Expires = DateTime.UtcNow.AddDays(5),
            SameSite = SameSiteMode.None
           
        };
        public static CookieOptions DevAccessTokenCookie = new CookieOptions()
        {
            HttpOnly = true,
            Secure = true,
            IsEssential = true,
            Expires = DateTime.UtcNow.AddMinutes(15),
            SameSite = SameSiteMode.None
        };
    }
}
