# ContentLib - A Lethal Company API for Custom Content

> [!NOTE]  
> This project is still a WIP!

ContentLib is a modular Custom Content API for Lethal Company.

## Contributing

If you are a developer, consider contributing to ContentLib!  
ContentLib aims to be a community-driven project, and needs contributors and maintainers to ensure this project lives on.

### About the Project

This project has been built to be modular and easily extensible, and aims to be very accessible for new contributors. New modules are generally built on top of the [ContentLib.Core](/src/ContentLib.Core/) module, which provides all the important base classes like [ContentDefinition](/src/ContentLib.Core/ContentDefinition.cs). An excellent example of a module implementation is [ContentLib.EnemyAPI](/src/ContentLib.EnemyAPI/).

### Working on the Project

You can look for open [issues](https://github.com/LC-ContentLib/LC-ContentLib/issues) that you could take on.

When you are ready to work on an issue:
- leave a message on the issue to let others that know you are working on it.
- fork this repository.
- create a new branch on your fork for the issue.
- when you are ready, open a pull request.

Your pull request will be reviewed and feedback will be given. When everything is fine, your work will be merged into ContentLib!

See these resources on code style and naming conventions to help write better code:

- https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions
- https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names

We enforce some coding style rules based on the above resources. These rules are configured in the [.editorconfig](/.editorconfig) file. These will show up as warnings or suggestions in your IDE, and they exist to ensure that code style is mostly consistent across all modules.

### Implementing New Modules

The first part of implementing a new module is planning. If you wish for a module to be made, open an [issue](https://github.com/LC-ContentLib/LC-ContentLib/issues) on it so we can start discussing it.

The next part is writing an API design document for the module. This is a document that describes what the public API for this module should expose and how it should be used.

Once the API design document has been approved, it will be moved to [issues](https://github.com/LC-ContentLib/LC-ContentLib/issues) so it can be picked up and implemented. Consider using an existing module implementation as reference. For example: [ContentLib.EnemyAPI](/src/ContentLib.EnemyAPI/).