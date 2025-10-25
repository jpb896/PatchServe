namespace PatchServe
{
    public interface IAuthentication
    {
        void Apply(ref WebClientExt webClient);
    }
}
