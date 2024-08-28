# Contributing Guidelines

Thank you for your interest in contributing to ContentLib! ContentLib aims to be a community-driven project, and needs contributors and maintainers to ensure this project lives on.

## Working on the Project

You can look for open [issues](https://github.com/LC-ContentLib/ContentLib/issues) that you could take on.

When you are ready to work on an issue:

- leave a message on the issue to let others that know you are working on it.
- fork this repository.
- create a new branch on your fork for the issue.
- when you are ready, open a pull request.

Your pull request will be reviewed and feedback will be given. When everything is fine, your work will be merged into ContentLib!

Make sure to read the rest of the guidelines on this page, as that will help make the review process go smoother. But don't worry about it too much, the most important thing is your contribution!

## Project Design

ContentLib has been built to be modular, easily extensible, and simple.

Each API module should only do one thing, and modules should generally work independent of each other to keep things simple.

However, each API module should probably depend on the `ContentLib.Core` module as it contains all the essential building blocks, like [ContentDefinition](https://github.com/LC-ContentLib/ContentLib/blob/main/src/ContentLib.Core/ContentDefinition.cs).

## Coding Style

We follow and enforce the following coding conventions:

- <https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions>
- <https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names>

These rules are enforced with the help of the [.editorconfig](https://github.com/LC-ContentLib/ContentLib/blob/main/.editorconfig) file. These will show up as warnings or suggestions in your IDE, and they exist to ensure that code style is mostly consistent across all modules.

### Clear Code

You should aim to write code that is easy to understand and follow.

For example, a `Register` method for a content class that derives from [ContentDefinition](https://github.com/LC-ContentLib/ContentLib/blob/main/src/ContentLib.Core/ContentDefinition.cs) should make it clear where the internal logic for injecting said content is in the API module.

Here's an example of [EnemyDefinition's](https://github.com/LC-ContentLib/ContentLib/blob/main/src/ContentLib.EnemyAPI/EnemyDefinition.cs) `Register` method:

```cs
public override void Register()
{
    // ...
    EnemyDefinitionInjector.Register(this);
    // ...
}
```

This makes it quite obvious where the injection logic is.

### No Magic

By magic, we mean things like Harmony's Patch Attribute system. This is bad because how it works can't simply be figured out by reading the code; you must read the Harmony documentation to know its rules. If you wish to use Harmony, please use it with manual patching.

[MonoMod](https://lethal.wiki/dev/fundamentals/patching-code#monomod) is an alternative tool for patching, which we prefer thanks to its HookGen's helper assemblies that contain Hooks for the game's methods. Using these assemblies also gives your IDE the ability to know if your Hook methods will work or not.

## Implementing New Modules

The first part of implementing a new API module is planning. If you wish for a module to be made, open an [issue](https://github.com/LC-ContentLib/ContentLib/issues) on it so we can start discussing it.

The next part is writing an API design document for the module. This is a document that describes what the public API for this module should expose and how it should be used. Internal behavior can also be in this design document, but it should be made clear what is internal and what is public.

Once the API design document has been approved, it will be moved to [issues](https://github.com/LC-ContentLib/ContentLib/issues) so it can be picked up and implemented. You should use an existing module implementation as reference, like [ContentLib.EnemyAPI](https://github.com/LC-ContentLib/ContentLib/tree/main/src/ContentLib.EnemyAPI).
