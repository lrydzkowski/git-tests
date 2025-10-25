# GitTests

A simple ASP.NET Core Web API project for testing GitHub Actions workflows.

## Development Workflow

This project uses a GitFlow-based branching strategy with automated CI/CD pipelines supporting both normal feature development and emergency hotfixes.

### Branching Strategy

- **`master`**: Production-ready code that triggers the full deployment pipeline
- **`develop`**: Integration branch where features are merged before release
- **`build-X/hotfix/Y`**: Emergency hotfix branches (e.g., `build-31/hotfix/1`)

### Normal Feature Development

1. **Create Feature Branch**: Start from `develop` and create a feature branch
2. **Development**: Implement your feature with appropriate testing
3. **Pull Request**: Create PR from feature branch to `develop`
4. **Code Review**: Team reviews and approves changes
5. **Merge to Develop**: Feature is integrated with other features
6. **Release**: When ready, create PR from `develop` to `master`

### Release Pipeline

When code is merged to `master`, the automated pipeline:

1. **Builds** the .NET application and creates Docker images
2. **Tags** with unique version numbers (handles conflicts automatically)
3. **Triggers** deployment to all environments (demo, test, production)
4. **Requires Manual Approval** for each environment deployment

**Important**: Merging to master does NOT automatically deploy to production. Each environment requires manual approval through GitHub's environment protection.

### Environment Approval Process

1. Navigate to **GitHub Actions** → Select the workflow run
2. Click **Review deployments** 
3. Select the environment(s) to deploy
4. Click **Approve and deploy**

### Hotfix Workflow

For critical production issues requiring immediate fixes:

1. **Create Hotfix Branch**: Branch from `master` using naming convention `build-X/hotfix/Y`
2. **Implement Fix**: Make minimal changes to address the critical issue
3. **Deploy**: Use the hotfix pipeline which skips demo and goes directly to test/prod
4. **Manual Trigger**: Go to Actions → "01-02 Build Hotfix Image" → Run workflow
5. **Approve Deployments**: Approve test and production deployments
6. **Sync Branches**: After deployment, merge hotfix back to `develop`

### Environment Deployment Order

- **Demo**: Safe environment for showcasing and validating new features
- **Test**: Pre-production environment that mirrors production setup
- **Production**: Live environment serving end users

### Version Tagging System

The pipeline uses an intelligent tagging system that automatically handles conflicts:

#### Tag Generation Process

1. **Base Tag Creation**:
   - Normal builds: `build-{run_number}` (e.g., `build-42`)
   - Hotfix builds: Branch name with slashes converted to dashes (e.g., `build-31/hotfix/1` → `build-31-hotfix-1`)

2. **Conflict Resolution**: If a tag already exists in the registry, the system automatically increments:
   - `build-42` exists → try `build-42-1`
   - `build-42-1` exists → try `build-42-2`
   - Continue until unique tag is found

3. **Docker Image Tagging**: Each build creates two Docker image tags:
   - `latest` - Always points to the most recent successful build
   - `{unique_version_tag}` - The specific version (e.g., `build-42-2`)

4. **Git Tag Synchronization**: A corresponding Git tag is created to match the Docker image version for full traceability

This ensures every build has a unique, traceable version identifier across both Git and the container registry.

### Pipeline Features

- **Container Registry**: Images stored in GitHub Container Registry
- **Webhook Deployments**: Uses external deployment system via HTTP endpoints
- **Environment Protection**: GitHub environments ensure manual approval before deployment
- **Concurrency Control**: Prevents multiple builds on the same branch

### Troubleshooting

**Build Failures**: Check the Actions tab for detailed error logs. Common issues include compilation errors or Docker build problems.

**Deployment Issues**: Verify webhook endpoints are accessible and API keys are properly configured in GitHub Secrets.

**Emergency Rollback**: Manually trigger production deployment with a previous version tag through the deployment workflow.

### Best Practices

- Keep feature branches focused and short-lived
- Write meaningful commit messages
- Test changes locally before creating pull requests
- Use hotfixes only for critical production issues
- Always sync `develop` branch after hotfix deployments
- Communicate with the team about hotfix deployments
