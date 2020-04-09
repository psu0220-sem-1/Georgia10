using System;
using System.Collections.Generic;
using System.Linq;
using Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Server.Controllers
{
    public class VolumeController: IVolumeController
    {
        private IMaterialController materialController;
        private IAddressController addressController;
        private readonly GTLContext _context;

        public VolumeController(GTLContext context)
        {
            materialController = ControllerFactory.CreateMaterialController(context);
            addressController = ControllerFactory.CreateAddressController(context);
            _context = context;

        }

        public Volume Create(int materialID, int homeLocationID, int currentLocationID)
        {
     
            try
            {
                var material = materialController.FindByID(materialID);
                var homeLocation = addressController.FindByID(homeLocationID);
                var currentLocation = addressController.FindByID(currentLocationID);

                var newVolume = new Volume { Material = material, CurrentLocation = currentLocation, HomeLocation = homeLocation };
                return newVolume;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        
        }

        public int Delete(Volume t)
        {
            _context.Remove(t);
            return _context.SaveChanges(); ;
        }

        public List<Volume> FindAll()
        {
            IQueryable<Volume> volumes = _context.Volumes
                .Include(v => v.Material)
                    .ThenInclude(m => m.MaterialAuthors)
                        .ThenInclude(ma => ma.Author)
                .Include(v => v.Material)
                    .ThenInclude(m => m.MaterialSubjects)
                        .ThenInclude(ms => ms.MaterialSubject)
                .Include(v => v.HomeLocation)
                    .ThenInclude(a => a.Zip)
                .Include(v => v.CurrentLocation)
                    .ThenInclude(a => a.Zip);

            return volumes.ToList();
        }

        public List<Volume> FindVolumesForMaterial(int materialId)
        {
            try
            {
                IQueryable<Volume> volumes = _context.Volumes.Where(v => v.MaterialId == materialId)
              .Include(v => v.Material)
                  .ThenInclude(m => m.MaterialAuthors)
                      .ThenInclude(ma => ma.Author)
              .Include(v => v.Material)
                  .ThenInclude(m => m.MaterialSubjects)
                      .ThenInclude(ms => ms.MaterialSubject)
              .Include(v => v.HomeLocation)
                  .ThenInclude(a => a.Zip)
              .Include(v => v.CurrentLocation)
                  .ThenInclude(a => a.Zip);

                return volumes.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
          
        }

        public Volume FindByID(int ID)
        {
            try
            {
                var volume = _context.Volumes
        .Include(v => v.Material)
            .ThenInclude(m => m.MaterialAuthors)
                .ThenInclude(ma => ma.Author)
        .Include(v => v.Material)
            .ThenInclude(m => m.MaterialSubjects)
            .ThenInclude(ms => ms.MaterialSubject)
        .Include(v => v.HomeLocation)
            .ThenInclude(a => a.Zip)
         .Include(v => v.CurrentLocation)
             .ThenInclude(a => a.Zip)
         .Single(v => v.VolumeId == ID);

                return volume;
            }
            catch
            {
                throw new NullReferenceException(message: $"Volume with id: {ID} not found");
            }
    
        }

        public Volume FindByType(Volume t)
        {
            throw new NotImplementedException();
        }

        public Volume Insert(Volume volume)
        {
            _context.Volumes.Add(volume);
            _context.SaveChanges();
            return volume;
        }

        public Volume Update(Volume t)
        {
            throw new NotImplementedException();
        }
    }
}
