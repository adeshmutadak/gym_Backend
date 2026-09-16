using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace gym_application.Models;

public partial class GymDbContext : DbContext
{
    public GymDbContext(DbContextOptions<GymDbContext> options)
    : base(options)
    {
    }

    public virtual DbSet<Announcement> Announcements { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Exercise> Exercises { get; set; }

    public virtual DbSet<Food> Foods { get; set; }

    public virtual DbSet<Gymsubscription> Gymsubscriptions { get; set; }

    public virtual DbSet<Trainer> Trainers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Announcement>(entity =>
        {
            entity.HasKey(e => e.AnnouncementId)
                .HasName("PRIMARY");

            entity.ToTable("announcements");

            entity.Property(e => e.AnnouncementId)
               // .ValueGeneratedNever()
                .HasColumnName("announcement_id");

            entity.Property(e => e.Audience)
                .HasMaxLength(20)
                .HasColumnName("audience");

            entity.Property(e => e.Body)
                .HasMaxLength(1000)
                .HasColumnName("body");

            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");

            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");

            entity.Property(e => e.Title)
                .HasMaxLength(140)
                .HasColumnName("title");

            entity.Property(e => e.VisibleFrom)
                .HasColumnType("datetime")
                .HasColumnName("visible_from");

            entity.Property(e => e.VisibleUntil)
                .HasColumnType("datetime")
                .HasColumnName("visible_until");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.ClientId)
                .HasName("PRIMARY");

            entity.ToTable("clients");

            entity.HasIndex(e => e.ClientCode, "client_code")
                .IsUnique();

            entity.HasIndex(e => e.SubscriptionId, "subscription_id");

            entity.HasIndex(e => e.TrainerId, "trainer_id");

            entity.HasIndex(e => e.UserId, "user_id")
                .IsUnique();

            entity.Property(e => e.ClientId)
               // .ValueGeneratedNever()
                .HasColumnName("client_id");

            entity.Property(e => e.ActivityLevel)
                .HasMaxLength(30)
                .HasColumnName("activity_level");

            entity.Property(e => e.AssignedOn)
                .HasColumnName("assigned_on");

            entity.Property(e => e.ClientCode)
                .HasMaxLength(20)
                .HasColumnName("client_code");

            entity.Property(e => e.DurationDays)
                .HasColumnName("duration_days");

            entity.Property(e => e.EmergencyContact)
                .HasMaxLength(15)
                .HasColumnName("emergency_contact");

            entity.Property(e => e.EndDate)
                .HasColumnName("end_date");

            entity.Property(e => e.Goal)
                .HasMaxLength(200)
                .HasColumnName("goal");

            entity.Property(e => e.HeightCm)
                .HasPrecision(5, 1)
                .HasColumnName("height_cm");

            entity.Property(e => e.IncludesPt)
                .HasColumnName("includes_pt");

            entity.Property(e => e.JoiningDate)
                .HasColumnName("joining_date");

            entity.Property(e => e.MedicalNotes)
                .HasMaxLength(300)
                .HasColumnName("medical_notes");

            entity.Property(e => e.MembershipStatus)
                .HasMaxLength(20)
                .HasColumnName("membership_status");

            entity.Property(e => e.PlanName)
                .HasMaxLength(80)
                .HasColumnName("plan_name");

            entity.Property(e => e.PlanType)
                .HasMaxLength(20)
                .HasColumnName("plan_type");

            entity.Property(e => e.PricePaid)
                .HasPrecision(10, 2)
                .HasColumnName("price_paid");

            entity.Property(e => e.StartDate)
                .HasColumnName("start_date");

            entity.Property(e => e.StartingWeightKg)
                .HasPrecision(5, 2)
                .HasColumnName("starting_weight_kg");

            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");

            entity.Property(e => e.SubscriptionId)
                .HasColumnName("subscription_id");

            entity.Property(e => e.TargetWeightKg)
                .HasPrecision(5, 2)
                .HasColumnName("target_weight_kg");

            entity.Property(e => e.TrainerId)
                .HasColumnName("trainer_id");

            entity.Property(e => e.UserId)
                .HasColumnName("user_id");

            entity.HasOne(d => d.Subscription)
                .WithMany(p => p.Clients)
                .HasForeignKey(d => d.SubscriptionId)
                .HasConstraintName("clients_ibfk_2");

            entity.HasOne(d => d.Trainer)
                .WithMany(p => p.Clients)
                .HasForeignKey(d => d.TrainerId)
                .HasConstraintName("clients_ibfk_3");

            entity.HasOne(d => d.User)
                .WithOne(p => p.Client)
                .HasForeignKey<Client>(d => d.UserId)
                .HasConstraintName("clients_ibfk_1");
        });

