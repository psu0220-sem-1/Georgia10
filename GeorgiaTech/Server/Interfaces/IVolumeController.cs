using System.Collections.Generic;
using Server.Models;

namespace Server.Controllers
{
    public interface IVolumeController: IController<Volume>
    {
        public Volume Create(int materialId, int homeLocationId, int currentLocationId);
        public List<Volume> FindVolumesForMaterial(int materialId);
    }
}
