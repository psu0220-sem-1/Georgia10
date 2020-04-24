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

        /// <summary>
        /// Create a volume entity object
        /// </summary>
        /// <param name="materialId"></param>
        /// <param name="homeLocationId"></param>
        /// <param name="currentLocationId"></param>
        /// <returns>The created volume</returns>
        public Volume Create(int materialId, int homeLocationId, int currentLocationId)
        {
     
            try
            {
                var material = materialController.FindByID(materialId);
                var homeLocation = addressController.FindByID(homeLocationId);
                var currentLocation = addressController.FindByID(currentLocationId);

                var newVolume = new Volume { Material = material, CurrentLocation = currentLocation, HomeLocation = homeLocation };
                return newVolume;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        
        }
        /// <summary>
        /// Deletes a volume entry from the database
        /// </summary>
        /// <param name="t"></param>
        /// <returns>The number of rows changed</returns>
        public int Delete(Volume volume)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                _context.Remove(volume);
                var changedRows = _context.SaveChanges();
                transaction.Commit();
                return changedRows;

            }
            catch
            {
                transaction.Rollback();
                throw;

            }
        }

        /// <summary>
        /// Find all the volumes in the database
        /// </summary>
        /// <returns>A list of volumes</returns>
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

        /// <summary>
        /// Find all volumes for a specific material
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns>A list of volumes for the specified material</returns>
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

        /// <summary>
        /// Inster a volume to the database
        /// </summary>
        /// <param name="volume"></param>
        /// <returns>The number of rows changed</returns>
        public int Insert(Volume volume)
        {
            _context.Volumes.Add(volume);
            return _context.SaveChanges();
        }

        /// <summary>
        /// Updates a volume entity in the database
        /// </summary>
        /// <param name="volume"></param>
        /// <returns>The number of rows changed</returns>
        public int Update(Volume volume)
        {
            if (!_context.ChangeTracker.HasChanges())
                return 0;

            int changedRows;
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                changedRows = _context.SaveChanges();
                transaction.Commit();
            }
            catch (DbUpdateException)
            {
                transaction.Rollback();
                throw;
            }

            return changedRows;
        }
    }
}
