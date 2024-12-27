pipeline {
    agent any

    stages {
        stage('ssh to server with user tuanhuy') {
            steps {
                sshagent(['ssh-remote-user-tuanhuy']) {
                    sh '''
                        ssh -o StrictHostKeyChecking=no -l tuanhuy 15.235.197.40 "cd /home/tuanhuy && ./deploy_da_co_so.sh"
                    '''
                }
            }
        }
    }

    post {
        always {
            echo 'Pipeline execution finished.'
        }
        success {
            echo 'Code checkout completed successfully!'
        }
        failure {
            echo 'Code checkout failed!'
        }
    }
}
