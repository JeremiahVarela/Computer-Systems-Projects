// pi-estimating program,written in c, serial (non-parallel.) implementation
#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>



// Global variables, denotes how many terms there are, and the sum for pi
double sum = 0.0; // final sum of int
int n;// number of terms to add
int thread_count; // Number of threads the program will have

//function to be called by threads
void* Thread_sum(void* rank);

// Main function
int main(int argc, char* argv[]) {
  // Thread handle
  pthread_t* thread_handles;
  
  // Get a number of terms ti add from the command line 
  thread_count = strtol(argv[1], NULL, 10);
  n = strtol(argv[1], NULL, 10);
  
  // Allocate memory for the thread handles
  thread_handles = malloc(thread_count * sizeof(pthread_t));
  
  long thread;
  for(thread = 0; thread < thread_count; thread++) {
    pthread_create(&thread_handles[thread], NULL, Thread_sum, (void*)thread);
  }
  
  for (thread = 0; thread < thread_count; thread++) {
    pthread_join(thread_handles[thread], NULL);
  }
  
  //deallocate mem
  free (thread_handles);
  double pi = 4.0 * sum;
  printf("The estimated value of pi is %f.\n", pi);
return 0;
}

void* Thread_sum(void* rank) {
  long my_rank = (long) rank;
  double factor;
  long long i;
  long long my_n = n/ thread_count;
  long long my_first_i = my_n *my_rank;
  long long my_last_i = my_first_i + my_n;
  
  
  // Make sure the sign for first term is correct
  if (my_first_i % 2 == 0) factor = 1.0;
  else factor = -1.0;

  // Add my_n consecutive terms, from my_first_i to my_last_i
  for (i = my_first_i; i < my_last_i; i++, factor = -factor) {
    sum += factor / (2 * i + 1);
  }
  
  return NULL;
}

