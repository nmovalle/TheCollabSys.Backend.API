﻿namespace TheCollabSys.Backend.API.Configuration;

public interface IServiceInstaller
{
    void Install(IServiceCollection services, IConfiguration configuration);
}
