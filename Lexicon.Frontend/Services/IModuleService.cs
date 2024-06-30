﻿using Lexicon.Frontend.Models;

namespace Lexicon.Frontend.Services;

public interface IModuleService
{
    Task<IEnumerable<Module>> GetModulesAsync();
    Task<Module> GetModuleAsync(int id);
    Task<bool> PutModuleAsync(int id, Module module);
}
