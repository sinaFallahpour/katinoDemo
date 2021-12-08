using System.Threading.Tasks;

namespace Katino.Config.Extentions
{
    public interface IHangfireUpdateJobAdvertisment
    {
        Task CheckExpire();
    }
}