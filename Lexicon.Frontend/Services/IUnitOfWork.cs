﻿using Lexicon.Frontend.ServicesImp;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Lexicon.Frontend.Services;

public interface IUnitOfWork
{
    IUserService UserService { get; }
    IModuleService ModuleService { get; }
    IActivityService ActivityService { get; }
    IDocumentService DocumentService { get; }
    IAuthService AuthService { get; }
    ICourseService CourseService { get; }
    IFileService FileService { get; }
    ISessionStorageService SessionStorageService { get; }
}
