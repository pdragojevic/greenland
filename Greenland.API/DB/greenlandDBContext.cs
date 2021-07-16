using System;
using Greenland.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Greenland.API.DB
{
    public partial class greenlandDBContext : DbContext
    {
        public greenlandDBContext()
        {
        }

        public greenlandDBContext(DbContextOptions<greenlandDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ActivityFeed> ActivityFeed { get; set; }
        public virtual DbSet<CompanyPosition> CompanyPosition { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<ForgotPassword> ForgotPassword { get; set; }
        public virtual DbSet<Task> Task { get; set; }
        public virtual DbSet<TaskEmployee> TaskEmployee { get; set; }
        public virtual DbSet<TaskTeam> TaskTeam { get; set; }
        public virtual DbSet<Team> Team { get; set; }
        public virtual DbSet<WorkingGroup> WorkingGroup { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=greenlandDB;Trusted_Connection=True;");
                //optionsBuilder.UseSqlServer("Server=tcp:greenlandserver.database.windows.net,1433;Initial Catalog=greenlandDBv2;Persist Security Info=False;User ID=GreenlandAgile;Password=agileGreenland123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActivityFeed>(entity =>
            {
                entity.HasKey(e => e.IdFeed)
                    .HasName("PK__Activity__1E08F7523F520A8A");

                entity.ToTable("Activity_Feed");

                entity.Property(e => e.IdFeed).HasColumnName("ID_feed");

                entity.Property(e => e.CreatedTime)
                    .HasColumnName("createdTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdEmployee).HasColumnName("ID_employee");

                entity.Property(e => e.Message)
                    .HasColumnName("message")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CompanyPosition>(entity =>
            {
                entity.HasKey(e => e.IdCompanyPosition)
                    .HasName("PK__Company___A03CE3F390C0B13F");

                entity.ToTable("Company_Position");

                entity.Property(e => e.IdCompanyPosition).HasColumnName("ID_company_position");

                entity.Property(e => e.NameCompanyPosition)
                    .IsRequired()
                    .HasColumnName("name_company_position")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Employees>(entity =>
            {
                entity.HasKey(e => e.IdEmployee)
                    .HasName("PK__Employee__D9EE4F361F58C6BC");

                entity.HasIndex(e => e.Username)
                    .HasName("UQ__Employee__F3DBC5722D4D6E2E")
                    .IsUnique();

                entity.Property(e => e.IdEmployee).HasColumnName("ID_employee");

                entity.Property(e => e.BirthDate)
                    .HasColumnName("birth_date")
                    .HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasColumnName("gender")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.HireDate)
                    .HasColumnName("hire_date")
                    .HasColumnType("date");

                entity.Property(e => e.IdCompanyPosition).HasColumnName("ID_company_position");

                entity.Property(e => e.IdTeam).HasColumnName("ID_team");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdCompanyPositionNavigation)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.IdCompanyPosition)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Company_Position");

                entity.HasOne(d => d.IdTeamNavigation)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.IdTeam)
                    .HasConstraintName("FK_ID_team");
            });

            modelBuilder.Entity<ForgotPassword>(entity =>
            {
                entity.HasKey(e => e.Guid)
                    .HasName("PK__forgot_p__497F6CB483460ABF");

                entity.ToTable("Forgot_Password");

                entity.Property(e => e.Guid)
                    .HasColumnName("guid")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.IdEmployee).HasColumnName("idEmployee");

                entity.Property(e => e.TimeCreated)
                    .HasColumnName("timeCreated")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.IdEmployeeNavigation)
                    .WithMany(p => p.ForgotPassword)
                    .HasForeignKey(d => d.IdEmployee)
                    .HasConstraintName("FK__forgot_pa__idEmp__1EA48E88");
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.HasKey(e => e.IdTask)
                    .HasName("PK__Task__E75142BB6DFAB09D");

                entity.Property(e => e.IdTask).HasColumnName("ID_task");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("creation_date")
                    .HasColumnType("date");

                entity.Property(e => e.Deadline)
                    .HasColumnName("deadline")
                    .HasColumnType("date");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .IsUnicode(false);

                entity.Property(e => e.Priority).HasColumnName("priority");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .IsUnicode(false)
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Summary)
                    .IsRequired()
                    .HasColumnName("summary")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TaskEmployee>(entity =>
            {
                entity.HasKey(e => e.IdTask)
                    .HasName("PK_Task_Employee_1");

                entity.ToTable("Task_Employee");

                entity.Property(e => e.IdTask)
                    .HasColumnName("ID_task")
                    .ValueGeneratedNever();

                entity.Property(e => e.IdEmployee).HasColumnName("ID_employee");

                entity.HasOne(d => d.IdEmployeeNavigation)
                    .WithMany(p => p.TaskEmployee)
                    .HasForeignKey(d => d.IdEmployee)
                    .HasConstraintName("FK__Task_Empl__ID_em__3587F3E0");
            });

            modelBuilder.Entity<TaskTeam>(entity =>
            {
                entity.HasKey(e => e.IdTask)
                    .HasName("PK_Task_Team_1");

                entity.ToTable("Task_Team");

                entity.Property(e => e.IdTask)
                    .HasColumnName("ID_task")
                    .ValueGeneratedNever();

                entity.Property(e => e.IdTeam).HasColumnName("ID_team");

                entity.HasOne(d => d.IdTeamNavigation)
                    .WithMany(p => p.TaskTeam)
                    .HasForeignKey(d => d.IdTeam)
                    .HasConstraintName("FK__Task_Team__ID_te__32AB8735");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(e => e.IdTeam)
                    .HasName("PK__Team__E45806398DBDFDFD");

                entity.HasIndex(e => e.TeamName)
                    .HasName("UQ__Team__29E35E0C49A1F119")
                    .IsUnique();

                entity.Property(e => e.IdTeam).HasColumnName("ID_team");

                entity.Property(e => e.IdTeamLeader).HasColumnName("ID_team_leader");

                entity.Property(e => e.IdWorkingGroup).HasColumnName("ID_working_group");

                entity.Property(e => e.TeamName)
                    .IsRequired()
                    .HasColumnName("team_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdTeamLeaderNavigation)
                    .WithMany(p => p.Team)
                    .HasForeignKey(d => d.IdTeamLeader)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ID_team_leader");

                entity.HasOne(d => d.IdWorkingGroupNavigation)
                    .WithMany(p => p.Team)
                    .HasForeignKey(d => d.IdWorkingGroup)
                    .HasConstraintName("FK_ID_working_group");
            });

            modelBuilder.Entity<WorkingGroup>(entity =>
            {
                entity.HasKey(e => e.IdWorkingGroup)
                    .HasName("PK__Working___AADBB61FAB648246");

                entity.ToTable("Working_Group");

                entity.Property(e => e.IdWorkingGroup).HasColumnName("ID_working_group");

                entity.Property(e => e.IdCoordinator).HasColumnName("ID_coordinator");

                entity.Property(e => e.WorkingGroupName)
                    .IsRequired()
                    .HasColumnName("working_group_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdCoordinatorNavigation)
                    .WithMany(p => p.WorkingGroup)
                    .HasForeignKey(d => d.IdCoordinator)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ID_coordinator");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
