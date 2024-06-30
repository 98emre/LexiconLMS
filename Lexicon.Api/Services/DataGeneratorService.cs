﻿using Bogus;
using Lexicon.Api.Entities;
using Lexicon.Api.Repositories;

namespace Lexicon.Api.Services;

public class DataGeneratorService(IUnitOfWork unitOfWork)
{
    public async Task GenerateDataAsync()
    {
        if (await unitOfWork.Users.CountAsync() == 0)
        {
            await GenerateUsersAsync();
            await GenerateCoursesAsync();
            await GenerateModulesAsync();
            await GenerateActivitiesAsync();
        }
    }

    private async Task GenerateModulesAsync()
    {
        foreach (var course in await unitOfWork.Courses.GetAllAsync())
        {
            for (var i = 0; i < 4; i++)
            {
                var module = new Faker<Module>("sv")
                    .RuleFor(m => m.Course, f => course)
                    .RuleFor(m => m.Name, f => f.Commerce.ProductName())
                    .RuleFor(m => m.Description, f => f.Commerce.ProductDescription())
                    .RuleFor(m => m.StartDate, f => f.Date.Past())
                    .RuleFor(m => m.EndDate, f => f.Date.Future())
                    // .RuleFor(m => m.Documents, f => faker.PickRandom(documents, 2).ToList())
                    .Generate();
                unitOfWork.Modules.Add(module);
            }
        }

        await unitOfWork.SaveAsync();
    }

    private async Task GenerateActivitiesAsync()
    {
        foreach (var module in await unitOfWork.Modules.GetAllAsync())
        {
            for (var i = 0; i < 4; i++)
            {
                var activity = new Faker<Activity>("sv")
                    .RuleFor(a => a.Module, f => module)
                    .RuleFor(a => a.Name, f => f.Name.JobTitle())
                    .RuleFor(a => a.Description, f => f.Name.JobDescriptor())
                    .RuleFor(a => a.Type, f => f.PickRandom<ActivityType>())
                    .RuleFor(a => a.StartDate, f => f.Date.Past())
                    .RuleFor(a => a.EndDate, f => f.Date.Future())
                    // .RuleFor(a => a.Documents, f => faker.PickRandom(documents, 1).ToList())
                    .Generate();
                unitOfWork.Activities.Add(activity);
            }
        }

        await unitOfWork.SaveAsync();
    }

    private async Task GenerateCoursesAsync()
    {
        var faker = new Faker("sv");
        unitOfWork.Courses.Add(new Course
        {
            Name = ".net 2024",
            Description = faker.Commerce.ProductDescription(),
            Users = faker.PickRandom(await unitOfWork.Users.GetAllAsync(), 5).ToList(),
            StartDate = faker.Date.Past(),
            EndDate = faker.Date.Future(),
            // Documents = null
        });

        await unitOfWork.SaveAsync();
    }

    private async Task GenerateUsersAsync()
    {
        // Create generic teacher and student for testing/demo purposes
        unitOfWork.Users.Add(new User
        {
            FirstName = "Sven",
            LastName = "Lärarsson",
            Email = "teacher@example.com",
            Password = "teacher",
            Role = UserRole.Teacher,
        });
        unitOfWork.Users.Add(new User
        {
            FirstName = "Anders",
            LastName = "Studentsson",
            Email = "student@example.com",
            Password = "student",
            Role = UserRole.Student,
        });

        // Create 2 random teachers
        for (var i = 0; i < 2; i++)
        {
            var user = new Faker<User>("sv")
                .RuleFor(u => u.FirstName, f => f.Person.FirstName)
                .RuleFor(u => u.LastName, f => f.Person.LastName)
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(u => u.Password, f => f.Internet.Password())
                .RuleFor(u => u.Role, f => UserRole.Teacher)
                .Generate();

            unitOfWork.Users.Add(user);
        }

        // Create 10 random students
        for (int i = 0; i < 10; i++)
        {
            var user = new Faker<User>(locale: "sv")
                .RuleFor(u => u.FirstName, f => f.Person.FirstName)
                .RuleFor(u => u.LastName, f => f.Person.LastName)
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(u => u.Password, f => f.Internet.Password())
                .RuleFor(u => u.Role, f => UserRole.Student)
                .Generate();

            unitOfWork.Users.Add(user);
        }

        await unitOfWork.SaveAsync();
    }
}