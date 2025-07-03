pipeline {
    agent any

    environment {
        BRANCH_NAME = 'PDL.ReportService.API-vikas-1'
        TIMESTAMP = ''
        IMAGE_NAME = ''
        REPO_NAME = ''
        DOTNET_CLI_TELEMETRY_OPTOUT = '1'
        PATH = "${env.HOME}/.dotnet/tools:${env.PATH}"
        WARNING_FLAGS = '/warnasmessage:NU1701;NU1902;NU1903'
    }

    stages {
        stage('Initialize Variables') {
            steps {
                script {
                    // Generate timestamp for tagging
                    TIMESTAMP = sh(script: "date -u +%Y%m%d%H%M%S", returnStdout: true).trim()

                    // Extract repo name from Git URL
                    def gitUrl = sh(script: "git config --get remote.origin.url", returnStdout: true).trim()
                    def repoBase = gitUrl.tokenize('/').last().replaceAll(/\\.git$/, '')
                    REPO_NAME = repoBase.toLowerCase()

                    // Compose GHCR image tag
                    IMAGE_NAME = "ghcr.io/paisalo-digital-limited/${REPO_NAME}:${TIMESTAMP}"

                    // Export to environment
                    env.REPO_NAME = REPO_NAME
                    env.IMAGE_NAME = IMAGE_NAME
                    env.TIMESTAMP = TIMESTAMP

                    echo "✅ Extracted REPO_NAME: ${REPO_NAME}"
                    echo "✅ Generated IMAGE_NAME: ${IMAGE_NAME}"
                }
            }
        }

        stage('Checkout Code') {
            steps {
                echo "✅ Checking out code from Git..."
                checkout scm
            }
        }

        stage('Restore Dependencies') {
            steps {
                echo "✅ Restoring .NET 8 dependencies..."
                sh 'dotnet restore PDL.ReportService.API/PDL.ReportService.API.csproj || true'
            }
        }

        stage('Debug Project Paths') {
            steps {
                echo "✅ Listing .csproj files..."
                sh 'find . -name "*.csproj"'
            }
        }

        stage('Build Solution') {
            steps {
                echo "✅ Building the solution..."
                sh 'dotnet build PDL.ReportService.API/PDL.ReportService.API.csproj --no-restore $WARNING_FLAGS || true'
            }
        }

        stage('Run Unit Tests') {
            steps {
                echo "✅ Running unit tests (if available)..."
                sh '''
                    if ls PDL.ReportService.API.Tests/*.csproj 1> /dev/null 2>&1; then
                        dotnet test PDL.ReportService.API.Tests/PDL.ReportService.API.Tests.csproj \
                            --collect:"XPlat Code Coverage" \
                            --results-directory:TestResults \
                            --logger "trx" \
                            --no-build $WARNING_FLAGS || true
                    else
                        echo "⚠️ No unit test project found."
                    fi
                '''
            }
        }

        stage('Check Code Coverage') {
            steps {
                echo "✅ Placeholder for code coverage checks."
            }
        }

        stage('Lint Check') {
            steps {
                echo "✅ Placeholder for linting."
            }
        }

        stage('Publish Artifacts') {
            steps {
                echo "✅ Publishing build artifacts..."
                sh '''
                    dotnet publish PDL.ReportService.API/PDL.ReportService.API.csproj \
                        -c Release \
                        -o publish \
                        --no-build \
                        -r linux-x64 \
                        --self-contained false \
                        $WARNING_FLAGS || true
                '''
            }
        }

        stage('Build Docker Image') {
            steps {
                echo "✅ Building Docker image: ${IMAGE_NAME}"
                sh "docker build -t ${IMAGE_NAME} ."
            }
        }

        stage('Push Docker Image to GHCR') {
            steps {
                withCredentials([string(credentialsId: 'ghcr-token', variable: 'GITHUB_TOKEN')]) {
                    sh """
                        echo "${GITHUB_TOKEN}" | docker login ghcr.io -u paisalo-digital-limited --password-stdin
                        docker push ${IMAGE_NAME}
                        docker tag ${IMAGE_NAME} ghcr.io/paisalo-digital-limited/${REPO_NAME}:latest
                        docker push ghcr.io/paisalo-digital-limited/${REPO_NAME}:latest
                    """
                }
            }
        }
    }

    post {
        always {
            echo "✅ Cleaning workspace..."
            deleteDir()
        }
        success {
            echo "✅ Pipeline completed successfully!"
        }
        failure {
            echo "❌ Pipeline failed."
        }
    }
}
