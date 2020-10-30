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
			    powershell(script: 'cd ./MyOnlineShop')
        	}
			steps {
			    powershell(script: '''
			        docker-compose build
			        docker images -a
			    ''')
        	}
        }
    }
}