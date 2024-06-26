﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MqttDashboard.Data;

#nullable disable

namespace MqttDashboard.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240626193529_inital")]
    partial class inital
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.6");

            modelBuilder.Entity("MqttDashboard.Models.MqttClientModel", b =>
                {
                    b.Property<Guid>("ClientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("DeviceName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("IpAddress")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("KeepAlive")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastAccessed")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SubscribedTopics")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ClientId");

                    b.ToTable("MqttClients");

                    b.HasData(
                        new
                        {
                            ClientId = new Guid("ed0a80cf-8ef9-4ec4-8b65-d5abee91bf73"),
                            Created = new DateTime(2023, 5, 12, 14, 30, 0, 0, DateTimeKind.Unspecified),
                            DeviceName = "Device Alpha",
                            IpAddress = "192.168.1.1",
                            KeepAlive = 60,
                            LastAccessed = new DateTime(2024, 6, 24, 9, 45, 0, 0, DateTimeKind.Unspecified),
                            Status = true,
                            SubscribedTopics = "[\"topic1\",\"topic2\"]"
                        },
                        new
                        {
                            ClientId = new Guid("d68b6951-3f69-439f-a859-199142adf975"),
                            Created = new DateTime(2023, 6, 11, 11, 20, 0, 0, DateTimeKind.Unspecified),
                            DeviceName = "Device Beta",
                            IpAddress = "192.168.1.2",
                            KeepAlive = 60,
                            LastAccessed = new DateTime(2024, 6, 23, 10, 15, 0, 0, DateTimeKind.Unspecified),
                            Status = false,
                            SubscribedTopics = "[\"topic3\",\"topic4\"]"
                        },
                        new
                        {
                            ClientId = new Guid("84f18f36-9edc-4df5-bb8a-8ebe18adfb74"),
                            Created = new DateTime(2023, 7, 21, 9, 45, 0, 0, DateTimeKind.Unspecified),
                            DeviceName = "Device Gamma",
                            IpAddress = "192.168.1.3",
                            KeepAlive = 60,
                            LastAccessed = new DateTime(2024, 6, 22, 16, 30, 0, 0, DateTimeKind.Unspecified),
                            Status = true,
                            SubscribedTopics = "[\"topic5\",\"topic6\"]"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
