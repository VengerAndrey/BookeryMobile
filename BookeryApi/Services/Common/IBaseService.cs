namespace BookeryApi.Services.Common
{
    public interface IBaseService
    {
        void SetBearerToken(string accessToken);
    }
}