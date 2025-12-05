# Task 049-implement-github-actions-cicd

## Summary
Set up GitHub Actions CI/CD pipeline for automated building, testing, and publishing TimeWarp.Flexbox NuGet packages to GitHub Packages (private) and optionally nuget.org (public releases).

## Todo List
- [ ] Update `source/timewarp-flexbox/timewarp-flexbox.csproj` with NuGet package metadata
  - Set `IsPackable` to true
  - Add `PackageId`, `Authors`, `Company` properties
  - Add `RepositoryUrl`, `PackageProjectUrl`, `RepositoryType`
  - Add `PackageLicenseExpression` (MIT)
  - Add `PackageReadmeFile` pointing to readme.md
  - Add ItemGroup to include readme.md in package
- [ ] Create `.github/workflows/ci.yml` for PR and main branch builds
  - Trigger on push to main/master and pull requests
  - Add path filters for source, test, and config files
  - Setup .NET 10
  - Restore, build, test steps
  - Pack validation step (no publish)
- [ ] Create `.github/workflows/publish-nuget.yml` for NuGet publishing
  - Trigger on version tags (v*) and workflow_dispatch
  - Trigger on GitHub releases
  - Setup permissions (contents: read, packages: write)
  - Build and test steps
  - Version extraction from tag or input
  - Pack with version override
  - Publish to GitHub Packages using GITHUB_TOKEN
  - Optional: Publish to nuget.org for stable releases (requires NUGET_API_KEY secret)
  - Upload artifacts
- [ ] Create `runfiles/check-version.cs` to verify package version availability (follow timewarp-nuru pattern)
- [ ] Add version check step to publish workflow
- [ ] Test CI workflow with a pull request
- [ ] Test publish workflow with a test tag or manual dispatch
- [ ] Document release process in repository

## Notes

### Reference Implementation
Based on analysis from `.agents/workspace/2025-12-06T10-00-00_github-nuget-cicd-setup.md` and patterns from TimeWarp.Nuru repository.

### TimeWarp.Nuru CI/CD Patterns to Follow
- Single `ci-cd.yml` workflow with conditional steps (alternative to separate files)
- Path filters to avoid unnecessary builds
- .NET 10 runfiles for build scripts (`scripts/build.cs`)
- Version check script before publishing
- `--skip-duplicate` flag for NuGet push
- Artifact upload with run number
- Conditional test skip on release events

### Key Configuration Details

**GitHub Packages URL**: `https://nuget.pkg.github.com/TimeWarpEngineering/index.json`

**Required Permissions**:
```yaml
permissions:
  contents: read
  packages: write
```

**Authentication** (using GITHUB_TOKEN, no secrets needed):
```yaml
dotnet nuget add source \
  --username ${{ github.actor }} \
  --password ${{ secrets.GITHUB_TOKEN }} \
  --store-password-in-clear-text \
  --name github \
  "https://nuget.pkg.github.com/TimeWarpEngineering/index.json"
```

**Path Filters** (recommended):
```yaml
paths:
  - 'source/**'
  - 'test/**'
  - 'benchmarks/**'
  - '.github/workflows/**'
  - 'Directory.Build.props'
  - 'Directory.Packages.props'
```

### Versioning Strategy Options
1. **Git Tags (Manual)**: `git tag v1.0.0 && git push origin v1.0.0`
2. **MinVer (Automated)**: Derives version from git tags + commit height
3. **GitHub Releases**: Create release in UI, triggers publish

### Secrets Required
- `GITHUB_TOKEN`: Automatic, no setup needed for GitHub Packages
- `NUGET_API_KEY`: Only needed if publishing to nuget.org (create in repository settings)

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
