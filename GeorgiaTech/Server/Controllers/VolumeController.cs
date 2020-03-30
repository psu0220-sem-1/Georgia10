﻿using System;
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

        public Volume FindByID(int ID)
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