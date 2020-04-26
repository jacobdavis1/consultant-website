﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using Microsoft.Extensions.Configuration;

namespace consultant_server.Database
{
    public partial class khbatlzvContext : DbContext
    {
        private IConfiguration _configuration;

        public khbatlzvContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public khbatlzvContext(DbContextOptions<khbatlzvContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Appointments> Appointments { get; set; }
        public virtual DbSet<Caseclient> Caseclient { get; set; }
        public virtual DbSet<Casenotes> Casenotes { get; set; }
        public virtual DbSet<Cases> Cases { get; set; }
        public virtual DbSet<Clients> Clients { get; set; }
        public virtual DbSet<PgStatStatements> PgStatStatements { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_configuration["Database:ConnectionString"]);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("btree_gin")
                .HasPostgresExtension("btree_gist")
                .HasPostgresExtension("citext")
                .HasPostgresExtension("cube")
                .HasPostgresExtension("dblink")
                .HasPostgresExtension("dict_int")
                .HasPostgresExtension("dict_xsyn")
                .HasPostgresExtension("earthdistance")
                .HasPostgresExtension("fuzzystrmatch")
                .HasPostgresExtension("hstore")
                .HasPostgresExtension("intarray")
                .HasPostgresExtension("ltree")
                .HasPostgresExtension("pg_stat_statements")
                .HasPostgresExtension("pg_trgm")
                .HasPostgresExtension("pgcrypto")
                .HasPostgresExtension("pgrowlocks")
                .HasPostgresExtension("pgstattuple")
                .HasPostgresExtension("tablefunc")
                .HasPostgresExtension("unaccent")
                .HasPostgresExtension("uuid-ossp")
                .HasPostgresExtension("xml2");

            modelBuilder.Entity<Appointments>(entity =>
            {
                entity.HasKey(e => e.Appointmentid)
                    .HasName("appointments_pkey");

                entity.ToTable("appointments");

                entity.Property(e => e.Appointmentid)
                    .HasColumnName("appointmentid")
                    .HasMaxLength(40);

                entity.Property(e => e.Appointmentdatetime)
                    .HasColumnName("appointmentdatetime")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.Appointmenttitle)
                    .HasColumnName("appointmenttitle")
                    .HasMaxLength(100);

                entity.Property(e => e.Caseid)
                    .HasColumnName("caseid")
                    .HasMaxLength(40);

                entity.HasOne(d => d.Case)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.Caseid)
                    .HasConstraintName("appointments_caseid_fkey");
            });

            modelBuilder.Entity<Caseclient>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("caseclient");

                entity.Property(e => e.Caseid)
                    .HasColumnName("caseid")
                    .HasMaxLength(40);

                entity.Property(e => e.Clientid)
                    .HasColumnName("clientid")
                    .HasMaxLength(40);

                entity.HasOne(d => d.Case)
                    .WithMany()
                    .HasForeignKey(d => d.Caseid)
                    .HasConstraintName("caseclient_caseid_fkey");

                entity.HasOne(d => d.Client)
                    .WithMany()
                    .HasForeignKey(d => d.Clientid)
                    .HasConstraintName("caseclient_clientid_fkey");
            });

            modelBuilder.Entity<Casenotes>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("casenotes");

                entity.Property(e => e.Caseid)
                    .HasColumnName("caseid")
                    .HasMaxLength(40);

                entity.Property(e => e.Noteid)
                    .HasColumnName("noteid")
                    .HasMaxLength(40);

                entity.HasOne(d => d.Case)
                    .WithMany()
                    .HasForeignKey(d => d.Caseid)
                    .HasConstraintName("casenotes_caseid_fkey");
            });

            modelBuilder.Entity<Cases>(entity =>
            {
                entity.HasKey(e => e.Caseid)
                    .HasName("cases_pkey");

                entity.ToTable("cases");

                entity.Property(e => e.Caseid)
                    .HasColumnName("caseid")
                    .HasMaxLength(40);

                entity.Property(e => e.Casetitle)
                    .HasColumnName("casetitle")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Clients>(entity =>
            {
                entity.HasKey(e => e.Clientid)
                    .HasName("clients_pkey");

                entity.ToTable("clients");

                entity.Property(e => e.Clientid)
                    .HasColumnName("clientid")
                    .HasMaxLength(40);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(100);

                entity.Property(e => e.Firstname)
                    .HasColumnName("firstname")
                    .HasMaxLength(100);

                entity.Property(e => e.Lastname)
                    .HasColumnName("lastname")
                    .HasMaxLength(100);

                entity.Property(e => e.Middlename)
                    .HasColumnName("middlename")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<PgStatStatements>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("pg_stat_statements");

                entity.Property(e => e.BlkReadTime).HasColumnName("blk_read_time");

                entity.Property(e => e.BlkWriteTime).HasColumnName("blk_write_time");

                entity.Property(e => e.Calls).HasColumnName("calls");

                entity.Property(e => e.Dbid)
                    .HasColumnName("dbid")
                    .HasColumnType("oid");

                entity.Property(e => e.LocalBlksDirtied).HasColumnName("local_blks_dirtied");

                entity.Property(e => e.LocalBlksHit).HasColumnName("local_blks_hit");

                entity.Property(e => e.LocalBlksRead).HasColumnName("local_blks_read");

                entity.Property(e => e.LocalBlksWritten).HasColumnName("local_blks_written");

                entity.Property(e => e.MaxTime).HasColumnName("max_time");

                entity.Property(e => e.MeanTime).HasColumnName("mean_time");

                entity.Property(e => e.MinTime).HasColumnName("min_time");

                entity.Property(e => e.Query).HasColumnName("query");

                entity.Property(e => e.Queryid).HasColumnName("queryid");

                entity.Property(e => e.Rows).HasColumnName("rows");

                entity.Property(e => e.SharedBlksDirtied).HasColumnName("shared_blks_dirtied");

                entity.Property(e => e.SharedBlksHit).HasColumnName("shared_blks_hit");

                entity.Property(e => e.SharedBlksRead).HasColumnName("shared_blks_read");

                entity.Property(e => e.SharedBlksWritten).HasColumnName("shared_blks_written");

                entity.Property(e => e.StddevTime).HasColumnName("stddev_time");

                entity.Property(e => e.TempBlksRead).HasColumnName("temp_blks_read");

                entity.Property(e => e.TempBlksWritten).HasColumnName("temp_blks_written");

                entity.Property(e => e.TotalTime).HasColumnName("total_time");

                entity.Property(e => e.Userid)
                    .HasColumnName("userid")
                    .HasColumnType("oid");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
