# Deployment Guide

This document outlines the deployment and release process for the Embedly .NET SDK.

## 🚀 Release Workflows

### 1. Continuous Integration (CI)

**Trigger**: Push to `main` or `develop` branches, pull requests
**File**: `.github/workflows/ci.yml`

**What it does**:
- Builds the SDK and examples
- Runs unit and integration tests
- Performs security scans
- Generates code coverage reports
- Creates NuGet packages for testing

### 2. Prerelease Deployment

**Trigger**: Push to `develop` branch or manual workflow dispatch
**File**: `.github/workflows/prerelease.yml`

**What it does**:
- Automatically calculates next version (e.g., `1.2.3-beta.20241201+abc123`)
- Builds and tests the SDK
- Publishes prerelease to NuGet.org
- Notifies team of new prerelease availability

### 3. Production Release

**Trigger**: Push git tag with format `v*` (e.g., `v1.0.0`)
**File**: `.github/workflows/release.yml`

**What it does**:
- Validates version format and builds
- Runs comprehensive tests including security scans
- Publishes stable package to NuGet.org
- Creates GitHub release with changelog
- Announces release

## 📦 Release Process

### Creating a Prerelease

Prereleases are automatically created from the `develop` branch:

```bash
# Make changes and commit to develop
git checkout develop
git add .
git commit -m "Add new feature"
git push origin develop

# A prerelease will be automatically created
# Example version: 1.2.3-beta.20241201+abc123
```

### Creating a Stable Release

1. **Ensure all changes are in `develop`**:
   ```bash
   git checkout develop
   git pull origin develop
   ```

2. **Merge to main**:
   ```bash
   git checkout main
   git pull origin main
   git merge develop
   git push origin main
   ```

3. **Create and push release tag**:
   ```bash
   # For a patch release (1.0.0 -> 1.0.1)
   git tag v1.0.1
   
   # For a minor release (1.0.0 -> 1.1.0)
   git tag v1.1.0
   
   # For a major release (1.0.0 -> 2.0.0)
   git tag v2.0.0
   
   # Push the tag to trigger release
   git push origin v1.0.1
   ```

