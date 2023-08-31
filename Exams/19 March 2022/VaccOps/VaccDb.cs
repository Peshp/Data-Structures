namespace VaccOps
{
    using Models;
    using Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class VaccDb : IVaccOps
    {
        private Dictionary<string, Doctor> doctors = new Dictionary<string, Doctor>();
        private Dictionary<string, Patient> patients = new Dictionary<string, Patient>();
        private Dictionary<string, List<Patient>> doctorsPatients = new Dictionary<string, List<Patient>>();
        private Dictionary<string, string> patientsDoctors = new Dictionary<string, string>();

        public void AddDoctor(Doctor doctor)
        {
            doctors.Add(doctor.Name, doctor);
            doctorsPatients[doctor.Name] = new List<Patient>();
        }

        public void AddPatient(Doctor doctor, Patient patient)
        {
            if(!this.Exist(doctor))
                throw new ArgumentException();

            patients.Add(patient.Name, patient);
            patientsDoctors.Add(patient.Name, doctor.Name);
            doctorsPatients[doctor.Name].Add(patient);
        }

        public void ChangeDoctor(Doctor oldDoctor, Doctor newDoctor, Patient patient)
        {
            if(!this.Exist(oldDoctor) || !this.Exist(newDoctor) || !this.Exist(patient))
                throw new ArgumentException();

            doctorsPatients[oldDoctor.Name].Remove(patient);
            doctorsPatients[newDoctor.Name].Add(patient);
            patientsDoctors[patient.Name] = newDoctor.Name;
        }

        public bool Exist(Doctor doctor)
            => doctors.Values.Contains(doctor);

        public bool Exist(Patient patient)
            => patients.Values.Contains(patient);

        public IEnumerable<Doctor> GetDoctors()
            => doctors.Values;

        public IEnumerable<Doctor> GetDoctorsByPopularity(int populariry)
            => this.GetDoctors().Where(d => d.Popularity == populariry);

        public IEnumerable<Doctor> GetDoctorsSortedByPatientsCountDescAndNameAsc()
            => this.GetDoctors().OrderByDescending(d => doctorsPatients[d.Name].Count)
                    .ThenBy(d => d.Name);

        public IEnumerable<Patient> GetPatients()
            => patients.Values;

        public IEnumerable<Patient> GetPatientsByTown(string town)
            => this.GetPatients().Where(p => p.Town == town);

        public IEnumerable<Patient> GetPatientsInAgeRange(int lo, int hi)
            => this.GetPatients().Where(p => p.Age >= lo && p.Age <= hi);

        public IEnumerable<Patient> GetPatientsSortedByDoctorsPopularityAscThenByHeightDescThenByAge()
            => this.GetPatients().OrderBy(p => doctors[patientsDoctors[p.Name]].Popularity)
                    .ThenByDescending(p => p.Height).ThenBy(p => p.Age);

        public Doctor RemoveDoctor(string name)
        {
            if (!doctors.ContainsKey(name))
                throw new ArgumentException();

            Doctor doctorToRemove = doctors[name];                     
            var patientsToRemove = doctorsPatients[name];

            doctors.Remove(name);
            doctorsPatients.Remove(name);
            foreach (var p in patientsToRemove)
            {
                patients.Remove(p.Name);
                patientsDoctors.Remove(p.Name);               
            }
            
            return doctorToRemove;
        }
    }
}
