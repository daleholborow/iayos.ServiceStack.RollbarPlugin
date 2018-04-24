
## Files sourced and adapted to ServiceStack IRequestLogger with all due respect and acknowledgements

All files in this directory are originally sourced from the open sourced Seq Logger repo code, located at:

- https://github.com/wwwlicious/servicestack-seq-requestlogsfeature/tree/master/src/ServiceStack.Seq.RequestLogsFeatureRollbar

These were then slowly trimmed, massaged and adapted to 

- renamed variable to be Rollbar-ish, not Seq-ish
- removed some files that were not relevant to Rollbar
- patching in Session details to bespoke Person object in Rollbar payloads
- other cosmetic tidying and refactoring.
