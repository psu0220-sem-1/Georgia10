using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server
{
    public partial class GTLContext : DbContext
    {
        public GTLContext(DbContextOptions<GTLContext> options) : base(options) { }

        public virtual DbSet<Acquire> Acquires { get; set; }
        public virtual DbSet<AcquireReason> AcquireReasons { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Card> Cards { get; set; }
        public virtual DbSet<Loan> Loans { get; set; }
        public virtual DbSet<Material> Materials { get; set; }
        public virtual DbSet<MaterialAuthor> MaterialAuthors { get; set; }
        public virtual DbSet<MaterialSubject> MaterialSubjects { get; set; }
        public virtual DbSet<MaterialSubjects> MaterialSubjectAssignments { get; set; }
        public virtual DbSet<MaterialType> MaterialTypes { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<MemberType> MemberTypes { get; set; }
        public virtual DbSet<Membership> Memberships { get; set; }
        public virtual DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public virtual DbSet<Staff> Staff { get; set; }
        public virtual DbSet<Volume> Volumes { get; set; }
        public virtual DbSet<ZipCode> ZipCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Acquire>(entity =>
            {
                entity.ToTable("Acquire");

                // key
                entity.HasKey(acquire => acquire.MaterialId)
                    .HasName("PK__Acquire__6BFE1D288709E86E");

                // properties
                entity.Property(acquire => acquire.MaterialId)
                    .HasColumnName("material_id")
                    .ValueGeneratedNever();

                entity.Property(acquire => acquire.AdditionalInfo)
                    .IsRequired()
                    .HasColumnName("additional_info")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(acquire => acquire.ReasonId)
                    .HasColumnName("reason_id");

                // relationships
                entity.HasOne(acquire => acquire.Material)
                    .WithOne(material => material.Acquire)
                    .HasForeignKey<Acquire>(acquire => acquire.MaterialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_acquire_material_id");

                entity.HasOne(acquire => acquire.Reason)
                    .WithMany()
                    .HasForeignKey(acquire => acquire.ReasonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_acquire_reason_id");
            });

            modelBuilder.Entity<AcquireReason>(entity =>
            {
                entity.ToTable("Acquire_Reason");

                // key
                entity.HasKey(acquireReason => acquireReason.ReasonId)
                    .HasName("PK__Acquire___846BB554B66E8A55");

                // properties
                entity.Property(acquireReason => acquireReason.ReasonId)
                    .HasColumnName("reason_id");

                entity.Property(acquireReason => acquireReason.Reason)
                    .HasColumnName("reason")
                    .HasMaxLength(42)
                    .IsUnicode(false);

                // relationships
                entity.HasMany<Acquire>()
                    .WithOne(acquire => acquire.Reason)
                    .HasForeignKey(acquire => acquire.ReasonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_acquire_reason_id");
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

                // relations
                // don't set a navigational property on the zip side
                entity.HasOne(address => address.Zip)
                    .WithMany()
                    .HasForeignKey(address => address.ZipCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_address_zip");

                entity.HasMany<Member>()
                    .WithOne(member => member.HomeAddress)
                    .HasForeignKey(member => member.HomeAddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_member_home_address_id");

                entity.HasMany<Member>()
                    .WithOne(member => member.CampusAddress)
                    .HasForeignKey(member => member.CampusAddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_member_campus_address_id");

                entity.HasMany<Volume>()
                    .WithOne(volume => volume.HomeLocation)
                    .HasForeignKey(volume => volume.HomeLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_volume_home_location_id");

                entity.HasMany<Volume>()
                    .WithOne(volume => volume.CurrentLocation)
                    .HasForeignKey(volume => volume.CurrentLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_volume_current_location_id");
            });

            modelBuilder.Entity<Author>(entity =>
            {
                // key
                entity.HasKey(author => author.AuthorId);

                // properties
                entity.Property(author => author.AuthorId)
                    .HasColumnName("author_id");

                entity.Property(author => author.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(author => author.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                // relationships
                entity.HasMany(author => author.AuthorMaterials)
                    .WithOne(materialAuthor => materialAuthor.Author)
                    .HasForeignKey(materialAuthor => materialAuthor.AuthorId)
                    .HasConstraintName("FK_material_authors_author_id");
            });

            modelBuilder.Entity<Card>(entity =>
            {
                entity.ToTable("Card");

                // key
                entity.HasKey(card => new { card.MemberId, card.PhotoPath })
                    .HasName("PK__Card__52CBC9551B281D9F");

                // properties
                entity.Property(card => card.MemberId)
                    .HasColumnName("member_id");

                entity.Property(card => card.PhotoPath)
                    .HasColumnName("photo_path")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                // relationships
                entity.HasOne(card => card.Member)
                    .WithMany(member => member.Cards)
                    .HasForeignKey(card => card.MemberId)
                    .HasConstraintName("FK_card_member_id");
            });

            modelBuilder.Entity<Loan>(entity =>
            {
                entity.ToTable("Loan");

                // key
                entity.HasKey(loan => loan.LoanId);

                // properties
                entity.Property(loan => loan.LoanId)
                    .HasColumnName("loan_id");

                entity.Property(loan => loan.DueDate)
                    .HasColumnName("due_date")
                    .HasColumnType("date");

                entity.Property(loan => loan.Extensions)
                    .HasColumnName("extensions");

                entity.Property(loan => loan.LoanDate)
                    .HasColumnName("loan_date")
                    .HasColumnType("date");

                entity.Property(loan => loan.MemberId)
                    .HasColumnName("member_id");

                entity.Property(loan => loan.ReturnedDate)
                    .HasColumnName("returned_date")
                    .HasColumnType("date");

                entity.Property(loan => loan.VolumeId)
                    .HasColumnName("volume_id");

                // relationships
                entity.HasOne(loan => loan.Member)
                    .WithMany(member => member.Loans)
                    .HasForeignKey(loan => loan.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_loan_member_id");

                entity.HasOne(loan => loan.Volume)
                    .WithMany(volume => volume.Loans)
                    .HasForeignKey(loan => loan.VolumeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_loan_volume_id");
            });

            modelBuilder.Entity<Material>(entity =>
            {
                entity.ToTable("Material");

                // key
                entity.HasKey(material => material.MaterialId);

                // properties
                entity.Property(material => material.MaterialId)
                    .HasColumnName("material_id");

                entity.Property(material => material.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .IsUnicode(false);

                entity.Property(material => material.Isbn)
                    .IsRequired()
                    .HasColumnName("isbn")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(material => material.Language)
                    .IsRequired()
                    .HasColumnName("language")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(material => material.Lendable)
                    .HasColumnName("lendable");

                entity.Property(material => material.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(material => material.TypeId)
                    .HasColumnName("type_id");

                // relationships
                entity.HasOne(material => material.Type)
                    .WithMany()
                    .HasForeignKey(material => material.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_material_type_id");

                entity.HasMany(material => material.MaterialSubjects)
                    .WithOne(materialSubjects => materialSubjects.Material)
                    .HasForeignKey(materialSubjects => materialSubjects.MaterialId)
                    .OnDelete(DeleteBehavior.ClientNoAction)
                    .HasConstraintName("FK_material_subject_assignment_material_id");

                entity.HasMany(material => material.MaterialAuthors)
                    .WithOne(materialAuthor => materialAuthor.Material)
                    .HasForeignKey(materialAuthor => materialAuthor.MaterialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_material_authors_material_id");

                entity.HasOne(material => material.Acquire)
                    .WithOne(acquire => acquire.Material)
                    .HasForeignKey<Acquire>(acquire => acquire.MaterialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_acquire_material_id");

                entity.HasMany(material => material.Volumes)
                    .WithOne(volume => volume.Material)
                    .HasForeignKey(volume => volume.MaterialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_volume_material_id");
            });

            modelBuilder.Entity<MaterialAuthor>(entity =>
            {
                entity.ToTable("Material_Author");

                // key
                entity.HasKey(materialAuthor => new { materialAuthor.MaterialId, materialAuthor.AuthorId })
                    .HasName("PK__Material__839B0B943E2277FF");

                // properties
                entity.Property(materialAuthor => materialAuthor.MaterialId)
                    .HasColumnName("material_id");

                entity.Property(materialAuthor => materialAuthor.AuthorId)
                    .HasColumnName("author_id");

                // relationships
                entity.HasOne(materialAuthor => materialAuthor.Author)
                    .WithMany(author => author.AuthorMaterials)
                    .HasForeignKey(materialAuthor => materialAuthor.AuthorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_material_authors_author_id");

                entity.HasOne(materialAuthor => materialAuthor.Material)
                    .WithMany(material => material.MaterialAuthors)
                    .HasForeignKey(materialAuthor => materialAuthor.MaterialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_material_authors_material_id");
            });

            modelBuilder.Entity<MaterialSubject>(entity =>
            {
                entity.ToTable("Material_Subject");

                // key
                entity.HasKey(materialSubject => materialSubject.SubjectId)
                    .HasName("PK__Material__5004F660A42AA2F8");

                // properties
                entity.Property(materialSubject => materialSubject.SubjectId)
                    .HasColumnName("subject_id");

                entity.Property(materialSubject => materialSubject.SubjectName)
                    .IsRequired()
                    .HasColumnName("subject")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                // relationships
                entity.HasMany<MaterialSubjects>()
                    .WithOne(materialSubjects => materialSubjects.MaterialSubject)
                    .HasForeignKey(materialSubjects => materialSubjects.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_material_subject_assignment_subject_id");
            });

            modelBuilder.Entity<MaterialSubjects>(entity =>
            {
                entity.ToTable("Material_Subject_Assignment");

                // key
                entity.HasKey(materialSubjects => new { materialSubjects.MaterialId, materialSubjects.SubjectId })
                    .HasName("PK__Material__7EFE524E1E40022E");

                // properties
                entity.Property(materialSubjects => materialSubjects.MaterialId)
                    .HasColumnName("material_id");

                entity.Property(materialSubjects => materialSubjects.SubjectId)
                    .HasColumnName("subject_id");

                // relationships
                entity.HasOne(materialSubjects => materialSubjects.Material)
                    .WithMany(material => material.MaterialSubjects)
                    .HasForeignKey(materialSubjects => materialSubjects.MaterialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_material_subject_assignment_material_id");

                entity.HasOne(materialSubjects => materialSubjects.MaterialSubject)
                    .WithMany()
                    .HasForeignKey(materialSubjects => materialSubjects.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_material_subject_assignment_subject_id");
            });

            modelBuilder.Entity<MaterialType>(entity =>
            {
                entity.ToTable("Material_Type");

                // key
                entity.HasKey(materialType => materialType.TypeId)
                    .HasName("PK__Material__2C000598CDAD1BE3");

                // properties
                entity.Property(materialType => materialType.TypeId)
                    .HasColumnName("type_id");

                entity.Property(materialType => materialType.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                // relationships
                entity.HasMany<Material>()
                    .WithOne(material => material.Type)
                    .HasForeignKey(material => material.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_material_type_id");
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

                entity.HasMany(member => member.Memberships)
                    .WithOne(membership => membership.Member)
                    .HasForeignKey(membership => membership.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_member_type_assignment_member_id");

                entity.HasMany(member => member.Cards)
                    .WithOne(card => card.Member)
                    .HasForeignKey(card => card.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_card_member_id");

                entity.HasMany<Staff>()
                    .WithOne(staff => staff.Member)
                    .HasForeignKey(staff => staff.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_staff_member_id");

                entity.HasMany(member => member.Loans)
                    .WithOne(loan => loan.Member)
                    .HasForeignKey(loan => loan.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_loan_member_id");
            });

            modelBuilder.Entity<MemberType>(entity =>
            {
                entity.ToTable("Member_Type");

                // key
                entity.HasKey(e => e.TypeId)
                    .HasName("PK__Member_T__2C0005984BECC0B7");

                entity.HasIndex(e => e.TypeName)
                    .HasName("UQ__Member_T__E3F852486B4A84B2")
                    .IsUnique();

                // properties
                entity.Property(e => e.TypeId)
                    .HasColumnName("type_id");

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                // relationships
                entity.HasMany<Membership>()
                    .WithOne(membership => membership.MemberType)
                    .HasForeignKey(membership => membership.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_member_type_assignment_type_id");
            });

            modelBuilder.Entity<Membership>(entity =>
            {
                entity.ToTable("Member_Type_Assignment");

                // key
                entity.HasKey(membership => new { membership.MemberId, membership.TypeId })
                    .HasName("PK__Member_T__205B856DB2C31985");

                // properties
                entity.Property(e => e.MemberId)
                    .HasColumnName("member_id");

                entity.Property(e => e.TypeId)
                    .HasColumnName("type_id");

                // relationships
                entity.HasOne(membership => membership.Member)
                    .WithMany(member => member.Memberships)
                    .HasForeignKey(membership => membership.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_member_type_assignment_member_id");

                entity.HasOne(membership => membership.MemberType)
                    .WithMany()
                    .HasForeignKey(membership => membership.TypeId)
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
                entity.ToTable("Staff");

                // key
                entity.HasKey(staff => new { staff.MemberId, staff.JobTitle })
                    .HasName("PK__Staff__0BCB6B8218681470");

                // properties
                entity.Property(staff => staff.MemberId)
                    .HasColumnName("member_id");

                entity.Property(staff => staff.JobTitle)
                    .HasColumnName("job_title")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                // relationships
                entity.HasOne(staff => staff.Member)
                    .WithMany()
                    .HasForeignKey(staff => staff.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_staff_member_id");
            });

            modelBuilder.Entity<Volume>(entity =>
            {
                entity.ToTable("Volume");

                // key
                entity.HasKey(volume => volume.VolumeId);

                // properties
                entity.Property(volume => volume.VolumeId)
                    .HasColumnName("volume_id");

                entity.Property(volume => volume.CurrentLocationId)
                    .HasColumnName("current_location_id");

                entity.Property(volume => volume.HomeLocationId)
                    .HasColumnName("home_location_id");

                entity.Property(volume => volume.MaterialId)
                    .HasColumnName("material_id");

                // relationships
                entity.HasOne(volume => volume.CurrentLocation)
                    .WithMany()
                    .HasForeignKey(volume => volume.CurrentLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_volume_current_location_id");

                entity.HasOne(volume => volume.HomeLocation)
                    .WithMany()
                    .HasForeignKey(volume => volume.HomeLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_volume_home_location_id");

                entity.HasOne(volume => volume.Material)
                    .WithMany(material => material.Volumes)
                    .HasForeignKey(volume => volume.MaterialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_volume_material_id");

                entity.HasMany(volume => volume.Loans)
                    .WithOne(loan => loan.Volume)
                    .HasForeignKey(loan => loan.VolumeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_loan_volume_id");
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
