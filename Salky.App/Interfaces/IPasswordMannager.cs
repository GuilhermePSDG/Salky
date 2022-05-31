namespace Salky.App.Interfaces
{
    public interface IPasswordMannager
    {
        /// <summary>
        /// </summary>
        /// <param name="OriginalHash"></param>
        /// <param name="Password"></param>
        /// <returns><see langword="true"/> if match, <see langword="false"/> oterwise</returns>
        public Task<bool> CheckPasswordSignInAsync(string OriginalHash, string Password);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Password"></param>
        /// <returns>A Hash of the <paramref name="Password"/></returns>
        public Task<string> CreateHashAsync(string Password);

    }
}