        modelBuilder.Entity<Exercise>(entity =>
        {
            entity.HasKey(e => e.ExerciseId)
                .HasName("PRIMARY");

            entity.ToTable("exercises");

            entity.Property(e => e.ExerciseId)
               // .ValueGeneratedNever()
                .HasColumnName("exercise_id");

            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");

            entity.Property(e => e.DefaultReps)
                .HasMaxLength(20)
                .HasColumnName("default_reps");

            entity.Property(e => e.DefaultSets)
                .HasColumnName("default_sets");

            entity.Property(e => e.Equipment)
                .HasMaxLength(50)
                .HasColumnName("equipment");

            entity.Property(e => e.ExerciseName)
                .HasMaxLength(100)
                .HasColumnName("exercise_name");

            entity.Property(e => e.Instructions)
                .HasMaxLength(500)
                .HasColumnName("instructions");

            entity.Property(e => e.MuscleGroup)
                .HasMaxLength(30)
                .HasColumnName("muscle_group");

            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");

            entity.Property(e => e.VideoUrl)
                .HasMaxLength(255)
                .HasColumnName("video_url");
        });

        modelBuilder.Entity<Food>(entity =>
        {
            entity.HasKey(e => e.FoodId)
                .HasName("PRIMARY");

            entity.ToTable("foods");

            entity.Property(e => e.FoodId)
               // .ValueGeneratedNever()
                .HasColumnName("food_id");

            entity.Property(e => e.CarbsG)
                .HasPrecision(6, 2)
                .HasColumnName("carbs_g");

            entity.Property(e => e.Category)
                .HasMaxLength(30)
                .HasColumnName("category");

            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");

            entity.Property(e => e.FatG)
                .HasPrecision(6, 2)
                .HasColumnName("fat_g");

            entity.Property(e => e.FoodName)
                .HasMaxLength(100)
                .HasColumnName("food_name");

            entity.Property(e => e.Kcal)
                .HasPrecision(7, 2)
                .HasColumnName("kcal");

            entity.Property(e => e.ProteinG)
                .HasPrecision(6, 2)
                .HasColumnName("protein_g");

            entity.Property(e => e.ServingSize)
                .HasPrecision(7, 2)
                .HasColumnName("serving_size");

            entity.Property(e => e.ServingUnit)
                .HasMaxLength(10)
                .HasColumnName("serving_unit");

            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");
        });

        modelBuilder.Entity<Gymsubscription>(entity =>
        {
            entity.HasKey(e => e.SubscriptionId)
                .HasName("PRIMARY");

            entity.ToTable("gymsubscriptions");

            entity.Property(e => e.SubscriptionId)
              //  .ValueGeneratedNever()
                .HasColumnName("subscription_id");

            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");

            entity.Property(e => e.Description)
                .HasMaxLength(300)
                .HasColumnName("description");

            entity.Property(e => e.DurationDays)
                .HasColumnName("duration_days");

            entity.Property(e => e.IncludesPt)
                .HasColumnName("includes_pt");

            entity.Property(e => e.PlanName)
                .HasMaxLength(80)
                .HasColumnName("plan_name");

            entity.Property(e => e.PlanType)
                .HasMaxLength(20)
                .HasColumnName("plan_type");

            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");

            entity.Property(e => e.PtSessionsWeek)
                .HasColumnName("pt_sessions_week");

            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");
        });

        modelBuilder.Entity<Trainer>(entity =>
        {
            entity.HasKey(e => e.TrainerId)
                .HasName("PRIMARY");

            entity.ToTable("trainers");

            entity.HasIndex(e => e.UserId, "user_id")
                .IsUnique();

            entity.Property(e => e.TrainerId)
             //   .ValueGeneratedNever()
                .HasColumnName("trainer_id");

            entity.Property(e => e.Bio)
                .HasMaxLength(400)
                .HasColumnName("bio");

            entity.Property(e => e.Certification)
                .HasMaxLength(80)
                .HasColumnName("certification");

            entity.Property(e => e.Discipline)
                .HasMaxLength(80)
                .HasColumnName("discipline");

            entity.Property(e => e.ExperienceYears)
                .HasColumnName("experience_years");

            entity.Property(e => e.JoiningDate)
                .HasColumnName("joining_date");

            entity.Property(e => e.MaxClients)
                .HasColumnName("max_clients");

            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");

            entity.Property(e => e.TrainerCode)
                .HasMaxLength(10)
                .HasColumnName("trainer_code");

            entity.Property(e => e.UserId)
                .HasColumnName("user_id");

            entity.HasOne(d => d.User)
                .WithOne(p => p.Trainer)
                .HasForeignKey<Trainer>(d => d.UserId)
                .HasConstraintName("trainers_ibfk_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId)
                .HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "email")
                .IsUnique();

            entity.Property(e => e.UserId)
             //   .ValueGeneratedNever()
                .HasColumnName("user_id");

            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");

            entity.Property(e => e.DateOfBirth)
                .HasColumnName("date_of_birth");

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");

            entity.Property(e => e.Gender)
                .HasMaxLength(20)
                .HasColumnName("gender");

            entity.Property(e => e.MustResetPassword)
                .HasColumnName("must_reset_password");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");

            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");

            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .HasColumnName("phone");

            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasColumnName("role");

            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


}