using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server
{
    public partial class GTLContext : DbContext
    {
        public GTLContext(DbContextOptions<GTLContext> options) : base(options) { }

        public virtual DbSet<Acquire> Acquire { get; set; }
        public virtual DbSet<AcquireReason> AcquireReason { get; set; }
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Author> Author { get; set; }
        public virtual DbSet<Card> Card { get; set; }
        public virtual DbSet<Loan> Loan { get; set; }
        public virtual DbSet<Material> Material { get; set; }
        public virtual DbSet<MaterialAuthor> MaterialAuthor { get; set; }
        public virtual DbSet<MaterialSubject> MaterialSubject { get; set; }
        public virtual DbSet<MaterialSubjectAssignment> MaterialSubjectAssignment { get; set; }
        public virtual DbSet<MaterialType> MaterialType { get; set; }
        public virtual DbSet<Member> Member { get; set; }
        public virtual DbSet<MemberType> MemberType { get; set; }
        public virtual DbSet<MemberTypeAssignment> MemberTypeAssignment { get; set; }
        public virtual DbSet<PhoneNumber> PhoneNumber { get; set; }
        public virtual DbSet<Staff> Staff { get; set; }
        public virtual DbSet<Volume> Volume { get; set; }
        public virtual DbSet<ZipCode> ZipCode { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Acquire>(entity =>
            {
                entity.HasKey(e => e.MaterialId)
                    .HasName("PK__Acquire__6BFE1D288709E86E");

                entity.Property(e => e.MaterialId)
                    .HasColumnName("material_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AdditionalInfo)
                    .IsRequired()
                    .HasColumnName("additional_info")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ReasonId).HasColumnName("reason_id");

                entity.HasOne(d => d.Material)
                    .WithOne(p => p.Acquire)
                    .HasForeignKey<Acquire>(d => d.MaterialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_acquire_material_id");

                entity.HasOne(d => d.Reason)
                    .WithMany(p => p.Acquire)
                    .HasForeignKey(d => d.ReasonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_acquire_reason_id");
            });

            modelBuilder.Entity<AcquireReason>(entity =>
            {
                entity.HasKey(e => e.ReasonId)
                    .HasName("PK__Acquire___846BB554B66E8A55");

                entity.ToTable("Acquire_Reason");

                entity.Property(e => e.ReasonId).HasColumnName("reason_id");

                entity.Property(e => e.Reason)
                    .HasColumnName("reason")
                    .HasMaxLength(42)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("address");

                // key
                entity.HasKey(address => address.AddressId);

                // properties
                entity.Property(address => address.AdditionalInfo)
                    .IsRequired(false)
                    .HasColumnName("additional_info")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(address => address.Street)
                    .IsRequired()
                    .HasColumnName("street")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(address => address.Zip)
                    .HasColumnName("zip");

                // relations
                // don't set a navigational property on the zip side
                entity.HasOne(address => address.Zip)
                    .WithMany()
                    .HasForeignKey(address => address.ZipCode)
                    .OnDelete(DeleteBehavior.ClientNoAction)
                    .HasConstraintName("FK_address_zip");

                entity.HasMany<Member>()
                    .WithOne(member => member.HomeAddress)
                    .HasForeignKey(member => member.HomeAddressId)
                    .HasConstraintName("FK_member_home_address_id");

                entity.HasMany<Member>()
                    .WithOne(member => member.CampusAddress)
                    .HasForeignKey(member => member.CampusAddressId)
                    .HasConstraintName("FK_member_campus_address_id");
            });

            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(e => e.AuthorId).HasColumnName("author_id");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Card>(entity =>
            {
                entity.HasKey(e => new { e.MemberId, e.PhotoPath })
                    .HasName("PK__Card__52CBC9551B281D9F");

                entity.Property(e => e.MemberId).HasColumnName("member_id");

                entity.Property(e => e.PhotoPath)
                    .HasColumnName("photo_path")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Card)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK_card_member_id");
            });

            modelBuilder.Entity<Loan>(entity =>
            {
                entity.Property(e => e.LoanId).HasColumnName("loan_id");

                entity.Property(e => e.DueDate)
                    .HasColumnName("due_date")
                    .HasColumnType("date");

                entity.Property(e => e.Extensions).HasColumnName("extensions");

                entity.Property(e => e.LoanDate)
                    .HasColumnName("loan_date")
                    .HasColumnType("date");

                entity.Property(e => e.MemberId).HasColumnName("member_id");

                entity.Property(e => e.ReturnedDate)
                    .HasColumnName("returned_date")
                    .HasColumnType("date");

                entity.Property(e => e.VolumeId).HasColumnName("volume_id");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Loan)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_loan_member_id");

                entity.HasOne(d => d.Volume)
                    .WithMany(p => p.Loan)
                    .HasForeignKey(d => d.VolumeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_loan_volume_id");
            });

            modelBuilder.Entity<Material>(entity =>
            {
                entity.Property(e => e.MaterialId).HasColumnName("material_id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .IsUnicode(false);

                entity.Property(e => e.Isbn)
                    .IsRequired()
                    .HasColumnName("isbn")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Language)
                    .IsRequired()
                    .HasColumnName("language")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Lendable).HasColumnName("lendable");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TypeId).HasColumnName("type_id");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Material)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_material_type_id");
            });

            modelBuilder.Entity<MaterialAuthor>(entity =>
            {
                entity.HasKey(e => new { e.MaterialId, e.AuthorId })
                    .HasName("PK__Material__839B0B943E2277FF");

                entity.ToTable("Material_Author");

                entity.Property(e => e.MaterialId).HasColumnName("material_id");

                entity.Property(e => e.AuthorId).HasColumnName("author_id");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.MaterialAuthor)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_material_authors_author_id");

                entity.HasOne(d => d.Material)
                    .WithMany(p => p.MaterialAuthor)
                    .HasForeignKey(d => d.MaterialId)
                    .HasConstraintName("FK_material_authors_material_id");
            });

            modelBuilder.Entity<MaterialSubject>(entity =>
            {
                entity.HasKey(e => e.SubjectId)
                    .HasName("PK__Material__5004F660A42AA2F8");

                entity.ToTable("Material_Subject");

                entity.Property(e => e.SubjectId).HasColumnName("subject_id");

                entity.Property(e => e.Subject)
                    .IsRequired()
                    .HasColumnName("subject")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MaterialSubjectAssignment>(entity =>
            {
                entity.HasKey(e => new { e.MaterialId, e.SubjectId })
                    .HasName("PK__Material__7EFE524E1E40022E");

                entity.ToTable("Material_Subject_Assignment");

                entity.Property(e => e.MaterialId).HasColumnName("material_id");

                entity.Property(e => e.SubjectId).HasColumnName("subject_id");

                entity.HasOne(d => d.Material)
                    .WithMany(p => p.MaterialSubjectAssignment)
                    .HasForeignKey(d => d.MaterialId)
                    .HasConstraintName("FK_material_subject_assignment_material_id");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.MaterialSubjectAssignment)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_material_subject_assignment_subject_id");
            });

            modelBuilder.Entity<MaterialType>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PK__Material__2C000598CDAD1BE3");

                entity.ToTable("Material_Type");

                entity.Property(e => e.TypeId).HasColumnName("type_id");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.ToTable("member");

                // key
                entity.HasKey(member => member.MemberId);


                // properties
                entity.Property(member => member.MemberId)
                    .HasColumnName("member_id");

                entity.Property(member => member.Ssn)
                    .IsRequired()
                    .HasColumnName("ssn")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(member => member.FName)
                    .IsRequired()
                    .HasColumnName("f_name")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(member => member.LName)
                    .IsRequired()
                    .HasColumnName("l_name")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(member => member.CampusAddressId)
                    .HasColumnName("campus_address_id");


                entity.Property(member => member.HomeAddressId)
                    .HasColumnName("home_address_id");

                // relations
                entity.HasOne(member => member.CampusAddress)
                    .WithMany()
                    .HasForeignKey(member => member.CampusAddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_member_campus_address_id");

                entity.HasOne(member => member.HomeAddress)
                    .WithMany()
                    .HasForeignKey(member => member.HomeAddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_member_home_address_id");
            });

            modelBuilder.Entity<MemberType>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PK__Member_T__2C0005984BECC0B7");

                entity.ToTable("Member_Type");

                entity.HasIndex(e => e.Type)
                    .HasName("UQ__Member_T__E3F852486B4A84B2")
                    .IsUnique();

                entity.Property(e => e.TypeId).HasColumnName("type_id");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MemberTypeAssignment>(entity =>
            {
                entity.HasKey(e => new { e.MemberId, e.TypeId })
                    .HasName("PK__Member_T__205B856DB2C31985");

                entity.ToTable("Member_Type_Assignment");

                entity.Property(e => e.MemberId).HasColumnName("member_id");

                entity.Property(e => e.TypeId).HasColumnName("type_id");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.MemberTypeAssignment)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_member_type_assignment_member_id");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.MemberTypeAssignment)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_member_type_assignment_type_id");
            });

            modelBuilder.Entity<PhoneNumber>(entity =>
            {
                entity.ToTable("Phone_Number");

                // key
                entity.HasKey(phone => new { phone.MemberId, phone.Number })
                    .HasName("PK__Phone_Nu__0882B39245F31267");

                // properties
                entity.Property(phone => phone.MemberId)
                    .HasColumnName("member_id");

                entity.Property(phone => phone.Number)
                    .HasColumnName("phone_number")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                // relations
                entity.HasOne(phone => phone.Member)
                    .WithMany(member => member.PhoneNumbers)
                    .HasForeignKey(phone => phone.MemberId)
                    .HasConstraintName("FK_phone_number_member_id");
            });

            modelBuilder.Entity<Staff>(entity =>
            {
                entity.HasKey(e => new { e.MemberId, e.JobTitle })
                    .HasName("PK__Staff__0BCB6B8218681470");

                entity.Property(e => e.MemberId).HasColumnName("member_id");

                entity.Property(e => e.JobTitle)
                    .HasColumnName("job_title")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Staff)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK_staff_member_id");
            });

            modelBuilder.Entity<Volume>(entity =>
            {
                entity.Property(e => e.VolumeId).HasColumnName("volume_id");

                entity.Property(e => e.CurrentLocationId).HasColumnName("current_location_id");

                entity.Property(e => e.HomeLocationId).HasColumnName("home_location_id");

                entity.Property(e => e.MaterialId).HasColumnName("material_id");

                entity.HasOne(d => d.CurrentLocation)
                    .WithMany(p => p.VolumeCurrentLocation)
                    .HasForeignKey(d => d.CurrentLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_volume_current_location_id");

                entity.HasOne(d => d.HomeLocation)
                    .WithMany(p => p.VolumeHomeLocation)
                    .HasForeignKey(d => d.HomeLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_volume_home_location_id");

                entity.HasOne(d => d.Material)
                    .WithMany(p => p.Volume)
                    .HasForeignKey(d => d.MaterialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_volume_material_id");
            });

            modelBuilder.Entity<ZipCode>(entity =>
            {
                entity.ToTable("Zip_Code");

                // key
                entity.HasKey(zip => zip.Code)
                    .HasName("PK__Zip_Code__30B369C4BB6ACBF8");

                // properties
                entity.Property(zip => zip.Code)
                    .HasColumnName("zip")
                    .ValueGeneratedNever();

                entity.Property(zip => zip.City)
                    .IsRequired()
                    .HasColumnName("city")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                // relationships
                // don't set the zip to have a navigational property, like in address, but the inverse
                entity.HasMany<Address>()
                    .WithOne(address => address.Zip)
                    .HasForeignKey(address => address.ZipCode);
            });

        }
    }
}
