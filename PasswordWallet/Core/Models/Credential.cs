namespace PasswordWallet.Core.Models
{
    /// <summary>
    /// A stored website or account credential (Table2).
    /// </summary>
    public sealed class Credential
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Notes { get; set; }
    }
}
