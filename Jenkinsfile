  
pipeline {
    agent any    
    stages {
        stage('Display Current Directory') {
			steps {
			    script {
			        def currentDir = powershell(returnStdout: true, script: 'Get-Location')
			        println currentDir
			    }
        	}
        }
		stage('Docker Build') {
			steps {
			    powershell(script: '''
			        docker-compose build
			        docker images -a
			    ''')
        	}
        }
        stage('Run Test Application') {
            steps {
                powershell(script: 'docker-compose up -d')    
            }
        }
        stage('Run Integration Tests') {
            steps {
                powershell(script: './IntegrationTests/ContainerTests.ps1') 
            }
        }
        stage('Stop Test Application') {
            steps {
                powershell(script: 'docker-compose down') 
                // powershell(script: 'docker volumes prune -f')   		
            }
             post {
	         success {
	              echo "Build successfull! You should deploy! :)"
				}
	         failure {
	              echo "Build failed! You should receive an e-mail! :("
				}
            }
        }
		stage('Push Images') {
			when { branch 'main' }
				steps {
					script {
						docker.withRegistry('https://index.docker.io/v1/', 'DockerHub') {
						def image = docker.image("dockerlakov/myonlineshop_catalog")
						image.push("1.0.${env.BUILD_ID}")
						image.push('latest')
						}
					}
				}
		}
    }
}