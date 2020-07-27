# Refactor Much

Refactor Much is a folder comparison utility for people who refactor much :D
Current version: 0.1a

- Detects files that where moved in the directory tree (left local path differs from right local path)
- Detects changed files
- Detects files that are similar in content (and therefore may be a refactor)
- Detects duplicates on left and right
- Context sensitive menu for removing, moving, diffing or viewing files (depending on match type)

## INSTALL AND RUN

- Clone this repo
- Open in Visual Studio 2019 and Build
- Copy [sample-config.json]() to `%LOCALAPPDATA%\RefactorMuch\RefactorMuch\1.0.0.0\app-config.json`
- Edit app-config.json to setup diff and view tools as in the example **REQUIRED**
- Run the executable
  > **Note:** The application won't work unless configuration is proper... settings and verifying configuration is not implemented yet.

## FUTURE

There is still much to do, this is still more an idea than an application, however, it helped me already to detect some refactored code in two very distinct lines of work in the same repository, each with their own refactors over the main branch.
