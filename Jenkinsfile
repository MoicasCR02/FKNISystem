pipeline {
    agent any

    environment {
        GIT_BRANCH = 'master'
        GIT_URL    = 'https://github.com/MoicasCR02/FKNISystem'

        SONARQUBE_SERVER = 'SonarQubeServer'
        SONAR_PROJECT_KEY = 'FKNI.Web'
        SONAR_PROJECT_NAME = 'FKNI.Web'
        SONAR_PROJECT_VERSION = '1.0.0'

        CX_PROJECT_NAME = 'FKNI.Web'
        CX_TEAM         = 'Company/Teams/DevSecOps'
        CX_PRESET       = 'Default'

        SOLUTION = 'FKNI.Web.sln'
        TEST_PROJECT = 'FKNI.Tests/FKNI.Tests.csproj'
    }

    stages {
        stage('Checkout') {
            steps {
                git branch: env.GIT_BRANCH, url: env.GIT_URL
            }
        }

        stage('Restore') {
            steps {
                bat "dotnet restore ${env.SOLUTION}"
            }
        }

        stage('Build') {
            steps {
                bat "dotnet build ${env.SOLUTION} --configuration Release --no-restore"
            }
        }

        stage('Unit Tests') {
            steps {
                // Genera un archivo fijo de resultados
                bat "dotnet test ${env.TEST_PROJECT} --configuration Release --no-build --logger \"trx;LogFileName=test_results.trx\""
            }
        }

        stage('SonarQube analysis') {
            steps {
                withSonarQubeEnv("${env.SONARQUBE_SERVER}") {
                    bat """
                        dotnet sonarscanner begin /k:"${env.SONAR_PROJECT_KEY}" /n:"${env.SONAR_PROJECT_NAME}" /v:"${env.SONAR_PROJECT_VERSION}"
                        dotnet build ${env.SOLUTION} --configuration Release
                        dotnet sonarscanner end
                    """
                }
            }
        }

        stage('Checkmarx scan') {
            steps {
                bat """
                    cx scan \
                        --project-name "${env.CX_PROJECT_NAME}" \
                        --team "${env.CX_TEAM}" \
                        --preset "${env.CX_PRESET}" \
                        --sast \
                        --file-source .
                """
            }
        }

        stage('Publish') {
            steps {
                // Publica la aplicación en carpeta ./publish
                bat "dotnet publish ${env.SOLUTION} --configuration Release --output publish"
                echo 'Publicación lista en ./publish'
            }
        }
    }

    post {
        always {
            // Publica resultados de pruebas
            junit '**/test_results.trx'

            // Archiva artefactos publicados
            archiveArtifacts artifacts: 'publish/**/*', fingerprint: true, onlyIfSuccessful: true
        }
        failure {
            echo 'Pipeline falló: revisa errores de build, tests, SonarQube o Checkmarx.'
        }
        success {
            echo 'Pipeline exitoso: build, pruebas, análisis de calidad y seguridad completados.'
        }
    }
}
