using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OpenImis.DB.SqlServer
{
    public partial class IMISContext : DbContext
    {
        public IMISContext()
        {
        }

        public IMISContext(DbContextOptions<IMISContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblBatchRun> TblBatchRun { get; set; }
        public virtual DbSet<TblCeilingInterpretation> TblCeilingInterpretation { get; set; }
        public virtual DbSet<TblClaim> TblClaim { get; set; }
        public virtual DbSet<TblClaimAdmin> TblClaimAdmin { get; set; }
        public virtual DbSet<TblClaimDedRem> TblClaimDedRem { get; set; }
        public virtual DbSet<TblClaimItems> TblClaimItems { get; set; }
        public virtual DbSet<TblClaimServices> TblClaimServices { get; set; }
        public virtual DbSet<TblConfirmationTypes> TblConfirmationTypes { get; set; }
        public virtual DbSet<TblControls> TblControls { get; set; }
        public virtual DbSet<TblEducations> TblEducations { get; set; }
        public virtual DbSet<TblExtracts> TblExtracts { get; set; }
        public virtual DbSet<TblFamilies> TblFamilies { get; set; }
        public virtual DbSet<TblFamilySMS> TblFamilySMS { get; set; }
        public virtual DbSet<TblFamilyTypes> TblFamilyTypes { get; set; }
        public virtual DbSet<TblFeedback> TblFeedback { get; set; }
        public virtual DbSet<TblFeedbackPrompt> TblFeedbackPrompt { get; set; }
        public virtual DbSet<TblFromPhone> TblFromPhone { get; set; }
        public virtual DbSet<TblGender> TblGender { get; set; }
        public virtual DbSet<TblHealthStatus> TblHealthStatus { get; set; }
        public virtual DbSet<TblHf> TblHf { get; set; }
        public virtual DbSet<TblHfcatchment> TblHfcatchment { get; set; }
        public virtual DbSet<TblHfsublevel> TblHfsublevel { get; set; }
        public virtual DbSet<TblIcdcodes> TblIcdcodes { get; set; }
        public virtual DbSet<TblIdentificationTypes> TblIdentificationTypes { get; set; }
        public virtual DbSet<TblImisdefaults> TblImisdefaults { get; set; }
        public virtual DbSet<TblInsuree> TblInsuree { get; set; }
        public virtual DbSet<TblInsureePolicy> TblInsureePolicy { get; set; }
        public virtual DbSet<TblItems> TblItems { get; set; }
        public virtual DbSet<TblLanguages> TblLanguages { get; set; }
        public virtual DbSet<TblLegalForms> TblLegalForms { get; set; }
        public virtual DbSet<TblLocations> TblLocations { get; set; }
        public virtual DbSet<TblLogins> TblLogins { get; set; }
        public virtual DbSet<TblOfficer> TblOfficer { get; set; }
        public virtual DbSet<TblOfficerVillages> TblOfficerVillages { get; set; }
        public virtual DbSet<TblPayer> TblPayer { get; set; }
        public virtual DbSet<TblPayerType> TblPayerType { get; set; }
        public virtual DbSet<TblPhotos> TblPhotos { get; set; }
        public virtual DbSet<TblPlitems> TblPlitems { get; set; }
        public virtual DbSet<TblPlitemsDetail> TblPlitemsDetail { get; set; }
        public virtual DbSet<TblPlservices> TblPlservices { get; set; }
        public virtual DbSet<TblPlservicesDetail> TblPlservicesDetail { get; set; }
        public virtual DbSet<TblPolicy> TblPolicy { get; set; }
        public virtual DbSet<TblPolicyRenewalDetails> TblPolicyRenewalDetails { get; set; }
        public virtual DbSet<TblPolicyRenewals> TblPolicyRenewals { get; set; }
        public virtual DbSet<TblPremium> TblPremium { get; set; }
        public virtual DbSet<TblProduct> TblProduct { get; set; }
        public virtual DbSet<TblProductItems> TblProductItems { get; set; }
        public virtual DbSet<TblProductServices> TblProductServices { get; set; }
        public virtual DbSet<TblProfessions> TblProfessions { get; set; }
        public virtual DbSet<TblRelations> TblRelations { get; set; }
        public virtual DbSet<TblRelDistr> TblRelDistr { get; set; }
        public virtual DbSet<TblRelIndex> TblRelIndex { get; set; }
        public virtual DbSet<TblReporting> TblReporting { get; set; }
        public virtual DbSet<TblServices> TblServices { get; set; }
        public virtual DbSet<TblSubmittedPhotos> TblSubmittedPhotos { get; set; }
        public virtual DbSet<TblUsers> TblUsers { get; set; }
        public virtual DbSet<TblUsersDistricts> TblUsersDistricts { get; set; }
        public virtual DbSet<TblRoleRight> TblRoleRight { get; set; }
        public virtual DbSet<TblUserRole> TblUserRole { get; set; }
        public virtual DbSet<TblIMISDefaultsPhone> TblIMISDefaultsPhone { get; set; }
        public virtual DbSet<TblVillages> TblVillages { get; set; }
        public virtual DbSet<TblWards> TblWards { get; set; }
        public virtual DbSet<TblDistricts> TblDistricts { get; set; }

        public virtual DbSet<TblRole> TblRole { get; set; }

        // Unable to generate entity type for table 'dbo.tblIMISDetaulsPhone'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.tblEmailSettings'. Please see the warning messages.


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblBatchRun>(entity =>
            {
                entity.HasKey(e => e.RunId);

                entity.ToTable("tblBatchRun");

                entity.Property(e => e.RunId).HasColumnName("RunID");

                entity.Property(e => e.AuditUserId).HasColumnName("AuditUserID");

                entity.Property(e => e.LegacyId).HasColumnName("LegacyID");

                entity.Property(e => e.RunDate).HasColumnType("datetime");

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.TblBatchRun)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_tblBatchRun_tblLocations");
            });

            modelBuilder.Entity<TblCeilingInterpretation>(entity =>
            {
                entity.HasKey(e => e.CeilingIntCode);

                entity.ToTable("tblCeilingInterpretation");

                entity.Property(e => e.CeilingIntCode)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.AltLanguage).HasMaxLength(100);

                entity.Property(e => e.CeilingIntDesc)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<TblClaim>(entity =>
            {
                entity.HasKey(e => e.ClaimId);

                entity.ToTable("tblClaim");

                entity.HasIndex(e => e.Hfid)
                    .HasName("NCI_tblClaim_HFID");

                entity.HasIndex(e => e.InsureeId)
                    .HasName("NCI_tblClaim_InsureeID");

                entity.HasIndex(e => new { e.DateFrom, e.DateTo })
                    .HasName("NCI_tblClaim_DateFromTo");

                entity.Property(e => e.ClaimId).HasColumnName("ClaimID");

                entity.Property(e => e.Adjustment).HasColumnType("ntext");

                entity.Property(e => e.ApprovalStatus).HasDefaultValueSql("((1))");

                entity.Property(e => e.AuditUserId).HasColumnName("AuditUserID");

                entity.Property(e => e.AuditUserIdprocess).HasColumnName("AuditUserIDProcess");

                entity.Property(e => e.AuditUserIdreview).HasColumnName("AuditUserIDReview");

                entity.Property(e => e.AuditUserIdsubmit).HasColumnName("AuditUserIDSubmit");

                entity.Property(e => e.ClaimCategory)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ClaimCode)
                    .IsRequired()
                    .HasMaxLength(8);

                entity.Property(e => e.ClaimStatus).HasDefaultValueSql("((2))");

                entity.Property(e => e.DateClaimed)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateFrom).HasColumnType("smalldatetime");

                entity.Property(e => e.DateProcessed).HasColumnType("smalldatetime");

                entity.Property(e => e.DateTo).HasColumnType("smalldatetime");

                entity.Property(e => e.Explanation).HasColumnType("ntext");

                entity.Property(e => e.FeedbackId)
                    .HasColumnName("FeedbackID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.FeedbackStatus).HasDefaultValueSql("((1))");

                entity.Property(e => e.GuaranteeId).HasMaxLength(50);

                entity.Property(e => e.Hfid).HasColumnName("HFID");

                entity.Property(e => e.Icdid).HasColumnName("ICDID");

                entity.Property(e => e.Icdid1).HasColumnName("ICDID1");

                entity.Property(e => e.Icdid2).HasColumnName("ICDID2");

                entity.Property(e => e.Icdid3).HasColumnName("ICDID3");

                entity.Property(e => e.Icdid4).HasColumnName("ICDID4");

                entity.Property(e => e.InsureeId).HasColumnName("InsureeID");

                entity.Property(e => e.LegacyId).HasColumnName("LegacyID");

                entity.Property(e => e.ProcessStamp).HasColumnType("datetime");

                entity.Property(e => e.RejectionReason).HasDefaultValueSql("((0))");

                entity.Property(e => e.ReviewStatus).HasDefaultValueSql("((1))");

                entity.Property(e => e.RowId)
                    .HasColumnName("RowID")
                    .IsRowVersion();

                entity.Property(e => e.RunId).HasColumnName("RunID");

                entity.Property(e => e.SubmitStamp).HasColumnType("datetime");

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityFromReview).HasColumnType("datetime");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");

                entity.Property(e => e.ValidityToReview).HasColumnType("datetime");

                entity.Property(e => e.VisitType)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.HasOne(d => d.AdjusterNavigation)
                    .WithMany(p => p.TblClaim)
                    .HasForeignKey(d => d.Adjuster)
                    .HasConstraintName("FK_tblClaim_tblUsers");

                entity.HasOne(d => d.ClaimAdmin)
                    .WithMany(p => p.TblClaim)
                    .HasForeignKey(d => d.ClaimAdminId)
                    .HasConstraintName("FK_tblClaim_tblClaimAdmin");

                entity.HasOne(d => d.FeedbackNavigation)
                    .WithMany(p => p.TblClaim)
                    .HasForeignKey(d => d.FeedbackId)
                    .HasConstraintName("FK_tblClaim_tblFeedback-FeedbackID");

                entity.HasOne(d => d.Hf)
                    .WithMany(p => p.TblClaim)
                    .HasForeignKey(d => d.Hfid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblClaim_tblHF");

                entity.HasOne(d => d.Icd)
                    .WithMany(p => p.TblClaim)
                    .HasForeignKey(d => d.Icdid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblClaim_tblICDCodes-ICDID");

                entity.HasOne(d => d.Insuree)
                    .WithMany(p => p.TblClaim)
                    .HasForeignKey(d => d.InsureeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblClaim_tblInsuree-InsureeID");

                entity.HasOne(d => d.Run)
                    .WithMany(p => p.TblClaim)
                    .HasForeignKey(d => d.RunId)
                    .HasConstraintName("FK_tblClaim_tblBatchRun");
            });

            modelBuilder.Entity<TblClaimAdmin>(entity =>
            {
                entity.HasKey(e => e.ClaimAdminId);

                entity.ToTable("tblClaimAdmin");

                entity.Property(e => e.ClaimAdminCode).HasMaxLength(8);

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("date");

                entity.Property(e => e.EmailId).HasMaxLength(200);

                entity.Property(e => e.Hfid).HasColumnName("HFId");

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.OtherNames).HasMaxLength(100);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.RowId).IsRowVersion();

                entity.Property(e => e.ValidityFrom).HasColumnType("datetime");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");

                entity.HasOne(d => d.Hf)
                    .WithMany(p => p.TblClaimAdmin)
                    .HasForeignKey(d => d.Hfid)
                    .HasConstraintName("FK_tblClaimAdmin_tblHF");
            });

            modelBuilder.Entity<TblClaimDedRem>(entity =>
            {
                entity.HasKey(e => e.ExpenditureId);

                entity.ToTable("tblClaimDedRem");

                entity.HasIndex(e => e.ClaimId)
                    .HasName("NCI_tblClaimDedRem_ClaimID");

                entity.HasIndex(e => e.InsureeId)
                    .HasName("NCI_tblClaimDedRem_InsureeID");

                entity.HasIndex(e => e.PolicyId)
                    .HasName("NCI_tblClaimDedRem_PolicyID");

                entity.Property(e => e.ExpenditureId).HasColumnName("ExpenditureID");

                entity.Property(e => e.AuditUserId).HasColumnName("AuditUserID");

                entity.Property(e => e.ClaimId).HasColumnName("ClaimID");

                entity.Property(e => e.DedIp).HasColumnName("DedIP");

                entity.Property(e => e.DedOp).HasColumnName("DedOP");

                entity.Property(e => e.InsureeId).HasColumnName("InsureeID");

                entity.Property(e => e.LegacyId).HasColumnName("LegacyID");

                entity.Property(e => e.PolicyId).HasColumnName("PolicyID");

                entity.Property(e => e.RemIp).HasColumnName("RemIP");

                entity.Property(e => e.RemOp).HasColumnName("RemOP");

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");

                entity.HasOne(d => d.Insuree)
                    .WithMany(p => p.TblClaimDedRem)
                    .HasForeignKey(d => d.InsureeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblClaimDedRem_tblInsuree");

                entity.HasOne(d => d.Policy)
                    .WithMany(p => p.TblClaimDedRem)
                    .HasForeignKey(d => d.PolicyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblClaimDedRem_tblPolicy");
            });

            modelBuilder.Entity<TblClaimItems>(entity =>
            {
                entity.HasKey(e => e.ClaimItemId);

                entity.ToTable("tblClaimItems");

                entity.HasIndex(e => e.ClaimId)
                    .HasName("NCI_tblClaimItems_ClaimID");

                entity.HasIndex(e => e.ItemId)
                    .HasName("tblClaimItems_tblClaimItems_ItemID");

                entity.HasIndex(e => e.ProdId)
                    .HasName("NCI_tblClaimItems_ProdID");

                entity.Property(e => e.ClaimItemId).HasColumnName("ClaimItemID");

                entity.Property(e => e.AuditUserId).HasColumnName("AuditUserID");

                entity.Property(e => e.AuditUserIdreview).HasColumnName("AuditUserIDReview");

                entity.Property(e => e.Availability)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ClaimId).HasColumnName("ClaimID");

                entity.Property(e => e.ClaimItemStatus).HasDefaultValueSql("((1))");

                entity.Property(e => e.Explanation).HasColumnType("ntext");

                entity.Property(e => e.ItemId).HasColumnName("ItemID");

                entity.Property(e => e.Justification).HasColumnType("ntext");

                entity.Property(e => e.LegacyId).HasColumnName("LegacyID");

                entity.Property(e => e.Limitation)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.PolicyId).HasColumnName("PolicyID");

                entity.Property(e => e.PriceOrigin)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ProdId).HasColumnName("ProdID");

                entity.Property(e => e.RejectionReason).HasDefaultValueSql("((0))");

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityFromReview).HasColumnType("datetime");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");

                entity.Property(e => e.ValidityToReview).HasColumnType("datetime");

                entity.HasOne(d => d.Claim)
                    .WithMany(p => p.TblClaimItems)
                    .HasForeignKey(d => d.ClaimId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblClaimItems_tblClaim-ClaimID");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.TblClaimItems)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblClaimItems_tblItems-ItemID");

                entity.HasOne(d => d.Prod)
                    .WithMany(p => p.TblClaimItems)
                    .HasForeignKey(d => d.ProdId)
                    .HasConstraintName("FK_tblClaimItems_tblProduct-ProdID");
            });

            modelBuilder.Entity<TblClaimServices>(entity =>
            {
                entity.HasKey(e => e.ClaimServiceId);

                entity.ToTable("tblClaimServices");

                entity.HasIndex(e => e.ClaimId)
                    .HasName("NCI_tblClaimServices_ClaimID");

                entity.HasIndex(e => e.ProdId)
                    .HasName("NCI_tblClaimServices_ProdID");

                entity.HasIndex(e => e.ServiceId)
                    .HasName("NCI_tblClaimServices_ServiceID");

                entity.Property(e => e.ClaimServiceId).HasColumnName("ClaimServiceID");

                entity.Property(e => e.AuditUserId).HasColumnName("AuditUserID");

                entity.Property(e => e.AuditUserIdreview).HasColumnName("AuditUserIDReview");

                entity.Property(e => e.ClaimId).HasColumnName("ClaimID");

                entity.Property(e => e.ClaimServiceStatus).HasDefaultValueSql("((1))");

                entity.Property(e => e.Explanation).HasColumnType("ntext");

                entity.Property(e => e.Justification).HasColumnType("ntext");

                entity.Property(e => e.LegacyId).HasColumnName("LegacyID");

                entity.Property(e => e.Limitation)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.PolicyId).HasColumnName("PolicyID");

                entity.Property(e => e.PriceOrigin)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ProdId).HasColumnName("ProdID");

                entity.Property(e => e.RejectionReason).HasDefaultValueSql("((0))");

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityFromReview).HasColumnType("datetime");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");

                entity.Property(e => e.ValidityToReview).HasColumnType("datetime");

                entity.HasOne(d => d.Claim)
                    .WithMany(p => p.TblClaimServices)
                    .HasForeignKey(d => d.ClaimId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblClaimServices_tblClaim-ClaimID");

                entity.HasOne(d => d.Prod)
                    .WithMany(p => p.TblClaimServices)
                    .HasForeignKey(d => d.ProdId)
                    .HasConstraintName("FK_tblClaimServices_tblProduct-ProdID");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.TblClaimServices)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblClaimServices_tblServices-ServiceID");
            });

            modelBuilder.Entity<TblConfirmationTypes>(entity =>
            {
                entity.HasKey(e => e.ConfirmationTypeCode);

                entity.ToTable("tblConfirmationTypes");

                entity.Property(e => e.ConfirmationTypeCode)
                    .HasMaxLength(3)
                    .ValueGeneratedNever();

                entity.Property(e => e.AltLanguage).HasMaxLength(50);

                entity.Property(e => e.ConfirmationType)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TblControls>(entity =>
            {
                entity.HasKey(e => e.FieldName);

                entity.ToTable("tblControls");

                entity.Property(e => e.FieldName)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.Adjustibility)
                    .IsRequired()
                    .HasMaxLength(1);

                entity.Property(e => e.Usage).HasMaxLength(200);
            });

            modelBuilder.Entity<TblEducations>(entity =>
            {
                entity.HasKey(e => e.EducationId);

                entity.ToTable("tblEducations");

                entity.Property(e => e.EducationId).ValueGeneratedNever();

                entity.Property(e => e.AltLanguage).HasMaxLength(50);

                entity.Property(e => e.Education)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TblExtracts>(entity =>
            {
                entity.HasKey(e => e.ExtractId);

                entity.ToTable("tblExtracts");

                entity.Property(e => e.ExtractId).HasColumnName("ExtractID");

                entity.Property(e => e.AppVersionBackend).HasColumnType("decimal(3, 1)");

                entity.Property(e => e.AuditUserId).HasColumnName("AuditUserID");

                entity.Property(e => e.ExtractDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ExtractFileName).HasMaxLength(255);

                entity.Property(e => e.ExtractFolder).HasMaxLength(255);

                entity.Property(e => e.Hfid).HasColumnName("HFID");

                entity.Property(e => e.LegacyId).HasColumnName("LegacyID");

                entity.Property(e => e.RowId).HasColumnName("RowID");

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblFamilies>(entity =>
            {
                entity.HasKey(e => e.FamilyId);

                entity.ToTable("tblFamilies");

                entity.HasIndex(e => e.InsureeId)
                    .HasName("NCI_tblFamilies_InsureeID");

                entity.HasIndex(e => e.LocationId)
                    .HasName("NCI_tblFamilies_LocationID");

                entity.HasIndex(e => e.ValidityTo)
                    .HasName("NCI_tblFamilies_ValidityTo");

                entity.Property(e => e.FamilyId).HasColumnName("FamilyID");

                entity.Property(e => e.AuditUserId).HasColumnName("AuditUserID");

                entity.Property(e => e.ConfirmationNo).HasMaxLength(12);

                entity.Property(e => e.ConfirmationType).HasMaxLength(3);

                entity.Property(e => e.Ethnicity).HasMaxLength(1);

                entity.Property(e => e.FamilyAddress).HasMaxLength(200);

                entity.Property(e => e.FamilyType).HasMaxLength(2);

                entity.Property(e => e.InsureeId).HasColumnName("InsureeID");

                entity.Property(e => e.IsOffline)
                    .HasColumnName("isOffline")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LegacyId).HasColumnName("LegacyID");

                entity.Property(e => e.Poverty).HasDefaultValueSql("((0))");

                entity.Property(e => e.RowId)
                    .HasColumnName("RowID")
                    .IsRowVersion();

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");

                entity.HasOne(d => d.ConfirmationTypeNavigation)
                    .WithMany(p => p.TblFamilies)
                    .HasForeignKey(d => d.ConfirmationType)
                    .HasConstraintName("FK_tblConfirmationType_tblFamilies");

                entity.HasOne(d => d.FamilyTypeNavigation)
                    .WithMany(p => p.TblFamilies)
                    .HasForeignKey(d => d.FamilyType)
                    .HasConstraintName("FK_tblFamilyTypes_tblFamilies");

                entity.HasOne(d => d.Insuree)
                    .WithMany(p => p.TblFamilies)
                    .HasForeignKey(d => d.InsureeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblFamilies_tblInsuree");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.TblFamilies)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_tblFamilies_tblLocations");
            });

            modelBuilder.Entity<TblFamilySMS>(entity =>
            {
                entity.HasKey(e => e.FamilyId);

                entity.ToTable("tblFamilySMS");
                entity.Property(e => e.FamilyId).HasColumnName("FamilyID");

                entity.Property(e => e.LanguageOfSMS).HasColumnName("LanguageOfSMS")
                    .HasMaxLength(5);

                entity.Property(e => e.ApprovalOfSMS).HasColumnName("ApprovalOfSMS");

                entity.Property(e => e.ValidityFrom).HasColumnType("datetime").HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");

                entity.HasOne(d => d.Family)
                    .WithMany(p => p.TblFamilySMS)
                    .HasForeignKey(d => d.FamilyId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_tblFamilySMS_tblFamily-FamilyID");
            });

            modelBuilder.Entity<TblFamilyTypes>(entity =>
            {
                entity.HasKey(e => e.FamilyTypeCode);

                entity.ToTable("tblFamilyTypes");

                entity.Property(e => e.FamilyTypeCode)
                    .HasMaxLength(2)
                    .ValueGeneratedNever();

                entity.Property(e => e.AltLanguage).HasMaxLength(50);

                entity.Property(e => e.FamilyType)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TblFeedback>(entity =>
            {
                entity.HasKey(e => e.FeedbackId);

                entity.ToTable("tblFeedback");

                entity.Property(e => e.FeedbackId).HasColumnName("FeedbackID");

                entity.Property(e => e.AuditUserId).HasColumnName("AuditUserID");

                entity.Property(e => e.CareRendered).HasDefaultValueSql("((0))");

                entity.Property(e => e.ChfofficerCode).HasColumnName("CHFOfficerCode");

                entity.Property(e => e.ClaimId).HasColumnName("ClaimID");

                entity.Property(e => e.DrugPrescribed).HasDefaultValueSql("((0))");

                entity.Property(e => e.DrugReceived).HasDefaultValueSql("((0))");

                entity.Property(e => e.FeedbackDate).HasColumnType("datetime");

                entity.Property(e => e.LegacyId).HasColumnName("LegacyID");

                entity.Property(e => e.PaymentAsked).HasDefaultValueSql("((0))");

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");

                entity.HasOne(d => d.Claim)
                    .WithMany(p => p.TblFeedback)
                    .HasForeignKey(d => d.ClaimId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblFeedback_tblClaim-ClaimID");
            });

            modelBuilder.Entity<TblFeedbackPrompt>(entity =>
            {
                entity.HasKey(e => e.FeedbackPromptId);

                entity.ToTable("tblFeedbackPrompt");

                entity.Property(e => e.FeedbackPromptId).HasColumnName("FeedbackPromptID");

                entity.Property(e => e.AuditUserId).HasColumnName("AuditUserID");

                entity.Property(e => e.ClaimId).HasColumnName("ClaimID");

                entity.Property(e => e.FeedbackPromptDate).HasColumnType("date");

                entity.Property(e => e.LegacyId).HasColumnName("LegacyID");

                entity.Property(e => e.OfficerId).HasColumnName("OfficerID");

                entity.Property(e => e.PhoneNumber).HasMaxLength(25);

                entity.Property(e => e.Smsstatus)
                    .HasColumnName("SMSStatus")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ValidityFrom).HasColumnType("datetime");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblFromPhone>(entity =>
            {
                entity.HasKey(e => e.FromPhoneId);

                entity.ToTable("tblFromPhone");

                entity.Property(e => e.Chfid)
                    .HasColumnName("CHFID")
                    .HasMaxLength(12);

                entity.Property(e => e.DocName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.DocStatus).HasMaxLength(3);

                entity.Property(e => e.DocType)
                    .IsRequired()
                    .HasMaxLength(3);

                entity.Property(e => e.LandedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OfficerCode).HasMaxLength(8);

                entity.Property(e => e.PhotoSumittedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblGender>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("tblGender");

                entity.Property(e => e.Code)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.AltLanguage).HasMaxLength(50);

                entity.Property(e => e.Gender).HasMaxLength(50);
            });

            modelBuilder.Entity<TblHealthStatus>(entity =>
            {
                entity.HasKey(e => e.HealthStatusId);

                entity.ToTable("tblHealthStatus");

                entity.Property(e => e.HealthStatusId).HasColumnName("HealthStatusID");

                entity.Property(e => e.AuditUserId).HasColumnName("AuditUserID");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.InsureeId).HasColumnName("InsureeID");

                entity.Property(e => e.LegacyId).HasColumnName("LegacyID");

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");

                entity.HasOne(d => d.Insuree)
                    .WithMany(p => p.TblHealthStatus)
                    .HasForeignKey(d => d.InsureeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblHealthStatus_tblInsuree");
            });

            modelBuilder.Entity<TblHf>(entity =>
            {
                entity.HasKey(e => e.HfId);

                entity.ToTable("tblHF");

                entity.Property(e => e.HfId).HasColumnName("HfID");

                entity.Property(e => e.AccCode).HasMaxLength(25);

                entity.Property(e => e.AuditUserId).HasColumnName("AuditUserID");

                entity.Property(e => e.EMail)
                    .HasColumnName("eMail")
                    .HasMaxLength(50);

                entity.Property(e => e.Fax).HasMaxLength(50);

                entity.Property(e => e.Hfaddress)
                    .HasColumnName("HFAddress")
                    .HasMaxLength(100);

                entity.Property(e => e.HfcareType)
                    .IsRequired()
                    .HasColumnName("HFCareType")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Hfcode)
                    .IsRequired()
                    .HasColumnName("HFCode")
                    .HasMaxLength(8);

                entity.Property(e => e.Hflevel)
                    .IsRequired()
                    .HasColumnName("HFLevel")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Hfname)
                    .IsRequired()
                    .HasColumnName("HFName")
                    .HasMaxLength(100);

                entity.Property(e => e.Hfsublevel)
                    .HasColumnName("HFSublevel")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.LegacyId).HasColumnName("LegacyID");

                entity.Property(e => e.LegalForm)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.PlitemId).HasColumnName("PLItemID");

                entity.Property(e => e.PlserviceId).HasColumnName("PLServiceID");

                entity.Property(e => e.RowId)
                    .HasColumnName("RowID")
                    .IsRowVersion();

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");

                entity.HasOne(d => d.HfsublevelNavigation)
                    .WithMany(p => p.TblHf)
                    .HasForeignKey(d => d.Hfsublevel)
                    .HasConstraintName("FK_tblHFSublevel_tblHF");

                entity.HasOne(d => d.LegalFormNavigation)
                    .WithMany(p => p.TblHf)
                    .HasForeignKey(d => d.LegalForm)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblLegalForms_tblHF");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.TblHf)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblHF_tblLocations");

                entity.HasOne(d => d.Plitem)
                    .WithMany(p => p.TblHf)
                    .HasForeignKey(d => d.PlitemId)
                    .HasConstraintName("FK_tblHF_tblPLItems-PLItemID");

                entity.HasOne(d => d.Plservice)
                    .WithMany(p => p.TblHf)
                    .HasForeignKey(d => d.PlserviceId)
                    .HasConstraintName("FK_tblHF_tblPLServices-PLService-ID");
            });

            modelBuilder.Entity<TblHfcatchment>(entity =>
            {
                entity.HasKey(e => e.HfcatchmentId);

                entity.ToTable("tblHFCatchment");

                entity.Property(e => e.HfcatchmentId).HasColumnName("HFCatchmentId");

                entity.Property(e => e.Hfid).HasColumnName("HFID");

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");

                entity.HasOne(d => d.Hf)
                    .WithMany(p => p.TblHfcatchment)
                    .HasForeignKey(d => d.Hfid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblHFCatchment_tbLHF");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.TblHfcatchment)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblHFCatchment_tblLocations");
            });

            modelBuilder.Entity<TblHfsublevel>(entity =>
            {
                entity.HasKey(e => e.Hfsublevel);

                entity.ToTable("tblHFSublevel");

                entity.Property(e => e.Hfsublevel)
                    .HasColumnName("HFSublevel")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.AltLanguage).HasMaxLength(50);

                entity.Property(e => e.HfsublevelDesc)
                    .HasColumnName("HFSublevelDesc")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TblIcdcodes>(entity =>
            {
                entity.HasKey(e => e.Icdid);

                entity.ToTable("tblICDCodes");

                entity.Property(e => e.Icdid).HasColumnName("ICDID");

                entity.Property(e => e.AuditUserId).HasColumnName("AuditUserID");

                entity.Property(e => e.Icdcode)
                    .IsRequired()
                    .HasColumnName("ICDCode")
                    .HasMaxLength(6);

                entity.Property(e => e.Icdname)
                    .IsRequired()
                    .HasColumnName("ICDName")
                    .HasMaxLength(255);

                entity.Property(e => e.LegacyId).HasColumnName("LegacyID");

                entity.Property(e => e.RowId)
                    .HasColumnName("RowID")
                    .IsRowVersion();

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblIdentificationTypes>(entity =>
            {
                entity.HasKey(e => e.IdentificationCode);

                entity.ToTable("tblIdentificationTypes");

                entity.Property(e => e.IdentificationCode)
                    .HasMaxLength(1)
                    .ValueGeneratedNever();

                entity.Property(e => e.AltLanguage).HasMaxLength(50);

                entity.Property(e => e.IdentificationTypes)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TblImisdefaults>(entity =>
            {
                entity.HasKey(e => e.DefaultId);

                entity.ToTable("tblIMISDefaults");

                entity.Property(e => e.DefaultId).HasColumnName("DefaultID");

                entity.Property(e => e.AppVersionBackEnd).HasColumnType("decimal(3, 1)");

                entity.Property(e => e.AppVersionClaim)
                    .HasColumnType("decimal(3, 1)")
                    .HasDefaultValueSql("((1.0))");

                entity.Property(e => e.AppVersionEnquire)
                    .HasColumnType("decimal(3, 1)")
                    .HasDefaultValueSql("((1.0))");

                entity.Property(e => e.AppVersionEnroll)
                    .HasColumnType("decimal(3, 1)")
                    .HasDefaultValueSql("((1.0))");

                entity.Property(e => e.AppVersionFeedback)
                    .HasColumnType("decimal(3, 1)")
                    .HasDefaultValueSql("((1.0))");

                entity.Property(e => e.AppVersionFeedbackRenewal).HasColumnType("decimal(3, 1)");

                entity.Property(e => e.AppVersionImis).HasColumnType("decimal(3, 1)");

                entity.Property(e => e.AppVersionRenewal)
                    .HasColumnType("decimal(3, 1)")
                    .HasDefaultValueSql("((1.0))");

                entity.Property(e => e.AssociatedPhotoFolder).HasMaxLength(255);

                entity.Property(e => e.DatabaseBackupFolder).HasMaxLength(255);

                entity.Property(e => e.FtpclaimFolder)
                    .HasColumnName("FTPClaimFolder")
                    .HasMaxLength(255);

                entity.Property(e => e.FtpenrollmentFolder)
                    .HasColumnName("FTPEnrollmentFolder")
                    .HasMaxLength(255);

                entity.Property(e => e.FtpfeedbackFolder)
                    .HasColumnName("FTPFeedbackFolder")
                    .HasMaxLength(255);

                entity.Property(e => e.Ftphost)
                    .HasColumnName("FTPHost")
                    .HasMaxLength(50);

                entity.Property(e => e.FtpoffLineExtractFolder)
                    .HasColumnName("FTPOffLineExtractFolder")
                    .HasMaxLength(255);

                entity.Property(e => e.Ftppassword)
                    .HasColumnName("FTPPassword")
                    .HasMaxLength(20);

                entity.Property(e => e.FtpphoneExtractFolder)
                    .HasColumnName("FTPPhoneExtractFolder")
                    .HasMaxLength(255);

                entity.Property(e => e.FtppolicyRenewalFolder)
                    .HasColumnName("FTPPolicyRenewalFolder")
                    .HasMaxLength(255);

                entity.Property(e => e.Ftpport)
                    .HasColumnName("FTPPort")
                    .HasDefaultValueSql("((21))");

                entity.Property(e => e.Ftpuser)
                    .HasColumnName("FTPUser")
                    .HasMaxLength(50);

                entity.Property(e => e.OffLineHf)
                    .HasColumnName("OffLineHF")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.OfflineChf)
                    .HasColumnName("OfflineCHF")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PolicyRenewalInterval).HasDefaultValueSql("((14))");

                entity.Property(e => e.Smsdlr).HasColumnName("SMSDlr");

                entity.Property(e => e.Smsip)
                    .HasColumnName("SMSIP")
                    .HasMaxLength(15);

                entity.Property(e => e.Smslink)
                    .HasColumnName("SMSLink")
                    .HasMaxLength(500);

                entity.Property(e => e.Smspassword)
                    .HasColumnName("SMSPassword")
                    .HasMaxLength(50);

                entity.Property(e => e.Smssource)
                    .HasColumnName("SMSSource")
                    .HasMaxLength(15);

                entity.Property(e => e.Smstype).HasColumnName("SMSType");

                entity.Property(e => e.SmsuserName)
                    .HasColumnName("SMSUserName")
                    .HasMaxLength(15);

                entity.Property(e => e.WinRarFolder).HasMaxLength(255);
            });

            modelBuilder.Entity<TblInsuree>(entity =>
            {
                entity.HasKey(e => e.InsureeId);

                entity.ToTable("tblInsuree");

                entity.HasIndex(e => new { e.Chfid, e.ValidityTo })
                    .HasName("IX_tblInsuree_VT-CHFID");

                entity.HasIndex(e => new { e.FamilyId, e.Chfid, e.IsHead, e.ValidityTo })
                    .HasName("IX_tblInsuree-IsHead_VT-Fid-CHF");

                entity.Property(e => e.InsureeId).HasColumnName("InsureeID");

                entity.Property(e => e.AuditUserId).HasColumnName("AuditUserID");

                entity.Property(e => e.Chfid)
                    .HasColumnName("CHFID")
                    .HasMaxLength(12);

                entity.Property(e => e.CurrentAddress).HasMaxLength(200);

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("date");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FamilyId).HasColumnName("FamilyID");

                entity.Property(e => e.Gender)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.GeoLocation).HasMaxLength(250);

                entity.Property(e => e.Hfid).HasColumnName("HFID");

                entity.Property(e => e.IsOffline)
                    .HasColumnName("isOffline")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LegacyId).HasColumnName("LegacyID");

                entity.Property(e => e.Marital)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.OtherNames)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Passport)
                    .HasColumnName("passport")
                    .HasMaxLength(25);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.PhotoDate).HasColumnType("date");

                entity.Property(e => e.PhotoId).HasColumnName("PhotoID");

                entity.Property(e => e.RowId)
                    .HasColumnName("RowID")
                    .IsRowVersion();

                entity.Property(e => e.TypeOfId).HasMaxLength(1);

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");

                entity.HasOne(d => d.EducationNavigation)
                    .WithMany(p => p.TblInsuree)
                    .HasForeignKey(d => d.Education)
                    .HasConstraintName("FK_tblEducations_tblInsuree");

                entity.HasOne(d => d.Family)
                    .WithMany(p => p.TblInsuree)
                    .HasForeignKey(d => d.FamilyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblInsuree_tblFamilies1-FamilyID");

                entity.HasOne(d => d.GenderNavigation)
                    .WithMany(p => p.TblInsuree)
                    .HasForeignKey(d => d.Gender)
                    .HasConstraintName("FK_tblInsuree_tblGender");

                entity.HasOne(d => d.Hf)
                    .WithMany(p => p.TblInsuree)
                    .HasForeignKey(d => d.Hfid)
                    .HasConstraintName("FK_tblInsuree_tblHF");

                entity.HasOne(d => d.Photo)
                    .WithMany(p => p.TblInsuree)
                    .HasForeignKey(d => d.PhotoId)
                    .HasConstraintName("FK_tblInsuree_tblPhotos");

                entity.HasOne(d => d.ProfessionNavigation)
                    .WithMany(p => p.TblInsuree)
                    .HasForeignKey(d => d.Profession)
                    .HasConstraintName("FK_tblProfessions_tblInsuree");

                entity.HasOne(d => d.RelationshipNavigation)
                    .WithMany(p => p.TblInsuree)
                    .HasForeignKey(d => d.Relationship)
                    .HasConstraintName("FK_tblRelations_tblInsuree");

                entity.HasOne(d => d.TypeOf)
                    .WithMany(p => p.TblInsuree)
                    .HasForeignKey(d => d.TypeOfId)
                    .HasConstraintName("FK_tblIdentificationTypes_tblInsuree");
            });

            modelBuilder.Entity<TblInsureePolicy>(entity =>
            {
                entity.HasKey(e => e.InsureePolicyId);

                entity.ToTable("tblInsureePolicy");

                entity.HasIndex(e => e.InsureeId)
                    .HasName("NCI_tblInsureePolicy_InsureeID");

                entity.HasIndex(e => e.PolicyId)
                    .HasName("NCI_tblInsureePolicy_PolicyID");

                entity.HasIndex(e => new { e.EffectiveDate, e.ExpiryDate })
                    .HasName("NCI_tblInsureePolicy_EffDate_Expiry");

                entity.Property(e => e.EffectiveDate).HasColumnType("date");

                entity.Property(e => e.EnrollmentDate).HasColumnType("date");

                entity.Property(e => e.ExpiryDate).HasColumnType("date");

                entity.Property(e => e.IsOffline)
                    .HasColumnName("isOffline")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.RowId).IsRowVersion();

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");

                entity.HasOne(d => d.Insuree)
                    .WithMany(p => p.TblInsureePolicy)
                    .HasForeignKey(d => d.InsureeId)
                    .HasConstraintName("FK_tblInsureePolicy_tblInsuree");

                entity.HasOne(d => d.Policy)
                    .WithMany(p => p.TblInsureePolicy)
                    .HasForeignKey(d => d.PolicyId)
                    .HasConstraintName("FK_tblInsureePolicy_tblPolicy");
            });

            modelBuilder.Entity<TblItems>(entity =>
            {
                entity.HasKey(e => e.ItemId);

                entity.ToTable("tblItems");

                entity.HasIndex(e => new { e.ValidityFrom, e.ValidityTo })
                    .HasName("NCI_tblItems_ValidityFrom_To");

                entity.Property(e => e.ItemId).HasColumnName("ItemID");

                entity.Property(e => e.AuditUserId).HasColumnName("AuditUserID");

                entity.Property(e => e.ItemCareType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ItemCode)
                    .IsRequired()
                    .HasMaxLength(6);

                entity.Property(e => e.ItemName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ItemPackage).HasMaxLength(255);

                entity.Property(e => e.ItemType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.LegacyId).HasColumnName("LegacyID");

                entity.Property(e => e.RowId)
                    .HasColumnName("RowID")
                    .IsRowVersion();

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblLanguages>(entity =>
            {
                entity.HasKey(e => e.LanguageCode);

                entity.ToTable("tblLanguages");

                entity.Property(e => e.LanguageCode)
                    .HasMaxLength(2)
                    .ValueGeneratedNever();

                entity.Property(e => e.LanguageName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TblLegalForms>(entity =>
            {
                entity.HasKey(e => e.LegalFormCode);

                entity.ToTable("tblLegalForms");

                entity.Property(e => e.LegalFormCode)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.AltLanguage).HasMaxLength(50);

                entity.Property(e => e.LegalForms)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TblLocations>(entity =>
            {
                entity.HasKey(e => e.LocationId);

                entity.ToTable("tblLocations");

                entity.HasIndex(e => e.ParentLocationId)
                    .HasName("NCI_tblLocations_ParentLocID");

                entity.HasIndex(e => new { e.ValidityFrom, e.ValidityTo })
                    .HasName("NCI_tblLocations_ValidityFromTo");

                entity.Property(e => e.LocationCode).HasMaxLength(8);

                entity.Property(e => e.LocationName).HasMaxLength(50);

                entity.Property(e => e.LocationType)
                    .IsRequired()
                    .HasMaxLength(1);

                entity.Property(e => e.RowId)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblLogins>(entity =>
            {
                entity.HasKey(e => e.LoginId);

                entity.ToTable("tblLogins");

                entity.Property(e => e.LogTime).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblLogins)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_tblLogins_tblUsers");
            });

            modelBuilder.Entity<TblOfficer>(entity =>
            {
                entity.HasKey(e => e.OfficerId);

                entity.ToTable("tblOfficer");

                entity.HasIndex(e => new { e.Code, e.LastName, e.OtherNames, e.ValidityTo, e.LocationId })
                    .HasName("NCI_tblOfficer_ValidityTo_LocationID");

                entity.Property(e => e.OfficerId).HasColumnName("OfficerID");

                entity.Property(e => e.AuditUserId).HasColumnName("AuditUserID");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(8);

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("date");

                entity.Property(e => e.EmailId).HasMaxLength(200);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LegacyId).HasColumnName("LegacyID");

                entity.Property(e => e.OfficerIdsubst).HasColumnName("OfficerIDSubst");

                entity.Property(e => e.OtherNames)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Permanentaddress)
                    .HasColumnName("permanentaddress")
                    .HasMaxLength(100);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.PhoneCommunication).HasDefaultValueSql("((0))");

                entity.Property(e => e.RowId)
                    .HasColumnName("RowID")
                    .IsRowVersion();

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");

                entity.Property(e => e.Veocode)
                    .HasColumnName("VEOCode")
                    .HasMaxLength(8);

                entity.Property(e => e.Veodob)
                    .HasColumnName("VEODOB")
                    .HasColumnType("date");

                entity.Property(e => e.VeolastName)
                    .HasColumnName("VEOLastName")
                    .HasMaxLength(100);

                entity.Property(e => e.VeootherNames)
                    .HasColumnName("VEOOtherNames")
                    .HasMaxLength(100);

                entity.Property(e => e.Veophone)
                    .HasColumnName("VEOPhone")
                    .HasMaxLength(25);

                entity.Property(e => e.WorksTo).HasColumnType("smalldatetime");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.TblOfficer)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_tblOfficer_tblLocations");

                entity.HasOne(d => d.OfficerIdsubstNavigation)
                    .WithMany(p => p.InverseOfficerIdsubstNavigation)
                    .HasForeignKey(d => d.OfficerIdsubst)
                    .HasConstraintName("FK_tblOfficer_tblOfficer");
            });

            modelBuilder.Entity<TblOfficerVillages>(entity =>
            {
                entity.HasKey(e => e.OfficerVillageId);

                entity.ToTable("tblOfficerVillages");

                entity.Property(e => e.RowId)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.TblOfficerVillages)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_tblOfficerVillages_tblLocations");

                entity.HasOne(d => d.Officer)
                    .WithMany(p => p.TblOfficerVillages)
                    .HasForeignKey(d => d.OfficerId)
                    .HasConstraintName("FK_tblOfficerVillages_tblOfficer");
            });

            modelBuilder.Entity<TblPayer>(entity =>
            {
                entity.HasKey(e => e.PayerId);

                entity.ToTable("tblPayer");

                entity.Property(e => e.PayerId).HasColumnName("PayerID");

                entity.Property(e => e.AuditUserId).HasColumnName("AuditUserID");

                entity.Property(e => e.EMail)
                    .HasColumnName("eMail")
                    .HasMaxLength(50);

                entity.Property(e => e.Fax).HasMaxLength(50);

                entity.Property(e => e.LegacyId).HasColumnName("LegacyID");

                entity.Property(e => e.PayerAddress).HasMaxLength(100);

                entity.Property(e => e.PayerName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PayerType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.RowId)
                    .HasColumnName("RowID")
                    .IsRowVersion();

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.TblPayer)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_tblPayer_tblLocations");

                entity.HasOne(d => d.PayerTypeNavigation)
                    .WithMany(p => p.TblPayer)
                    .HasForeignKey(d => d.PayerType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPayer_tblPayerType");
            });

            modelBuilder.Entity<TblPayerType>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("tblPayerType");

                entity.Property(e => e.Code)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.AltLanguage).HasMaxLength(50);

                entity.Property(e => e.PayerType)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TblPhotos>(entity =>
            {
                entity.HasKey(e => e.PhotoId);

                entity.ToTable("tblPhotos");

                entity.HasIndex(e => new { e.PhotoFileName, e.InsureeId, e.ValidityTo })
                    .HasName("NCI_tblPhotos_InsureeIDValidityTo");

                entity.Property(e => e.PhotoId).HasColumnName("PhotoID");

                entity.Property(e => e.AuditUserId)
                    .HasColumnName("AuditUserID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Chfid)
                    .HasColumnName("CHFID")
                    .HasMaxLength(12);

                entity.Property(e => e.InsureeId).HasColumnName("InsureeID");

                entity.Property(e => e.OfficerId).HasColumnName("OfficerID");

                entity.Property(e => e.PhotoDate).HasColumnType("date");

                entity.Property(e => e.PhotoFileName).HasMaxLength(250);

                entity.Property(e => e.PhotoFolder)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.RowId)
                    .HasColumnName("RowID")
                    .IsRowVersion();

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblPlitems>(entity =>
            {
                entity.HasKey(e => e.PlitemId);

                entity.ToTable("tblPLItems");

                entity.Property(e => e.PlitemId).HasColumnName("PLItemID");

                entity.Property(e => e.AuditUserId).HasColumnName("AuditUserID");

                entity.Property(e => e.DatePl)
                    .HasColumnName("DatePL")
                    .HasColumnType("date");

                entity.Property(e => e.LegacyId).HasColumnName("LegacyID");

                entity.Property(e => e.PlitemName)
                    .IsRequired()
                    .HasColumnName("PLItemName")
                    .HasMaxLength(100);

                entity.Property(e => e.RowId)
                    .HasColumnName("RowID")
                    .IsRowVersion();

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.TblPlitems)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_tblPLItems_tblLocations");
            });

            modelBuilder.Entity<TblPlitemsDetail>(entity =>
            {
                entity.HasKey(e => e.PlitemDetailId);

                entity.ToTable("tblPLItemsDetail");

                entity.Property(e => e.PlitemDetailId).HasColumnName("PLItemDetailID");

                entity.Property(e => e.AuditUserId).HasColumnName("AuditUserID");

                entity.Property(e => e.ItemId).HasColumnName("ItemID");

                entity.Property(e => e.LegacyId).HasColumnName("LegacyID");

                entity.Property(e => e.PlitemId).HasColumnName("PLItemID");

                entity.Property(e => e.RowId)
                    .HasColumnName("RowID")
                    .IsRowVersion();

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.TblPlitemsDetail)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPLItemsDetail_tblItems-ItemID");

                entity.HasOne(d => d.Plitem)
                    .WithMany(p => p.TblPlitemsDetail)
                    .HasForeignKey(d => d.PlitemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPLItemsDetail_tblPLItems-PLItemID");
            });

            modelBuilder.Entity<TblPlservices>(entity =>
            {
                entity.HasKey(e => e.PlserviceId);

                entity.ToTable("tblPLServices");

                entity.Property(e => e.PlserviceId).HasColumnName("PLServiceID");

                entity.Property(e => e.AuditUserId).HasColumnName("AuditUserID");

                entity.Property(e => e.DatePl)
                    .HasColumnName("DatePL")
                    .HasColumnType("date");

                entity.Property(e => e.LegacyId).HasColumnName("LegacyID");

                entity.Property(e => e.PlservName)
                    .IsRequired()
                    .HasColumnName("PLServName")
                    .HasMaxLength(100);

                entity.Property(e => e.RowId)
                    .HasColumnName("RowID")
                    .IsRowVersion();

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.TblPlservices)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_tblPLServices_tblLocations");
            });

            modelBuilder.Entity<TblPlservicesDetail>(entity =>
            {
                entity.HasKey(e => e.PlserviceDetailId);

                entity.ToTable("tblPLServicesDetail");

                entity.Property(e => e.PlserviceDetailId).HasColumnName("PLServiceDetailID");

                entity.Property(e => e.AuditUserId).HasColumnName("AuditUserID");

                entity.Property(e => e.LegacyId).HasColumnName("LegacyID");

                entity.Property(e => e.PlserviceId).HasColumnName("PLServiceID");

                entity.Property(e => e.RowId)
                    .HasColumnName("RowID")
                    .IsRowVersion();

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");

                entity.HasOne(d => d.Plservice)
                    .WithMany(p => p.TblPlservicesDetail)
                    .HasForeignKey(d => d.PlserviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPLServicesDetail_tblPLServices-PLServiceID");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.TblPlservicesDetail)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPLServicesDetail_tblServices-ServiceID");
            });

            modelBuilder.Entity<TblPolicy>(entity =>
            {
                entity.HasKey(e => e.PolicyId);

                entity.ToTable("tblPolicy");

                entity.HasIndex(e => new { e.FamilyId, e.ProdId })
                    .HasName("IX_tblPolicy_FamilyId_ProdId");

                entity.HasIndex(e => new { e.ValidityTo, e.PolicyStatus })
                    .HasName("IX_tblPolicy_ValidityTo");

                entity.HasIndex(e => new { e.ValidityTo, e.EffectiveDate, e.ExpiryDate })
                    .HasName("IX_tblPolicy_Dates");

                entity.HasIndex(e => new { e.ProdId, e.ValidityTo, e.EffectiveDate, e.ExpiryDate })
                    .HasName("IX_tblpolicy_PId_VT_ED_EX");

                entity.Property(e => e.PolicyId).HasColumnName("PolicyID");

                entity.Property(e => e.AuditUserId).HasColumnName("AuditUserID");

                entity.Property(e => e.EffectiveDate).HasColumnType("date");

                entity.Property(e => e.EnrollDate).HasColumnType("date");

                entity.Property(e => e.ExpiryDate).HasColumnType("date");

                entity.Property(e => e.FamilyId).HasColumnName("FamilyID");

                entity.Property(e => e.IsOffline)
                    .HasColumnName("isOffline")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LegacyId).HasColumnName("LegacyID");

                entity.Property(e => e.OfficerId).HasColumnName("OfficerID");

                entity.Property(e => e.PolicyStage)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.PolicyStatus).HasDefaultValueSql("((1))");

                entity.Property(e => e.ProdId).HasColumnName("ProdID");

                entity.Property(e => e.RowId)
                    .HasColumnName("RowID")
                    .IsRowVersion();

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");

                entity.HasOne(d => d.Family)
                    .WithMany(p => p.TblPolicy)
                    .HasForeignKey(d => d.FamilyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPolicy_tblFamilies-FamilyID");

                entity.HasOne(d => d.Officer)
                    .WithMany(p => p.TblPolicy)
                    .HasForeignKey(d => d.OfficerId)
                    .HasConstraintName("FK_tblPolicy_tblOfficer-OfficerID");

                entity.HasOne(d => d.Prod)
                    .WithMany(p => p.TblPolicy)
                    .HasForeignKey(d => d.ProdId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPolicy_tblProduct-ProductID");
            });

            modelBuilder.Entity<TblPolicyRenewalDetails>(entity =>
            {
                entity.HasKey(e => e.RenewalDetailId);

                entity.ToTable("tblPolicyRenewalDetails");

                entity.Property(e => e.RenewalDetailId).HasColumnName("RenewalDetailID");

                entity.Property(e => e.InsureeId).HasColumnName("InsureeID");

                entity.Property(e => e.LegacyId).HasColumnName("LegacyID");

                entity.Property(e => e.RenewalId).HasColumnName("RenewalID");

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");

                entity.HasOne(d => d.Insuree)
                    .WithMany(p => p.TblPolicyRenewalDetails)
                    .HasForeignKey(d => d.InsureeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPolicyRenewalDetails_tblInsuree");

                entity.HasOne(d => d.Renewal)
                    .WithMany(p => p.TblPolicyRenewalDetails)
                    .HasForeignKey(d => d.RenewalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPolicyRenewalDetails_tblPolicyRenewals");
            });

            modelBuilder.Entity<TblPolicyRenewals>(entity =>
            {
                entity.HasKey(e => e.RenewalId);

                entity.ToTable("tblPolicyRenewals");

                entity.Property(e => e.RenewalId).HasColumnName("RenewalID");

                entity.Property(e => e.InsureeId).HasColumnName("InsureeID");

                entity.Property(e => e.LegacyId).HasColumnName("LegacyID");

                entity.Property(e => e.NewOfficerId).HasColumnName("NewOfficerID");

                entity.Property(e => e.NewProdId).HasColumnName("NewProdID");

                entity.Property(e => e.PhoneNumber).HasMaxLength(25);

                entity.Property(e => e.PolicyId).HasColumnName("PolicyID");

                entity.Property(e => e.RenewalDate).HasColumnType("date");

                entity.Property(e => e.RenewalPromptDate).HasColumnType("date");

                entity.Property(e => e.RenewalWarnings).HasDefaultValueSql("((0))");

                entity.Property(e => e.ResponseDate).HasColumnType("datetime");

                entity.Property(e => e.Smsstatus).HasColumnName("SMSStatus");

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");

                entity.HasOne(d => d.Insuree)
                    .WithMany(p => p.TblPolicyRenewals)
                    .HasForeignKey(d => d.InsureeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPolicyRenewals_tblInsuree");

                entity.HasOne(d => d.NewOfficer)
                    .WithMany(p => p.TblPolicyRenewals)
                    .HasForeignKey(d => d.NewOfficerId)
                    .HasConstraintName("FK_tblPolicyRenewals_tblOfficer");

                entity.HasOne(d => d.NewProd)
                    .WithMany(p => p.TblPolicyRenewals)
                    .HasForeignKey(d => d.NewProdId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPolicyRenewals_tblProduct");

                entity.HasOne(d => d.Policy)
                    .WithMany(p => p.TblPolicyRenewals)
                    .HasForeignKey(d => d.PolicyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPolicyRenewals_tblPolicy");
            });

            modelBuilder.Entity<TblPremium>(entity =>
            {
                entity.HasKey(e => e.PremiumId);

                entity.ToTable("tblPremium");

                entity.HasIndex(e => new { e.PolicyId, e.ValidityTo })
                    .HasName("IX_tblPremium_ProdId");

                entity.Property(e => e.AuditUserId).HasColumnName("AuditUserID");

                entity.Property(e => e.IsOffline)
                    .HasColumnName("isOffline")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IsPhotoFee)
                    .HasColumnName("isPhotoFee")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LegacyId).HasColumnName("LegacyID");

                entity.Property(e => e.PayDate).HasColumnType("date");

                entity.Property(e => e.PayType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.PayerId).HasColumnName("PayerID");

                entity.Property(e => e.PolicyId).HasColumnName("PolicyID");

                entity.Property(e => e.Receipt)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.RowId)
                    .HasColumnName("RowID")
                    .IsRowVersion();

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");

                entity.HasOne(d => d.Payer)
                    .WithMany(p => p.TblPremium)
                    .HasForeignKey(d => d.PayerId)
                    .HasConstraintName("FK_tblPremium_tblPayer");

                entity.HasOne(d => d.Policy)
                    .WithMany(p => p.TblPremium)
                    .HasForeignKey(d => d.PolicyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPremium_tblPolicy");
            });

            modelBuilder.Entity<TblProduct>(entity =>
            {
                entity.HasKey(e => e.ProdId);

                entity.ToTable("tblProduct");

                entity.Property(e => e.ProdId).HasColumnName("ProdID");

                entity.Property(e => e.AccCodePremiums).HasMaxLength(25);

                entity.Property(e => e.AccCodeRemuneration).HasMaxLength(25);

                entity.Property(e => e.AuditUserId).HasColumnName("AuditUserID");

                entity.Property(e => e.CeilingInterpretation)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ConversionProdId).HasColumnName("ConversionProdID");

                entity.Property(e => e.DateFrom).HasColumnType("smalldatetime");

                entity.Property(e => e.DateTo).HasColumnType("smalldatetime");

                entity.Property(e => e.DedIpinsuree).HasColumnName("DedIPInsuree");

                entity.Property(e => e.DedIppolicy).HasColumnName("DedIPPolicy");

                entity.Property(e => e.DedIptreatment).HasColumnName("DedIPTreatment");

                entity.Property(e => e.DedOpinsuree).HasColumnName("DedOPInsuree");

                entity.Property(e => e.DedOppolicy).HasColumnName("DedOPPolicy");

                entity.Property(e => e.DedOptreatment).HasColumnName("DedOPTreatment");

                entity.Property(e => e.InsurancePeriod).HasDefaultValueSql("((12))");

                entity.Property(e => e.LegacyId).HasColumnName("LegacyID");

                entity.Property(e => e.Level1)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Level2)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Level3)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Level4)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.MaxCeilingPolicyIp).HasColumnName("MaxCeilingPolicyIP");

                entity.Property(e => e.MaxCeilingPolicyOp).HasColumnName("MaxCeilingPolicyOP");

                entity.Property(e => e.MaxIpinsuree).HasColumnName("MaxIPInsuree");

                entity.Property(e => e.MaxIppolicy).HasColumnName("MaxIPPolicy");

                entity.Property(e => e.MaxIptreatment).HasColumnName("MaxIPTreatment");

                entity.Property(e => e.MaxOpinsuree).HasColumnName("MaxOPInsuree");

                entity.Property(e => e.MaxOppolicy).HasColumnName("MaxOPPolicy");

                entity.Property(e => e.MaxOptreatment).HasColumnName("MaxOPTreatment");

                entity.Property(e => e.MaxPolicyExtraMemberIp).HasColumnName("MaxPolicyExtraMemberIP");

                entity.Property(e => e.MaxPolicyExtraMemberOp).HasColumnName("MaxPolicyExtraMemberOP");

                entity.Property(e => e.PeriodRelPrices)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.PeriodRelPricesIp)
                    .HasColumnName("PeriodRelPricesIP")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.PeriodRelPricesOp)
                    .HasColumnName("PeriodRelPricesOP")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ProductCode)
                    .IsRequired()
                    .HasMaxLength(8);

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.RenewalDiscountPerc).HasDefaultValueSql("((0))");

                entity.Property(e => e.RenewalDiscountPeriod).HasDefaultValueSql("((0))");

                entity.Property(e => e.RowId)
                    .HasColumnName("RowID")
                    .IsRowVersion();

                entity.Property(e => e.ShareContribution)
                    .HasColumnType("decimal(5, 2)")
                    .HasDefaultValueSql("((100.00))");

                entity.Property(e => e.StartCycle1).HasMaxLength(5);

                entity.Property(e => e.StartCycle2).HasMaxLength(5);

                entity.Property(e => e.StartCycle3).HasMaxLength(5);

                entity.Property(e => e.StartCycle4).HasMaxLength(5);

                entity.Property(e => e.Sublevel1)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Sublevel2)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Sublevel3)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Sublevel4)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");

                entity.Property(e => e.WeightAdjustedAmount)
                    .HasColumnType("decimal(5, 2)")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.WeightInsuredPopulation)
                    .HasColumnType("decimal(5, 2)")
                    .HasDefaultValueSql("((100.00))");

                entity.Property(e => e.WeightNumberFamilies)
                    .HasColumnType("decimal(5, 2)")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.WeightNumberInsuredFamilies)
                    .HasColumnType("decimal(5, 2)")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.WeightNumberVisits)
                    .HasColumnType("decimal(5, 2)")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.WeightPopulation)
                    .HasColumnType("decimal(5, 2)")
                    .HasDefaultValueSql("((0.00))");

                entity.HasOne(d => d.CeilingInterpretationNavigation)
                    .WithMany(p => p.TblProduct)
                    .HasForeignKey(d => d.CeilingInterpretation)
                    .HasConstraintName("FK_tblProduct_tblCeilingInterpretation");

                entity.HasOne(d => d.ConversionProd)
                    .WithMany(p => p.InverseConversionProd)
                    .HasForeignKey(d => d.ConversionProdId)
                    .HasConstraintName("FK_tblProduct_tblProduct");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.TblProduct)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_tblProduct_tblLocation");

                entity.HasOne(d => d.Sublevel1Navigation)
                    .WithMany(p => p.TblProductSublevel1Navigation)
                    .HasForeignKey(d => d.Sublevel1)
                    .HasConstraintName("FK_tblHFSublevel_tblProduct_1");

                entity.HasOne(d => d.Sublevel2Navigation)
                    .WithMany(p => p.TblProductSublevel2Navigation)
                    .HasForeignKey(d => d.Sublevel2)
                    .HasConstraintName("FK_tblHFSublevel_tblProduct_2");

                entity.HasOne(d => d.Sublevel3Navigation)
                    .WithMany(p => p.TblProductSublevel3Navigation)
                    .HasForeignKey(d => d.Sublevel3)
                    .HasConstraintName("FK_tblHFSublevel_tblProduct_3");

                entity.HasOne(d => d.Sublevel4Navigation)
                    .WithMany(p => p.TblProductSublevel4Navigation)
                    .HasForeignKey(d => d.Sublevel4)
                    .HasConstraintName("FK_tblHFSublevel_tblProduct_4");
            });

            modelBuilder.Entity<TblProductItems>(entity =>
            {
                entity.HasKey(e => e.ProdItemId);

                entity.ToTable("tblProductItems");

                entity.HasIndex(e => e.ItemId)
                    .HasName("NCI_tblProductItems_ItemID");

                entity.HasIndex(e => new { e.ValidityFrom, e.ValidityTo })
                    .HasName("NCI_tblProductItems_ValidityFromTo");

                entity.Property(e => e.ProdItemId).HasColumnName("ProdItemID");

                entity.Property(e => e.AuditUserId).HasColumnName("AuditUserID");

                entity.Property(e => e.CeilingExclusionAdult).HasMaxLength(1);

                entity.Property(e => e.CeilingExclusionChild).HasMaxLength(1);

                entity.Property(e => e.ItemId).HasColumnName("ItemID");

                entity.Property(e => e.LegacyId).HasColumnName("LegacyID");

                entity.Property(e => e.LimitationType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.LimitationTypeE)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.LimitationTypeR)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.PriceOrigin)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ProdId).HasColumnName("ProdID");

                entity.Property(e => e.RowId)
                    .HasColumnName("RowID")
                    .IsRowVersion();

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.TblProductItems)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblProductItems_tblItems-ItemID");

                entity.HasOne(d => d.Prod)
                    .WithMany(p => p.TblProductItems)
                    .HasForeignKey(d => d.ProdId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblProductItems_tblProduct-ProductID");
            });

            modelBuilder.Entity<TblProductServices>(entity =>
            {
                entity.HasKey(e => e.ProdServiceId);

                entity.ToTable("tblProductServices");

                entity.HasIndex(e => e.ServiceId)
                    .HasName("NCI_tblProductServices_ServiceID");

                entity.HasIndex(e => new { e.ValidityFrom, e.ValidityTo })
                    .HasName("NCI_tblProductServices_ValidityFromTo");

                entity.Property(e => e.ProdServiceId).HasColumnName("ProdServiceID");

                entity.Property(e => e.AuditUserId).HasColumnName("AuditUserID");

                entity.Property(e => e.CeilingExclusionAdult).HasMaxLength(1);

                entity.Property(e => e.CeilingExclusionChild).HasMaxLength(1);

                entity.Property(e => e.LegacyId).HasColumnName("LegacyID");

                entity.Property(e => e.LimitationType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.LimitationTypeE)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.LimitationTypeR)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.PriceOrigin)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ProdId).HasColumnName("ProdID");

                entity.Property(e => e.RowId)
                    .HasColumnName("RowID")
                    .IsRowVersion();

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");

                entity.HasOne(d => d.Prod)
                    .WithMany(p => p.TblProductServices)
                    .HasForeignKey(d => d.ProdId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblProductServices_tblProduct-ProductID");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.TblProductServices)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblProductServices_tblServices-ServiceID");
            });

            modelBuilder.Entity<TblProfessions>(entity =>
            {
                entity.HasKey(e => e.ProfessionId);

                entity.ToTable("tblProfessions");

                entity.Property(e => e.ProfessionId).ValueGeneratedNever();

                entity.Property(e => e.AltLanguage).HasMaxLength(50);

                entity.Property(e => e.Profession)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TblRelations>(entity =>
            {
                entity.HasKey(e => e.RelationId);

                entity.ToTable("tblRelations");

                entity.Property(e => e.RelationId).ValueGeneratedNever();

                entity.Property(e => e.AltLanguage).HasMaxLength(50);

                entity.Property(e => e.Relation)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TblRelDistr>(entity =>
            {
                entity.HasKey(e => e.DistrId);

                entity.ToTable("tblRelDistr");

                entity.Property(e => e.DistrId).HasColumnName("DistrID");

                entity.Property(e => e.AuditUserId).HasColumnName("AuditUserID");

                entity.Property(e => e.DistrCareType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.LegacyId).HasColumnName("LegacyID");

                entity.Property(e => e.ProdId).HasColumnName("ProdID");

                entity.Property(e => e.RowId)
                    .HasColumnName("RowID")
                    .IsRowVersion();

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");

                entity.HasOne(d => d.Prod)
                    .WithMany(p => p.TblRelDistr)
                    .HasForeignKey(d => d.ProdId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRelDistr_tblProduct");
            });

            modelBuilder.Entity<TblRelIndex>(entity =>
            {
                entity.HasKey(e => e.RelIndexId);

                entity.ToTable("tblRelIndex");

                entity.Property(e => e.RelIndexId).HasColumnName("RelIndexID");

                entity.Property(e => e.AuditUserId).HasColumnName("AuditUserID");

                entity.Property(e => e.CalcDate).HasColumnType("datetime");

                entity.Property(e => e.LegacyId).HasColumnName("LegacyID");

                entity.Property(e => e.ProdId).HasColumnName("ProdID");

                entity.Property(e => e.RelCareType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.RelIndex).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");

                entity.HasOne(d => d.Prod)
                    .WithMany(p => p.TblRelIndex)
                    .HasForeignKey(d => d.ProdId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRelIndex_tblProduct");
            });

            modelBuilder.Entity<TblReporting>(entity =>
            {
                entity.HasKey(e => e.ReportingId);

                entity.ToTable("tblReporting");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.ReportingDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("date");
            });

            modelBuilder.Entity<TblServices>(entity =>
            {
                entity.HasKey(e => e.ServiceId);

                entity.ToTable("tblServices");

                entity.HasIndex(e => new { e.ValidityFrom, e.ValidityTo })
                    .HasName("NCI_tblServices_ValidityFromTo");

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.Property(e => e.AuditUserId).HasColumnName("AuditUserID");

                entity.Property(e => e.LegacyId).HasColumnName("LegacyID");

                entity.Property(e => e.RowId)
                    .HasColumnName("RowID")
                    .IsRowVersion();

                entity.Property(e => e.ServCareType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ServCategory)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ServCode)
                    .IsRequired()
                    .HasMaxLength(6);

                entity.Property(e => e.ServLevel)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ServName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ServType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblSubmittedPhotos>(entity =>
            {
                entity.HasKey(e => e.PhotoId);

                entity.ToTable("tblSubmittedPhotos");

                entity.HasIndex(e => e.Chfid)
                    .HasName("NCI_tblSubmittedPhotos_CHFID");

                entity.HasIndex(e => e.OfficerCode)
                    .HasName("NCI_tblSubmittedPhotos_OfficerID");

                entity.Property(e => e.Chfid)
                    .HasColumnName("CHFID")
                    .HasMaxLength(12);

                entity.Property(e => e.ImageName).HasMaxLength(50);

                entity.Property(e => e.OfficerCode).HasMaxLength(8);

                entity.Property(e => e.PhotoDate).HasColumnType("date");

                entity.Property(e => e.RegisterDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<TblUsers>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("tblUsers");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.AuditUserId).HasColumnName("AuditUserID");

                entity.Property(e => e.DummyPwd).HasMaxLength(25);

                entity.Property(e => e.EmailId).HasMaxLength(200);

                entity.Property(e => e.Hfid).HasColumnName("HFID");

                entity.Property(e => e.LanguageId)
                    .IsRequired()
                    .HasColumnName("LanguageID")
                    .HasMaxLength(2);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LegacyId).HasColumnName("LegacyID");

                entity.Property(e => e.LoginName)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.OtherNames)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(256);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.PrivateKey).HasMaxLength(50);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.TblUsers)
                    .HasForeignKey(d => d.LanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblLanguages_tblUsers");
            });

            modelBuilder.Entity<TblUsersDistricts>(entity =>
            {
                entity.HasKey(e => e.UserDistrictId);

                entity.ToTable("tblUsersDistricts");

                entity.Property(e => e.UserDistrictId).HasColumnName("UserDistrictID");

                entity.Property(e => e.AuditUserId).HasColumnName("AuditUserID");

                entity.Property(e => e.LegacyId).HasColumnName("LegacyID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.ValidityFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidityTo).HasColumnType("datetime");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.TblUsersDistricts)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblUsersDistricts_tblLocations");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblUsersDistricts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblUsersDistricts_tblUsers");
            });

            modelBuilder.Entity<TblRoleRight>(entity =>
            {
                entity.HasKey(e => new {e.RoleRightID, e.RoleID });
            });

            modelBuilder.Entity<TblUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserID, e.RoleID });
                
                        
            });

            modelBuilder.Entity<TblIMISDefaultsPhone>(entity =>
            {
                entity.HasKey(e => e.RuleName);
            });

            modelBuilder.Entity<TblVillages>(entity =>
            {
                entity.HasKey(e => e.VillageId);
            });

            modelBuilder.Entity<TblWards>(entity =>
            {
                entity.HasKey(e => e.WardId);
            });

            modelBuilder.Entity<TblDistricts>(entity =>
            {
                entity.HasKey(e => e.DistrictId);
            });
        }
    }
}