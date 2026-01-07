pipeline {
    agent any
    //aja
    environment {
        SOLUTION = 'FKNI.Web.sln'
        TEST_PROJECT = 'FKNI.Tests/FKNI.Tests.csproj'
    }

    stages {
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
                bat """
                mkdir TestResults
                dotnet test ${env.TEST_PROJECT} --configuration Release --no-build --logger \"trx;LogFileName=test_results.trx\" --results-directory TestResults
                """
            }
        }
    }

    post {
        always {
            step([$class: 'MSTestPublisher', testResultsFile: 'TestResults/test_results.trx'])
               }
         }

    }
}

