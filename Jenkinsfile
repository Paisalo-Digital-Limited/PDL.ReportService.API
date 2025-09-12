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
        SONAR_PROJECT_KEY = 'sonar-token'
        SONAR_HOST_URL = 'https://pdlsonar.paisalo.in:9999'
    }

    stages {
        stage('Initialize Variables') {
            steps {
                script {
                    TIMESTAMP = sh(script: "date -u +%Y%m%d%H%M%S", returnStdout: true).trim()
                    def gitUrl = sh(script: "git config --get remote.origin.url", returnStdout: true).trim()
                    def repoBase = gitUrl.tokenize('/').last().replaceAll(/\\.git$/, '')
                    REPO_NAME = repoBase.toLowerCase()
                    IMAGE_NAME = "ghcr.io/paisalo-digital-limited/${REPO_NAME}:${TIMESTAMP}"

                    env.REPO_NAME = REPO_NAME
                    env.IMAGE_NAME = IMAGE_NAME
                    env.TIMESTAMP = TIMESTAMP

                    echo " Extracted REPO_NAME: ${REPO_NAME}"
                    echo " Generated IMAGE_NAME: ${IMAGE_NAME}"
                }
            }
        }

        stage('Checkout Code') {
            steps {
                echo " Checking out code from Git..."
                checkout scm
            }
        }

        stage('Restore Dependencies') {
            steps {
                echo " Restoring .NET 8 dependencies..."
                sh 'dotnet restore PDL.ReportService.API/PDL.ReportService.API.csproj || true'
            }
        }

        stage('Debug Project Paths') {
            steps {
                echo " Listing .csproj files..."
                sh 'find . -name "*.csproj"'
            }
        }

        stage('Build Solution') {
            steps {
                echo " Building the solution..."
                sh 'dotnet build PDL.ReportService.API/PDL.ReportService.API.csproj --no-restore $WARNING_FLAGS || true'
            }
        }

        stage('SonarQube SAST Scan') {
            steps {
                withCredentials([string(credentialsId: 'sonar-token', variable: 'SONAR_TOKEN')]) {
                    script {
                        try {
                            echo " Running SonarQube SAST Scan..."
                            sh """
                                dotnet tool install --global dotnet-sonarscanner || true
                                export PATH="\$PATH:\$HOME/.dotnet/tools"

                                dotnet sonarscanner begin \
                                    /k:"${SONAR_PROJECT_KEY}" \
                                    /d:sonar.host.url="${SONAR_HOST_URL}" \
                                    /d:sonar.login="${SONAR_TOKEN}"

                                dotnet build PDL.ReportService.API/PDL.ReportService.API.csproj

                                dotnet sonarscanner end \
                                    /d:sonar.login="${SONAR_TOKEN}"
                            """
                        } catch (e) {
                            echo " SonarQube SAST Scan failed, continuing..."
                        }
                    }
                }
            }
        }

        stage('SonarQube SCA Scan') {
            steps {
                echo " SonarQube SCA Scan (placeholder)..."
                // Add third-party dependency scanner here
            }
        }

        stage('SonarQube Final Scan') {
            steps {
                echo " SonarQube Final Scan (placeholder)..."
                // Add any final Sonar validation or gates
            }
        }

        stage('Run Unit Tests') {
            steps {
                echo " Running unit tests (if available)..."
                sh '''
                    if ls PDL.ReportService.API.Tests/*.csproj 1> /dev/null 2>&1; then
                        dotnet test PDL.ReportService.API.Tests/PDL.ReportService.API.Tests.csproj \
                            --collect:"XPlat Code Coverage" \
                            --results-directory:TestResults \
                            --logger "trx" \
                            --no-build $WARNING_FLAGS || true
                    else
                        echo " No unit test project found."
                    fi
                '''
            }
        }

        stage('Check Code Coverage') {
            steps {
                echo " Placeholder for code coverage checks..."
                // Add code to enforce threshold here
            }
        }

        stage('Lint Check') {
            steps {
                echo " Placeholder for linting (e.g., dotnet format)..."
            }
        }

        stage('Publish Artifacts') {
            steps {
                echo " Publishing build artifacts..."
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
                echo " Building Docker image: ${IMAGE_NAME}"
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
                        
                        #  remove both tags from local storage after pushing
                        docker rmi ghcr.io/paisalo-digital-limited/${REPO_NAME}:${TIMESTAMP} || true
                        docker rmi ghcr.io/paisalo-digital-limited/${REPO_NAME}:latest || true
                    """
                }
            }
        }
    }

    post {
        always {
            echo " Declarative: Post Actions - Cleaning workspace..."
            deleteDir()
        }
        success {
            echo " Declarative: Post Actions - Pipeline completed successfully!"
        }
        failure {
            echo " Declarative: Post Actions - Pipeline failed."
        }
    }
}
