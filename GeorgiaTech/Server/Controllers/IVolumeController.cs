using System.Collections.Generic;
using Server.Models;

namespace Server.Controllers
{
    public interface IVolumeController: IController<Volume>
    {
        public Volume Create(int materialID, int homeLocationID, int currentLocationID);
    }
}
