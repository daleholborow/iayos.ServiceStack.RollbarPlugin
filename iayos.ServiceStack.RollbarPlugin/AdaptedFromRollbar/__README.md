
## Files sourced and adapted to ServiceStack IRequestLogger with all due respect and acknowledgements

All files in this directory are originally sourced from the open sourced Rollbar repo code, located at:

- https://github.com/rollbar/Rollbar.NET/tree/master/Rollbar

And slowly massaged and adapted to 

- remove NewtonSoft.Json attributes and code
- strip out most the Rollbar library entire and only make use of its DTO and utility classes
- remove all dependencies on the Rollbar nuget package itself
- Other cosmetic tidying and refactoring, including:
-- renaming Rolbar Exception dto to not class with Exception class for example)
