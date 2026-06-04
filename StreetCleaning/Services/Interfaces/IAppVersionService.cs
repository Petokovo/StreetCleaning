namespace StreetCleaning.Services.Interfaces
{
    public interface IAppVersionService
    {
        /// <summary>
        /// Gets the current version of the application.
        /// </summary>
        /// <returns>A string representing the current version.</returns>
        string GetCurrentVersion();
    }
}
