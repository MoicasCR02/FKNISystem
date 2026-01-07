pipeline {
    agent any

    environment {
        GIT_BRANCH = 'master'
        GIT_URL    = 'https://github.com/MoicasCR02/FKNISystem'

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
                // Genera el archivo .trx en la carpeta del workspace
                bat "dotnet test ${env.TEST_PROJECT} --configuration Release --no-build --logger \"trx;LogFileName=test_results.trx\" --results-directory \"${env.WORKSPACE}\\TestResults\""
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
            // Publica resultados de pruebas desde la carpeta fija
            junit 'TestResults/test_results.trx'

            // Archiva artefactos publicados
            archiveArtifacts artifacts: 'publish/**/*', fingerprint: true, onlyIfSuccessful: true
        }
        failure {
            echo 'Pipeline falló: revisa errores de build o tests.'
        }
        success {
            echo 'Pipeline exitoso: build, pruebas y publicación completados.'
        }
    }
}
