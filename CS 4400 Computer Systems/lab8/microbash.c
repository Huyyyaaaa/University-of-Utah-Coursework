#include <unistd.h>
#include <stdlib.h>

int main(int argc, char* argv[])
{
  
  char* argv1[] = {argv[1], NULL};
  char* argv2[] = {argv[2],NULL};
  char* argv3[] = {argv[3], NULL};

  int fd[2];
  int fd2[2];

  pipe(fd);
  pipe(fd2);
 
  // First child
  if (fork() == 0) {
    dup2(fd[1], 1);
    close(fd[0]);
    close(fd2[0]);
    close(fd2[1]);
    if(execv(argv1[0], argv1) == -1)
      exit(1);
  }
 
  // Second child
  if (fork() == 0) {
    dup2(fd[0], 0);
    dup2(fd2[1],1);
    close(fd[1]);
    close(fd2[0]);
    if(execv(argv2[0], argv2) == -1)
      exit(1);
  }
  
  //third child
  if(fork() == 0){
    dup2(fd2[0],0);
    close(fd[0]);
    close(fd[1]);
    close(fd2[1]);
    if(execv(argv3[0], argv3)==-1)
      exit(1);
  }

  close(fd[0]);
  close(fd[1]);
  close(fd2[0]);
  close(fd2[1]);

  // Parent
  int status;
  wait(&status);
  wait(&status);
  wait(&status);
}

