pipeline {
    agent any

    // Variables de configuración (ajústalas a tu entorno) ok
    environment {
        // Git
        GIT_BRANCH = 'master'
        GIT_URL    = 'https://github.com/MoicasCR02/FKNISystem'

        // SonarQube
        // Este nombre debe coincidir con el "Name" del servidor configurado en Jenkins (Manage Jenkins > Configure System > SonarQube servers)
        SONARQUBE_SERVER = 'SonarQubeServer'
        SONAR_PROJECT_KEY = 'FKNI.Web'
        SONAR_PROJECT_NAME = 'FKNI.web'
        SONAR_PROJECT_VERSION = '1.0.0'
        // Usa credenciales seguras en Jenkins (Credentials) y referéncialas con withCredentials
        // Si usas token de login clásico:
        // SONAR_LOGIN_TOKEN = credentials('SONAR_TOKEN_ID')

        // Checkmarx (CxCLI o CxFlow según tu instalación)
        // Ajusta estos valores a tu proyecto de Checkmarx
        CX_PROJECT_NAME = 'FKNI.Web'
        CX_TEAM         = 'Company/Teams/DevSecOps'
        CX_PRESET       = 'Default'
        // Referencia a credenciales configuradas en Jenkins
        // CX_USERNAME = credentials('CX_USERNAME_ID')
        // CX_PASSWORD = credentials('CX_PASSWORD_ID')

        // .NET
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
                // Genera logs TRX para que Jenkins los publique
                bat "dotnet test ${env.TEST_PROJECT} --configuration Release --no-build --logger trx"
            }
        }

        stage('SonarQube analysis') {
            steps {
                // Requiere: SonarScanner for .NET instalado en el agente
                withSonarQubeEnv("${env.SONARQUBE_SERVER}") {
                    // Si usas token, agrega /d:sonar.login=$(SONAR_LOGIN_TOKEN) con withCredentials
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
                // Ejemplo con CxCLI; ajusta el comando a tu instalación (ruta de cx, flags, etc.)
                // Si usas credenciales, envuélvelas con withCredentials y pásalas como parámetros seguros.
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

        stage('Quality gates (opcional)') {
            steps {
                // Si tu Jenkins tiene el plugin "Quality Gates" de Sonar, puedes esperar el resultado:
                // waitForQualityGate() // Falla el pipeline si el quality gate no se cumple
                echo 'Verificación de Quality Gate de Sonar y políticas de seguridad (configurar según tu plugin).'
            }
        }

        stage('Publish (opcional)') {
            steps {
                bat "dotnet publish ${env.SOLUTION} -c Release -o ./publish"
                echo 'Publicación lista en ./publish (integra copia a servidor o build de Docker según tu flujo).'
            }
        }
    }

    post {
        always {
            // Publica resultados de pruebas
            junit '**/TestResults/*.trx'

            // Archiva artefactos (opcional)
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
