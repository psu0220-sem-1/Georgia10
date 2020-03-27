using System;
using System.Collections.Generic;
using System.Linq;
using Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Server.Controllers
{
    public class VolumeController: IVolumeController
    {
        private MaterialController materialController;
        private AddressController addressController;
        private readonly GTLContext _context;

        public VolumeController(GTLContext context)
        {
            materialController = new MaterialController();
            addressController = new AddressController();
            _context = context;

        }

        public Volume Create(int materialID, int homeLocationID, int currentLocationID)
        {
            var material = materialController.FindByID(materialID);
            var homeLocation = addressController.FindByID(homeLocationID);
            var currentLocation = addressController.FindByID(currentLocationID);

            var volume = new Volume { Material = material, CurrentLocation = currentLocation, HomeLocation = homeLocation };

            return volume;
        }

        public int Delete(Volume t)
        {
            throw new NotImplementedException();
        }

        public List<Volume> FindAll()
        {
            IQueryable<Volume> volumes = _context.Volume
                .Include(v => v.Material)
                    .ThenInclude(m => m.MaterialAuthor)
                        .ThenInclude(ma => ma.Author)
                .Include(v => v.Material)
                    .ThenInclude(m => m.MaterialSubjectAssignment)
                        .ThenInclude(ms => ms.Subject)
                .Include(v => v.HomeLocation)
                    .ThenInclude(a => a.ZipNavigation)
                .Include(v => v.CurrentLocation)
                    .ThenInclude(a => a.ZipNavigation);

            return volumes.ToList();
        }

        public Volume FindByID(int ID)
        {
            var volume = _context.Volume
                .Include(v => v.Material)
                    .ThenInclude(m => m.MaterialAuthor)
                        .ThenInclude(ma => ma.Author)
                .Include(v => v.Material)
                    .ThenInclude(m => m.MaterialSubjectAssignment)
                        .ThenInclude(ms => ms.Subject)
                .Include(v => v.HomeLocation)
                    .ThenInclude(a => a.ZipNavigation)
                .Include(v => v.CurrentLocation)
                    .ThenInclude(a => a.ZipNavigation).Single(v => v.VolumeId == ID);

            return volume;
        }

        public Volume FindByType(Volume t)
        {
            throw new NotImplementedException();
        }

        public Volume Insert(Volume volume)
        {
            _context.Volume.Add(volume);
            _context.SaveChanges();
            return volume;
        }

        public Volume Update(Volume t)
        {
            throw new NotImplementedException();
        }
    }
}
