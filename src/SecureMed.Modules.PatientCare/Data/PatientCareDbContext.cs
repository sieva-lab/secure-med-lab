using Microsoft.EntityFrameworkCore;
using SecureMed.Modules.PatientCare.Domain;
using SecureMed.SharedKernel.Infrastructure;
using SecureMed.SharedKernel.Infrastructure.Security;

namespace SecureMed.Modules.PatientCare.Data
{
    public sealed class PatientCareDbContext(
        DbContextOptions<PatientCareDbContext> options,
        TimeProvider timeProvider,
        IEncryptionService encryptionService)
        : ModuleDbContext(options, timeProvider, encryptionService)
    {
        public override string Schema => "patient_care";

        public DbSet<Patient> Patients => Set<Patient>();
        // DbSet<Appointment> Appointments => Set<Appointment>();
    }
}