4. **Monitor the release**:
   - Check GitHub Actions for successful deployment
   - Verify package appears on [NuGet.org](https://www.nuget.org/packages/Embedly.SDK)
   - Review generated release notes on GitHub

## 🔧 Required Secrets

Configure these secrets in GitHub repository settings:

### Production Secrets

| Secret | Description | Environment |
|--------|-------------|-------------|
| `NUGET_API_KEY` | NuGet.org API key for publishing | `nuget-production` |
| `EMBEDLY_STAGING_API_KEY` | Embedly staging API key for integration tests | Repository |
| `EMBEDLY_STAGING_ORG_ID` | Embedly staging organization ID | Repository |

### Setting up NuGet API Key

1. Go to [NuGet.org](https://www.nuget.org/)
2. Sign in and go to Account Settings → API Keys
3. Create new API key with:
   - **Key Name**: `Embedly-SDK-Release`
   - **Package Owner**: Your account
   - **Scopes**: `Push new packages and package versions`
   - **Packages**: `Embedly.SDK`
4. Copy the generated key to GitHub Secrets

### Setting up GitHub Environments

1. Go to repository Settings → Environments
2. Create environments:
   - `nuget-production`: For stable releases
   - `nuget-prerelease`: For prerelease packages

3. Configure protection rules:
   - **Required reviewers**: Team leads for production
   - **Deployment branches**: `main` for production, `develop` for prerelease

## 📋 Version Strategy

### Semantic Versioning

We follow [Semantic Versioning (SemVer)](https://semver.org/):

- **MAJOR** (`2.0.0`): Breaking changes
- **MINOR** (`1.1.0`): New features, backward compatible
- **PATCH** (`1.0.1`): Bug fixes, backward compatible

### Prerelease Versions

Format: `MAJOR.MINOR.PATCH-beta.TIMESTAMP+COMMIT`

Example: `1.2.3-beta.20241201+abc123`

- **beta**: Prerelease identifier
- **TIMESTAMP**: `YYYYMMDDHHMM` format
- **COMMIT**: Short git commit hash

### Release Branches

- `main`: Stable, production-ready code
- `develop`: Integration branch for new features
- `feature/*`: Feature development branches

## 🔍 Quality Gates

### Automated Checks

All releases must pass:

1. **Build Validation**: All projects build successfully
2. **Unit Tests**: 100% of unit tests pass
3. **Integration Tests**: Integration tests pass (when credentials available)
4. **Security Scan**: No security vulnerabilities detected
5. **Code Coverage**: Minimum coverage thresholds met

### Manual Validation

Before creating release tags:

1. **Feature Testing**: All new features tested manually
2. **Documentation**: README and docs updated
3. **Examples**: Example code verified and working
4. **Breaking Changes**: Breaking changes documented and justified

## 📊 Monitoring Releases

### GitHub Actions Dashboard

Monitor deployments at: `https://github.com/[owner]/[repo]/actions`

Key metrics to watch:
- Build success rate
- Test pass rate
- Deployment frequency
- Time to deployment

### NuGet Package Health

Monitor package at: `https://www.nuget.org/packages/Embedly.SDK`

Track:
- Download statistics
- Version adoption
- User feedback and issues

### Release Notifications

Teams are notified via:
- GitHub release notifications
- Workflow run summaries
- Package publish confirmations

## 🚨 Rollback Procedures

### Rolling Back a Release

If a release has critical issues:

1. **Immediate**: Deprecate the problematic version on NuGet
2. **Quick Fix**: Create hotfix branch and emergency release
3. **Communication**: Notify users via GitHub release notes

```bash
# Emergency hotfix process
git checkout main
git checkout -b hotfix/v1.0.2
# Fix the issue
git commit -m "hotfix: critical issue"
git push origin hotfix/v1.0.2
# Create PR to main, then tag v1.0.2
```

### Package Deprecation

To deprecate a NuGet package version:

```bash
# Using .NET CLI
dotnet nuget delete Embedly.SDK 1.0.1 --api-key [API-KEY] --source https://api.nuget.org/v3/index.json

# Or mark as deprecated with reason
# (Requires direct NuGet.org interface)
```

## 📈 Release Metrics

### Success Criteria

- ✅ All automated tests pass
- ✅ No security vulnerabilities
- ✅ Package published successfully
- ✅ Documentation updated
- ✅ Examples working
- ✅ GitHub release created

### Key Performance Indicators

- **Lead Time**: Time from commit to production
- **Deployment Frequency**: How often we release
- **Mean Time to Recovery**: Time to fix issues
- **Change Failure Rate**: Percentage of releases causing issues

## 🔧 Troubleshooting

### Common Issues

**Build Failures**:
- Check .NET version compatibility
- Verify all dependencies are available
- Review recent code changes

**Test Failures**:
- Check if API credentials are configured
- Verify test environment connectivity
- Review failing test logs

**NuGet Publish Failures**:
- Verify API key is valid and has permissions
- Check if version already exists
- Ensure package format is correct

**Release Creation Failures**:
- Verify GitHub token permissions
- Check tag format is correct
- Ensure release notes are valid

### Getting Help

1. **Check Workflow Logs**: GitHub Actions provides detailed logs
2. **Review Documentation**: This guide and README
3. **Contact Team**: Reach out to maintainers
4. **Create Issue**: Document problems for team review

## 📚 Additional Resources

- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [NuGet Package Publishing](https://docs.microsoft.com/en-us/nuget/nuget-org/publish-a-package)
- [Semantic Versioning Guide](https://semver.org/)
- [Embedly API Documentation](https://docs.embedly.ng)