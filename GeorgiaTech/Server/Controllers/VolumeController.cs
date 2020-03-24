using System;
using System.Collections.Generic;
using Server.Models;

namespace Server.Controllers
{
    public class VolumeController: IVolumeController
    {
        private MemberController memberController;
        public VolumeController()
        {
            memberController = new MemberController();
        }

        public Volume Create(int materialID, int homeLocationID, int currentLocationID)
        {
            throw new NotImplementedException();
        }

        public int Delete(Volume t)
        {
            throw new NotImplementedException();
        }

        public List<Volume> FindAll()
        {
            throw new NotImplementedException();
        }

        public Volume FindByID(int ID)
        {
            throw new NotImplementedException();
        }

        public Volume FindByType(Volume t)
        {
            throw new NotImplementedException();
        }

        public Volume Insert(Volume t)
        {
            throw new NotImplementedException();
        }

        public Volume Update(Volume t)
        {
            throw new NotImplementedException();
        }
    }
}
