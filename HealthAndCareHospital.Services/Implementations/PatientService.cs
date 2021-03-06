﻿namespace HealthAndCareHospital.Services.Implementations
{
    using AutoMapper.QueryableExtensions;
    using HealthAndCareHospital.Data;
    using HealthAndCareHospital.Data.Models;
    using HealthAndCareHospital.Services.Models.Doctor;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class PatientService : IPatientService
    {
        private readonly HealthAndCareHospitalDbContext db;

        public PatientService(HealthAndCareHospitalDbContext db)
        {
            this.db = db;

        }

        public async Task<IEnumerable<PatientServiceModel>> All()
        {
            return await this.db.Patients
                .ProjectTo<PatientServiceModel>()
                .ToListAsync();
        }

        public async Task<bool> Create(string name, string EGN, int age, string email)
        {
            var doctor = await this.db.Doctors
                .Where(d => d.Email == email)
                .FirstOrDefaultAsync();

            if (doctor == null)
            {
                return false;
            }

            var patient = new Patient
            {
                Name = name,
                EGN = EGN,
                Age = age,
                Doctor = doctor,
                DoctorId = doctor.Id
            };

            if (patient == null)
            {
                return false;
            }

            doctor.Patients.Add(patient);
            this.db.Add(patient);
            await this.db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var patient = await this.db.Patients
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
            if (patient == null)
            {
                return false;
            }

            this.db.Remove(patient);
            await this.db.SaveChangesAsync();
            return true;
        }

        public async Task<PatientServiceModel> Details(int id)
        {
            return await this.db.Patients
                .Where(p => p.Id == id)
                .ProjectTo<PatientServiceModel>()
                .FirstOrDefaultAsync();
        }

        public async Task<bool> Edit(int id, string name, string EGN, int age, string email)
        {
            var doctor = await this.db.Doctors
             .Where(d => d.Email == email)
             .FirstOrDefaultAsync();

            if (doctor == null)
            {
                return false;
            }

            var patient = await this.db.Patients
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (patient == null)
            {
                return false;
            }

            patient.Name = name;
            patient.EGN = EGN;
            patient.Age = age;
            patient.Doctor = doctor;
            patient.DoctorId = doctor.Id;

            await this.db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PatientExists(int id)
        {
            return await this.db.Patients.AnyAsync(e => e.Id == id);
        }
    }
}
