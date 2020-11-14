  
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
						
						def catalogImage = docker.image("dockerlakov/myonlineshop-catalog")
						catalogImage.push("1.0.${env.BUILD_ID}")
						catalogImage.push('latest')
						
						def identityImage = docker.image("dockerlakov/myonlineshop-identity")
						identityImage.push("1.0.${env.BUILD_ID}")
						identityImage.push('latest')
						
						def statisticsImage = docker.image("dockerlakov/myonlineshop-statistics")
						statisticsImage.push("1.0.${env.BUILD_ID}")
						statisticsImage.push('latest')
						
						def orderingImage = docker.image("dockerlakov/myonlineshop-ordering")
						orderingImage.push("1.0.${env.BUILD_ID}")
						orderingImage.push('latest')
						
						def shoppingCartImage = docker.image("dockerlakov/myonlineshop-shoppingcart")
						shoppingCartImage.push("1.0.${env.BUILD_ID}")
						shoppingCartImage.push('latest')
						
						def shoppingCartGatewayImage = docker.image("dockerlakov/myonlineshop-shoppingcartgateway")
						shoppingCartGatewayImage.push("1.0.${env.BUILD_ID}")
						shoppingCartGatewayImage.push('latest')
						
						def webMVCImage = docker.image("dockerlakov/myonlineshop-webmvc")
						webMVCImage.push("1.0.${env.BUILD_ID}")
						webMVCImage.push('latest')
						
						def webMVCAdminImage = docker.image("dockerlakov/myonlineshop-webmvcadmin")
						webMVCAdminImage.push("1.0.${env.BUILD_ID}")
						webMVCAdminImage.push('latest')
						
						def watchdogImage = docker.image("dockerlakov/myonlineshop-watchdog")
						watchdogImage.push("1.0.${env.BUILD_ID}")
						watchdogImage.push('latest')
						}
					}
				}
		}
    }
}