namespace Reconocimientos.Interfaces
{
    public interface ILoginService
    {
        public bool IsAdmin(int rolId);
        public bool IsReclutador(int rolId);
        public bool IsCoordinadorRS(int rolId);
    }
}